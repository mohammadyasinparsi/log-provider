using System;
using Microsoft.AspNetCore.Http;
using Serilog;
using Serilog.Configuration;

namespace Kernel.LogProvider.SerilogSinkEnrich.Enriches
{
    public static class ElkEnrichExtensions
    {
        public static LoggerConfiguration WithElkEnrich(this LoggerEnrichmentConfiguration enrich,
            IHttpContextAccessor httpContextAccessor)
        {
            if (enrich == null)
                throw new ArgumentNullException(nameof(LoggerEnrichmentConfiguration));

            return enrich.With(new[]
            {
                new WithElkEnrich(httpContextAccessor)
            });
        }
    }
}