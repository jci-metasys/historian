using HistoricalDataFetcher.Classes.Endpoints;
using HistoricalDataFetcher.Classes.Models.Collection;
using HistoricalDataFetcher.Classes.Models.Collection.TimeSeries;
using HistoricalDataFetcher.Classes.Services;
using HistoricalDataFetcher.Classes.Utilities;
using HistoricalDataFetcher.DataStorage.Interfaces;
using HistoricalDataFetcher.DataStorage.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HistoricalDataFetcher.QuickExtract
{
    class TimeSeriesQuickstartEndPoint : TimeSeriesEndPoint
    {

        public TimeSeriesQuickstartEndPoint(IDataStore<SamplesDataStoreModel> dataStore) : base(dataStore)
        {
        }

        /// <summary>
        /// Async call to run the selected URL as a task
        /// </summary>
        /// <param name="taskUrl">Complete URL for the Network Devices</param>
        /// <returns>bool: True = complete</returns>
        public new async Task<bool> RunAsync(string taskUrl)
        {
            _sampleData = new List<SamplesDataStoreModel>();
            _url = taskUrl;

            var pointCollection = new List<PointBatchCollectionItem>();
            var availableSampleList = new List<AvailableSampleCollectionItem>();
            var rawSampleList = new List<SampleCollectionItem>();
            var networkDeviceCollection = new List<NetworkDeviceCollectionItem>();

            try
            {
                networkDeviceCollection.AddRange(await BuildNetworkDeviceCollectionAsync(taskUrl));
                pointCollection.AddRange(await BuildPointCollectionAsync(networkDeviceCollection));
                availableSampleList.AddRange(await BuildAvailableSamplesListAsync(pointCollection));
                rawSampleList.AddRange(await BuildRawSamplesListAsync(availableSampleList));
                _sampleData.AddRange(BuildSampleDataCollection(rawSampleList));
            }
            catch (Exception)
            {
                return false;
            }

            await SaveDataAsync(_sampleData, pointCollection);

            return true;
        }

        /// <summary>
        /// Save data by combining Samples and Points into a single set
        /// </summary>
        /// <param name="samples">List of samples</param>
        /// <param name="points">List of points</param>
        /// <returns></returns>
        public async Task SaveDataAsync(List<SamplesDataStoreModel> samples, List<PointBatchCollectionItem> points)
        {
            var result = (
                    from sample in samples
                    join point in points on sample.PointGuid.ToString() equals point.Id
                    select new SamplesDataStoreModel
                    {
                        IsReliable = sample.IsReliable,
                        TimeStamp = sample.TimeStamp,
                        Units = sample.Units,
                        Value = sample.Value,
                        PointGuid = sample.PointGuid,
                        PointName = point.Name,
                        ItemReference = point.ItemReference,
                        PointType = EnumSetService.Instance.GetEnumMemberDescription(point.Type)
                    }
                    ).ToList();
            await _dataStore.SetDataAsync(result);
        }

        /// <summary>
        /// Build a list of available network devices
        /// </summary>
        /// <param name="url">Complete URL for the network device location</param>
        /// <returns>IEnumberable of NetworkDeviceCollectionItem </returns>
        private async Task<IEnumerable<NetworkDeviceCollectionItem>> BuildNetworkDeviceCollectionAsync(string url)
        {
            IEnumerable<NetworkDeviceCollectionItem> networkDevices;
            _stopWatch.Start();

            try
            {
                networkDevices = await GetSinglePageAsync<NetworkDeviceCollectionItem>(url);
                LoggerService.LogApiRequest(url, networkDevices.Count(), TimeSpan.FromMilliseconds(_stopWatch.Elapsed.TotalMilliseconds).ToString());
            }
            catch (Exception ex)
            {
                LoggerService.LogException(url, TimeSpan.FromMilliseconds(_stopWatch.Elapsed.TotalMilliseconds).ToString(), ex);
                networkDevices = new List<NetworkDeviceCollectionItem>();
            }

            _stopWatch.Stop();
            _stopWatch.Reset();
            return networkDevices;
        }

        /// <summary>
        /// Build a list of child objects based on the list of Network Devices passed in
        /// </summary>
        /// <param name="networkDeviceCollection">List of Network devices</param>
        /// <returns>IEnumberable of PointBatchCollectionItem</returns>
        private async Task<IEnumerable<PointBatchCollectionItem>> BuildPointCollectionAsync(IEnumerable<NetworkDeviceCollectionItem> networkDeviceCollection)
        {
            var pointCollection = new List<PointBatchCollectionItem>();

            foreach (var device in networkDeviceCollection)
            {
                string url = $"{ApiRequest.UrlBase}{device.Objects}?pageSize=20";
                _stopWatch.Start();

                try
                {
                    var points = await GetSinglePageAsync<PointBatchCollectionItem>(url);
                    pointCollection.AddRange(points);
                    LoggerService.LogApiRequest(url, points.Count, TimeSpan.FromMilliseconds(_stopWatch.Elapsed.TotalMilliseconds).ToString());
                }
                catch (Exception ex)
                {
                    LoggerService.LogException(url, TimeSpan.FromMilliseconds(_stopWatch.Elapsed.TotalMilliseconds).ToString(), ex);
                }

                _stopWatch.Stop();
                _stopWatch.Reset();
            }

            return pointCollection;
        }

        /// <summary>
        /// Build a list of available samples based on the attributes being used
        /// </summary>
        /// <param name="pointCollection">List of points to grab an available sample list from</param>
        /// <returns>IEnumerable of AvailableSampleCollectionItem</returns>
        private async Task<IEnumerable<AvailableSampleCollectionItem>> BuildAvailableSamplesListAsync(IEnumerable<PointBatchCollectionItem> pointCollection)
        {
            var availablePointSamples = new List<AvailableSampleCollectionItem>();
            var url = string.Empty;

            foreach (var pointItem in pointCollection)
            {
                _stopWatch.Start();

                try
                {
                    url = $"{ApiRequest.UrlBase}{pointItem.Attributes}?pageSize=20";
                    var availableSamples = (await GetSinglePageAsync<AvailableSampleCollectionItem>(url)).ToList();
                    LoggerService.LogApiRequest(url, availableSamples.Count, TimeSpan.FromMilliseconds(_stopWatch.Elapsed.TotalMilliseconds).ToString());
                    availableSamples.ForEach(x => x.Point = pointItem);
                    availablePointSamples.AddRange(availableSamples);
                }
                catch (Exception ex)
                {
                    LoggerService.LogException(url, TimeSpan.FromMilliseconds(_stopWatch.Elapsed.TotalMilliseconds).ToString(), ex);
                }

                _stopWatch.Stop();
                _stopWatch.Reset();
            }

            return availablePointSamples;
        }

        /// <summary>
        /// Build a complete list of all samples within the attributes selected
        /// </summary>
        /// <param name="availablePointSamples">List of available point samples</param>
        /// <returns>IEnumberable of SampleCollectionItem</returns>
        private async Task<IEnumerable<SampleCollectionItem>> BuildRawSamplesListAsync(IEnumerable<AvailableSampleCollectionItem> availablePointSamples)
        {
            var pointSamples = new List<SampleCollectionItem>();

            foreach (var availableSample in availablePointSamples)
            {
                _stopWatch.Start();
                var samplesUrl = availableSample.Samples.Split("?")[0];
                samplesUrl = $"{ApiRequest.UrlBase}{samplesUrl}?startTime={DateTime.Now.AddDays(-1)}&endTime={DateTime.Now}&page=1&pageSize=1000&sort=timestamp";

                try
                {
                    var samples = (await GetSinglePageAsync<SampleCollectionItem>(samplesUrl)).ToList();
                    LoggerService.LogApiRequest(samplesUrl, samples.Count, TimeSpan.FromMilliseconds(_stopWatch.Elapsed.TotalMilliseconds).ToString());
                    samples.ForEach(x => x.Point = availableSample.Point);
                    pointSamples.AddRange(samples);
                }
                catch (Exception ex)
                {
                    LoggerService.LogException(samplesUrl, TimeSpan.FromMilliseconds(_stopWatch.Elapsed.TotalMilliseconds).ToString(), ex);
                }

                _stopWatch.Stop();
                _stopWatch.Reset();
            }

            return pointSamples;
        }

        /// <summary>
        /// Format the raw samples into a format ready to save 
        /// </summary>
        /// <param name="pointSamplesList">The raw sample list</param>
        /// <returns>IEnumberable of SamplesDataStoreModel</returns>
        private IEnumerable<SamplesDataStoreModel> BuildSampleDataCollection(IEnumerable<SampleCollectionItem> pointSamplesList)
        {
            return pointSamplesList.Select(sample => new SamplesDataStoreModel
            {
                PointName = sample.Point.Name,
                PointGuid = new Guid(sample.Point.Id),
                ItemReference = sample.Point.ItemReference,
                IsReliable = sample.IsReliable,
                TimeStamp = sample.Timestamp,
                Units = GetEnumSetInformation(sample.Value.Units),
                Value = sample.Value.Value
            }).ToList();
        }

        /// <summary>
        /// Makes a single API call and does not loop through all the available pages
        /// </summary>
        /// <typeparam name="T">Expected item returned from the API call</typeparam>
        /// <param name="startUrl">The URL to call</param>
        /// <returns>ICollection of "T"</returns>
        private async Task<ICollection<T>> GetSinglePageAsync<T>(string startUrl)
        {
            var collection = new List<T>();
            string nextUrl = startUrl;
            var points = await GetCollectionAsync<T>(nextUrl);
            collection.AddRange(points.Items);

            return collection;
        }
    }
}
