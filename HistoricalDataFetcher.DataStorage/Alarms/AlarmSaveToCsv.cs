using CsvHelper;
using HistoricalDataFetcher.DataStorage.Interfaces;
using HistoricalDataFetcher.DataStorage.Models;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace HistoricalDataFetcher.DataStorage.Alarms
{
    public class AlarmSaveToCsv : IDataStore<AlarmDataStoreModel>
    {
        private string _fileName = @".\alarms.csv";
        /// <summary>
        /// Saves the alarm data to a CSV file
        /// </summary>
        /// <param name="items">IEnumerable of AlarmDataStoreModel</param>
        /// <returns></returns>
        public Task SetDataAsync(IEnumerable<AlarmDataStoreModel> items)
        {
            return Task.Run(() =>
            {
                var fileExists = File.Exists(_fileName);            
                using (var writer = new StreamWriter(_fileName, fileExists))
                {                    
                    var csv = new CsvWriter(writer);
                    csv.Configuration.HasHeaderRecord = !fileExists;                    
                    csv.WriteRecords(items);
                }
            });
        }
    }
}
