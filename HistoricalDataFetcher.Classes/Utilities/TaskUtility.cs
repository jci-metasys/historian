using HistoricalDataFetcher.Classes.Endpoints.Base;
using HistoricalDataFetcher.Classes.Enums;
using HistoricalDataFetcher.Classes.Utilities.TaskCreators;

namespace HistoricalDataFetcher.Classes.Utilities
{
    public class TaskUtility
    {
        public TaskUtility (TaskTypeEnum taskType, BaseEndPoint endPoint, ITaskCreator taskCreator)
        {
            TaskType = taskType;
            EndPoint = endPoint;
            TaskCreator = taskCreator;
        }

        public TaskTypeEnum TaskType { get; }

        public BaseEndPoint EndPoint { get; }

        public ITaskCreator TaskCreator { get; }

    }
}
