using System;
using System.IO;
using System.Windows;
using MessageBoxLibrary;

namespace W10_Logon_BG_Changer
{
    /// <summary>
    ///     Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            // This will hook any Unhandled Exception on the application
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            base.OnStartup(e);
        }

        void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            try
            {
                DateTime dtLogFileCreated = DateTime.Now;
                StreamWriter sw = new StreamWriter("log-" + dtLogFileCreated.Day + dtLogFileCreated.Month
                        + dtLogFileCreated.Year + "-" + dtLogFileCreated.Second
                        + dtLogFileCreated.Minute + dtLogFileCreated.Hour + ".log");

                sw.WriteLine("### W10_Logon_BG_Changer Crash ###");
                sw.WriteLine(HandleException((Exception)e.ExceptionObject));
                sw.Close();

                WpfMessageBox.Show(LanguageLibrary.Language.Default.default_error_msg,
                    LanguageLibrary.Language.Default.title_error, MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                Current.Shutdown();
            }
        }

        /// <summary>
        /// Handles any exception and returns the formated output
        /// </summary>
        /// <param name="ex">The exception</param>
        public string HandleException(Exception ex)
        {
            string auxError = ex.Message;
            Exception inner = ex.InnerException;
            while (inner != null)
            {
                auxError += "\n" + inner.Message;
                inner = inner.InnerException;
            }

            auxError += "\n" + ex.StackTrace;
            return auxError;
        }
    }
}