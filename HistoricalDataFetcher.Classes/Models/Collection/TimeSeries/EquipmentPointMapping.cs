namespace HistoricalDataFetcher.Classes.Models.Collection.TimeSeries
{
    public class EquipmentPointMapping
    {
        /// <summary>
        /// Equipment Name
        /// </summary>
        public string EquipmentName { get; set; }
        /// <summary>
        /// Equipment Short Name
        /// </summary>
        public string ShortName { get; set; }
        /// <summary>
        /// Equipment Label
        /// </summary>
        public string Label { get; set; }
        /// <summary>
        /// Equipment Category
        /// </summary>
        public string Category { get; set; }
        /// <summary>
        /// Is Display Data
        /// </summary>
        public bool IsDisplayData { get; set; }
        /// <summary>
        /// List of Mapped Equipment URL
        /// </summary>
        public string MappedEquipment { get; set; }
        /// <summary>
        /// List of Mapped Points URL
        /// </summary>
        public string MappedPoint { get; set; }
    }
}
