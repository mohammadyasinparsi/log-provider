using System.Collections.Generic;
using Serilog.Core;
using Serilog.Events;

namespace Kernel.LogProvider.SerilogSinkEnrich.Enriches.EnrichHelpers.logEnrichModels
{
    /// <summary>
    /// Fields related to HTTP activity. Use the url field set to store the url of the request.
    /// </summary>
    public class HttpModel
    {
        public HttpModel()
        {
            Request = new HttpRequestModel();
            Response = new HttpResponseModel();
        }

        public HttpRequestModel Request { get; set; }

        public HttpResponseModel Response { get; set; }

        // public string Version { get; set; }

        public void AddPropertiesToLogEventProperties(IDictionary<string, LogEventProperty> logEventProperties,
            ILogEventPropertyFactory propertyFactory)
        {
            Request?.AddPropertiesToLogEventProperties(logEventProperties, propertyFactory);
            Response?.AddPropertiesToLogEventProperties(logEventProperties, propertyFactory);
        }
    }
}