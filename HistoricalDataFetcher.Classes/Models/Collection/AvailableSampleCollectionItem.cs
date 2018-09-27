using HistoricalDataFetcher.Classes.Models.Collection.TimeSeries;

namespace HistoricalDataFetcher.Classes.Models.Collection
{
    public class AvailableSampleCollectionItem
    {
        /// <summary>
        /// Available Samples URL
        /// </summary>
        public string Samples { get; set; }
        /// <summary>
        /// Available Samples Attribute ID
        /// </summary>
        public string Attribute { get; set; }
        /// <summary>
        /// Point containing available samples
        /// </summary>
        public PointBatchCollectionItem Point { get; set; }
    }
}
