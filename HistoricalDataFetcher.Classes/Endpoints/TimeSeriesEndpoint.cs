using HistoricalDataFetcher.Classes.DataLayer.Fqrs;
using HistoricalDataFetcher.Classes.Endpoints.Base;
using HistoricalDataFetcher.Classes.Models.Collection;
using HistoricalDataFetcher.Classes.Services;
using HistoricalDataFetcher.Classes.Utilities;
using HistoricalDataFetcher.DataStorage.Interfaces;
using HistoricalDataFetcher.DataStorage.Models;
using HistoricalDataFetcher.DataStorage.TimeSeries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HistoricalDataFetcher.Classes.Endpoints
{
    public class TimeSeriesEndPoint : BaseEndPoint
    {
        protected List<SamplesDataStoreModel> _sampleData;
        protected readonly IDataStore<SamplesDataStoreModel> _dataStore;

        public TimeSeriesEndPoint(IDataStore<SamplesDataStoreModel> dataStore)
        {
            _dataStore = dataStore;
            _sampleData = new List<SamplesDataStoreModel>();
        }

        public int SampleDataCount => _sampleData.Count;

        /// <summary>
        /// Run URL task
        /// </summary>
        /// <param name="taskUrl">Complete URL for samples</param>
        /// <returns>bool: True = success</returns>
        public override async Task<bool> RunAsync(string taskUrl)
        {
            _sampleData = new List<SamplesDataStoreModel>();
            var nextLink = taskUrl;

            try
            {
                while (!string.IsNullOrWhiteSpace(nextLink))
                {
                    string[] nextArray = nextLink.Split('/');
                    string pointGuid = nextArray[Array.IndexOf(nextArray, "objects") + 1];
                    _stopWatch.Start();
                    var sampleCollection = await GetCollectionAsync<SampleCollectionItem>($"{ApiRequest.UrlBase}{nextLink}");
                    _stopWatch.Stop();

                    if (sampleCollection != null)
                    {
                        _sampleData.AddRange(BuildSampleDataCollection(sampleCollection.Items, pointGuid));
                        LoggerService.LogApiRequest(nextLink, sampleCollection.Items.Count, TimeSpan.FromMilliseconds(_stopWatch.Elapsed.TotalMilliseconds).ToString());
                        nextLink = !string.IsNullOrEmpty(sampleCollection.Next) ? sampleCollection.Next : null;
                    }
                    else
                    {
                        LoggerService.LogApiRequest(nextLink, 0, TimeSpan.FromMilliseconds(_stopWatch.Elapsed.TotalMilliseconds).ToString());
                        nextLink = null;
                    }

                    _stopWatch.Reset();
                }

                await SaveDataAsync();
            }
            catch (Exception ex)
            {
                _stopWatch.Stop();
                LoggerService.LogException(nextLink, TimeSpan.FromMilliseconds(_stopWatch.Elapsed.TotalMilliseconds).ToString(), ex);
                _stopWatch.Reset();
                return false;
            }

            return true;
        }

        /// <summary>
        /// Save data to the selected datastore
        /// </summary>
        /// <returns></returns>
        public override async Task SaveDataAsync()
        {
            if (_dataStore is TimeSeriesSaveToCsv)
            {
                var points = await (new FqrRepository()).GetAllAsync();
                var result =(
                    from sample in _sampleData
                    join point in points on sample.PointGuid equals point.Guid
                    select new SamplesDataStoreModel{
                        IsReliable = sample.IsReliable,
                        TimeStamp = sample.TimeStamp,
                        Units = sample.Units,
                        Value = sample.Value,
                        PointGuid = sample.PointGuid,
                        PointName = point.PointName,
                        ItemReference = point.Fqr,
                        PointType = point.PointType}
                    ).ToList();
                _sampleData = result;
            }
            await _dataStore.SetDataAsync(_sampleData);
        }

        /// <summary>
        /// Builds the Samples list from the point samples returned
        /// </summary>
        /// <param name="pointSamplesList">IEnumberable of SampleCollectionItem</param>
        /// <param name="pointGuid">Point Guid</param>
        /// <returns>IEnumerable of SamplesDataStore Model</returns>
        private IEnumerable<SamplesDataStoreModel> BuildSampleDataCollection(IEnumerable<SampleCollectionItem> pointSamplesList, string pointGuid)
        {
            return pointSamplesList.Select(sample => new SamplesDataStoreModel
            {
                PointGuid = new Guid(pointGuid),
                IsReliable = sample.IsReliable,
                TimeStamp = sample.Timestamp,
                Units = GetEnumSetInformation(sample.Value.Units),
                Value = sample.Value.Value
            });
        }
    }
}