using HistoricalDataFetcher.Classes.EnumSet;
using HistoricalDataFetcher.DataStorage.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HistoricalDataFetcher.Tests
{
    [TestClass]
    public class EnumDescDataStoreTests
    {
        //private string _enumFile = "EnumList.csv";
        EnumDescCollection edc;
        [TestInitialize]
        public void InitializeTest()
        {
            // Adding JSON file into IConfiguration.
            IConfiguration config = new ConfigurationBuilder()
                .AddJsonFile(@"C:\appsettings.json", true, true)
                .Build();
            DBDataModel.DBConnectionString = $"Server={config["dbcomputer"]};Integrated Security=false;User Id={config["dbuser"]};Password={config["dbpassword"]}";

            edc = new EnumDescCollection
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
        public void EnumDescDataStoreSaveToCsv()
        {
            IEnumDescRepository saveData = new EnumDescSaveToCsv();
            ((EnumDescSaveToCsv)saveData).FileName = @".\EnumFileSaveTest.csv";

            saveData.SaveDataEnumDescSetAsync(edc).Wait();
            Assert.IsTrue(true);
        }
        [TestMethod]
        public void EnumDescDataStoreGetFromCsv()
        {
            IEnumDescRepository saveData = new EnumDescSaveToCsv();
            ((EnumDescSaveToCsv)saveData).FileName = @".\EnumFileGetTest.csv";
            saveData.SaveDataEnumDescSetAsync(edc).Wait();

            EnumDescCollection edc2 = saveData.GetDataEnumDescSetAsync().Result;
            Assert.IsTrue(edc2.Count == edc.Count);
            bool pass = true;
            foreach (EnumDescDataStoreModel enumDesc in edc2)
            {
                if (!edc.Exists(x => x.SetId == enumDesc.SetId && x.MemberId == enumDesc.MemberId))
                    pass = false;
            }
            Assert.IsTrue(pass);
        }


        [TestMethod]
        public void EnumDescDataStoreSaveToSqlServer()
        {
            IEnumDescRepository saveData = new EnumDescSaveToSqlServer();
            saveData.SaveDataEnumDescSetAsync(edc).Wait();
            //saveData.SaveDataEnumDescSet(edc);
            Assert.IsTrue(true);
        }


        [TestMethod]
        public void EnumDescDataStoreGetFromSqlServer()
        {
            IEnumDescRepository dataStore = new EnumDescSaveToSqlServer();
            EnumDescCollection enumDesc = dataStore.GetDataEnumDescSetAsync().Result;
            //saveData.SaveDataEnumDescSet(edc);
            Assert.IsTrue(enumDesc.Count > 0);
        }
    }
}
