using HistoricalDataFetcher.Classes.DataLayer.Cache;
using HistoricalDataFetcher.Classes.Utilities;
using HistoricalDataFetcher.DataStorage.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HistoricalDataFetcher.Tests
{
    [TestClass]
    public class ControllerTest
    {
        private IConfiguration _config;
        [TestInitialize]
        public void InitializeTest()
        {
            // Adding JSON file into IConfiguration.
            _config = new ConfigurationBuilder()
                .AddJsonFile(@"C:\appsettings.json", true, true)
                .Build();
            DBDataModel.DBConnectionString = $"Server={_config["dbcomputer"]};Integrated Security=false;User Id={_config["dbuser"]};Password={_config["dbpassword"]}";
            ApiRequest.InitializeAsync(new NullCache(), _config["username"], _config["password"], _config["host"], true).Wait();
        }

        //[TestMethod]
        //public void InitializeAPIRequest()
        //{
        //    Assert.IsTrue(((Task<bool>)ApiRequest.Initialize(new NullCache(), _config["username"], _config["password"], _config["host"])).Result);
        //}
    }
}
