using HistoricalDataFetcher.DataStorage.Models;
using System.Threading.Tasks;

namespace HistoricalDataFetcher.Classes.EnumSet
{
    public interface IEnumDescRepository
    {
        /// <summary>
        /// Save the data to the implemented destination
        /// </summary>
        /// <param name="items">EnumDescCollection</param>
        /// <returns></returns>
        Task SaveDataEnumDescSetAsync(EnumDescCollection items);

        /// <summary>
        /// Get the full list of EnumSet descriptions
        /// </summary>
        /// <returns>EnumDescCollection</returns>
        Task<EnumDescCollection> GetDataEnumDescSetAsync();

        /// <summary>
        /// Checks the destination to verify if the data exists
        /// </summary>
        /// <returns>bool: True = Data Exists</returns>
        Task<bool> DataExistsAsync();
    }
}
