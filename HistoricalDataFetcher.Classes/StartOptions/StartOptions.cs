using CommandLine;

namespace HistoricalDataFetcher.Classes.StartOptions
{
    public class StartOptions
    {
        [Option('h', "host", Required = true, HelpText = "Base URL <server.com> of the Metasys Application")]
        public string Host { get; set; }
        [Option('u', "username", Required = true, HelpText = "Username for the Metasys Application")]
        public string Username { get; set; }
        [Option('p', "password", Required = true, HelpText = "Password for the Metasys Application")]
        public string Password { get; set; }
        [Option('s', "service", Default = "time", HelpText = "Comma separated list of the service you wish to run({time}{,audit}{,alarm}).  Minimum of 1 service is required")]
        public string Service { get; set; }
        [Option('d', "dest", Default = "SqlServer", HelpText = "The Destination the data should be saved to ({Csv} | {SqlServer})")]
        public string Destination { get; set; }
        [Option('x', "dbconnection", HelpText = "(Default: local) Connection string required to connect to the desired DB")]
        public string DBConnectionString { get; set; }
        [Option('f', "fqrs", HelpText = "The absolute path to the file containing the fullly qualified references")]
        public string FqrPath { get; set; }
    }
}