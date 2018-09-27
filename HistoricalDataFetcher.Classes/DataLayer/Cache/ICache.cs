namespace HistoricalDataFetcher.Classes.DataLayer.Cache
{
    public interface ICache
    {
        int GetCacheSize();
        string Get(string key);
        string Get(int index);
        void Add(string key, string value);
        void Persist();
    }
}
