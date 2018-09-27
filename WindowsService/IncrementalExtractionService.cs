using HistoricalDataFetcher.Classes.Controller;
using HistoricalDataFetcher.Classes.Services;
using HistoricalDataFetcher.Classes.StartOptions;
using log4net;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;

namespace HistoricalDataFetcher.WindowsService
{
    public class IncrementalExtractionService
    {
        private IncrementalServiceOptions _startOptions;
        private JobManager _jobManager;
        private JobAndTaskService _jobAndTaskService;
        private Controller _controller;
        private DateTime _cutOffEndTime;

        private readonly ILog _log;

        public IncrementalExtractionService()
        {
            _log = LogManager.GetLogger(GetType());

            if (_log == null)
            {
                throw new ArgumentNullException("IncrementalExtractionService: Failed to create _log object.");
            }

            _log.Info("IncrementalExtractionService Initialized.");
        }

        public void Start()
        {
            _log.Info("Incremental Extraction Service Starting.");

             var initialized = InitAsync().Result;

            if (initialized)
            {
                ProcessUnFinishedJobsJobsAsync();
                DoWork();
            }
        }

        private async void ProcessUnFinishedJobsJobsAsync()
        {
            while (true)
            {
                await _jobManager.HandleUnFinishedJobsAsync();
                await Task.Delay(TimeSpan.FromHours(_startOptions.TimeIntervalInHours));
            }
        }

        private async void DoWork()
        {
            while (true)
            {
                await CreateAndProcessJobsAsync();
                await Task.Delay(TimeSpan.FromHours(_startOptions.TimeIntervalInHours));
            }
        }

        public void Stop()
        {
            _log.Info("Incremental Extraction Service Stopping.");
        }

        private async Task<bool> InitAsync()
        {
            _jobManager = new JobManager();
            _controller = new Controller();
            _jobAndTaskService = new JobAndTaskService();

            _startOptions = GetOptions();

            if (_startOptions == null)
            {
                _log.Error("Incremental Extraction Service: failed to populate start options.");
                throw new Exception("Incremental Extraction Service: failed to populate start options.");
            }

            if (!await _controller.SetDBSettingsAsync(_startOptions))
            {
                _log.Error("Incremental Extraction Service: failed to log in to Database.");
                throw  new Exception("Incremental Extraction Service: failed to log in to Database.");
            }

            if (!await _controller.InitApiRequestAsync(_startOptions))
            {
                _log.Error("Incremental Extraction Service: failed to get access token.");
                throw new Exception("Incremental Extraction Service: failed to get access token.");
            }

            //Load the EnumFile
            if (! await _controller.InitStartEnumDownloadAsync())
            {
                _log.Error("Incremental Extraction Service: failed to load EnumSet, refer to Controller.InitStartEnumDownloadAsync log entry for more details.");
                throw new Exception("Incremental Extraction Service: failed to load EnumSet.");
            }

            //Initialize TaskEnumUtility
            _controller.SetEndPoints(_startOptions);

            return true;
        }

        private IncrementalServiceOptions GetOptions()
        {
            try
            {
                // put your own implementation of how start options are created
                // this is just a quick way to get you started
                var builder = new ConfigurationBuilder().AddJsonFile("appsettings.json", false, true);

                var configuration = builder.Build();

                var result = configuration.Get<IncrementalServiceOptions>();

                return FillSecureOptions(result);
            }
            catch(Exception ex)
            {
                _log.Info($"IncrementalExtractionService.GetOptions:  something went wrong while populate start options. Exception stack trace: {ex.StackTrace}");
                return null;
            }
        }

        private IncrementalServiceOptions FillSecureOptions(IncrementalServiceOptions options)
        {
            // put your own secure implementation of how to get these information 
            options.Username = "";
            options.Password = "";
            options.DBConnectionString = "Server=(local);Integrated Security=SSPI;Database=DataExtractor";

            return options;
        }
        private async Task CreateAndProcessJobsAsync()
        {
            _log.Info($"IncrementalExtractionService.CreateAndProcessJobsAsync execute: {DateTime.Now}");

            //get the last job from DB, use the later time between job.EndTime and _startOptions.StartTime to be the cutoffStartTime
            var latestJob = await _jobAndTaskService.GetLatestJobAsync();

            // takes the later date from the two
            var cutoffStartTime = (latestJob != null && latestJob.EndTime > _startOptions.StartTime) ? latestJob.EndTime : _startOptions.StartTime;

            //don't get data that is less than 3 days old, so  we don't lose any data
            var cutOffEndTime = DateTime.Now.AddHours(-72);

            var newJobStartTime = cutoffStartTime;
            var newJobEndTime = newJobStartTime.AddHours(_startOptions.TimeIntervalInHours);

            while (newJobEndTime <= cutOffEndTime)
            {
                var success =
                    await _jobManager.CreateAndProcessJobsAsync(newJobStartTime, newJobEndTime);

                if (!success)
                {
                    _log.Info($"IncrementalExtractionService.CreateAndProcessJobsAsync: something went wrong while creating new job startTime-{newJobStartTime}, endTime-{newJobEndTime}, refer to JobManager log entry for more details.");
                    break;
                }

                newJobStartTime = newJobEndTime;
                newJobEndTime = newJobStartTime.AddHours(_startOptions.TimeIntervalInHours);
            }
        }
    }
}
