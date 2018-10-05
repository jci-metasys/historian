using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using HistoricalDataFetcher.Classes.Utilities;
using HistoricalDataFetcher.Classes.Utilities.TaskCreators;
using HistoricalDataFetcher.Classes.DataLayer.Cache;
using HistoricalDataFetcher.Classes.DataLayer.Fqrs;
using HistoricalDataFetcher.Classes.Endpoints;
using HistoricalDataFetcher.Classes.Enums;
using HistoricalDataFetcher.Classes.EnumSet;
using HistoricalDataFetcher.Classes.Models;
using HistoricalDataFetcher.Classes.Services;
using HistoricalDataFetcher.DataStorage.Interfaces;
using HistoricalDataFetcher.DataStorage.Models;
using HistoricalDataFetcher.DataStorage.TimeSeries;
using log4net;
using HistoricalDataFetcher.DataStorage.Alarms;

namespace HistoricalDataFetcher.Classes.Controller
{
    public partial class Controller
    {
        //private List<IBaseEndPoint> _endpoints;
        private IDataStore<SamplesDataStoreModel> _timeSeriesDataStore;
        private IDataStore<AuditDataStoreModel> _auditDataStore;
        private IDataStore<AlarmDataStoreModel> _alarmDataStore;
        //private IDataStore<FqrGuidDataModel> _fqrGuidDataStore;
        private ICache _cache;
        private IEnumerable<FqrModel> _fqrList;
        public string SetupEnumFileName { get; set; } = "SetupEnumList.csv";
        private ILog _log;


        public Controller()
        {
            _log = LogManager.GetLogger(GetType());
        }

        /// <summary>
        /// Starts the process to Cache the EnumSets from the datastore. 
        /// </summary>
        /// <returns></returns>
        public async Task<bool> InitStartEnumDownloadAsync()
        {
            try
            {
                await EnumSetService.Instance.RunLoadAsync();
            }
            catch (Exception ex)
            {
                _log.Error($"Controller.InitStartEnumDownloadAsync: failed to load EnumSet. Stack trace: {ex.StackTrace}");
                return false;
            }
            return true;
        }

        /// <summary>
        /// log into database using provided connection string to make sure the connection string is accurate
        /// </summary>
        /// <param name="startOptions">Parsed StartOptions</param>
        /// <returns>bool: True = success</returns>
        public async Task<bool> SetDBSettingsAsync(StartOptions.StartOptions startOptions)
        {
            //Set the DB Connection String
            var isDBLoggedIn = await DBDataModel.CheckDBConnectionAsync(startOptions.DBConnectionString);

            //Set the Save Destination
            if (startOptions.Destination == null || "csv".Equals(startOptions.Destination.ToLower()))
            {
                SetDataSaveDestination(DestinationSaveEnum.Csv);
            }
            else if ("sqlserver".Equals(startOptions.Destination.ToLower()))
                SetDataSaveDestination(DestinationSaveEnum.SqlServer);

            if (!isDBLoggedIn)
            {
                _log.Error("The Database connection is not correct.");
                return false;
            }

            return true;
        }

