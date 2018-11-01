namespace HistoricalDataFetcher.Classes.DataLayer.Cache
{
    public class NullCache : ICache
    {
        public int GetCacheSize()
        {
            return 0;
        }

        public string Get(string key)
        {
            return string.Empty;
        }

        public string Get(int index)
        {
            return string.Empty;
        }

        public void Add(string key, string value)
        {
        }

        public void Persist()
        {
        }
    }
}
