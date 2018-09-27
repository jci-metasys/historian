using Microsoft.SqlServer.Server;
using System.Collections.Generic;
using System.Data;

namespace HistoricalDataFetcher.DataStorage.Models
{
    public class EnumDescDataStoreModel
    {
        /// <summary>
        /// Enum Set Id
        /// </summary>
        public int SetId { get; set; }
        /// <summary>
        /// Enum Member Id
        /// </summary>
        public int MemberId { get; set; }
        /// <summary>
        /// Enum Set Description
        /// </summary>
        public string SetDesc { get; set; }
        /// <summary>
        /// Enum Member Description
        /// </summary>
        public string MemberDesc { get; set; }

    }
    public class EnumDescCollection : List<EnumDescDataStoreModel>, IEnumerable<SqlDataRecord>
    {
        /// <summary>
        /// Sets the Enumerator to assist in saving the data to a SQL Database
        /// </summary>
        /// <returns></returns>
        IEnumerator<SqlDataRecord> IEnumerable<SqlDataRecord>.GetEnumerator()
        {
            var sdr = new SqlDataRecord(
            new SqlMetaData("SetId", SqlDbType.Int),
            new SqlMetaData("MemberId", SqlDbType.Int),
            new SqlMetaData("SetDesc", SqlDbType.VarChar, 300),
            new SqlMetaData("MemberDesc", SqlDbType.VarChar, 300));

            foreach (EnumDescDataStoreModel desc in this)
            {
                sdr.SetInt32(0, desc.SetId);
                sdr.SetInt32(1, desc.MemberId);
                sdr.SetSqlString(2, desc.SetDesc);
                sdr.SetSqlString(3, desc.MemberDesc);

                yield return sdr;
            }
        }
    }
}
