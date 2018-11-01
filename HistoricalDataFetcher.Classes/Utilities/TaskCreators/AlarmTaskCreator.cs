using HistoricalDataFetcher.Classes.DataLayer.Fqrs;
using HistoricalDataFetcher.Classes.Enums;
using HistoricalDataFetcher.Classes.Models;
using HistoricalDataFetcher.Classes.Models.Collection;
using System;
using System.Collections.Generic;

namespace HistoricalDataFetcher.Classes.Utilities.TaskCreators
{
    public class AlarmTaskCreator : ITaskCreator
    {
        public TaskTypeEnum TaskType { get; }

        public int PageSize { get; }

        public AlarmTaskCreator()
        {
            TaskType = TaskTypeEnum.Alarm;
            PageSize = 1000;
        }

        /// <summary>
        /// Add Tasks to the 
        /// </summary>
        /// <param name="job"></param>
        public void CreateTasks(JobEntity job)
        {
            var points = (new FqrRepository()).GetAllAsync().Result;

            foreach (var point in points)
            {
                job.Tasks.Add(new TaskQueueEntity
                {
                    JobId = job.Id,
                    Parent = job,
                    TaskType = this.TaskType,
                    TaskUrl = $"/objects/{point.Guid.ToString()}/alarms?startTime={String.Format("{0:s}", job.StartTime)}&endTime={String.Format("{0:s}", job.EndTime)}&pageSize={PageSize}",
                    IsCompleted = false
                });
            }
        }
    }
}
