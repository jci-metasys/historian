using System;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace HistoricalDataFetcher.DataStorage.Models
{
    public class DBDataModel
    {
        /// <summary>
        /// Database connection string (Connection should contain the server name, Database name, and Security/Login info)
        /// </summary>
        public static string DBConnectionString { get; set; } = "Server=(local);Integrated Security=SSPI;Database=DataExtractor";

        /// <summary>
        /// Assigns the DBConnectionString and checks the connection
        /// </summary>
        /// <param name="dbConnection">Database connection string</param>
        /// <returns>bool: True = success</returns>
        public static async Task<bool> CheckDBConnectionAsync(string dbConnection)
        {
            if (!string.IsNullOrWhiteSpace(dbConnection))
            {
                DBConnectionString = dbConnection;
            }

            try
            {
                using (var connection = new SqlConnection(DBConnectionString))
                {
                    await connection.OpenAsync();
                }
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }
    }
}
