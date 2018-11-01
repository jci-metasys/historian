using HistoricalDataFetcher.Classes.DataLayer.Cache;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace HistoricalDataFetcher.Tests
{
    [TestClass]
    public class CacheTests
    {
        [TestCleanup()]
        public void Cleanup()
        {
           File.Delete(@".\cache-test.txt");
           File.Delete(@".\historical-data-cache.txt");
        }

        [TestMethod]
        public void CacheObjectCreatedTest()
        {
            ICache cacheObject = new CachedMemory(0, @".\cache-test.txt");
            Assert.IsTrue(File.Exists(@".\cache-test.txt"));
            Assert.IsNotNull(cacheObject);
        }

        [TestMethod]
        public void CacheObjectCreatedTestBadPath()
        {
            ICache cacheObject = new CachedMemory(0, @".\");
            Assert.IsTrue(File.Exists(@".\historical-data-cache.txt"));
            Assert.IsNotNull(cacheObject);
        }

        [TestMethod]
        public void CacheObjectAddItemToCache()
        {
            ICache cacheObject = new CachedMemory(100, @".\cache-test.txt");
            Assert.IsTrue(File.Exists(@".\cache-test.txt"));
            Assert.AreEqual(cacheObject.GetCacheSize(), 0);

            cacheObject.Add("unittest-1", "item");
            Assert.AreEqual(cacheObject.GetCacheSize(), 1);
        }

        [TestMethod]
        public void CacheObjectAddItemToCachePastCapacitySize()
        {
            ICache cacheObject = new CachedMemory(1, @".\cache-test.txt");
            Assert.IsTrue(File.Exists(@".\cache-test.txt"));
            Assert.AreEqual(cacheObject.GetCacheSize(), 0);

            cacheObject.Add("unittest-1", "item");
            Assert.AreEqual(cacheObject.GetCacheSize(), 1);
            Assert.AreEqual(cacheObject.Get("unittest-1"), "item");

            cacheObject.Add("unittest-2", "item");
            Assert.AreEqual(cacheObject.GetCacheSize(), 1);
            Assert.AreEqual(cacheObject.Get("unittest-2"), "item");
        }

        [TestMethod]
        public void CacheObjectAddItemToCacheDuplicateKey()
        {
            ICache cacheObject = new CachedMemory(1, @".\cache-test.txt");
            Assert.IsTrue(File.Exists(@".\cache-test.txt"));
            Assert.AreEqual(cacheObject.GetCacheSize(), 0);

            cacheObject.Add("unittest", "item");
            Assert.AreEqual(cacheObject.GetCacheSize(), 1);
            Assert.AreEqual(cacheObject.Get("unittest"), "item");

            cacheObject.Add("unittest", "12345");
            Assert.AreEqual(cacheObject.GetCacheSize(), 1);
            Assert.AreEqual(cacheObject.Get("unittest"), "12345");
        }

        [TestMethod]
        public void CacheObjectGetValueByIndex()
        {
            ICache cacheObject = new CachedMemory(1, @".\cache-test.txt");
            Assert.IsTrue(File.Exists(@".\cache-test.txt"));
            Assert.AreEqual(cacheObject.GetCacheSize(), 0);

            cacheObject.Add("unittest", "item");
            Assert.AreEqual(cacheObject.GetCacheSize(), 1);
            Assert.AreEqual(cacheObject.Get("unittest"), "item");

            cacheObject.Add("unittest", "12345");
            Assert.AreEqual(cacheObject.GetCacheSize(), 1);
            Assert.AreEqual(cacheObject.Get("unittest"), "12345");
        }
    }
}
