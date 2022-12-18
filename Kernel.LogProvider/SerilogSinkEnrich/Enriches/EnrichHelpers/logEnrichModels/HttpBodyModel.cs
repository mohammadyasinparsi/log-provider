using System.Collections.Generic;
using Serilog.Core;
using Serilog.Events;

namespace Kernel.LogProvider.SerilogSinkEnrich.Enriches.EnrichHelpers.logEnrichModels
{
    public class HttpBodyModel
    {
        private const string ObjectName = "Request.Body";

        /// <summary>
        /// Content length of data posted by request (in bytes)
        /// type: long
        /// example: 1024
        /// </summary>
        public long? ContentLength { get; set; }

        /// <summary>
        /// The full HTTP request body.
        /// type: keyword
        /// example: Hello world
        /// </summary>
        public string Content { get; set; }

        public void AddPropertiesToLogEventProperties(IDictionary<string, LogEventProperty> logEventProperties,
            ILogEventPropertyFactory propertyFactory)
        {
            logEventProperties[$"{ObjectName}.{nameof(ContentLength)}"] =
                propertyFactory.CreateProperty($"{ObjectName}.{nameof(ContentLength)}", ContentLength?.ToString());
            logEventProperties[$"{ObjectName}.{nameof(Content)}"] =
                propertyFactory.CreateProperty($"{ObjectName}.{nameof(Content)}", Content);
        }
    }
}