using HistoricalDataFetcher.Classes.DataLayer.Cache;
using HistoricalDataFetcher.Classes.Services;
using HistoricalDataFetcher.Classes.Utilities;
using HistoricalDataFetcher.DataStorage.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace HistoricalDataFetcher.Tests
{
    [TestClass]
    public class JobManagerTests
    {
        private IConfiguration _config;
        [TestInitialize]
        public void TestInitialize()
        {
            // Adding JSON file into IConfiguration.
            _config = new ConfigurationBuilder()
                .AddJsonFile(@"C:\appsettings.json", true, true)
                .Build();
            DBDataModel.DBConnectionString = $"Server={_config["dbcomputer"]};Integrated Security=false;User Id={_config["dbuser"]};Password={_config["dbpassword"]};Database=DataExtractor";
            ApiRequest.InitializeAsync(new NullCache(), _config["username"], _config["password"], _config["host"]).Wait();
        }
    }
}
