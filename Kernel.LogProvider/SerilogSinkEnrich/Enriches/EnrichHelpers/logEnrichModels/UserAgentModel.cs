using System.Collections.Generic;
using System.Text;
using Serilog.Core;
using Serilog.Events;

namespace Kernel.LogProvider.SerilogSinkEnrich.Enriches.EnrichHelpers.logEnrichModels
{
    public class UserAgentModel
    {
        private const string ObjectName = "UserAgent";

        /// <summary>
        /// This user agent is on mobile device.
        /// type: boolean
        /// example: false
        /// </summary>
        public bool? IsMobileDevice { get; set; }

        /// <summary>
        /// Name of the user agent.
        /// type: keyword
        /// example: Safari
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Unparsed version of the user_agent.
        /// type: keyword
        /// example: Mozilla/5.0 (iPhone; CPU iPhone OS 12_1 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) Version/12.0 Mobile/15E148 Safari/604.1
        /// </summary>
        public string Original { get; set; }

        /// <summary>
        /// Platform name of user agent.
        /// type: keyword
        /// example: Postman App
        /// </summary>
        public string Platform { get; set; }

        /// <summary>
        /// Type of the user agent.
        /// type: keyword
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Is user agent a crawler?
        /// type: boolean
        /// example: false
        /// </summary>
        public bool? IsCrawler { get; set; }

        /// <summary>
        /// Version of the user agent.
        /// type: keyword
        /// example: 12.0
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// Name of the device.
        /// type: keyword
        /// example: iPhone
        /// </summary>
        public string DeviceName { get; set; }

        /// <summary>
        /// Manufacturer of the device.
        /// type: keyword
        /// example: Apple
        /// </summary>
        public string DeviceManufacturer { get; set; }

        public void AddPropertiesToLogEventProperties(IDictionary<string, LogEventProperty> logEventProperties,
            ILogEventPropertyFactory propertyFactory)
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.AppendLine($"{nameof(IsMobileDevice)} = {IsMobileDevice}");
            stringBuilder.AppendLine(".....................................................................");
            stringBuilder.AppendLine($"{nameof(Name)} = {Name}");
            stringBuilder.AppendLine(".....................................................................");

            stringBuilder.AppendLine($"{nameof(Original)} = {Original}");
            stringBuilder.AppendLine(".....................................................................");

            stringBuilder.AppendLine($"{nameof(Platform)} = {Platform}");
            stringBuilder.AppendLine(".....................................................................");

            stringBuilder.AppendLine($"{nameof(Type)} = {Type}");
            stringBuilder.AppendLine(".....................................................................");

            stringBuilder.AppendLine($"{nameof(IsCrawler)} = {IsCrawler}");
            stringBuilder.AppendLine(".....................................................................");

            stringBuilder.AppendLine($"{nameof(Version)} = {Version}");
            stringBuilder.AppendLine(".....................................................................");

            stringBuilder.AppendLine($"{nameof(DeviceName)} = {DeviceName}");
            stringBuilder.AppendLine(".....................................................................");

            stringBuilder.AppendLine($"{nameof(DeviceManufacturer)} = {DeviceManufacturer}");

            logEventProperties[$"{ObjectName}"] =
                propertyFactory.CreateProperty($"{ObjectName}", stringBuilder.ToString());
        }
    }
}