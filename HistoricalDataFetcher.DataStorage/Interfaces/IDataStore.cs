using System.Collections.Generic;
using System.Threading.Tasks;

namespace HistoricalDataFetcher.DataStorage.Interfaces
{
    public interface IDataStore<T>
    {
        /// <summary>
        /// Reference to implement a custom way to save your data
        /// </summary>
        /// <param name="items">IEnumerable of T items</param>
        /// <returns></returns>
        Task SetDataAsync(IEnumerable<T> items);
    }
}
