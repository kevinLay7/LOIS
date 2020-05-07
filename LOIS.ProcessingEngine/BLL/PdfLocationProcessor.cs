using LOIS.BLL.Documents;
using LOIS.BLL.Factories;
using LOIS.BLL.services;
using LOIS.Core.Models;
using NLog;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LOIS.ProcessingEngine
{
    /// <summary>
    /// Takes a UNC path and will process PDF files in it.
    /// </summary>
    public class PdfLocationProcessor : IDisposable
    {
        private ConcurrentQueue<FileInfo> newFileQueue;
        private ConcurrentQueue<PDFInfo> pdfInfoQueue;
        private FileSystemWatcher fsw;
        private Logger logger = LogManager.GetCurrentClassLogger();
        private string pickupLocation;
        private string badScanLocation;
        private CancellationToken cancellationToken;

        public PdfLocationProcessor(string path, CancellationToken token)
        {
            newFileQueue = new ConcurrentQueue<FileInfo>();
            pdfInfoQueue = new ConcurrentQueue<PDFInfo>();
            pickupLocation = path;
            badScanLocation = System.IO.Path.Combine(path, "BadScans");
            cancellationToken = token;
        }

        public void BeginProcessing()
        {
            logger.Info("Staring Location Processor for " + pickupLocation);

            //Start task to proccess any pickedup files (create the PdfInfo instance for processing /merging)
            Task.Factory.StartNew(() => ProcessPickedupFiles(), cancellationToken, TaskCreationOptions.LongRunning, TaskScheduler.Default);
            
            //Start the task to process the PdfInfo instances for each document
            Task.Factory.StartNew(() => ProcessPdfInfoList(), cancellationToken, TaskCreationOptions.LongRunning, TaskScheduler.Default);
            
            //Get previously existing files
            GetCurrentFiles();

            //Start filesystemwatcher
            StartFileSystemWatcher();
        }

        #region File System Watcher

        private void StartFileSystemWatcher()
        {
            fsw = new FileSystemWatcher();
            fsw.Path = pickupLocation;
            fsw.Filter = "*.pdf";
            fsw.NotifyFilter = NotifyFilters.FileName;
            fsw.InternalBufferSize = 65536; //max buffer size
            fsw.Created += FSWFileCreated;
            fsw.EnableRaisingEvents = true;
            fsw.Error += FSWError;
        }

        private void FSWError(object sender, ErrorEventArgs e)
        {
            logger.Error($"FileSystemWatcher {pickupLocation} encountered an error. Restarting");
            StartFileSystemWatcher();
        }

        private void FSWFileCreated(object sender, FileSystemEventArgs e)
        {
            var fi = new FileInfo(e.FullPath);
            newFileQueue.Enqueue(fi);
            logger.Debug("FSW pickedup file " + fi.Name);
        }

        #endregion

        #region Processing

        /// <summary>
        /// Takes the pickedup files and creates their PDFinfo instance.  This includes barcode processing.
        /// </summary>
        private void ProcessPickedupFiles()
        {
            var rectangle = new LOIS.BLL.DTO.Rectangle()
            {
                StartX = Properties.Settings.Default.startPointX,
                StartY = Properties.Settings.Default.startPointY,
                Width = Properties.Settings.Default.rectW,
                Height = Properties.Settings.Default.rectH,
                RefImageHeight = Properties.Settings.Default.refImageHeight,
                RefImageWidth = Properties.Settings.Default.refImageWidth
            };

            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    Parallel.ForEach(newFileQueue, new ParallelOptions() { MaxDegreeOfParallelism = 2, CancellationToken = cancellationToken }, (file) =>
                    {
                        FileInfo currentFile;
                        newFileQueue.TryDequeue(out currentFile);

                        logger.Debug($"Processing file: {currentFile.Name}");

                        int counter = 0;
                        while (true)
                        {
                            try
                            {
                                if (currentFile.Length < 10 && counter >= 5)
                                {
                                    logger.Info($"{currentFile} is bad.  Moving to badscan folder.");
                                    MoveFileToBadScans(currentFile);
                                    break;
                                }

                                var pdfinfo = PdfInfoFactory.CreatePdfInfo(currentFile.FullName, rectangle);

                                logger.Info(pdfinfo.isBarcode
                                    ? $"PdfInfo created for {currentFile.Name}.  Found barcode {pdfinfo.Barcode}"
                                    : $"PdfInfo created for {currentFile.Name}.");

                                pdfInfoQueue.Enqueue(pdfinfo);
                                break;
                            }
                            catch (IOException)
                            {
                                //Most likely the file is still being written
                                Thread.Sleep(500);
                                counter++;
                            }
                            catch (Exception ex)
                            {
                                logger.Error($"Error processing file {currentFile.Name}. Exception: {ex}");
                                logger.Info($"Moving {currentFile.Name} to the badscan folder");
                                MoveFileToBadScans(currentFile);
                                break;
                            }
                        }
                    });
                }
                catch (Exception ex)
                {
                    logger.Error("Error at ProcessPickedupFiles.  " + ex.Message);
                }
                
                Thread.Sleep(5000);
            }
        }

        /// <summary>
        /// Processes the PdfInfoList to combine pdfs based on barcode location.  I.e. it will combine pdfs between two given barcode files.
        /// </summary>
        private void ProcessPdfInfoList()
        {
            try
            {
                List<PDFInfo> filesToProcess = new List<PDFInfo>();
                var sw = new Stopwatch();

                while (!cancellationToken.IsCancellationRequested)
                {
                    //Dequeue all the files in to the fileToProcess list
                    while (pdfInfoQueue.Count > 0)
                    {
                        PDFInfo tempfile;
                        pdfInfoQueue.TryDequeue(out tempfile);
                        filesToProcess.Add(tempfile);
                    }

                    filesToProcess = filesToProcess.OrderBy(x => x.FilePath).ToList();
                    var barcodeIndex = filesToProcess.Where(x => x.isBarcode).Select(x => filesToProcess.IndexOf(x)).ToArray();

                    if (barcodeIndex.Length > 0)
                    {
                        logger.Trace($"BarcodeFiles Count: {barcodeIndex.Length}");
                        if (sw.IsRunning == false)
                            sw.Start();

                        if (barcodeIndex.Length > 1 || sw.ElapsedMilliseconds >= 60000)
                        {
                            //The barcode file should be the first item in the list, if it isn't then remove items before the first barcode file
                            if (barcodeIndex[0] != 0)
                            {
                                logger.Info($"Found files that come before the first file with a Barcode.  Moving to BadScans");
                                for (int i = 0; i < barcodeIndex[0]; i++)
                                {
                                    MoveFileToBadScans(filesToProcess[0].FilePath);
                                    filesToProcess.Remove(filesToProcess[0]);
                                }
                                continue;
                            }

                            //Reset the stopwatch
                            sw.Reset();

                            var lastFileIndex = filesToProcess.Count;

                            if (barcodeIndex.Length >= 2)
                                lastFileIndex = barcodeIndex[1];
                            logger.Info($"****** Start index: {barcodeIndex[0]}  End: {lastFileIndex}   Length: {filesToProcess.Count}");
                            ProcessRelatedFiles(filesToProcess, barcodeIndex[0], lastFileIndex);
                        }
                    }

                    if (barcodeIndex.Length < 3)
                        System.Threading.Thread.Sleep(5000);
                }
            }
            catch (Exception ex)
            {

                throw;
            }
            
        }

        private void ProcessRelatedFiles(List<PDFInfo> processList, int firstIndexInclusive, int lastIndexExclusive)
        {
            var pdfFileToUse = processList[firstIndexInclusive];

            logger.Info($"Creating PDF entry starting with {pdfFileToUse.FilePath}");

            //List used to cleanup files that were processed
            var processedFiles = new List<PDFInfo>();
            processedFiles.Add(pdfFileToUse);

            using (var creator = new PdfCreator())
            {
                for(var i = firstIndexInclusive; i < lastIndexExclusive; i++)
                {
                    var pdfinfo = processList[i];
                    for(int j = 0; j < pdfinfo.Images.Count; j++)
                    {
                        logger.Info($"Appending file {pdfinfo.FilePath} to {pdfFileToUse.FilePath}");

                        try
                        {
                            creator.AppendImageToPdf(pdfinfo.Images[j]);
                        }
                        catch (Exception ex)
                        {
                            logger.Error($"ERROR appending image to pdf.  File: {pdfinfo.FilePath}");
                            throw;
                        }

                        //free up memory
                        logger.Debug("Clearing memory for pdf " + pdfinfo.FilePath);
                        Array.Clear(pdfinfo.Images[j], 0, pdfinfo.Images[j].Length);
                    }
                    processedFiles.Add(pdfinfo);
                }

                pdfFileToUse.Pdf = creator.GetPdfDocumentBytes();
            }

            try
            {
                //Todo fix so that badsacans still remove the items from the process list
                SaveInfoToDatabase(pdfFileToUse);
                Cleanup(processedFiles, processList);
            }
            catch (Exception ex)
            {
                logger.Error("Error: " + ex.StackTrace);

                for (int i = firstIndexInclusive; i < lastIndexExclusive; i++)
                {
                    MoveFileToBadScans(processList[i].FilePath);
                    processList.Remove(processList[i]);
                }                
            }
           
        }

        #endregion

        private void Cleanup(List<PDFInfo> processedList, List<PDFInfo> processList)
        {
            foreach(var file in processedList)
            {
                logger.Info($"Cleaning up file {file}");
                processList.Remove(file);

                //Move file
                try
                {
                    if (Properties.Settings.Default.DeleteFilesAfterProcessing)
                    {
                        File.Delete(file.FilePath);
                        logger.Info($"Deleted file {file}");
                    }
                    else
                    {
                        File.Move(file.FilePath, Path.Combine(Properties.Settings.Default.FileBackupLocation, new FileInfo(file.FilePath).Name));
                        logger.Info($"Moved file to backup {file}");
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Couldn't move file {file.FilePath}   " + e);
                }

                file.Dispose();
            }
        }

        private void SaveInfoToDatabase(PDFInfo pdf)
        {
            logger.Info($"Getting req info from the database for {pdf.Barcode}");

            var barcode = int.Parse(pdf.Barcode);
            var req = RequisitionFactory.CreateRequisition(barcode, new ProlisService());

            if(req != null)
            {
                var doc = new RequisitionDocument()
                {
                    RequisitionId = req.RequisitionNo,
                    Document = pdf.Pdf,
                    markedDeleted = false
                };

                logger.Info("Req info received.  Saving info.");

                var db = new RequisitionService();
                db.SaveRequisition(req);

                var docService = new LOIS.BLL.services.DocumentService();
                docService.SaveDocument(doc);

                if (req.Patient != null)
                    db.SavePatient(req.Patient);

                logger.Debug("Saved for " + pdf.Barcode);
            }

        }

        private void GetCurrentFiles()
        {
            var directory = new System.IO.DirectoryInfo(pickupLocation);
            var currentlyExistingFiles = directory.GetFiles("*.pdf").OrderBy(x => x.Name);

            foreach(var file in currentlyExistingFiles)
            {
                 try
                 {
                     if(file.Length < 10)
                     {
                         logger.Info($"Bad file {file.Name}.  Moving to bad scan folder.");
                         MoveFileToBadScans(file);
                     }
                     
                     logger.Debug($"Pickup file {file}");
                     newFileQueue.Enqueue(file);
                 }
                 catch (Exception ex)
                 {
                     logger.Error($"Error picking up file: {file.FullName}.  Exception: {ex}");
                 }
             }
        }


        #region BadScan

        private void MoveFileToBadScans(FileInfo file)
        {
            if (Directory.Exists(badScanLocation))
            {
                logger.Info($"Moving file {file.Name} to bad scans.");
                File.Move(file.FullName, Path.Combine(badScanLocation, file.Name));
            }
            else
            {
                Directory.CreateDirectory(badScanLocation);
                MoveFileToBadScans(file);
            }
        }

        private void MoveFileToBadScans(string path)
        {
            var file = new FileInfo(path);
            if (Directory.Exists(badScanLocation))
            {
                logger.Info($"Moving file {file.Name} to bad scans.");
                File.Move(file.FullName, Path.Combine(badScanLocation, file.Name));
            }
            else
            {
                logger.Info("Creating BadScans folder");
                Directory.CreateDirectory(badScanLocation);
                MoveFileToBadScans(file);
            }
        }

        #endregion


        public void Dispose()
        {
            try
            {
                fsw.Dispose();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
