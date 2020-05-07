using LOIS.ProcessingEngine.BLL;
using NLog;
using System;
using System.Windows;

namespace LOIS.ProcessingEngine
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Logger logger = LogManager.GetCurrentClassLogger();
        private ProcessManager processManager;

        public MainWindow()
        {
            InitializeComponent();

            AppDomain currentDomain = AppDomain.CurrentDomain;
            currentDomain.UnhandledException += CurrentDomain_UnhandledException;

            processManager = ProcessManager.Instance;
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var exception = (Exception)e.ExceptionObject;
            logger.Error("Unhandled execption: " + exception.Message + "\nStacktrace: " + exception.StackTrace);
        }
        
        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            logger.Info("Starting....");
            processManager.Start();
        }

        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            logger.Info("Stopping....");
            processManager.Stop();
            logger.Info("Stopped");
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var win = new ConfigWindow();
            win.Show();
        }
    }
}
