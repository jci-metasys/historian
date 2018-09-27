using HistoricalDataFetcher.Classes.Endpoints.Base;
using HistoricalDataFetcher.Classes.EnumSet;
using HistoricalDataFetcher.Classes.Models.Collection;
using HistoricalDataFetcher.Classes.Services;
using HistoricalDataFetcher.Classes.Utilities;
using HistoricalDataFetcher.DataStorage.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HistoricalDataFetcher.Classes.Endpoints
{
    public sealed class EnumSetEndPoint : BaseEndPoint
    {
        private EnumDescCollection _enumDescData;
        private static readonly string _membersEndPoint = "/members";
        private readonly int _pageSize;
        
        public IEnumDescRepository DataStore { get; set; } = new EnumDescSaveToCsv();
        public string ApiEndpoint { get { return _apiEndpoint; } }


        /// <summary>
        /// Constructor that builds out the EnumFile.csv file
        /// </summary>
        public EnumSetEndPoint() : base()
        {
            _pageSize = 1000;
            _apiEndpoint = "/api/v1/enumSets";
        }

        /// <summary>
        /// Run URL to build the EnumDescCollection
        /// </summary>
        /// <param name="taskUrl">Complete URL</param>
        /// <returns>bool: True = success</returns>
        public override async Task<bool> RunAsync(string taskUrl = null)
        {
            _url = taskUrl;
            _enumDescData = new EnumDescCollection();
            _enumDescData.AddRange(await BuildEnumDescCollectionAsync());

            return true;
        }

        /// <summary>
        /// Build the EnumSet description collection from the URL
        /// </summary>
        /// <returns>EnumDescCollection</returns>
        private async Task<EnumDescCollection> BuildEnumDescCollectionAsync()
        {
            var enumDescData = new EnumDescCollection();
            var enumSetCollection = new BatchCollection<EnumSetBatchCollectionItem>();
            var nextURL = $"{ _url}?pageSize={_pageSize}";

            try
            {
                _stopWatch.Start();
                var enumSetBatch = new List<EnumSetBatchCollectionItem>();
                while (!string.IsNullOrWhiteSpace(nextURL))
                {
                    enumSetCollection = await GetCollectionAsync<EnumSetBatchCollectionItem>($"{ApiRequest.UrlBase}{nextURL}");
                    if (enumSetCollection != null)
                    {
                        enumSetBatch.AddRange(enumSetCollection.Items);
                        nextURL = enumSetCollection.Next;
                    }
                    else
                    {
                        nextURL = string.Empty;
                    }
                    _stopWatch.Stop();
                    LoggerService.LogApiRequest(nextURL, enumSetCollection.Items.Count, TimeSpan.FromMilliseconds(_stopWatch.Elapsed.TotalMilliseconds).ToString());
                    _stopWatch.Reset();
                }
                foreach (var item in enumSetBatch)
                {
                    enumDescData.AddRange(await GetMemberListAsync(item.Id.ToString(), item.Description));
                }
            }
            catch (Exception ex)
            {
                _stopWatch.Stop();
                LoggerService.LogException(nextURL, TimeSpan.FromMilliseconds(_stopWatch.Elapsed.TotalMilliseconds).ToString(), ex);
                _stopWatch.Reset();
                throw;
            }

            return enumDescData;
        }

        /// <summary>
        /// Build the EnumSet with the entire list of Enum Members
        /// </summary>
        /// <param name="setId">Set Id</param>
        /// <param name="setDescription">Set Description</param>
        /// <returns>EnumDescCollection</returns>
        private async Task<EnumDescCollection> GetMemberListAsync(string setId, string setDescription)
        {
            var memberList = new List<EnumMemberBatchCollectionItem>();
            var enumMemberBatch = new BatchCollection<EnumMemberBatchCollectionItem>();
            var nextURL = $"{_url}/{setId}{_membersEndPoint}?pageSize={_pageSize}";
            var enumDesc = new EnumDescCollection();

            try
            {
                _stopWatch.Start();
                var setIdInt = 0;
                int.TryParse(setId, out setIdInt);

                while (!string.IsNullOrWhiteSpace(nextURL))
                {
                    enumMemberBatch = await GetCollectionAsync<EnumMemberBatchCollectionItem>($"{ApiRequest.UrlBase}{nextURL}");
                    if(enumMemberBatch != null)
                    {
                        memberList.AddRange(enumMemberBatch.Items);
                        nextURL = enumMemberBatch.Next;
                    }
                    else
                    {
                        nextURL = string.Empty;
                    }
                    _stopWatch.Stop();
                    LoggerService.LogApiRequest(nextURL, memberList.Count, TimeSpan.FromMilliseconds(_stopWatch.Elapsed.TotalMilliseconds).ToString());
                    _stopWatch.Reset();
                }
                foreach (var member in memberList)
                {
                    enumDesc.Add(new EnumDescDataStoreModel { SetId = setIdInt, MemberId = member.Id, SetDesc = setDescription, MemberDesc = member.Description });
                }
            }
            catch (Exception ex)
            {
                _stopWatch.Stop();
                LoggerService.LogException(nextURL, TimeSpan.FromMilliseconds(_stopWatch.Elapsed.TotalMilliseconds).ToString(), ex);
                _stopWatch.Reset();

                throw;
            }

            return enumDesc;
        }

        /// <summary>
        /// Save the EnumSet description colelction to the data store
        /// </summary>
        /// <returns></returns>
        public override async Task SaveDataAsync()
        {
            await DataStore.SaveDataEnumDescSetAsync(_enumDescData);
        }
    }
}
