namespace HistoricalDataFetcher.Classes.Models.Collection.Alarms
{
    public class TriggerValue
    {
        /// <summary>
        /// Alarm Trigger Value
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Alarm Trigger Value Units (analog only)
        /// </summary>
        public string Units { get; set; }

        /// <summary>
        /// Alarm Trigger Value enum member route (digital only)
        /// </summary>
        public string ValueEnumMember { get; set; }
    }
}
