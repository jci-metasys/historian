using HistoricalDataFetcher.Classes.Enums;
using HistoricalDataFetcher.Classes.Models;

namespace HistoricalDataFetcher.Classes.Utilities.TaskCreators
{
    public interface ITaskCreator
    {
        TaskTypeEnum TaskType { get; }

        int PageSize { get; }

        void CreateTasks(JobEntity job);
    }
}
