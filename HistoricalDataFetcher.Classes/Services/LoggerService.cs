using log4net;
using System;
using System.Text;

namespace HistoricalDataFetcher.Classes.Services
{
    public class LoggerService
    {
        /// <summary>
        /// Log Exceptions
        /// </summary>
        /// <param name="url">URL of the exception</param>
        /// <param name="timeToRunCall">Time elapsed for the call</param>
        /// <param name="ex">Exception</param>
        public static void LogException(string url, string timeToRunCall, Exception ex)
        {
            var logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            var sb = new StringBuilder();
            sb.AppendLine($"Failed while calling {url}");
            sb.AppendLine($"Time elapsed for endpoint call: {timeToRunCall}");

            logger.Error(sb.ToString(), ex);
        }

        /// <summary>
        /// Log API Request
        /// </summary>
        /// <param name="url">URL</param>
        /// <param name="numberRecords">Returned number of records</param>
        /// <param name="timeToRunCall">Time elapsed for call</param>
        /// <param name="additionalMessage">Additional messages to log</param>
        public static void LogApiRequest(string url, int numberRecords, string timeToRunCall, string additionalMessage = "")
        {
            var logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            var sb = new StringBuilder();
            sb.AppendLine("Call successful!");
            sb.AppendLine($"Url called: {url}");
            sb.AppendLine($"Number of records returned from url: {numberRecords}");
            sb.AppendLine($"Time elapsed: {timeToRunCall}");

            if (!string.IsNullOrEmpty(additionalMessage))
            {
                sb.AppendLine($"Additional info: {additionalMessage}");
            }

            logger.Info(sb.ToString());
        }
    }
}
