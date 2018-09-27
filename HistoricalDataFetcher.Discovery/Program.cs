using CommandLine;
using HistoricalDataFetcher.Classes.Controller;
using HistoricalDataFetcher.Classes.StartOptions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

[assembly: log4net.Config.XmlConfigurator(ConfigFile = "log4net.config")]
namespace HistoricalDataFetcher.Discovery
{
    class Program
    {
        private static Controller _con;
        static void Main(string[] args)
        {
            _con = new Controller();
            CommandLine.Parser.Default.ParseArguments<StartOptions>(args).WithParsed(opts => RunOptionsAsync(opts).Wait()).WithNotParsed<StartOptions>((errs) => HandleParseError(errs));
            
            Console.WriteLine("Done!");
        }

        /// <summary>
        /// Run parsed Options
        /// </summary>
        /// <param name="opts">Start Options parsed from the arguments</param>
        /// <returns></returns>
        private static async Task RunOptionsAsync(StartOptions opts)
        {
            if (!await _con.SetDBSettingsAsync(opts))
            {
                Console.WriteLine(@"Could not log in:
                The Host, Username, and Password combination did not work, please try again!");
                return;
            }
            else if (!await _con.InitApiRequestAsync(opts))
            {
                Console.WriteLine(@"Could not log in:
                The Host, Username, and Password combination did not work, please try again!");
                return;
            }

            try
            {
                //Load CSV to DB
                await _con.RunDiscoveryEnumSetAsync();
            }
            catch
            {
                Console.WriteLine(@"Failed to run Discovery Enumset!");
                return;
            }

            //Load the EnumFile
            if (!await _con.InitStartEnumDownloadAsync())
            {
                Console.WriteLine(@"Failed to load Enumset!");
                return;
            }

            //Initialize TaskEnumUtility
            _con.SetEndPoints(opts);

            if (!string.IsNullOrWhiteSpace(opts.FqrPath))
            {
                try
                {
                    //Load CSV to DB
                    await _con.ReadFqrsFromFileAsync(opts.FqrPath);
                }
                catch
                {
                    Console.WriteLine($"Failed to Read FQRs from the file provided! file path: {opts.FqrPath}");
                    return;
                }
                
            }
            
            return;
        }

        /// <summary>
        /// Handle the parsing error from the arguments passed in
        /// </summary>
        /// <param name="error">List of errors from pasring the arguemnts</param>
        private static void HandleParseError(IEnumerable<Error> error)
        {
            System.Console.WriteLine(@"There was an error with the arguments: re-enter the arguments based on the usage output, please try again!");
        }
    }
}