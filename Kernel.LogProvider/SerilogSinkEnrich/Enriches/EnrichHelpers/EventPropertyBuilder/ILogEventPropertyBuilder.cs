using System.Collections.Generic;
using Serilog.Events;

namespace Kernel.LogProvider.SerilogSinkEnrich.Enriches.EnrichHelpers.EventPropertyBuilder
{
    public interface ILogEventPropertyBuilder
    {
        void BuildDiagnoseLog();
        void BuildLogEventPropertyDirectory();
        void Reset();
        IDictionary<string, LogEventProperty> GetLogEventPropertyDirectory();
    }
}