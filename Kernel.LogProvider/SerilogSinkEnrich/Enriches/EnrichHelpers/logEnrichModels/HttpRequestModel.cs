using System.Collections.Generic;
using System.Text;
using Serilog.Core;
using Serilog.Events;

namespace Kernel.LogProvider.SerilogSinkEnrich.Enriches.EnrichHelpers.logEnrichModels
{
    public class HttpRequestModel
    {
        private const string ObjectName = "Request";

        /// <summary>
        /// HTTP request method.
        /// The field value must be normalized to lowercase for querying. See the documentation section "Implementing ECS".
        /// type: keyword
        /// example: get, post, put
        /// </summary>
        public string Method { get; set; }

        /// <summary>
        /// Content encoding of request
        /// type: keyword
        /// example: UTF-8
        /// </summary>
        public string ContentEncoding { get; set; }

        /// <summary>
        /// User of this request is authenticated?
        /// type: boolean
        /// example: false
        /// </summary>
        public bool? IsAuthenticated { get; set; }

        /// <summary>
        /// Content type of request
        /// type: keyword
        /// example: application/json, application/oct-stream
        /// </summary>
        public string ContentType { get; set; }

        /// <summary>
        /// Headers list of request in key=value format
        /// type: object
        /// </summary>
        public IDictionary<string, string> Headers { get; set; }

        /// <summary>
        /// Cookies list of request in key=value format
        /// type: object
        /// </summary>
        public IDictionary<string, string> Cookies { get; set; }

        /// <summary>
        /// Keys list of files uploaded by request
        /// type: object
        /// </summary>
        public List<string> Files { get; set; }

        /// <summary>
        /// Form posted data of request in key=value format
        /// type: object
        /// </summary>
        public IDictionary<string, string> Form { get; set; }

        public HttpBodyModel Body { get; set; }

        public void AddPropertiesToLogEventProperties(IDictionary<string, LogEventProperty> logEventProperties,
            ILogEventPropertyFactory propertyFactory)
        {
            Body?.AddPropertiesToLogEventProperties(logEventProperties, propertyFactory);
            logEventProperties[$"{ObjectName}.{nameof(Method)}"] =
                propertyFactory.CreateProperty($"{ObjectName}.{nameof(Method)}", Method);

            logEventProperties[$"{ObjectName}.{nameof(ContentEncoding)}"] =
                propertyFactory.CreateProperty($"{ObjectName}.{nameof(ContentEncoding)}", ContentEncoding);

            logEventProperties[$"{ObjectName}.{nameof(ContentType)}"] =
                propertyFactory.CreateProperty($"{ObjectName}.{nameof(ContentType)}", ContentType);

            logEventProperties[$"{ObjectName}.{nameof(IsAuthenticated)}"] =
                propertyFactory.CreateProperty($"{ObjectName}.{nameof(IsAuthenticated)}", IsAuthenticated);

            if (Headers != null)
            {
                StringBuilder str = new StringBuilder();
                int index = 1;
                foreach (var item in Headers)
                {
                    str.AppendLine($"{index}- {item.Key} = {item.Value}");
                    index++;
                }

                logEventProperties[$"{ObjectName}.{nameof(Headers)}"] =
                    propertyFactory.CreateProperty($"{ObjectName}.{nameof(Headers)}", str.ToString());
            }

            if (Cookies != null)
            {
                StringBuilder str = new StringBuilder();
                int index = 1;
                foreach (var item in Cookies)
                {
                    str.AppendLine($"{index}- {item.Key} = {item.Value}");
                    index++;
                }

                logEventProperties[$"{ObjectName}.{nameof(Cookies)}"] =
                    propertyFactory.CreateProperty($"{ObjectName}.{nameof(Cookies)}", str.ToString());
            }

            if (Files != null)
            {
                StringBuilder str = new StringBuilder();
                for (var i = 0; i < Files.Count; i++)
                {
                    str.AppendLine($"File-{i} = {Files[i]}");
                }

                logEventProperties[$"{ObjectName}.{nameof(Files)}"] =
                    propertyFactory.CreateProperty($"{ObjectName}.{nameof(Files)}", str.ToString());
            }

            if (Form != null)
            {
                StringBuilder str = new StringBuilder();
                int index = 1;
                foreach (var item in Form)
                {
                    str.AppendLine($"{index}- {item.Key} = {item.Value}");
                    index++;
                }

                logEventProperties[$"{ObjectName}.{nameof(Form)}"] =
                    propertyFactory.CreateProperty($"{ObjectName}.{nameof(Form)}", str.ToString());
            }
        }
    }
}