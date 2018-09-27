using System;

namespace HistoricalDataFetcher.Classes.Models
{
    public class FqrModel
    {
        /// <summary>
        /// FQR/Item reference
        /// </summary>
        public string Fqr { get; set; }
        /// <summary>
        /// Guid or object Id
        /// </summary>
        public Guid Guid { get; set; }
        /// <summary>
        /// Object/Point name
        /// </summary>
        public string PointName { get; set; }
        /// <summary>
        /// Object/Point type
        /// </summary>
        public string PointType { get; set; }
    }
}
