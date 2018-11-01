namespace HistoricalDataFetcher.Classes.Models.Collection.TimeSeries
{
    public class PointBatchCollectionItem
    {
        /// <summary>
        /// Point Id
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// Point Item Reference
        /// </summary>
        public string ItemReference { get; set; }
        /// <summary>
        /// Point Name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Point Type
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// Point Self URL
        /// </summary>
        public string Self { get; set; }
        /// <summary>
        /// Point Parent URL
        /// </summary>
        public string Parent { get; set; }
        /// <summary>
        /// Point list of Object URL
        /// </summary>
        public string Objects { get; set; }
        /// <summary>
        /// Point's Network Device
        /// </summary>
        public string NetworkDevice { get; set; }
        /// <summary>
        /// List of Points as children to this point URL
        /// </summary>
        public string Points { get; set; }
        /// <summary>
        /// Points Attributes with samples URL
        /// </summary>
        public string Attributes { get; set; }
        /// <summary>
        /// List of Alarms on this point URL
        /// </summary>
        public string Alarms { get; set; }
        /// <summary>
        /// List of Audits on this point URL
        /// </summary>
        public string Audits { get; set; }
    }
}
