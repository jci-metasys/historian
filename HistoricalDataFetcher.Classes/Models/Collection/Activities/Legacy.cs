namespace HistoricalDataFetcher.Classes.Models.Collection.Activities
{
    public class Legacy
    {
        /// <summary>
        /// Audit Id
        /// </summary>
        public int AuditId { get; set; }
        /// <summary>
        /// FQR/Item Reference
        /// </summary>
        public string FullyQualifiedItemReference { get; set; }
        /// <summary>
        /// Item name
        /// </summary>
        public string ItemName { get; set; }
        /// <summary>
        /// Class
        /// </summary>
        public Class @Class { get; set; }
        /// <summary>
        /// Original Application
        /// </summary>
        public OriginApplication OriginApplication { get; set; }
        /// <summary>
        /// Description
        /// </summary>
        public Description Description { get; set; }
        /// <summary>
        /// Product Name
        /// </summary>
        public int ProductName { get; set; }
    }
}
