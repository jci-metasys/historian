using Dapper;
using HistoricalDataFetcher.Classes.Models;
using HistoricalDataFetcher.DataStorage.Models;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace HistoricalDataFetcher.Classes.DataLayer.JobsAndTasks
{
    public class TaskQueueRepository : DBDataModel
    {
        public async Task CreateAsync(TaskQueueEntity task)
        {
            using (var connection = new SqlConnection(DBConnectionString))
            {
                await connection.OpenAsync();
                task.Id = await connection.QueryFirstOrDefaultAsync<int>("InsertTask", new { jobId = task.JobId, taskUrl = task.TaskUrl }, commandType: CommandType.StoredProcedure);
                task.IsCompleted = false;
            }
        }

        public async Task CreateTasksInAJobAsync(JobEntity job)
        {
            using (var connection = new SqlConnection(DBConnectionString))
            {
                await connection.OpenAsync();
                foreach (var task in job.Tasks)
                {
                    task.Id = await connection.QueryFirstOrDefaultAsync<int>("InsertTask", new { jobId = job.Id, taskType = (int)task.TaskType, taskUrl = task.TaskUrl }, commandType: CommandType.StoredProcedure);
                    task.JobId = job.Id;
                    task.Parent = job;
                    task.IsCompleted = false;
                }
            }
        }

        public async Task<IEnumerable<TaskQueueEntity>> GetByJobIdAsync(int jobId)
        {
            IEnumerable<TaskQueueEntity> result = new List<TaskQueueEntity>();
            using (var connection = new SqlConnection(DBConnectionString))
            {
                await connection.OpenAsync();
                result = await connection.QueryAsync<TaskQueueEntity>("GetTasksByJobId", new { jobId }, commandType: CommandType.StoredProcedure);
            }

            return result;
        }

        public async Task<bool> UpdateToCompleteAsync(int taskId)
        {
            using (var connection = new SqlConnection(DBConnectionString))
            {
                await connection.OpenAsync();
                await connection.ExecuteAsync("UpdateTaskToComplete", new { taskId }, commandType: CommandType.StoredProcedure);
            }

            return true;
        }

        public async Task<bool> DeleteByJobIdAsync(int jobId)
        {
            using (var connection = new SqlConnection(DBConnectionString))
            {
                await connection.OpenAsync();
                await connection.ExecuteAsync("DeleteTasksByJob", new { jobId }, commandType: CommandType.StoredProcedure);
            }

            return true;
        }
    }
}
