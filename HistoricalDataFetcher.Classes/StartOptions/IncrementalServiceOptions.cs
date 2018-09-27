using System;

namespace HistoricalDataFetcher.Classes.StartOptions
{
    public class IncrementalServiceOptions : StartOptions
    {
        public DateTime StartTime { get; set; }
        public double TimeIntervalInHours { get; set; }
    }

}
