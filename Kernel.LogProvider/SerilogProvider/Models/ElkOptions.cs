using System;
using Kernel.LogProvider.SerilogProvider.Enum;
using Kernel.Utilites.ValueCheck;

namespace Kernel.LogProvider.SerilogProvider.Models
{
    public class ElkOptions
    {
        public ElkOptions(Uri endpoint, LogCategories minimumLogLevel = LogCategories.Error,
            LogCategories minimumConsoleLevel = LogCategories.Information)
        {
            Precondition.NullCheck(endpoint, "Endpoint");
            Precondition.NullCheck(minimumLogLevel, "minimum Log Level");
            Precondition.NullCheck(minimumConsoleLevel, "minimum console log Level");
            Endpoint = endpoint;
            MinimumLogLevel = minimumLogLevel;
            MinimumConsoleLevel = minimumConsoleLevel;
        }

        public Uri Endpoint { get; }
        public LogCategories MinimumLogLevel { get; }
        public LogCategories MinimumConsoleLevel { get; }

        public string Username { get; set; }
        public string Password { get; set; }
    }
}