using Dapper;
using HistoricalDataFetcher.DataStorage.Interfaces;
using HistoricalDataFetcher.DataStorage.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace HistoricalDataFetcher.DataStorage.TimeSeries
{
    public class TimeSeriesSaveToSqlServer : DBDataModel, IDataStore<SamplesDataStoreModel>
    {        
        /// <summary>
        /// Saves the data into the Database
        /// </summary>
        /// <param name="items">IEnumerable of SampelsDataStoreModel</param>
        /// <returns></returns>
        public async Task SetDataAsync(IEnumerable<SamplesDataStoreModel> items)
        {
            if (!items.Any())
                return;
            using (var connection = new SqlConnection(DBConnectionString))
            {
                var samples = new SamplesDataStoreCollection();
                samples.AddRange(items);

                await connection.OpenAsync();

                await connection.ExecuteAsync("InsertTimeSeriesData", new { SampleListUDT = samples?.AsTableValuedParameter("dbo.TimeSeriesHistoricalListUDT") }, commandType: CommandType.StoredProcedure);
            }
        }
    }
}
