using Dapper;
using HistoricalDataFetcher.DataStorage.Interfaces;
using HistoricalDataFetcher.DataStorage.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace HistoricalDataFetcher.DataStorage.Alarms
{
    public class AlarmSaveToSqlServer : DBDataModel, IDataStore<AlarmDataStoreModel>
    {
        /// <summary>
        /// Insert Alarm data into the Database
        /// </summary>
        /// <param name="items">IEnumerable of AlarmDataStoreModel</param>
        /// <returns></returns>
        public async Task SetDataAsync(IEnumerable<AlarmDataStoreModel> items)
        {
            if (!items.Any())
                return;
            using (var connection = new SqlConnection(DBConnectionString))
            {
                await connection.OpenAsync();

                foreach (var alarm in items)
                {
                    await connection.ExecuteAsync("InsertAlarmData", new
                    {   id = alarm.Id,
                        itemReference  = alarm.ItemReference,
                        name = alarm.Name,
                        message = alarm.Message,
                        description = alarm.Description,
                        isAckRequired = alarm.IsAckRequired,
                        type = alarm.Type,
                        priority = alarm.Priority,
                        triggerValue = alarm.TriggerValue,
                        triggerValueUnits = alarm.TriggerValueUnits,
                        triggerValueHref = alarm.TriggerValueHref,
                        creationTime = alarm.CreationTime,
                        isAcknowledged = alarm.IsAcknowledged,
                        isDiscarded = alarm.IsDiscarded,
                        category = alarm.Category
                    }, commandType: CommandType.StoredProcedure);

                    await connection.ExecuteAsync("InsertAnnotation", alarm.Annotations, commandType: CommandType.StoredProcedure);
                }
            }
        }
    }
}
