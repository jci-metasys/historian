using CommandLine;

namespace HistoricalDataFetcher.Classes.StartOptions
{
    public class ConsoleOptions : StartOptions
    {
        [Option('m', "min", Default = 0, Hidden = true, HelpText = "The number of minutes you wish to query")]
        public int Min { get; set; }
        [Option('t', "hrs", Default = 0, Hidden = true, HelpText = "The number of hours you wish to query")]
        public int Hrs { get; set; }
        [Option('D', "days", Default = 0, HelpText = "The number of days you wish to query")]
        public int Days { get; set; }
        [Option('M', "month", Default = 0, HelpText = "The number of months you wish to query")]
        public int Months { get; set; }
    }
}
