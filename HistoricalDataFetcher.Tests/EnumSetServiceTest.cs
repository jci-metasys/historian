using HistoricalDataFetcher.Classes.DataLayer.Cache;
using HistoricalDataFetcher.Classes.EnumSet;
using HistoricalDataFetcher.Classes.Services;
using HistoricalDataFetcher.Classes.Utilities;
using HistoricalDataFetcher.DataStorage.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace HistoricalDataFetcher.Tests
{
    [TestClass]
    public class EnumSetServiceTests
    {
        private EnumSetService _es;
        private EnumDescCollection _edc = new EnumDescCollection { new EnumDescDataStoreModel { SetId = 1050, MemberId = 0, SetDesc = "(0-8) Stages", MemberDesc = "0 Stages" },
                new EnumDescDataStoreModel { SetId = 1050, MemberId = 1, SetDesc = "(0-8) Stages", MemberDesc = "1 Stages" },
                new EnumDescDataStoreModel { SetId = 1050, MemberId = 2, SetDesc = "(0-8) Stages", MemberDesc = "2 Stages" },
                new EnumDescDataStoreModel { SetId = 1050, MemberId = 3, SetDesc = "(0-8) Stages", MemberDesc = "3 Stages" },
                new EnumDescDataStoreModel { SetId = 1050, MemberId = 4, SetDesc = "(0-8) Stages", MemberDesc = "4 Stages" },
                new EnumDescDataStoreModel { SetId = 1050, MemberId = 5, SetDesc = "(0-8) Stages", MemberDesc = "5 Stages" },
                new EnumDescDataStoreModel { SetId = 1050, MemberId = 6, SetDesc = "(0-8) Stages", MemberDesc = "6 Stages" },
                new EnumDescDataStoreModel { SetId = 1050, MemberId = 7, SetDesc = "(0-8) Stages", MemberDesc = "7 Stages" },
                new EnumDescDataStoreModel { SetId = 1050, MemberId = 8, SetDesc = "(0-8) Stages", MemberDesc = "8 Stages" }
        };

        [TestInitialize]
        public void TestInitialize()
        {
            // Adding JSON file into IConfiguration.
            IConfiguration config = new ConfigurationBuilder()
                .AddJsonFile(@"C:\appsettings.json", true, true)
                .Build();
            DBDataModel.DBConnectionString = $"Server={config["dbcomputer"]};Integrated Security=false;User Id={config["dbuser"]};Password={config["dbpassword"]}";
            ApiRequest.InitializeAsync(new NullCache(), config["username"], config["password"], config["host"]).Wait();
            //ApiRequest.Username = ConfigurationManager.AppSettings["username"];
            //ApiRequest.Password = ConfigurationManager.AppSettings["password"];
            _es = EnumSetService.Instance;
            if (_es.DataStore is EnumDescSaveToCsv)
            {
                _es.DataStore = new EnumDescSaveToSqlServer();
                _es.RunLoadAsync().Wait();
            }
        }

        [TestMethod]
        public void EnumSetValidSetId()
        {
            Assert.IsTrue("(0-8) Stages".Equals(_es.GetEnumSetDescription("1050")));
        }

        [TestMethod]
        public void EnumSetInvalidSetId()
        {
            Assert.IsTrue("Enum Set description does not exist".Equals(_es.GetEnumSetDescription("99587")));
        }

        [TestMethod]
        public void EnumMemberValidSetId()
        {
            Assert.IsTrue("0 Stages".Equals(_es.GetEnumMemberDescription("1050", "0")));
        }

        [TestMethod]
        public void EnumMemberInvalidSetId()
        {
            Assert.IsTrue("Enum Member does not Exist".Equals(_es.GetEnumMemberDescription("99587", "0")));
        }

        [TestMethod]
        public void EnumServiceLoadCsvToDB()
        {
            string fileName = "EnumServiceLoadCSVTest.csv";
            if (File.Exists($"Complete-{fileName}"))
                System.IO.File.Move($"Complete-{fileName}", fileName);
            else if (!File.Exists(fileName))
            {
                EnumDescSaveToCsv csv = new EnumDescSaveToCsv();
                csv.FileName = fileName;
                csv.SaveDataEnumDescSetAsync(_edc).Wait();
            }
            _es.SetupLoadFileToDBAsync(fileName).Wait();
            Assert.IsTrue(File.Exists($"Complete-{fileName}"));
        }
    }
}
