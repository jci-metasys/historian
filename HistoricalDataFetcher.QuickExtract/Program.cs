using System;
using CommandLine;
using HistoricalDataFetcher.Classes.Controller;
using HistoricalDataFetcher.Classes.Services;
using HistoricalDataFetcher.Classes.StartOptions;
using HistoricalDataFetcher.Classes.Utilities;
using HistoricalDataFetcher.DataStorage.TimeSeries;
using System.Collections.Generic;
using System.Threading.Tasks;

[assembly: log4net.Config.XmlConfigurator(ConfigFile = "log4net.config")]
namespace HistoricalDataFetcher.QuickExtract
{
    class Program
    {
        private static Controller _con;

        static void Main(string[] args)
        {
            _con = new Controller();
            CommandLine.Parser.Default.ParseArguments<QuickStartOptions>(args).WithParsed(opts => RunOptionsAsync(opts).Wait()).WithNotParsed<QuickStartOptions>((errs) => HandleParseError(errs));
            
        }

        /// <summary>
        /// Run parsed options
        /// </summary>
        /// <param name="opts">Parsed QuickstartOptions</param>
        /// <returns></returns>
        private static async Task RunOptionsAsync(QuickStartOptions opts)
        {
            opts.InvalidCertificate = true;

            if (!await _con.InitApiRequestAsync(opts))
            {
                System.Console.WriteLine(@"Could not log in: The Host, Username, and Password combination did not work, please try again!");
                return;
            }

            //Initialize TaskEnumUtility
            _con.SetEndPoints(opts);

            //Load EnumFile into Memory
            if (!await _con.InitStartEnumDownloadAsync())
            {
                Console.WriteLine(@"Failed to load Enumset!");
                return;
            }

            LoggerService.LogApiRequest("", 0, "", "STARTING EXTRACTION!!");
            System.Console.WriteLine("Running the Quickstart option.  Please be patient as we extract your data!");

            //Run a quick sample list
            var qsTime = new TimeSeriesQuickstartEndPoint(new TimeSeriesSaveToCsv());
            await qsTime.RunAsync($"{ApiRequest.UrlBase}/networkDevices?page=1&pageSize=10");

            System.Console.WriteLine("Done!");
        }

        /// <summary>
        /// Handles the error from parsing the arugments into the QuickstartOptions
        /// </summary>
        /// <param name="error">List of errors from parsing the arguments</param>
        private static void HandleParseError(IEnumerable<Error> error)
        {
            System.Console.WriteLine(@"There was an error with the arguments: re-enter the arguments based on the usage output, please try again!");
        }
    }
}
