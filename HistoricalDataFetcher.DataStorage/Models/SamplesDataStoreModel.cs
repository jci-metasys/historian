using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.Data;

namespace HistoricalDataFetcher.DataStorage.Models
{
    public class SamplesDataStoreModel
    {
        /// <summary>
        /// Sample Value
        /// </summary>
        public double Value { get; set; }
        /// <summary>
        /// Sample Units
        /// </summary>
        public string Units { get; set; }
        /// <summary>
        /// Date and Time of record
        /// </summary>
        public DateTime TimeStamp { get; set; }
        /// <summary>
        /// Sample field IsReliable
        /// </summary>
        public bool IsReliable { get; set; }
        /// <summary>
        /// Point name the sample is attached to
        /// </summary>
        public string PointName { get; set; }
        /// <summary>
        /// Point Id
        /// </summary>
        public Guid PointGuid { get; set; }
        /// <summary>
        /// Point type
        /// </summary>
        public string PointType { get; set; }
        /// <summary>
        /// Point FQR or ItemReference
        /// </summary>
        public string ItemReference { get; set; }
    }

    public class SamplesDataStoreCollection : List<SamplesDataStoreModel>, IEnumerable<SqlDataRecord>
    {
        /// <summary>
        /// Returns an Enumerator to insert format the data for a SQL Database
        /// </summary>
        /// <returns></returns>
        IEnumerator<SqlDataRecord> IEnumerable<SqlDataRecord>.GetEnumerator()
        {
            var sdr = new SqlDataRecord(
                new SqlMetaData("PointName", SqlDbType.VarChar, 400),
                new SqlMetaData("PointGuid", SqlDbType.UniqueIdentifier),
                new SqlMetaData("PointType", SqlDbType.VarChar, 400),
                new SqlMetaData("ItemReference", SqlDbType.VarChar, 400),
                new SqlMetaData("IsReliable", SqlDbType.Bit),
                new SqlMetaData("TimeStamp", SqlDbType.DateTime),
                new SqlMetaData("Units", SqlDbType.VarChar, 100),
                new SqlMetaData("Value", SqlDbType.VarChar, 400));

            foreach (var desc in this)
            {
                sdr.SetSqlString(0, desc.PointName);
                sdr.SetSqlGuid(1, desc.PointGuid);
                sdr.SetSqlString(2, desc.PointType);
                sdr.SetSqlString(3, desc.ItemReference);
                sdr.SetSqlBoolean(4, desc.IsReliable);
                sdr.SetSqlDateTime(5, desc.TimeStamp);
                sdr.SetSqlString(6, desc.Units);
                sdr.SetSqlString(7, desc.Value.ToString());


                yield return sdr;
            }
        }
    }
}
