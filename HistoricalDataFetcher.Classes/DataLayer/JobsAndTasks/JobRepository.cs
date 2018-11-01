using Dapper;
using HistoricalDataFetcher.Classes.Enums;
using HistoricalDataFetcher.Classes.Models;
using HistoricalDataFetcher.DataStorage.Models;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace HistoricalDataFetcher.Classes.DataLayer.JobsAndTasks
{
    public class JobRepository : DBDataModel
    {
        public async Task CreateAsync(JobEntity job)
        {
            using (var connection = new SqlConnection(DBConnectionString))
            {
                await connection.OpenAsync();
                job.Id = await connection.QueryFirstOrDefaultAsync<int>("InsertJob", new { startTime = job.StartTime, endTime = job.EndTime, dateCreated = job.DateCreated }, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<IEnumerable<JobEntity>> GetJobsByStatusAsync(JobStatusEnum status)
        {
            IEnumerable<JobEntity> result = new List<JobEntity>();

            using (var connection = new SqlConnection(DBConnectionString))
            {
                await connection.OpenAsync();
                result =  await connection.QueryAsync<JobEntity>("GetJobsByStatus", new { status }, commandType: CommandType.StoredProcedure);

                foreach(var job in result)
                {
                    job.Tasks = (await connection.QueryAsync<TaskQueueEntity>("GetTasksByJobId", new { jobId = job.Id }, commandType: CommandType.StoredProcedure)).AsList();
                    job.Tasks.ForEach(t => t.Parent = job);
                }
            }

            return result;
        }

        public async Task<IEnumerable<JobEntity>> GetUnSuccessfulAsync()
        {
            IEnumerable<JobEntity> result = new List<JobEntity>();

            using (var connection = new SqlConnection(DBConnectionString))
            {
                await connection.OpenAsync();
                result = await connection.QueryAsync<JobEntity>("GetUnsuccessfulJobs", commandType: CommandType.StoredProcedure);

                foreach (var job in result)
                {
                    job.Tasks = (await connection.QueryAsync<TaskQueueEntity>("GetTasksByJobId", new { jobId = job.Id }, commandType: CommandType.StoredProcedure)).AsList();
                    job.Tasks.ForEach(t => t.Parent = job);
                }
            }

            return result;
        }

        public async Task<JobEntity> GetLatestAsync()
        {
            JobEntity job = null;
            using (var connection = new SqlConnection(DBConnectionString))
            {
                await connection.OpenAsync();
                job = await connection.QueryFirstOrDefaultAsync<JobEntity>("GetLatestJob", commandType: CommandType.StoredProcedure);
                if (job != null)
                {
                    job.Tasks = (await connection.QueryAsync<TaskQueueEntity>("GetTasksByJobId", new { jobId = job.Id }, commandType: CommandType.StoredProcedure)).AsList();
                    job.Tasks.ForEach(t => t.Parent = job);
                }
            }

            return job;
        }

        public async Task<bool> UpdateAsync(JobEntity job, JobStatusEnum status)
        {
            using (var connection = new SqlConnection(DBConnectionString))
            {
                await connection.OpenAsync();
                await connection.ExecuteAsync("UpdateJobStatus", new { jobId = job.Id, status, deleteTasks = status == JobStatusEnum.Success }, commandType: CommandType.StoredProcedure);
            }

            return true;
        }
    }
}
