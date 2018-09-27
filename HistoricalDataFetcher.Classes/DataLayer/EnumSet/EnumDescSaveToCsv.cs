using System.IO;
using System.Threading.Tasks;
using CsvHelper;
using HistoricalDataFetcher.DataStorage.Models;
using log4net;

namespace HistoricalDataFetcher.Classes.EnumSet
{
    public class EnumDescSaveToCsv : IEnumDescRepository
    {
        public string FileName { get; set; } = @"EnumList.csv";

        /// <summary>
        /// Checks to see if the data exists
        /// </summary>
        /// <returns>bool: True = Data Exists</returns>
        public Task<bool> DataExistsAsync()
        {
            return Task.FromResult(File.Exists(FileName));
        }

        /// <summary>
        /// Read EnumSet descriptions from a file
        /// </summary>
        /// <returns>EnumDescCollection</returns>
        public Task<EnumDescCollection> GetDataEnumDescSetAsync()
        {
            var edc = new EnumDescCollection();
            using (var reader = new StreamReader(FileName))
            {
                var csv = new CsvReader(reader);// CsvWriter(writer);
                var result = csv.GetRecords<EnumDescDataStoreModel>();
                foreach (EnumDescDataStoreModel item in result)
                {
                    edc.Add(item);
                }
            }
            return Task.FromResult(edc);
        }

        /// <summary>
        /// Save the EnumSet description to a file
        /// </summary>
        /// <param name="items">EnumDescCollection</param>
        /// <returns></returns>
        public Task SaveDataEnumDescSetAsync(EnumDescCollection items)
        {
            return Task.Run(() =>
            {
                using (var writer = new StreamWriter(FileName))
                {
                    var csv = new CsvWriter(writer);
                    csv.WriteRecords(items);
                }

                LogManager.GetLogger(GetType()).Info($"EnumDescSaveToCsv.SaveDataEnumDescSetAsync: successly saved to {FileName}.");
            });
        }
    }
}
