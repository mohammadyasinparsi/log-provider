using System;
using System.Linq;
using Kernel.LogProvider.SerilogSinkEnrich.Enriches.EnrichHelpers.EventPropertyBuilder;
using Microsoft.AspNetCore.Http;
using Serilog.Core;
using Serilog.Events;

namespace Kernel.LogProvider.SerilogSinkEnrich.Enriches
{
    public class WithElkEnrich : ILogEventEnricher
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public WithElkEnrich(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            if (logEvent == null) throw new ArgumentNullException(nameof(logEvent));

            var logEventPropertyBuilder = new LogEventPropertyBuilder(logEvent, propertyFactory, _httpContextAccessor);
            var director = new LogEventPropertyDirectoryDirector();
            director.Construct(logEventPropertyBuilder);
            var logEventPropertyDirectory = logEventPropertyBuilder.GetLogEventPropertyDirectory();

            var properties = logEvent.Properties.Select(x => x.Key).ToList();
            foreach (var property in properties)
                logEvent.RemovePropertyIfPresent(property);

            foreach (var property in logEventPropertyDirectory
                .OrderBy(x => x.Key))
                logEvent.AddPropertyIfAbsent(property.Value);
        }
    }
}