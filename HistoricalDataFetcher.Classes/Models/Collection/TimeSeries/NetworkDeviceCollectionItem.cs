namespace HistoricalDataFetcher.Classes.Models.Collection
{
    public class NetworkDeviceCollectionItem
    {
        /// <summary>
        /// Network Device Id
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// Network Device Item Reference
        /// </summary>
        public string ItemReference { get; set; }
        /// <summary>
        /// Network Device Name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Network Device Type
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// Network Device Description
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Network Device Firmware version
        /// </summary>
        public string FirmwareVersion { get; set; }
        /// <summary>
        /// Network Device Category
        /// </summary>
        public string Category { get; set; }
        /// <summary>
        /// Network Device TimeZone
        /// </summary>
        public string TimeZone { get; set; }
        /// <summary>
        /// Network Device Self URL
        /// </summary>
        public string Self { get; set; }
        /// <summary>
        /// Netowrk Device Parent device URL
        /// </summary>
        public string Parent { get; set; }
        /// <summary>
        /// List of Network Devices under this Network Device URL
        /// </summary>
        public string NetworkDevices { get; set; }
        /// <summary>
        /// List of Equipment under this Network Device URL
        /// </summary>
        public string Equipment { get; set; }
        /// <summary>
        /// List of Spaces under this Network Device URL
        /// </summary>
        public string Spaces { get; set; }
        /// <summary>
        /// List of Objects under this Network Device URL
        /// </summary>
        public string Objects { get; set; }
        /// <summary>
        /// List of Attributes with samples URL
        /// </summary>
        public string Attributes { get; set; }
        /// <summary>
        /// List of Alarms under this Network Device URL
        /// </summary>
        public string Alarms { get; set; }
    }
}
