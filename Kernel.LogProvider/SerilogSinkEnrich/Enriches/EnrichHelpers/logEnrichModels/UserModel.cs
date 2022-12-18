using System.Collections.Generic;
using System.Text;
using Serilog.Core;
using Serilog.Events;

namespace Kernel.LogProvider.SerilogSinkEnrich.Enriches.EnrichHelpers.logEnrichModels
{
    /// <summary>
    /// The user fields describe information about the user that is relevant to the event.
    /// Fields can have one entry or multiple entries. If a user has more than one id, provide an array that includes all of them.
    /// </summary>
    public class UserModel
    {
        private const string ObjectName = "User";

        /// <summary>
        /// User email address.
        /// type: keyword
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// User Claims
        /// </summary>
        public IDictionary<string, string> Claims { get; set; }

        /// <summary>
        /// One or multiple unique identifiers of the user.
        /// type: keyword
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// Short name or login of the user.
        /// type: keyword
        /// example: Spongebob
        /// </summary>
        public string Name { get; set; }

        public void AddPropertiesToLogEventProperties(IDictionary<string, LogEventProperty> logEventProperties,
            ILogEventPropertyFactory propertyFactory)
        {
            logEventProperties[$"{ObjectName}.{nameof(Email)}"] =
                propertyFactory.CreateProperty($"{ObjectName}.{nameof(Email)}", Email);

            logEventProperties[$"{ObjectName}.{nameof(UserId)}"] =
                propertyFactory.CreateProperty($"{ObjectName}.{nameof(UserId)}", UserId);

            logEventProperties[$"{ObjectName}.{nameof(Name)}"] =
                propertyFactory.CreateProperty($"{ObjectName}.{nameof(Name)}", Name);

            if (Claims == null || Claims.Count <= 0) return;
            StringBuilder stringBuilder = new StringBuilder();
            int index = 1;
            foreach (var item in Claims)
            {
                stringBuilder.AppendLine($"{index}- {item.Key} = {item.Value}");
                index++;
            }

            logEventProperties[$"{ObjectName}.{nameof(Claims)}"] =
                propertyFactory.CreateProperty($"{ObjectName}.{nameof(Claims)}", stringBuilder.ToString());
        }
    }
}