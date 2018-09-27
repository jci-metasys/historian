using HistoricalDataFetcher.Classes.Enums;
using HistoricalDataFetcher.Classes.Models;
using System;

namespace HistoricalDataFetcher.Classes.Utilities.TaskCreators
{
    public class ActivityTaskCreator : ITaskCreator
    {
        public TaskTypeEnum TaskType { get; }

        public int PageSize { get; }

        public ActivityTaskCreator()
        {
            TaskType = TaskTypeEnum.Audit;
            PageSize = 1000;
        }

        public void CreateTasks(JobEntity job)
        {
            throw new NotImplementedException();
        }
    }
}
