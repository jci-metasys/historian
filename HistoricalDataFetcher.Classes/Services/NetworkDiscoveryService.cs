using HistoricalDataFetcher.Classes.Models;
using HistoricalDataFetcher.Classes.Models.Collection;
using HistoricalDataFetcher.Classes.Models.Collection.TimeSeries;
using HistoricalDataFetcher.Classes.Utilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace HistoricalDataFetcher.Classes.Services
{
    public class NetworkDiscoveryService
    {
        private string _acceptHeader = "application/vnd.metasysapi.v1+json";

        /// <summary>
        /// Get object information using a list of FQR's
        /// </summary>
        /// <param name="fqrStringCollection">IEnumberable of FQR strings</param>
        /// <returns>IEnumerable of FqrModel</returns>
        public async Task<IEnumerable<FqrModel>> GetFqrGuidListAsync(IEnumerable<string> fqrStringCollection)
        {
            var usedNetworkDevices = new List<NetworkDeviceCollectionItem>();
            var networkDeviceUrl = $"{ApiRequest.UrlBase}/networkDevices?page=1&pageSize=1000";
            var allNetworkDevices = (await GetAllItemsAsync<NetworkDeviceCollectionItem>(networkDeviceUrl)).ToList();

            //Get the network devices for the specified points
            foreach (var fqr in fqrStringCollection)
            {
                var fqrItem = fqr.Split('/')[0];
                var device = allNetworkDevices.Find(x => x.ItemReference == fqrItem);
                if (device != null)
                {
                    usedNetworkDevices.Add(device);
                }
            }

            usedNetworkDevices = usedNetworkDevices.GroupBy(x => x.Id).Select(x => x.First()).ToList();
            var pointCollection = new List<PointBatchCollectionItem>();

            //Get all of the points for the network devices 
            foreach (var device in usedNetworkDevices)
            {
                pointCollection.AddRange(await GetAllItemsAsync<PointBatchCollectionItem>($"{ApiRequest.UrlBase}{device.Objects}?pageSize=1000"));
            }

            pointCollection = pointCollection.GroupBy(x => x.Id).Select(x => x.First()).ToList();

            //For all of the points that are passed in, match them to the list of points. Once we have the 
            //point mapped, add a new object to the fqrGuidList list with the FQR and GUID
            fqrStringCollection = fqrStringCollection.Select(f => f.ToLower()).ToList();

            return pointCollection.Where(p => fqrStringCollection.Contains(p.ItemReference.ToLower()))
                                  .Select(p => new FqrModel
                                  {
                                      Fqr = p.ItemReference,
                                      Guid = new Guid(p.Id),
                                      PointName = p.Name,
                                      PointType = EnumSetService.Instance.GetEnumMemberDescription(p.Type)
                                  }).ToList();
        }        

        /// <summary>
        /// Get all items using the startUrl and then calling the Next url from the result
        /// </summary>
        /// <typeparam name="T">Return Type</typeparam>
        /// <param name="startUrl">Complete URL</param>
        /// <returns>IEnumerable of "T"</returns>
        private async Task<IEnumerable<T>> GetAllItemsAsync<T>(string startUrl)
        {
            var collection = new List<T>();
            string nextUrl = startUrl;
            var stopWatch = new Stopwatch();

            while (!string.IsNullOrWhiteSpace(nextUrl))
            {
                try
                {
                    stopWatch.Start();
                    var points = await GetCollectionAsync<T>(nextUrl);
                    collection.AddRange(points.Items);
                    stopWatch.Stop();

                    LoggerService.LogApiRequest(nextUrl, points.Items.Count, TimeSpan.FromMilliseconds(stopWatch.Elapsed.TotalMilliseconds).ToString());
                    nextUrl = !string.IsNullOrWhiteSpace(points.Next) ? $"{ApiRequest.UrlBase}{points.Next}" : null;
                    
                    stopWatch.Reset();
                }
                catch (Exception ex)
                {
                    stopWatch.Stop();
                    LoggerService.LogException(nextUrl, TimeSpan.FromMilliseconds(stopWatch.Elapsed.TotalMilliseconds).ToString(), ex);
                    stopWatch.Reset();

                    nextUrl = string.Empty;
                }
            }

            return collection;
        }

        /// <summary>
        /// Get the collection result and serialize to BatchCollection of "T"
        /// </summary>
        /// <typeparam name="T">Collection Item Model</typeparam>
        /// <param name="url">Complete URL</param>
        /// <returns>BatchCollection of "T"</returns>
        private async Task<BatchCollection<T>> GetCollectionAsync<T>(string url)
        {
            var responseJson = await ApiRequest.RunEndpointCallAsync(url, _acceptHeader);
            return JsonConvert.DeserializeObject<BatchCollection<T>>(responseJson);
        }
    }
}
