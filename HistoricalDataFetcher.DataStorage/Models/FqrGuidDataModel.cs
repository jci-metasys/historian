using System;

namespace HistoricalDataFetcher.DataStorage.Models
{
    public class FqrGuidDataModel
    {
        /// <summary>
        /// FQR or Item Reference
        /// </summary>
        public string Fqr { get; set; }
        /// <summary>
        /// Object Id
        /// </summary>
        public Guid Guid { get; set; }
        /// <summary>
        /// Object or Point name
        /// </summary>
        public string PointName { get; set; }
        /// <summary>
        /// Object or Point type
        /// </summary>
        public string PointType { get; set; }
    }
}
