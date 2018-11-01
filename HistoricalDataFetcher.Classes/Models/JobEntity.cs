using HistoricalDataFetcher.Classes.Enums;
using System;
using System.Collections.Generic;

namespace HistoricalDataFetcher.Classes.Models
{
    public class JobEntity
    {
        /// <summary>
        /// Job Id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Job Created date
        /// </summary>
        public DateTime DateCreated { get; set; }
        /// <summary>
        /// Job data extraction start date
        /// </summary>
        public DateTime StartTime { get; set; }
        /// <summary>
        /// Job data extraction end date
        /// </summary>
        public DateTime EndTime { get; set; }
        /// <summary>
        /// Job Status (JonStatusEnum)
        /// </summary>
        public JobStatusEnum Status { get; set; }
        /// <summary>
        /// List of Tasksthe Job needs to run
        /// </summary>
        public List<TaskQueueEntity> Tasks { get; set; } = new List<TaskQueueEntity>();
    }
}
