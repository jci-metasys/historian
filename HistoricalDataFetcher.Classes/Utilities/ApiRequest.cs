using HistoricalDataFetcher.Classes.DataLayer.Cache;
using HistoricalDataFetcher.Classes.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace HistoricalDataFetcher.Classes.Utilities
{
    public static class ApiRequest
    {
        private static string _username;
        private static string _password;

        private static string _httpHost;
        private static string _urlBase;
        private static ICache _cache;
        private static Timer _timer;
        private static AccessToken _accessTokenInstance;

        public static string HttpHost
        {
            get
            {
                return _httpHost;
            }
        }

        public static string UrlBase
        {
            get
            {
                return _urlBase;
            }
        }

        /// <summary>
        /// Sets the UrlBase and logs in to test the connection
        /// </summary>
        /// <param name="cache">Cache to use</param>
        /// <param name="username">Username of the Metasys server</param>
        /// <param name="password">Password of the Metasys server</param>
        /// <param name="httpHost">Host of the Metasys server</param>
        /// <returns></returns>
        public static async Task<bool> InitializeAsync(ICache cache, string username, string password, string httpHost)
        {
            _cache = cache;
            _username = username;
            _password = password;
            _httpHost = httpHost;
            _urlBase = $"https://{_httpHost}/api";

            return await AuthenticateAsync();
        }

        /// <summary>
        /// Authenticates and runs the URL with the acceptHeader
        /// </summary>
        /// <param name="url">Complete URL</param>
        /// <param name="acceptHeader">header used to make the call</param>
        /// <returns>Endpoint Result</returns>
        public static async Task<string> RunEndpointCallAsync(string url, string acceptHeader)
        {
            if (_accessTokenInstance == null)
            {
                await AuthenticateAsync();
            }

            var result = _cache.Get(url);
            return !string.IsNullOrEmpty(result) ? result : await RunEndpointHelperAsync(url, acceptHeader);
        }

        /// <summary>
        /// Runs the URL using the required header info
        /// </summary>
        /// <param name="url">Complete URL</param>
        /// <param name="acceptHeader">header used to make the call</param>
        /// <returns>EndPoint result</returns>
        private static async Task<string> RunEndpointHelperAsync(string url, string acceptHeader)
        {
            string rawJsonToReturn = string.Empty;
            var uri = new Uri(url);

            using (var handler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true,
                AllowAutoRedirect = false
            })
            using (var httpClient = new HttpClient(handler))
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessTokenInstance.Access_Token);
                var response = await httpClient.GetAsync(uri);
                
                if (response.StatusCode == HttpStatusCode.Moved || response.StatusCode == HttpStatusCode.Redirect)
                {
                    return await RunEndpointHelperAsync(response.Headers.Location.ToString(), acceptHeader);
                }
                else
                {
                    rawJsonToReturn = await response.Content.ReadAsStringAsync();
                }
            }

            _cache.Add(url, rawJsonToReturn);
            _cache.Persist();

            return rawJsonToReturn ?? string.Empty;
        }

        /// <summary>
        /// Authenticate access using the host location
        /// </summary>
        /// <returns>bool: True = success</returns>
        private static async Task<bool> AuthenticateAsync()
        {
            var body = $"{{\"username\": \"{_username}\",\"password\": \"{_password}\"}}";

            var uri = new Uri($"{UrlBase}/api/authentication/login");
    
            using (var handler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true
            })
            using (var httpClient = new HttpClient(handler))
            {
                httpClient.DefaultRequestHeaders.Accept.Clear();

                using (var content = new StringContent(body, Encoding.UTF8, "application/json"))
                using (var response = await httpClient.PostAsync(uri, content))
                {
                    return SetAuthenticationToken(JsonConvert.DeserializeObject<AccessToken>(await response.Content.ReadAsStringAsync()));
                }
            }
        }

        /// <summary>
        /// Set the authentication token for subsequent calls 
        /// </summary>
        /// <param name="token">Access token</param>
        /// <returns>bool: True = success</returns>
        private static bool SetAuthenticationToken(AccessToken token)
        {
            if (token?.Access_Token == null)
            {
                return false;
            }

            _accessTokenInstance = token;

            //Have the interval expire the token 5 minutes before the token is set to expire to ensure there is some overlap
            var interval = (_accessTokenInstance.Expires.AddMinutes(-5) - DateTime.Now).TotalMilliseconds;
            _timer = new Timer(interval);

            // Hook up the Elapsed event for the timer.
            _timer.Elapsed += OnTimedEvent;
            _timer.Enabled = true;

            return true;
        }

        private static void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            _accessTokenInstance = null;
            _timer.Enabled = false;
        }
    }
}
