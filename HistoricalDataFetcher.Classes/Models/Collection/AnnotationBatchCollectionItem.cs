using System;

namespace HistoricalDataFetcher.Classes.Models.Collection
{
    public class AnnotationBatchCollectionItem
    {
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
