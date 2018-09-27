using HistoricalDataFetcher.Classes.Enums;

namespace HistoricalDataFetcher.Classes.Models
{
    public class TaskQueueEntity
    {
        /// <summary>
        /// Task Id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Parent Job Id
        /// </summary>
        public int JobId { get; set; }
        /// <summary>
        /// Reference to Job entity
        /// </summary>
        public JobEntity Parent { get; set; }
        /// <summary>
        /// Task type (TaskTypeEnum)
        /// </summary>
        public TaskTypeEnum TaskType { get; set; }
        /// <summary>
        /// Task URL to call against
        /// </summary>
        public string TaskUrl { get; set; }
        /// <summary>
        /// Is Task complete
        /// </summary>
        public bool IsCompleted { get; set; } = false;
    }
}
