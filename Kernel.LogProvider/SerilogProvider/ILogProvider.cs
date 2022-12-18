using System;
using System.Collections.Generic;

namespace Kernel.LogProvider.SerilogProvider
{
    public interface ILogProvider
    {
        void LogInformation(string name = "", Exception exception = null, IDictionary<string, string> payload = null);
        void LogWarning(string name = "", Exception exception = null, IDictionary<string, string> payload = null);

        void LogError(string name = "", Exception exception = null, IDictionary<string, string> payload = null);

        void LogFetal(string name = "", Exception exception = null, IDictionary<string, string> payload = null);
    }
}