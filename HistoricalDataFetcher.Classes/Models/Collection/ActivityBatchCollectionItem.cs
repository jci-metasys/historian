using HistoricalDataFetcher.Classes.Models.Collection.Activities;
using System.Collections.Generic;

namespace HistoricalDataFetcher.Classes.Models.Collection
{
    public class ActivityBatchCollectionItem
    {
        public string UniqueIdentifier { get; set; }
        public CreationUTC CreationUTC { get; set; }
        public object ServerName { get; set; }
        public object ServerUTC { get; set; }
        public Actiontype Actiontype { get; set; }
        public object SubsytemDescription { get; set; }
        public bool Discarded { get; set; }
        public object Status { get; set; }
        public int NbrParameters { get; set; }
        public PreData PreData { get; set; }
        public PostData PostData { get; set; }
        public List<object> Parameters { get; set; }
        public object ErrorString { get; set; }
        public int EditMethod { get; set; }
        public object PrimaryObject { get; set; }
        public object SecondaryObject { get; set; }
        public int TransactionSequence { get; set; }
        public User User { get; set; }
        public object Signature { get; set; }
        public List<object> Annotations { get; set; }
        public Legacy Legacy { get; set; }
        public string PointId { get; set; }
    }
}
