using CommandLine;
using HistoricalDataFetcher.Classes.Controller;
using HistoricalDataFetcher.Classes.StartOptions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

[assembly: log4net.Config.XmlConfigurator(ConfigFile = "log4net.config")]
namespace HistoricalDataFetcher.Console
{
    class Program
    {
        private static Controller _con;
        private static Stopwatch _stopWatch;

        static void Main(string[] args)
        {
            _con = new Controller();
            _stopWatch = new Stopwatch();
            
            var result = CommandLine.Parser.Default.ParseArguments<ConsoleOptions>(args).WithParsed(opts => RunOptionsAsync(opts).Wait()).WithNotParsed<ConsoleOptions>((errs) => HandleParseError(errs));
            
            if (_stopWatch.Elapsed.Milliseconds > 0)
                System.Console.WriteLine("Total processing time was " + TimeSpan.FromMilliseconds(_stopWatch.Elapsed.TotalMilliseconds).ToString());
        }

        /// <summary>
        /// Run parsed arguments as ConsoleOptions
        /// </summary>
        /// <param name="opts">Parsed ConsoleOptions</param>
        /// <returns></returns>
        public static async Task RunOptionsAsync(ConsoleOptions opts)
        {
            if (!await _con.SetDBSettingsAsync(opts))
            {
                System.Console.WriteLine(@"Could not log in:
                The Database Host, Username, and Password combination did not work, please try again!");
                return;
            }
            else if (!await _con.InitApiRequestAsync(opts))
            {
                System.Console.WriteLine(@"Could not log in:
                The Host, Username, and Password combination did not work, please try again!");
                return;
            }

            //Load the EnumFile
            if (!await _con.InitStartEnumDownloadAsync())
            {
                System.Console.WriteLine(@"Failed to load Enumset!");
                return;
            }

            //Initialize TaskEnumUtility
            _con.SetEndPoints(opts);

            System.Console.WriteLine("Please be patient while your data is being extracted!");

            //Create and process the Jobs
            var success = await _con.CreateAndProcessJobAsync(opts);
            if(success)
            {
                System.Console.WriteLine("Complete: Your data has been extracted.");
            }
            else
            {
                System.Console.WriteLine("Error: There was a problem extracting your data and completing the job");
            }
        }

        /// <summary>
        /// Handles when the argumants can not be parsed into ConsoleOptions
        /// </summary>
        /// <param name="error"></param>
        private static void HandleParseError(IEnumerable<Error> error)
        {
            System.Console.WriteLine(@"There was an error with the arguments: re-enter the arguments based on the usage output, please try again!");
        }
    }
}
