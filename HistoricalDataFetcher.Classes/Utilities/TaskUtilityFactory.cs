using HistoricalDataFetcher.Classes.Endpoints.Base;
using HistoricalDataFetcher.Classes.Enums;
using HistoricalDataFetcher.Classes.Utilities.TaskCreators;
using System.Collections.Generic;
using System.Linq;

namespace HistoricalDataFetcher.Classes.Utilities
{
    public static class TaskUtilityFactory
    {
        private static List<TaskUtility> _taskUtilities = new List<TaskUtility>();

        public static void AddTaskUtility(TaskUtility taskUtility)
        {
            _taskUtilities.Add(taskUtility);
        }

        public static IEnumerable<TaskTypeEnum> SupportedTaskTypes
        {
            get
            {
                return _taskUtilities.Select(t => t.TaskType).Distinct();
            }
        }

        /// <summary>
        /// Returns an EndPoint
        /// </summary>
        /// <param name="enumValue">Task Type</param>
        /// <returns>BaseEndPoint</returns>
        public static BaseEndPoint GetEndPoint(TaskTypeEnum enumValue)
        {
            return _taskUtilities.FirstOrDefault(t => t.TaskType == enumValue)?.EndPoint;
        }

        /// <summary>
        /// Returns a Task Creator
        /// </summary>
        /// <param name="enumValue">Task Type</param>
        /// <returns>Task Creator</returns>
        public static ITaskCreator GetTaskCreator(TaskTypeEnum enumValue)
        {
            return _taskUtilities.FirstOrDefault(t => t.TaskType == enumValue)?.TaskCreator;
        }
    }
}
