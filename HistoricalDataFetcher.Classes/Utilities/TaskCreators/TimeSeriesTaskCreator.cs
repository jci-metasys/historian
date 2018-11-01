using HistoricalDataFetcher.Classes.DataLayer.Fqrs;
using HistoricalDataFetcher.Classes.Enums;
using HistoricalDataFetcher.Classes.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;

namespace HistoricalDataFetcher.Classes.Utilities.TaskCreators
{
    public class TimeSeriesTaskCreator : ITaskCreator
    {
        private string acceptHeader = "application/vnd.metasysapi.v1+json";
        public TaskTypeEnum TaskType { get; }
        public int PageSize { get; }

        public TimeSeriesTaskCreator()
        {
            TaskType = TaskTypeEnum.TimeSeries;
            PageSize = 10000;
        }

        /// <summary>
        /// Adds tasks to the passed in Job using the FQR's from the Database
        /// </summary>
        /// <param name="job">Job to add Tasks to</param>
        public void CreateTasks(JobEntity job)
        {
            var fqrGuids = (new FqrRepository()).GetAllAsync().Result;

            foreach (var fqrGuid in fqrGuids)
            {
                var response = ApiRequest.RunEndpointCallAsync($"{ApiRequest.UrlBase}/objects/{fqrGuid.Guid}/attributes", acceptHeader).Result;
                JArray items;

                try
                {
                    JObject jsonObject = JObject.Parse(response);
                    items = JArray.Parse(jsonObject?.SelectToken("items").ToString());
                }
                catch (Exception)
                {
                    items = new JArray();
                }

                foreach (var item in items)
                {
                    var url = item.SelectToken("samples")?.ToString();

                    if (!string.IsNullOrWhiteSpace(url))
                    {
                        job.Tasks.Add(new TaskQueueEntity
                        {
                            JobId = job.Id,
                            Parent = job,
                            TaskType = this.TaskType,
                            TaskUrl = ConvertAttributeLink(url, job),
                            IsCompleted = false
                        });
                    }
                }
            }
        }

        /// <summary>
        /// Convert the attribute link to use the standard page size and the job's start and end time
        /// </summary>
        /// <param name="originalString">Complete URL</param>
        /// <param name="job">Job that is being processed</param>
        /// <returns></returns>
        private string ConvertAttributeLink(string originalString, JobEntity job)
        {
            var result = originalString.Substring(0, originalString.IndexOf("?") + 1);

            var queryStrings = originalString.Substring(originalString.IndexOf("?") + 1).Split('&').ToList();

            //remove pagesize
            queryStrings.Remove(queryStrings.FirstOrDefault(a => a.ToLower().IndexOf("pagesize") != -1));

            //remove startTime
            queryStrings.Remove(queryStrings.FirstOrDefault(a => a.ToLower().IndexOf("starttime") != -1));

            //remove endTime
            queryStrings.Remove(queryStrings.FirstOrDefault(a => a.ToLower().IndexOf("endtime") != -1));

            queryStrings.Add($"pageSize={PageSize}");
            queryStrings.Add($"startTime={string.Format("{0:s}", job.StartTime)}");
            queryStrings.Add($"endTime={string.Format("{0:s}", job.EndTime)}");

            result += string.Join("&", queryStrings);

            return result;
        }
    }
}
