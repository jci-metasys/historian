using CsvHelper;
using HistoricalDataFetcher.DataStorage.Interfaces;
using HistoricalDataFetcher.DataStorage.Models;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace HistoricalDataFetcher.DataStorage.TimeSeries
{
    public class TimeSeriesSaveToCsv : IDataStore<SamplesDataStoreModel>
    {
        private string _fileName = @".\timeseries.csv";
        /// <summary>
        /// Saves the formated samples
        /// </summary>
        /// <param name="items">IEnumberable of SamplesDataStoreModel</param>
        /// <returns></returns>
        public Task SetDataAsync(IEnumerable<SamplesDataStoreModel> items)
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
