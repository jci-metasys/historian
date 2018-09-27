using System;

namespace HistoricalDataFetcher.Classes.Models
{
    public class DateRange
    {
        public string StartDate { get; }
        public string EndDate { get; }

        public DateRange(DateTime startDate, DateTime endDate)
        {
            this.StartDate = startDate.ToUniversalTime().ToString("s", System.Globalization.CultureInfo.InvariantCulture);
            this.EndDate = endDate.ToUniversalTime().ToString("s", System.Globalization.CultureInfo.InvariantCulture);
        }

        public bool Equals(DateRange dr)
        {
            if (this.StartDate.Equals(dr.StartDate) && this.EndDate.Equals(dr.EndDate))
                return true;

            return false;
        }
    }
}
