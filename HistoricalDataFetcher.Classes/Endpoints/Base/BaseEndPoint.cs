using HistoricalDataFetcher.Classes.Models.Collection;
using HistoricalDataFetcher.Classes.Services;
using HistoricalDataFetcher.Classes.Utilities;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace HistoricalDataFetcher.Classes.Endpoints.Base
{
    public abstract class BaseEndPoint
    {
        protected string _url;
        protected string _apiEndpoint;
        protected Stopwatch _stopWatch;

        private string _acceptHeader = "application/vnd.metasysapi.v1+json";

        /// <summary>
        /// Run the passed in URL task
        /// </summary>
        /// <param name="taskUrl">Complete URL</param>
        /// <returns>bool: True = success</returns>
        public abstract Task<bool> RunAsync(string taskUrl);

        protected BaseEndPoint()
        {
            _stopWatch = new Stopwatch();
        }

        /// <summary>
        /// Get the page Batch result (includes Next and Previous links)
        /// </summary>
        /// <typeparam name="T">Data result from API call</typeparam>
        /// <param name="url">Complete URL</param>
        /// <returns>BacthCollection of "T"</returns>
        public virtual async Task<BatchCollection<T>> GetCollectionAsync<T>(string url)
        {
            var responseJson = await ApiRequest.RunEndpointCallAsync(url, _acceptHeader);
            return JsonConvert.DeserializeObject<BatchCollection<T>>(responseJson);
        }

        /// <summary>
        /// Get the Page Collection Result
        /// </summary>
        /// <typeparam name="T">Data result from API call</typeparam>
        /// <param name="url">Complete URL</param>
        /// <returns>ICollection of "T"</returns>
        public virtual async Task<ICollection<T>> GetCollectionItemsAsync<T>(string url)
        {
            return (await GetCollectionAsync<T>(url)).Items;
        }

        /// <summary>
        /// Get the single item result
        /// </summary>
        /// <typeparam name="T">Data result from API call</typeparam>
        /// <param name="url">Complete URL</param>
        /// <returns></returns>
        public virtual async Task<T> GetSingleItemAsync<T>(string url)
        {
            var responseJson = await ApiRequest.RunEndpointCallAsync(url, _acceptHeader);
            return JsonConvert.DeserializeObject<T>(responseJson);
        }

        /// <summary>
        /// Get the EnumSet member description
        /// </summary>
        /// <param name="enumSetLink">EnumSet link</param>
        /// <returns>Member Description</returns>
        protected string GetEnumSetInformation(string enumSetLink)
        {
            return EnumSetService.Instance.GetEnumMemberDescription(enumSetLink);
        }

        public abstract Task SaveDataAsync();
    }
}
