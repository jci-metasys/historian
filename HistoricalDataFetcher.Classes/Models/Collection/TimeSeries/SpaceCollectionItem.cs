namespace HistoricalDataFetcher.Classes.Models.Collection.TimeSeries
{
    public class SpaceCollectionItem
    {
        /// <summary>
        /// Space Id
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// Space Item Reference
        /// </summary>
        public string ItemReference { get; set; }
        /// <summary>
        /// Spance Name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Space Type
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// Space Self URL
        /// </summary>
        public string Self { get; set; }
        /// <summary>
        /// Space is located in URL
        /// </summary>
        public string IsLocatedIn { get; set; }
        /// <summary>
        /// Space Contains URL
        /// </summary>
        public string Contains { get; set; }
        /// <summary>
        /// Space is served by URL
        /// </summary>
        public string ServedBy { get; set; }
        /// <summary>
        /// Hosts URL
        /// </summary>
        public string Hosts { get; set; }
        /// <summary>
        /// Network Device item
        /// </summary>
        public NetworkDeviceCollectionItem NetworkDevice { get; set; }
    }
}
