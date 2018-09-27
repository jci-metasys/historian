namespace HistoricalDataFetcher.Classes.Models.Collection.TimeSeries
{
    public class EquipmentBatchCollectionItem
    {
        /// <summary>
        /// Equipment Id
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// Equipment Item Reference
        /// </summary>
        public string ItemReference { get; set; }
        /// <summary>
        /// Equipment Name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Equipment Type
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// Equipment Self URL
        /// </summary>
        public string Self { get; set; }
        /// <summary>
        /// List of Spaces Served URL
        /// </summary>
        public string SpacesServed { get; set; }
        /// <summary>
        /// List of Equipment Served URL
        /// </summary>
        public string EquipmentServed { get; set; }
        /// <summary>
        /// Equipment is Served By URL
        /// </summary>
        public string ServedBy { get; set; }
        /// <summary>
        /// Equipment is Hosted By URL
        /// </summary>
        public string HostedBy { get; set; }
        /// <summary>
        /// Equipment has mappings URL
        /// </summary>
        public string HasMappings { get; set; }
        /// <summary>
        /// Equipment parent Network Device
        /// </summary>
        public NetworkDeviceCollectionItem NetworkDevice { get; set; }
    }
}
