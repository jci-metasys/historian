namespace HistoricalDataFetcher.Classes.Models.Collection
{
    public class EnumMemberBatchCollectionItem
    {
        /// <summary>
        /// Enum Member Id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Enum Member Description
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Enum Member Self URL
        /// </summary>
        public string Self { get; set; }
        /// <summary>
        /// Enum Member Set/Parent URL
        /// </summary>
        public string Set { get; set; }
    }
}
