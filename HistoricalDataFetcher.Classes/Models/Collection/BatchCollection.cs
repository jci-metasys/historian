using System.Collections.Generic;

namespace HistoricalDataFetcher.Classes.Models.Collection
{
    public class BatchCollection<T>
    {
        /// <summary>
        /// Batch Collection total results
        /// </summary>
        public int Total { get; set; }
        /// <summary>
        /// Next Page URL
        /// </summary>
        public string Next { get; set; }
        /// <summary>
        /// Previous Page URL
        /// </summary>
        public string Previous { get; set; }
        /// <summary>
        /// Items List of "T"
        /// </summary>
        public IList<T> Items { get; set; }
    }
}
