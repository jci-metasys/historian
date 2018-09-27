using Dapper;
using HistoricalDataFetcher.DataStorage.Models;
using log4net;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace HistoricalDataFetcher.Classes.EnumSet
{
    public class EnumDescSaveToSqlServer : DBDataModel, IEnumDescRepository
    {
        private readonly ILog _log;
        
        public EnumDescSaveToSqlServer()
        {
            _log = LogManager.GetLogger(GetType());
        }
        private EnumDescCollection _edcCollection;
        
        /// <summary>
        /// Checks to see if the data exists
        /// </summary>
        /// <returns>bool: True = Data exists</returns>
        public async Task<bool> DataExistsAsync()
        {
            try
            {
                //Check to make sure a DB connection exists and that the records exist
                _edcCollection = await GetDataEnumDescSetAsync();
            }
            catch (Exception ex)
            {
                _log.Error($"EnumDescSaveToSqlServer.DataExistsAsync: failed to get data from database. Stack trace: {ex.StackTrace}");
                return false;
            }
            return _edcCollection != null && _edcCollection.Count > 0;
        }

        /// <summary>
        /// Queries the Database for the EnumSet descriptions
        /// </summary>
        /// <returns>EnumDescCollection</returns>
        public async Task<EnumDescCollection> GetDataEnumDescSetAsync()
        {
            if (_edcCollection != null)
                return _edcCollection;
            _edcCollection = new EnumDescCollection();

            using (var conn = new SqlConnection(DBConnectionString)) // "Data Source=.;Initial Catalog=MyDb;Integrated Security=True;"))
            {
                await conn.OpenAsync();
                _edcCollection.AddRange(await conn.QueryAsync<EnumDescDataStoreModel>("GetEnumList", commandType: CommandType.StoredProcedure));                                
                conn.Close();
            }
            return _edcCollection;
        }

        /// <summary>
        /// Saves the EnumDescData into the Database
        /// </summary>
        /// <param name="items">EnumDescCollection</param>
        /// <returns></returns>
        public async Task SaveDataEnumDescSetAsync(EnumDescCollection items)
        {

            using (var conn = new SqlConnection(DBConnectionString))
            {
                await conn.OpenAsync();
                await conn.ExecuteAsync("InsertEnumList", new { EnumListUDT = items.AsTableValuedParameter("dbo.EnumListUDT") }, commandType: CommandType.StoredProcedure);
                conn.Close();
            }
            _edcCollection = null;
        }
    }
}