        /// <summary>
        /// try to get an access token from authentication service using provided information in startOptions
        /// </summary>
        /// <param name="startOptions">options</param>
        /// <returns>bool: True = success</returns>
        public async Task<bool> InitApiRequestAsync(StartOptions.StartOptions startOptions)
        {
            //Initialize cache and ApiRequest class
            _cache = new CachedMemory(CachedMemory.CACHE_SIZE_NO_LIMIT, @".\data-extractor-cache.txt");

            var isLoggedIn = await ApiRequest.InitializeAsync(_cache, startOptions.Username, startOptions.Password, startOptions.Host, startOptions.InvalidCertificate);

            if (isLoggedIn)
            {
                return true;
            }
            else
            {
                _log.Error(@"Could not log in:
                The Host, Username, and Password combination did not work, please try again!");

                return false;
            }
        }

        /// <summary>
        /// Sets the Endpoints DataStore for each service
        /// </summary>
        /// <param name="value">
        /// DestinationSaveEnum to determine the DataStore to save to.
        /// </param>
        private void SetDataSaveDestination(DestinationSaveEnum value)
        {
            switch (value)
            {
                case DestinationSaveEnum.Csv:
                    _timeSeriesDataStore = new TimeSeriesSaveToCsv();
                    _alarmDataStore = new AlarmSaveToCsv();
                    EnumSetService.Instance.DataStore = new EnumDescSaveToSqlServer();
                    break;
                case DestinationSaveEnum.SqlServer:
                    _timeSeriesDataStore = new TimeSeriesSaveToSqlServer();
                    _alarmDataStore = new AlarmSaveToSqlServer();
                    //_fqrGuidDataStore = new FqrGuidSaveToSqlServer();
                    EnumSetService.Instance.DataStore = new EnumDescSaveToSqlServer();
                    break;
                case DestinationSaveEnum.Custom:
                    //TODO: Add code for the custom data store here
                    break;
            }
        }

        /// <summary>
        /// Read FQR's from a file, Identifies the objects, saves the data to a Database
        /// </summary>
        /// <param name="fqrPath">File name to the FQR list</param>
        /// <returns></returns>
        public async Task ReadFqrsFromFileAsync(string fqrPath)
        {
            var fqrList = new List<string>();

            var exists = File.Exists(fqrPath);
            if (exists)
            {
                using (var reader = new StreamReader(fqrPath))
                {
                    while (!reader.EndOfStream)
                    {
                        fqrList.Add(reader.ReadLine());
                    }
                }
            }

            //Remove any duplicate FQRs in the list
            fqrList = fqrList.GroupBy(x => x).Select(x => x.First()).ToList();

            _fqrList = await new NetworkDiscoveryService().GetFqrGuidListAsync(fqrList);

            await SaveFqrGuidsToDatabaseAsync();
        }

        /// <summary>
        /// set up endpoint supports for the options
        /// </summary>
        /// <param name="startOptions">Parsed StartOptions</param>
        public void SetEndPoints(StartOptions.StartOptions startOptions)
        {
            startOptions.Service = string.IsNullOrEmpty(startOptions.Service) ? "time" : startOptions.Service;
            if (startOptions.Service != string.Empty)
            {
                string[] service = startOptions.Service.Split(',');
                List<TaskTypeEnum> tasksToRun = new List<TaskTypeEnum>();
                string[] time = new string[] { "time", "timeseries" };
                string[] alarm = new string[] { "alarm", "alarms" };
                string[] aud = new string[] { "audit", "audits" };

                foreach (string s in service)
                {
                    if (time.Contains(s.ToLower()))
                        tasksToRun.Add(TaskTypeEnum.TimeSeries);
                    else if (aud.Contains(s.ToLower()))
                        tasksToRun.Add(TaskTypeEnum.Audit);
                    else if (alarm.Contains(s.ToLower()))
                        tasksToRun.Add(TaskTypeEnum.Alarm);
                }
                SetTaskUtilityFactory(tasksToRun);
            }
        }

        /// <summary>
        /// Endpoint Services to create tasks against
        /// </summary>
        /// <param name="tasksToSupport">List of Services to run as tasks</param>
        private void SetTaskUtilityFactory(IEnumerable<TaskTypeEnum> tasksToSupport)
        {
            foreach (var task in tasksToSupport)
            {
                switch (task)
                {
                    case TaskTypeEnum.TimeSeries:
                        TaskUtilityFactory.AddTaskUtility(new TaskUtility(task, new TimeSeriesEndPoint(_timeSeriesDataStore),new TimeSeriesTaskCreator()));
                        break;
                    case TaskTypeEnum.Alarm:
                        TaskUtilityFactory.AddTaskUtility(new TaskUtility(task, new AlarmEndPoint(_alarmDataStore), new AlarmTaskCreator()));
                        break;
                    case TaskTypeEnum.Audit:
                        TaskUtilityFactory.AddTaskUtility(new TaskUtility(task, new ActivityEndPoint(_auditDataStore), new ActivityTaskCreator()));
                        break;
                }
            }
        }

        /// <summary>
        /// Create a job based on the identified time frame and process the job immediatly
        /// </summary>
        /// <param name="consoleOptions">Parsed ConsoleOptions</param>
        /// <returns>bool: True = success</returns>
        public async Task<bool> CreateAndProcessJobAsync(StartOptions.ConsoleOptions consoleOptions)
        {
            var startDate = DateTime.Now;
            var endDate = DateTime.Now.AddDays(-3);
            if(consoleOptions.Min > 0)
            {
                startDate = startDate.AddMinutes(consoleOptions.Min * -1);
            }
            if(consoleOptions.Hrs > 0)
            {
                startDate = startDate.AddHours(consoleOptions.Hrs * -1);
            }
            if(consoleOptions.Days > 0)
            {
                startDate = startDate.AddDays(consoleOptions.Days * -1);
            }
            if(consoleOptions.Months > 0)
            {
                startDate = startDate.AddMonths(consoleOptions.Months * -1);
            }
            if(consoleOptions.Min + consoleOptions.Hrs + consoleOptions.Days + consoleOptions.Months == 0)
            {
                startDate = startDate.AddDays(-7);
            }
            if(startDate >= endDate)
            {
                startDate = endDate.AddDays(-4);
            }

            return await new JobManager().CreateAndProcessJobsAsync(startDate, endDate);
        }

        /// <summary>
        /// Used to setup the Database for the user.  Setup for SqlServer Only!
        /// </summary>
        public async Task RunDiscoveryEnumSetAsync()
        {
            //Get the Application Base Directory
            var cwd = new DirectoryInfo(Directory.GetCurrentDirectory());//.Parent.Parent.Parent.Parent;
            //Load EnumSets into the DB from CSV or API
            await EnumSetService.Instance.SetupLoadFileToDBAsync($"{cwd.FullName}\\Input Files\\{SetupEnumFileName}");            
        }

        /// <summary>
        /// Saves the FQR list and point info to the Database
        /// </summary>
        /// <returns></returns>
        private async Task SaveFqrGuidsToDatabaseAsync()
        {
            var fqrGuidDataModelList = _fqrList.Select(x => new FqrGuidDataModel
            {
                Fqr = x.Fqr,
                Guid = x.Guid,
                PointName = x.PointName,
                PointType = x.PointType
            }).ToList();
            
            await new FqrRepository().SetDataAsync(fqrGuidDataModelList);
        }
    }
}
