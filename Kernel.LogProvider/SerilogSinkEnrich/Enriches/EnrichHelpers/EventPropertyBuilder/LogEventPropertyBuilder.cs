using System.Collections.Generic;
using Kernel.LogProvider.SerilogSinkEnrich.Enriches.EnrichHelpers.ElkEnrichBuilders;
using Kernel.LogProvider.SerilogSinkEnrich.Enriches.EnrichHelpers.logEnrichModels;
using Microsoft.AspNetCore.Http;
using Serilog.Core;
using Serilog.Events;

namespace Kernel.LogProvider.SerilogSinkEnrich.Enriches.EnrichHelpers.EventPropertyBuilder
{
    public class LogEventPropertyBuilder : ILogEventPropertyBuilder
    {
        private readonly ILogEventPropertyFactory _propertyFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly LogEvent _logEvent;
        private DiagnoseLogModel _diagnoseLogModel;
        private IDictionary<string, LogEventProperty> _logEventPropertyDictionary;

        public LogEventPropertyBuilder(LogEvent logEvent, ILogEventPropertyFactory propertyFactory,
            IHttpContextAccessor httpContextAccessor)
        {
            _logEvent = logEvent;
            _propertyFactory = propertyFactory;
            _httpContextAccessor = httpContextAccessor;
            Reset();
        }

        public void BuildDiagnoseLog()
        {
            var graylogEnrichBuilder = new ElkEnrichBuilder(_logEvent, _httpContextAccessor);
            var graylogEnrichDirector = new ElkEnrichDirector();
            graylogEnrichDirector.Construct(graylogEnrichBuilder);
            _diagnoseLogModel = graylogEnrichBuilder.GetModel();
        }

        public void BuildLogEventPropertyDirectory()
        {
            _diagnoseLogModel.AddPropertiesToLogEventProperties(_logEventPropertyDictionary, _propertyFactory);
        }

        public IDictionary<string, LogEventProperty> GetLogEventPropertyDirectory()
        {
            var resultDirectory = _logEventPropertyDictionary;
            Reset();
            return resultDirectory;
        }

        public void Reset()
        {
            _logEventPropertyDictionary = new Dictionary<string, LogEventProperty>();
            _diagnoseLogModel = new DiagnoseLogModel();
        }
    }
}