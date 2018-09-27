using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;

namespace HistoricalDataFetcher.Classes.Models
{
    public class AccessToken
    {
        /// <summary>
        /// Access Token string
        /// </summary>
        [JsonProperty(PropertyName = "access_token")]
        public string Access_Token { get; set; }
        /// <summary>
        /// Date the Acces Token expires
        /// </summary>
        [JsonProperty(PropertyName = ".expires")]
        [JsonConverter(typeof(IsoDateTimeConverter))]
        public DateTime Expires { get; set; }
        /// <summary>
        /// Access Token Type
        /// </summary>
        [JsonProperty(PropertyName = "token_type")]
        public string Token_Type { get; set; }
    }
}
