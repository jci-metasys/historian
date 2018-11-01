using HistoricalDataFetcher.Classes.Endpoints.Base;
using HistoricalDataFetcher.Classes.Models.Collection;
using HistoricalDataFetcher.Classes.Services;
using HistoricalDataFetcher.Classes.Utilities;
using HistoricalDataFetcher.DataStorage.Interfaces;
using HistoricalDataFetcher.DataStorage.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HistoricalDataFetcher.Classes.Endpoints
{
    public class AlarmEndPoint : BaseEndPoint
    {
        private readonly IDataStore<AlarmDataStoreModel> _dataStore;
        private List<AlarmDataStoreModel> _alarmDataModels;
        public AlarmEndPoint(IDataStore<AlarmDataStoreModel> dataStore) : base()
        {
            _dataStore = dataStore;
        }

        /// <summary>
        /// Run the URL task for Alarms
        /// </summary>
        /// <param name="taskUrl">Complete Alarm URL</param>
        /// <returns>bool: True = Complete</returns>
        public override async Task<bool> RunAsync(string taskUrl = null)
        {
            _alarmDataModels = new List<AlarmDataStoreModel>();
            var nextLink = taskUrl;

            try
            {
                _stopWatch.Start();
                while (!string.IsNullOrWhiteSpace(nextLink))
                {
                    var alarmCollection = await GetCollectionAsync<AlarmBatchCollectionItem>($"{ApiRequest.UrlBase}{nextLink}");

                    _stopWatch.Stop();
                    LoggerService.LogApiRequest(taskUrl, alarmCollection.Items.Count, TimeSpan.FromMilliseconds(_stopWatch.Elapsed.TotalMilliseconds).ToString());
                    _stopWatch.Reset();

                    if (alarmCollection != null)
                    {
                        nextLink = alarmCollection.Next;

                        foreach (var alarm in alarmCollection.Items)
                        {
                            var a = new AlarmDataStoreModel
                            {
                                Id = alarm.Id,
                                ItemReference = alarm.ItemReference,
                                Name = alarm.Name,
                                Message = alarm.Message,
                                Description = alarm.Description,
                                IsAckRequired = alarm.IsAckRequired,
                                Type = GetEnumSetInformation(alarm.Type),
                                Priority = alarm.Priority,
                                CreationTime = alarm.CreationTime,
                                IsAcknowledged = alarm.IsAcknowledged,
                                IsDiscarded = alarm.IsDiscarded,
                                Category = GetEnumSetInformation(alarm.Category),
                                TriggerValue = alarm.TriggerValue.Value,
                                TriggerValueUnits = alarm.TriggerValue.Units,
                                TriggerValueHref = alarm.TriggerValue?.ValueEnumMember
                            };

                            a.Annotations = await GetAnnotationsAsync(alarm.Annotations);
                            a.Annotations.ForEach(annotation => annotation.ParentId = a.Id);

                            _alarmDataModels.Add(a);
                        }
                    }
                    else
                    {
                        nextLink = string.Empty;
                    }
                }

                await SaveDataAsync();
            }
            catch (Exception ex)
            {
                _stopWatch.Stop();
                LoggerService.LogException(taskUrl, TimeSpan.FromMilliseconds(_stopWatch.Elapsed.TotalMilliseconds).ToString(), ex);
                _stopWatch.Reset();

                return false;
            }

            return true;
        }

        /// <summary>
        /// Call the annotations URL 
        /// </summary>
        /// <param name="annotationLink">Complete annotations link</param>
        /// <returns>List of AnnotationDataStoreModel items</returns>
        private async Task<List<AnnotationDataStoreModel>> GetAnnotationsAsync(string annotationLink)
        {
            var annotations = new List<AnnotationBatchCollectionItem>();
            var annotationCollection = new BatchCollection<AnnotationBatchCollectionItem>();
            var annotationDataModels = new List<AnnotationDataStoreModel>();

            var nextLink = annotationLink;
            try
            {
                while (!string.IsNullOrWhiteSpace(nextLink))
                {
                    _stopWatch.Start();
                    annotationCollection = await GetCollectionAsync<AnnotationBatchCollectionItem>($"{ApiRequest.UrlBase}{nextLink}");

                    if (annotationCollection != null)
                    {
                        annotations.AddRange(annotationCollection.Items);
                        nextLink = annotationCollection.Next;
                    }
                    else
                    {
                        nextLink = string.Empty;
                    }
                    _stopWatch.Stop();
                    LoggerService.LogApiRequest(nextLink, annotationCollection.Items.Count, TimeSpan.FromMilliseconds(_stopWatch.Elapsed.TotalMilliseconds).ToString());
                    _stopWatch.Reset();
                }

                annotations.ForEach(a => annotationDataModels.Add(new AnnotationDataStoreModel
                {
                    Text = a.Text,
                    User = a.User,
                    CreationTime = a.CreationTime,
                    Action = a.Action
                }));
            }
            catch (Exception ex)
            {
                _stopWatch.Stop();
                LoggerService.LogException(nextLink, TimeSpan.FromMilliseconds(_stopWatch.Elapsed.TotalMilliseconds).ToString(), ex);
                _stopWatch.Reset();

                throw;
            }

            return annotationDataModels;
        }

        /// <summary>
        /// Saves the Data to the selected Datastore
        /// </summary>
        /// <returns></returns>
        public override async Task SaveDataAsync()
        {
            await _dataStore.SetDataAsync(_alarmDataModels);
        }
    }
}
