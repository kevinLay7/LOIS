using NLog;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LOIS.ProcessingEngine.BLL
{
    public sealed class ProcessManager
    {
        static readonly ProcessManager _instance = new ProcessManager();
        public static ProcessManager Instance = _instance;

        private static Logger logger = LogManager.GetCurrentClassLogger();
        private static CancellationTokenSource tokenSource;

        ProcessManager()
        {
            tokenSource = new CancellationTokenSource();
        }
        
        public void Start()
        {
            var token = tokenSource.Token;
            string[] locations = Properties.Settings.Default.FileLocations.Split(';').Where(x => x != "").ToArray();

            foreach (var location in locations)
            {
                //Task.Run(() => new PdfLocationProcessor(location, token).BeginProcessing(), token, TaskCreationOptions.LongRunning);
                Task.Factory.StartNew(() => new PdfLocationProcessor(location, token).BeginProcessing(), token, TaskCreationOptions.LongRunning, TaskScheduler.Default);
            }
        }

        public void Stop()
        {
            tokenSource.Cancel();
        }
    }
}
