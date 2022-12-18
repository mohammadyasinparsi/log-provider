using System.Collections.Generic;
using Serilog.Core;
using Serilog.Events;

namespace Kernel.LogProvider.SerilogSinkEnrich.Enriches.EnrichHelpers.logEnrichModels
{
    public class HttpResponseModel
    {
        private const string ObjectName = "Response";

        /// <summary>
        /// HTTP response status code.
        /// type: long
        /// example: 404
        /// </summary>
        public long? StatusCode { get; set; }

        public void AddPropertiesToLogEventProperties(IDictionary<string, LogEventProperty> logEventProperties,
            ILogEventPropertyFactory propertyFactory)
        {
            logEventProperties[$"{ObjectName}.{nameof(StatusCode)}"] =
                propertyFactory.CreateProperty($"{ObjectName}.{nameof(StatusCode)}", StatusCode?.ToString());
        }
    }
}