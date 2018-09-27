using System;

namespace HistoricalDataFetcher.Classes.Models.Collection.Activities
{
    public class CreationUTC
    {
        /// <summary>
        /// Product Name
        /// </summary>
        public int ProductName { get; set; }
        /// <summary>
        /// Date Time
        /// </summary>
        public DateTime DatetimeValue { get; set; }
        /// <summary>
        /// Time Zone
        /// </summary>
        public TimeZone TimeZone { get; set; }
        /// <summary>
        /// Local Off Set object
        /// </summary>
        public object LocalOffset { get; set; }
    }
}
