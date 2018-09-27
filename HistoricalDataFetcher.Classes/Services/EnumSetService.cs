using HistoricalDataFetcher.Classes.Endpoints;
using HistoricalDataFetcher.Classes.EnumSet;
using HistoricalDataFetcher.DataStorage.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HistoricalDataFetcher.Classes.Services
{
    public class EnumSetService
    {
        private static EnumSetService _instance;
        private static readonly string _apiEndpoint = "/v1/enumSets";
        private Dictionary<string, EnumDescDataStoreModel> _enumMembers;
        private Dictionary<string, string> _enumSets;
        private static EnumDescCollection _enumDesc;
        private EnumSetEndPoint _enumSetEndpoint;
        public IEnumDescRepository DataStore { get; set; } = new EnumDescSaveToCsv();
        public string EnumFile { get; set; } = "EnumList.csv";


        /// <summary>
        /// Get a single instance of EnumSetService
        /// </summary>
        public static EnumSetService Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new EnumSetService();
                return _instance;
            }
        }

        private EnumSetService()
        {
            _enumSetEndpoint = new EnumSetEndPoint();
            _enumSets = new Dictionary<string, string>();
            _enumMembers = new Dictionary<string, EnumDescDataStoreModel>();
        }

        /// <summary>
        /// Loads the EnumSet Descriptions from the Datastore into Memory for use.
        /// </summary>
        public async Task RunLoadAsync()
        {
            if (DataStore is EnumDescSaveToCsv)
                ((EnumDescSaveToCsv)DataStore).FileName = EnumFile;
            _enumSetEndpoint.DataStore = DataStore;
            //Check EnumList.csv file
            //If no file make API call
            if (!await DataStore.DataExistsAsync())
            {
                //build EnumDesc Collection
                await BuildEnumDescCollectionAsync();
            }
            _enumDesc = await DataStore.GetDataEnumDescSetAsync();

            foreach (EnumDescDataStoreModel enumDesc in _enumDesc)
            {
                if (!_enumSets.Any(x => x.Key.Equals(enumDesc.SetId.ToString())))
                    _enumSets.Add(enumDesc.SetId.ToString(), enumDesc.SetDesc);
                _enumMembers.Add(enumDesc.SetId.ToString() + ":" + enumDesc.MemberId.ToString(), enumDesc);
            }
        }

        /// <summary>
        /// Used to setup the initial load of EnumSet Descriptions from a csv file and will store into the Database
        /// </summary>
        /// <param name="filePath">
        /// The Filename used to load the csv file from
        /// </param>
        public async Task SetupLoadFileToDBAsync(string filePath)
        {
            //Grab the Setup Enum File 
            var csv = new EnumDescSaveToCsv {
                    FileName = filePath
                };

            _enumSetEndpoint.DataStore = new EnumDescSaveToSqlServer();
            bool fileExists = false;
            //Load from the file if it Exists
            if (await csv.DataExistsAsync())
            {
                fileExists = true;
                await _enumSetEndpoint.DataStore.SaveDataEnumDescSetAsync(await csv.GetDataEnumDescSetAsync());
            }
            else
                await BuildEnumDescCollectionAsync(); //Build the List from the API and save it

            //Rename the file
            if (fileExists)
            {
                //Commented out Temporarily, will uncomment with final build 

                //File.Delete(filePath);
                //string[] pathParts = filePath.Split('\\');
                //string newFilePath = filePath.Replace(pathParts[pathParts.Length - 1], $"Complete-{pathParts[pathParts.Length - 1]}");
                //System.IO.File.Move(filePath, newFilePath);
            }                
        }

        /// <summary>
        /// Get the EnumSet description
        /// </summary>
        /// <param name="setId">Set Id</param>
        /// <returns>The given set Description</returns>
        public string GetEnumSetDescription(string setId)
        {
            var description = string.Empty;
            if (!_enumSets.TryGetValue(setId, out description))
            {
                description = "Enum Set description does not exist";
            }

            return description;
        }

        /// <summary>
        /// Returns the Member Description
        /// </summary>
        /// <param name="setId">Set Id</param>
        /// <param name="memberId">Member Id</param>
        /// <returns>Member Description</returns>
        public string GetEnumMemberDescription(string setId, string memberId)
        {
            //Check _enumList for description
            EnumDescDataStoreModel dataModel;
            //If description exists return description
            if (_enumMembers.TryGetValue(setId + ":" + memberId, out dataModel))
                return dataModel.MemberDesc;

            //If not Exists return CheckEnumMember(setId, memberId)
            return "Enum Member does not Exist";// CheckEnumMember(setId, memberId);
        }

        /// <summary>
        /// Pass in the Enum Set Link to get the Enum Member Description 
        /// </summary>
        /// <param name="enumSetLink">EnumSetLink  "enumSets/{id}/members/{membersId}"</param>
        /// <returns></returns>
        public string GetEnumMemberDescription(string enumSetLink)
        {
            var splitString = enumSetLink.Split('/').ToList();
            var enumSetIdIndex = splitString.IndexOf("enumSets") + 1;
            var memeberIdIndex = splitString.IndexOf("members") + 1;

            return GetEnumMemberDescription(splitString[enumSetIdIndex], splitString[memeberIdIndex]);
        }

        /// <summary>
        /// Build the EnumDescCollection using the API Endpoint and saves the data to the DataStore
        /// </summary>
        private async Task BuildEnumDescCollectionAsync()
        {
            await _enumSetEndpoint.RunAsync(_apiEndpoint);
            await _enumSetEndpoint.SaveDataAsync(); 
        }
    }
}
