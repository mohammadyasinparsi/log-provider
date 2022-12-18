using System.Collections.Generic;
using Serilog.Core;
using Serilog.Events;

namespace Kernel.LogProvider.SerilogSinkEnrich.Enriches.EnrichHelpers.logEnrichModels
{
    /// <summary>
    /// The agent fields contain the data about the software entity, if any, that collects, detects,
    /// or observes events on a host, or takes measurements on a host.
    /// Examples include Beats. Agents may also run on observers.
    /// ECS agent.* fields shall be populated with details of the agent running on
    /// the host or observer where the event happened or the measurement was taken.
    /// </summary>
    public class ApplicationModel
    {
        private const string ObjectName = "Application";

        /// <summary>
        /// Custom name of the agent.
        /// This is a name that can be given to an agent. This can be helpful if for example two Filebeat instances
        /// are running on the same host but a human readable separation is needed on which Filebeat instance data is coming from.
        /// If no name is given, the name is often left empty.
        /// type: keyword
        /// example: foo
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Version of the agent.
        /// type: keyword
        /// example: 6.0.0-rc2
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// Name of Environment
        /// </summary>
        public string Environment { get; set; }

        public string IsHttps { get; set; }
        public string Protocol { get; set; }

        public void AddPropertiesToLogEventProperties(IDictionary<string, LogEventProperty> logEventProperties,
            ILogEventPropertyFactory propertyFactory)
        {
            logEventProperties[$"{ObjectName}.{nameof(Name)}"] =
                propertyFactory.CreateProperty($"{ObjectName}.{nameof(Name)}", Name);

            logEventProperties[$"{ObjectName}.{nameof(Environment)}"] =
                propertyFactory.CreateProperty($"{ObjectName}.{nameof(Environment)}", Environment);

            logEventProperties[$"{ObjectName}.{nameof(Version)}"] =
                propertyFactory.CreateProperty($"{ObjectName}.{nameof(Version)}", Version);

            logEventProperties[$"{ObjectName}.{nameof(IsHttps)}"] =
                propertyFactory.CreateProperty($"{ObjectName}.{nameof(IsHttps)}", IsHttps);

            logEventProperties[$"{ObjectName}.{nameof(Protocol)}"] =
                propertyFactory.CreateProperty($"{ObjectName}.{nameof(Protocol)}", Protocol);
        }
    }
}