using HistoricalDataFetcher.Classes.Enums;
using HistoricalDataFetcher.Classes.Models;
using HistoricalDataFetcher.Classes.Models.Collection.TimeSeries;
using HistoricalDataFetcher.Classes.Utilities;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HistoricalDataFetcher.Classes.Services
{
    public class JobManager
    {
        private readonly JobAndTaskService _jobAndTaskService;
        private readonly ILog _log;

        public JobManager()
        {
            _jobAndTaskService = new JobAndTaskService();

            _log = LogManager.GetLogger(GetType());

            if (_log == null)
            {
                throw new ArgumentNullException("JobManager: Failed to create _log object.");
            }
        }

        /// <summary>
        /// Handle the unfished Jobs
        /// </summary>
        /// <returns>bool: True = complete</returns>
        public async Task<bool> HandleUnFinishedJobsAsync()
        {
            var jobs = await _jobAndTaskService.GetUnfinishedJobsAsync();

            var unfishedJobRunGuid = Guid.NewGuid().ToString().Substring(0, 8);

            if (!jobs.Any())
            {
                _log.Info($"JobManager.HandleUnFinishedJobsAsync: no unfinished jobs found, date checked: {DateTime.Now}");

                return true;
            }
            
            _log.Info($"JobManager.HandleUnFinishedJobsAsync {unfishedJobRunGuid}: {jobs.Count()} unfinished jobs found, date checked: {DateTime.Now}");
            try
            {
                var tasks = new List<Task>();
                foreach (var job in jobs)
                {
                    if (job.Status == JobStatusEnum.Created)
                    {
                        await PopulateTasksAsync(job);
                    }
                    tasks.Add(ProcessJobAsync(job));
                }

                Task.WaitAll(tasks.ToArray());

                if (tasks.Any(t => t.Status == TaskStatus.Faulted))
                {
                    _log.Info($"JobManager.HandleUnFinishedJobsAsync {unfishedJobRunGuid}: one or more unfinished jobs wasn't processed successfully.");
                    return false;
                }
            }
            catch (Exception ex)
            {
                _log.Info($"JobManager.HandleUnFinishedJobsAsync {unfishedJobRunGuid}: something went wrong, exception stack trace: {ex.StackTrace}");
                return false;
            }

            _log.Info($"JobManager.HandleUnFinishedJobsAsync {unfishedJobRunGuid}: all unfinished jobs processed successfully.");
            return true;
        }

        /// <summary>
        /// Create and Process a Job using the Start Time and End Time as parameters for the range of data to be pulled
        /// </summary>
        /// <param name="startTime">Start Date and Time of the range for the data</param>
        /// <param name="endTime">End Date and Time of the range for the data</param>
        /// <returns></returns>
        public async Task<bool> CreateAndProcessJobsAsync(DateTime startTime, DateTime endTime)
        {
            _log.Info($"JobManager.CreateAndProcessJobsAsync: startTime-{startTime}, endTime-{endTime}");

            JobEntity newJob = null;

            try
            {
                newJob = await CreateNewJobAsync(startTime, endTime);
            }
            catch (Exception ex)
            {
                // something goes wrong while creating a new job, we can't move on any further
                _log.Info($"JobManager.CreateAndProcessJobsAsync:  failed to create new job, startTime - {startTime}, endTime - {endTime}. Exception stack trace: {ex.StackTrace}");
                return false;
            }

            try
            {
                await PopulateTasksAsync(newJob);

                await ProcessJobAsync(newJob);
            }
            catch (Exception ex)
            {
                // if something goes wrong while processing a job, we log it and move on, because this job will be picked up again by windows service
                _log.Info($"JobManager.CreateAndProcessJobsAsync:  something went wrong while processing job with Id {newJob.Id}. Exception stack trace: {ex.StackTrace}");
            }

            return true;
        }

        /// <summary>
        /// Create a new Job
        /// </summary>
        /// <param name="startTime">Start Date and Time of the data range</param>
        /// <param name="endTime">End Date and Time of the data range</param>
        /// <returns></returns>
        private async Task<JobEntity> CreateNewJobAsync(DateTime startTime, DateTime endTime)
        {
            _log.Info($"JobManager.CreateNewJobsAsync: startTime-{startTime}, endTime-{endTime}");
            var job = await _jobAndTaskService.CreateJobAsync(startTime, endTime);

            return job;
        }

        /// <summary>
        /// Update a JobEntity with the passed in JobStatus
        /// </summary>
        /// <param name="job">JobEntity</param>
        /// <param name="jobStatus">new JobStatusEnum</param>
        /// <returns>bool: True = success</returns>
        private async Task<bool> UpdateJobStatusAsync(JobEntity job, JobStatusEnum jobStatus)
        {
            _log.Info($"JobManager.UpdateJobStatusAsync, jobId-{job.Id}, status-{jobStatus.ToString()}");
            return await _jobAndTaskService.UpdateJobStatusAsync(job, jobStatus);
        }

        /// <summary>
        /// Populate Tasks using the Task Creator as a source
        /// </summary>
        /// <param name="job">Job to add Tasks to</param>
        /// <returns></returns>
        private async Task PopulateTasksAsync(JobEntity job)
        {
            _log.Info($"JobManager.PopulateTasksAsync, jobId-{job.Id}");
                        
            foreach (var taskType in TaskUtilityFactory.SupportedTaskTypes)
            {
                var taskCreator = TaskUtilityFactory.GetTaskCreator(taskType);

                taskCreator.CreateTasks(job);
            }

            await _jobAndTaskService.PersistTasksAsync(job);

            await _jobAndTaskService.UpdateJobStatusAsync(job, JobStatusEnum.TasksCreated);
        }

        /// <summary>
        /// process all incomplete tasks in a job, if no apropriate endpoints available, the process result will be false 
        /// </summary>
        /// <param name="job">the job entity</param>
        /// <returns>if all tasks are completed</returns>
        private async Task ProcessJobAsync(JobEntity job)
        {
            _log.Info($"JobManager.ProcessJobAsync, jobId-{job.Id}");

            bool jobProcessSuccessful = true;

            await _jobAndTaskService.UpdateJobStatusAsync(job, JobStatusEnum.Executing);

            foreach(var task in job.Tasks.Where(t => !t.IsCompleted))
            {
                var endpoint = TaskUtilityFactory.GetEndPoint(task.TaskType);

                if (endpoint != null)
                {
                    await endpoint.RunAsync(task.TaskUrl)
                                  .ContinueWith(async (a) =>
                                  {
                                      if (a.Result)
                                          await _jobAndTaskService.MarkTaskCompleteAsync(task);
                                      else
                                          jobProcessSuccessful = false;
                                  });
                }
                else
                {
                    //can't handle this task
                    jobProcessSuccessful = false;
                }
            }

            if (jobProcessSuccessful)
                await _jobAndTaskService.UpdateJobStatusAsync(job, JobStatusEnum.Success);
            else
                await _jobAndTaskService.UpdateJobStatusAsync(job, JobStatusEnum.Error);
        }
    }
}
