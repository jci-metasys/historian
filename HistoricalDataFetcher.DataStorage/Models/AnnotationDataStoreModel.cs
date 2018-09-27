using System;

namespace HistoricalDataFetcher.DataStorage.Models
{
    public class AnnotationDataStoreModel
    {
        /// <summary>
        /// the unique identifier of the operation that this annotation belongs to, 
        /// can be either audits or alarms
        /// </summary>
        public string ParentId { get; set; }

        /// <summary>
        /// Annotation text
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Annotation created by
        /// </summary>
        public string User { get; set; }

        /// <summary>
        /// Annotation creation time
        /// </summary>
        public DateTime CreationTime { get; set; }

        /// <summary>
        /// On which action annotation is added
        /// </summary>
        public string Action { get; set; }
    }
}
