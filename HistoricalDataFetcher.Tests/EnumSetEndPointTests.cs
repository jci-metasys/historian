using HistoricalDataFetcher.Classes.DataLayer.Cache;
using HistoricalDataFetcher.Classes.Endpoints;
using HistoricalDataFetcher.Classes.EnumSet;
using HistoricalDataFetcher.Classes.Utilities;
using HistoricalDataFetcher.DataStorage.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HistoricalDataFetcher.Tests
{
    [TestClass]
    public class EnumSetEndPointTests
    {
        private EnumSetEndPoint _enumSetEndPoint;
        private EnumDescCollection _edc;
        [TestInitialize]
        public void InitializeTest()
        {
            // Adding JSON file into IConfiguration.
            IConfiguration config = new ConfigurationBuilder()
                .AddJsonFile(@"C:\appsettings.json", true, true)
                .Build();
            DBDataModel.DBConnectionString = $"Server={config["dbcomputer"]};Integrated Security=false;User Id={config["dbuser"]};Password={config["dbpassword"]};Initial Catalog=DataExtractor";
            ApiRequest.InitializeAsync(new NullCache(), config["username"], config["password"], config["host"]).Wait();
            _enumSetEndPoint = new EnumSetEndPoint();
            _edc = new EnumDescCollection
            {
                new EnumDescDataStoreModel { SetId = 1050, MemberId = 0, SetDesc = "(0-8) Stages", MemberDesc = "0 Stages" },
                new EnumDescDataStoreModel { SetId = 1050, MemberId = 1, SetDesc = "(0-8) Stages", MemberDesc = "1 Stages" },
                new EnumDescDataStoreModel { SetId = 1050, MemberId = 2, SetDesc = "(0-8) Stages", MemberDesc = "2 Stages" },
                new EnumDescDataStoreModel { SetId = 1050, MemberId = 3, SetDesc = "(0-8) Stages", MemberDesc = "3 Stages" },
                new EnumDescDataStoreModel { SetId = 1050, MemberId = 4, SetDesc = "(0-8) Stages", MemberDesc = "4 Stages" },
                new EnumDescDataStoreModel { SetId = 1050, MemberId = 5, SetDesc = "(0-8) Stages", MemberDesc = "5 Stages" },
                new EnumDescDataStoreModel { SetId = 1050, MemberId = 6, SetDesc = "(0-8) Stages", MemberDesc = "6 Stages" },
                new EnumDescDataStoreModel { SetId = 1050, MemberId = 7, SetDesc = "(0-8) Stages", MemberDesc = "7 Stages" },
                new EnumDescDataStoreModel { SetId = 1050, MemberId = 8, SetDesc = "(0-8) Stages", MemberDesc = "8 Stages" }
            };
        }
        [TestMethod]
        public void EnumSetEndPointRunAsync()
        {
            IEnumDescRepository csv = new EnumDescSaveToCsv();
            ((EnumDescSaveToCsv)csv).FileName = @".\EnumSetFileSaveTest.csv";
            _enumSetEndPoint.DataStore = csv;

            _enumSetEndPoint.RunAsync($"https://{ApiRequest.HttpHost}/{_enumSetEndPoint.ApiEndpoint}").Wait();
            _enumSetEndPoint.SaveDataAsync();

            EnumDescCollection edc = csv.GetDataEnumDescSetAsync().Result;
            bool hasPassed = true;

            foreach(EnumDescDataStoreModel dm in edc)
            {
                if (!_edc.Exists(x => x.SetId == dm.SetId && x.MemberId == dm.MemberId))
                {
                    hasPassed = false;
                    break;
                }
            }

            Assert.IsTrue(hasPassed);
        }
    }
}
