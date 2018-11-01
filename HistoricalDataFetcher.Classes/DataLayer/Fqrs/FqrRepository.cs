using Dapper;
using HistoricalDataFetcher.DataStorage.Models;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace HistoricalDataFetcher.Classes.DataLayer.Fqrs
{
    public class FqrRepository : DBDataModel
    {
        /// <summary>
        /// Inserts the list of FQR's into the Database
        /// </summary>
        /// <param name="items">IEnumberable of FqrGuidDataModel</param>
        /// <returns></returns>
        public async Task SetDataAsync(IEnumerable<FqrGuidDataModel> items)
        {
            //Clear the FqrGuid table so that only the new list of items is populated.
            await DeleteFqrGuidEntriesAsync();

            using (var connection = new SqlConnection($"{DBConnectionString}"))
            {
                await connection.OpenAsync();
                await connection.ExecuteAsync("InsertFqrGuid", items, commandType: CommandType.StoredProcedure);
                connection.Close();
            }
        }

        /// <summary>
        /// Get all FqrGuidDataModel items
        /// </summary>
        /// <returns>IEnumberable of FqrGuidDataModel</returns>
        public async Task<IEnumerable<FqrGuidDataModel>> GetAllAsync()
        {
            IEnumerable<FqrGuidDataModel> result = new List<FqrGuidDataModel>();

            using (var connection = new SqlConnection($"{DBConnectionString}"))
            {
                await connection.OpenAsync();
                result = await connection.QueryAsync<FqrGuidDataModel>("GetAllFqrGuids", commandType: CommandType.StoredProcedure);
                connection.Close();
            }

            return result;
        }

        /// <summary>
        /// Delete all Fqr entries
        /// </summary>
        /// <returns></returns>
        private async Task DeleteFqrGuidEntriesAsync()
        {
            using (var connection = new SqlConnection($"{DBConnectionString}"))
            {
                await connection.OpenAsync();
                await connection.ExecuteAsync("DeleteAllFqrGuid", commandType: CommandType.StoredProcedure);
                connection.Close();
            }
        }
    }
}
