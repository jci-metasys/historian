using HistoricalDataFetcher.Classes.Models.Collection.TimeSeries;
using System;

namespace HistoricalDataFetcher.Classes.Models.Collection
{
    public class SampleCollectionItem
    {
        /// <summary>
        /// Object of Sample Value
        /// </summary>
        public SampleValue Value { get; set; }
        /// <summary>
        /// Timestamp of a sample
        /// </summary>
        public DateTime Timestamp { get; set; }
        /// <summary>
        /// Is sample reliable
        /// </summary>
        public bool IsReliable { get; set; }
        /// <summary>
        /// Object reference to a Point
        /// </summary>
        public PointBatchCollectionItem Point { get; set; }
    }

    public class SampleValue
    {
        /// <summary>
        /// Sample value
        /// </summary>
        public double Value { get; set; }
        /// <summary>
        /// Sample value units
        /// </summary>
        public string Units { get; set; }
    }
}
