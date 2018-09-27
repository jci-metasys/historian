using System;
using System.IO;
using System.Runtime.Serialization;

namespace HistoricalDataFetcher.Classes.DataLayer.Cache
{
    public class CachedMemory : ICache
    {
        public static readonly int CACHE_SIZE_NO_LIMIT = -1;

        private System.Collections.Specialized.OrderedDictionary _cache = null;
        private readonly string _persistenceFilePath = null;
        private int _cacheSizeLimit;
        private const string defaultCacheFileFath = @".\historical-data-cache.txt";

        public CachedMemory(int cacheSizeLimit, string cacheFilePath)
        {
            this._persistenceFilePath = cacheFilePath;
            this._cacheSizeLimit = cacheSizeLimit <= 0 ? CACHE_SIZE_NO_LIMIT : cacheSizeLimit;
            InitializeCache(cacheFilePath, cacheSizeLimit);
        }

        public int GetCacheSize()
        {
            return this._cache.Count;
        }

        public string Get(string key)
        {
            try
            {
                return _cache[key] as string;
            }
            catch (Exception)
            {
                return string.Empty;
            }
            
        }

        public string Get(int index)
        {
            try
            {
                return _cache[index] as string;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        public void Add(string key, string value)
        {
            //An ordered dictionary would throw an exception if we try to insert the same key again, so we have to make sure that the newly
            //introduced key is not a duplicate.
            if (this._cache.Contains(key))
            {
                this._cache.Remove(key);
            }
            else
            {
                if (this._cacheSizeLimit != CACHE_SIZE_NO_LIMIT && this._cache.Count == this._cacheSizeLimit)
                {
                    this._cache.RemoveAt(0);
                }
            }

            this._cache.Add(key, value);
        }

        public void Persist()
        {
            using (var fileStream = new FileStream(_persistenceFilePath, FileMode.Open))
            {
                IFormatter bf = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                bf.Serialize(fileStream, this._cache);
                fileStream.Close();
            }
        }

        private void InitializeCache(string cacheFilePath, int cacheSizeLimit)
        {
            this._cacheSizeLimit = cacheSizeLimit;

            try
            {
                using (var fileStream = new FileStream(cacheFilePath, FileMode.OpenOrCreate))
                {
                    InitializeCacheHelper(fileStream);
                }
            }
            catch (Exception ex)
            {
                using (var fileStream = new FileStream(defaultCacheFileFath, FileMode.OpenOrCreate))
                {
                    InitializeCacheHelper(fileStream);
                }
            }


            //In case the deserialized OrderedDictionary had more contents than the limit, we need to shrink it to make its size equal to the limit
            if (this._cacheSizeLimit != CACHE_SIZE_NO_LIMIT && this._cache.Keys.Count > this._cacheSizeLimit)
            {
                int difference = this._cache.Keys.Count - this._cacheSizeLimit;

                for (int i = 0; i < difference; i++)
                {
                    _cache.RemoveAt(0);
                }
            }
        }

        private void InitializeCacheHelper(FileStream fileStream)
        {
            IFormatter bf = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            fileStream.Position = 0;

            try
            {
                this._cache = (System.Collections.Specialized.OrderedDictionary)bf.Deserialize(fileStream);
            }
            catch (Exception ex)
            {
                this._cache = new System.Collections.Specialized.OrderedDictionary(0);
            }

            fileStream.Close();
        }
    }
}
