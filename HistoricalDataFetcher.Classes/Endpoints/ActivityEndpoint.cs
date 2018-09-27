using HistoricalDataFetcher.Classes.Endpoints.Base;
using HistoricalDataFetcher.DataStorage.Interfaces;
using HistoricalDataFetcher.DataStorage.Models;
using System;
using System.Threading.Tasks;

namespace HistoricalDataFetcher.Classes.Endpoints
{
    public class ActivityEndPoint : BaseEndPoint
    {
        private readonly IDataStore<AuditDataStoreModel> _dataStore;

        public ActivityEndPoint(IDataStore<AuditDataStoreModel> dataStore)
        {
            _dataStore = dataStore;
        }

        /// <summary>
        /// Run the URL task 
        /// </summary>
        /// <param name="taskUrl">Complete URL</param>
        /// <returns>bool: True = Complete</returns>
        public override Task<bool> RunAsync(string taskUrl)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Saves the Data 
        /// </summary>
        /// <returns></returns>
        public override async Task SaveDataAsync()
        {
            throw new NotImplementedException();
        }

    }
}
