using System.Collections.Generic;
using Serilog.Core;
using Serilog.Events;

namespace Kernel.LogProvider.SerilogSinkEnrich.Enriches.EnrichHelpers.logEnrichModels
{
    /// <summary>
    /// These fields can represent errors of any kind.
    /// Use them for errors that happen while fetching events or in cases where the event itself contains an error.
    /// </summary>
    public class ErrorModel
    {
        private const string ObjectName = "Error";

        /// <summary>H
        /// Error message.
        /// type: text
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Error code describing the error.
        /// type: keyword
        /// </summary>
        public string ExceptionType { get; set; }

        /// <summary>
        /// Error message.
        /// type: text
        /// </summary>
        public string StackTrace { get; set; }

        /// <summary>
        /// Log name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Event Severity.
        /// This contains high-level information about the contents of the event. It is more generic than event.action,
        /// in the sense that typically a category contains multiple actions.
        /// Warning: In future versions of ECS, we plan to provide a list of acceptable values for this field, please use with caution.
        /// type: keyword
        /// example: user-management
        /// </summary>
        public string ActionSeverityTitle { get; set; }

        public void AddPropertiesToLogEventProperties(IDictionary<string, LogEventProperty> logEventProperties,
            ILogEventPropertyFactory propertyFactory)
        {
            logEventProperties[$"{ObjectName}.{nameof(ExceptionType)}"] =
                propertyFactory.CreateProperty($"{ObjectName}.{nameof(ExceptionType)}", ExceptionType);

            logEventProperties[$"{ObjectName}.{nameof(Message)}"] =
                propertyFactory.CreateProperty($"{ObjectName}.{nameof(Message)}", Message);

            logEventProperties[$"{ObjectName}.{nameof(StackTrace)}"] =
                propertyFactory.CreateProperty($"{ObjectName}.{nameof(StackTrace)}", StackTrace);

            if (!string.IsNullOrEmpty(Name))
                logEventProperties[$"{ObjectName}.{nameof(Name)}"] =
                    propertyFactory.CreateProperty($"{ObjectName}.{nameof(Name)}",
                        Name);

            if (!string.IsNullOrEmpty(ActionSeverityTitle))
                logEventProperties[$"{ObjectName}.{nameof(ActionSeverityTitle)}"] =
                    propertyFactory.CreateProperty($"{ObjectName}.{nameof(ActionSeverityTitle)}",
                        ActionSeverityTitle);
        }
    }
}