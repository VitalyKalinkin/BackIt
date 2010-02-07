using System;
using System.Configuration;
using Lattyf.BackIt.Core.Common;
using Lattyf.BackIt.Core.Configuration;
using Lattyf.BackIt.Core.LocalStorage;
using Lattyf.BackIt.Core.Scanner;
using log4net;

namespace Lattyf.BackIt.ConsoleRunner
{
    internal class Program
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof (Program));

        private static void Main(string[] args)
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            if (args.Length < 1)
            {
                Console.WriteLine("Usage: ConsoleRunner.exe <outputpath>");
                return;
            }

            _log.InfoFormat("Starting ConsoleRunner...");

            _log.InfoFormat("Checking configuration file...");

            CoreConfiguration config;
            try
            {
                config = (CoreConfiguration) ConfigurationManager.GetSection("CoreConfiguration");
            }
            catch (Exception ex)
            {
                _log.Fatal("Can't read configuration file", ex);
                return;
            }

            if (config == null)
            {
                _log.FatalFormat("Can't read configuration file.");
                return;
            }

            var storage = LocalStorage.Instance;
            CommonUtil.UseIt(storage);

            var scanner = new FileSystemScanner();
            scanner.Initialize(config);
            scanner.Scan();

            _log.InfoFormat("Done!");
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            _log.Fatal("Caught unhanded exception", (Exception) e.ExceptionObject);
        }
    }
}