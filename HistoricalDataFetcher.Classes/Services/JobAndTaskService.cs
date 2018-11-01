using HistoricalDataFetcher.Classes.DataLayer.JobsAndTasks;
using HistoricalDataFetcher.Classes.Enums;
using HistoricalDataFetcher.Classes.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HistoricalDataFetcher.Classes.Services
{
    public class JobAndTaskService
    {
        private JobRepository _jobRepository;
        private TaskQueueRepository _taskRepository;

        public JobAndTaskService()
        {
            _jobRepository = new JobRepository();
            _taskRepository = new TaskQueueRepository();
        }

        /// <summary>
        /// Create a Job and save it in the Database with the period of time the Job will pull historical data from
        /// </summary>
        /// <param name="startTime">Start Date and Time to pull data from</param>
        /// <param name="endTime">End Date and Time to pull data from</param>
        /// <returns>JobEntity</returns>
        public async Task<JobEntity> CreateJobAsync(DateTime startTime, DateTime endTime)
        {
            var job = new JobEntity
            {
                DateCreated = DateTime.Now,
                StartTime = startTime,
                EndTime = endTime,
                Status = JobStatusEnum.Created
            };

            await _jobRepository.CreateAsync(job);

            return job;
        }

        /// <summary>
        /// Saves the Tasks in the Database
        /// </summary>
        /// <param name="job">JobEntity with Tasks</param>
        /// <returns></returns>
        public async Task PersistTasksAsync(JobEntity job)
        {
            await _taskRepository.CreateTasksInAJobAsync(job);
        }

        /// <summary>
        /// Get a list of Jobs by Status
        /// </summary>
        /// <param name="status">JobStatusEnum</param>
        /// <returns>IEnumberable of JobEntity</returns>
        public async Task<IEnumerable<JobEntity>> GetJobByStatusAsync(JobStatusEnum status)
        {
            return await _jobRepository.GetJobsByStatusAsync(status);
        }

        /// <summary>
        /// Return a list of Unfinished Jobs
        /// </summary>
        /// <returns>IEnumerable of JobEntity</returns>
        public async Task<IEnumerable<JobEntity>> GetUnfinishedJobsAsync()
        {
            return await _jobRepository.GetUnSuccessfulAsync();
        }

        /// <summary>
        /// Return the latest Job
        /// </summary>
        /// <returns>JobEntity</returns>
        public async Task<JobEntity> GetLatestJobAsync()
        {
            return await _jobRepository.GetLatestAsync();
        }

        /// <summary>
        /// Marks the passed in Task as complete
        /// </summary>
        /// <param name="task">The task to mark as complete</param>
        /// <returns>bool: True = success</returns>
        public async Task<bool> MarkTaskCompleteAsync(TaskQueueEntity task)
        {
            var sucessful = await _taskRepository.UpdateToCompleteAsync(task.Id);

            if (sucessful)
            {
                task.IsCompleted = true;

                return true;
            }

            return false;
        }

        /// <summary>
        /// Update the Job Status to the passed in status
        /// </summary>
        /// <param name="job">JobEntity to update</param>
        /// <param name="status">new JobStatusEnum</param>
        /// <returns>bool: True = success</returns>
        public async Task<bool> UpdateJobStatusAsync(JobEntity job, JobStatusEnum status)
        {
            var sucessful = await _jobRepository.UpdateAsync(job, status);

            if (sucessful)
            {
                job.Status = status;

                return true;
            }

            return false;
        }
    }
}
