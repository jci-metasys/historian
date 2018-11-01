using System;
using System.Collections.Generic;

namespace HistoricalDataFetcher.DataStorage.Models
{
    public class AlarmDataStoreModel
    {
        /// <summary>
        /// Alarm Unique Identifier (GUID)
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Item fully qualified reference
        /// </summary>
        public string ItemReference { get; set; }

        /// <summary>
        /// Item name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Alarm message
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Item Description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Is acknowledge required for alarm
        /// </summary>
        public bool IsAckRequired { get; set; }

        /// <summary>
        /// Alarm type route
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Alarm priority
        /// </summary>
        public int Priority { get; set; }

        /// <summary>
        /// Alarm trigger value details
        /// </summary>
        public string TriggerValue { get; set; }

        public string TriggerValueUnits { get; set; }

        public string TriggerValueHref { get; set; }

        /// <summary>
        /// Alarm creation time
        /// </summary>
        public DateTime CreationTime { get; set; }

        /// <summary>
        /// Is alarm already acknowledged
        /// </summary>
        public bool IsAcknowledged { get; set; }

        /// <summary>
        /// Is alarm discarded
        /// </summary>
        public bool IsDiscarded { get; set; }

        /// <summary>
        /// Alarm Category route
        /// </summary>
        public string Category { get; set; }

        public List<AnnotationDataStoreModel> Annotations { get; set; } = new List<AnnotationDataStoreModel>();
    }
}
