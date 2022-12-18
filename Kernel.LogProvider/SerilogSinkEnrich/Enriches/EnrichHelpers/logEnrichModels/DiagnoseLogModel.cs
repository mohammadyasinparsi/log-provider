using System.Collections.Generic;
using System.Text;
using Serilog.Core;
using Serilog.Events;

namespace Kernel.LogProvider.SerilogSinkEnrich.Enriches.EnrichHelpers.logEnrichModels
{
    public class DiagnoseLogModel
    {
        private const string PayloadsName = "Payloads";
        public string LogDateTime { get; set; }

        /// <summary>
        /// This field should be populated when the event’s timestamp does not include timezone information already (e.g. default Syslog timestamps).
        /// It’s optional otherwise.
        /// Acceptable timezone formats are: a canonical ID (e.g. "Europe/Amsterdam"), abbreviated (e.g. "EST") or an HH:mm differential (e.g. "-05:00").
        /// type: keyword
        /// </summary>
        public string Timezone { get; set; }

        public IDictionary<string, string> Payloads { get; set; }

        public ApplicationModel ApplicationModel { get; set; }

        public ClientModel Client { get; set; }

        public ErrorModel Errors { get; set; }

        public HttpModel Http { get; set; }

        public UrlModel Url { get; set; }

        public UserModel User { get; set; }

        public UserAgentModel UserAgent { get; set; }

        public void AddPropertiesToLogEventProperties(IDictionary<string, LogEventProperty> logEventProperties,
            ILogEventPropertyFactory propertyFactory)
        {
            if (!string.IsNullOrEmpty(Timezone))
                logEventProperties[$"{nameof(Timezone)}"] =
                    propertyFactory.CreateProperty($"{nameof(Timezone)}", Timezone);

            if (!string.IsNullOrEmpty(LogDateTime))
                logEventProperties[$"{nameof(LogDateTime)}"] =
                    propertyFactory.CreateProperty($"{nameof(LogDateTime)}", LogDateTime);

            if (Payloads != null)
            {
                StringBuilder stringBuilder = new StringBuilder();
                int index = 1;
                foreach (var item in Payloads)
                {
                    stringBuilder.AppendLine($"{index}- {item.Key} = {item.Value}");
                    index++;
                }

                logEventProperties[$"{PayloadsName}"] =
                    propertyFactory.CreateProperty($"{PayloadsName}",
                        stringBuilder.ToString());
            }

            ApplicationModel?.AddPropertiesToLogEventProperties(logEventProperties, propertyFactory);
            Client?.AddPropertiesToLogEventProperties(logEventProperties, propertyFactory);
            Errors?.AddPropertiesToLogEventProperties(logEventProperties, propertyFactory);
            Http?.AddPropertiesToLogEventProperties(logEventProperties, propertyFactory);
            Url?.AddPropertiesToLogEventProperties(logEventProperties, propertyFactory);
            User?.AddPropertiesToLogEventProperties(logEventProperties, propertyFactory);
            UserAgent?.AddPropertiesToLogEventProperties(logEventProperties, propertyFactory);
        }
    }
}