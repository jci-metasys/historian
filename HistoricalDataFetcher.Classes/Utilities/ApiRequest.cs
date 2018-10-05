using HistoricalDataFetcher.Classes.DataLayer.Cache;
using HistoricalDataFetcher.Classes.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
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

        private static bool _allowCertificateError;

        private static List<X509Certificate2> ValidX509Certificates
        {
            get; set;
        }

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
        public static async Task<bool> InitializeAsync(ICache cache, string username, string password, string httpHost, bool allowCertificateError)
        {
            _cache = cache;
            _username = username;
            _password = password;
            _httpHost = httpHost;
            _urlBase = $"https://{_httpHost}/api/v1";

            _allowCertificateError = allowCertificateError;

            if (!_allowCertificateError)
                PopulateValidX509Certificates();
            else
                ValidX509Certificates = new List<X509Certificate2>();

            return await AuthenticateAsync();
        }


        /// <summary>
        /// populate certificates from local certificate store
        /// </summary>
        private static void PopulateValidX509Certificates()
        {
            ValidX509Certificates = new List<X509Certificate2>();

            using (var localMachineStore = new X509Store(StoreLocation.LocalMachine))
            {
                localMachineStore.Open(OpenFlags.ReadOnly);

                foreach (var certificate in localMachineStore.Certificates)
                {
                    ValidX509Certificates.Add(certificate);
                }
            }
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
                ServerCertificateCustomValidationCallback = HandleCertificateError,
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

            var uri = new Uri($"{UrlBase}/login");

            using (var handler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = HandleCertificateError
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

        private static bool HandleCertificateError(HttpRequestMessage httpRequest, X509Certificate2 x509Certificate, X509Chain x509Chain, SslPolicyErrors sslPolicyErrors)
        {
            if (_allowCertificateError)
                return true;

            if (sslPolicyErrors == SslPolicyErrors.RemoteCertificateNameMismatch)
            {
                if (ValidX509Certificates.Any(cer => cer.Thumbprint == x509Certificate.Thumbprint))
                    return true;
            }
            return false;
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

            //Have the interval expire the token 30 seconds before the token is set to expire to ensure there is some overlap
            var interval = (_accessTokenInstance.Expires.AddSeconds(-30) - DateTime.Now).TotalMilliseconds;
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
