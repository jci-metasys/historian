using log4net;
using System.Reflection;
using Topshelf;

[assembly: log4net.Config.XmlConfigurator(ConfigFile = "log4net.config")]
namespace HistoricalDataFetcher.WindowsService
{
    class Program
    {
        private static ILog _log;

        static void Main(string[] args)
        {
            _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
            _log.Info("Incremental Data Extractor Main.");

            HostFactory.Run(configurator =>
            {
                configurator.Service<IncrementalExtractionService>(settings =>
                {
                    settings.ConstructUsing(s => new IncrementalExtractionService());
                    settings.WhenStarted(s => s.Start());
                    settings.WhenStopped(s => s.Stop());
                });

                configurator.RunAsLocalSystem();
                configurator.SetServiceName("IncrementalDataExtractor");
                configurator.SetDisplayName("Incremental Data Extractor");
                configurator.SetDescription("Retrieve data from Metasys API on a set time interval.");
                configurator.StartAutomatically();
            });
        }
    }
}
