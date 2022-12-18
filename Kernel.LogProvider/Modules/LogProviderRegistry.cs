using System;
using Elasticsearch.Net;
using Kernel.LogProvider.Helpers;
using Kernel.LogProvider.SerilogProvider.Enum;
using Kernel.LogProvider.SerilogProvider.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Kernel.Utilites.ValueCheck;
using Kernel.LogProvider.SerilogSinkEnrich.Enriches;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.Elasticsearch;

namespace Kernel.LogProvider.Modules
{
    public static class LogProviderRegistry
    {
        public static void UseLogProvider(this IServiceProvider serviceProvider)
        {
            var httpContextAccessor = serviceProvider.GetRequiredService<IHttpContextAccessor>();
            var logOptions = serviceProvider.GetRequiredService<LogOptions>();
            ElasticsearchSinkOptions elasticsearchSinkOptions =
                logOptions.ElkOptions.CreateElkOptions(logOptions.ApplicationInfo.ServiceName);

            if (logOptions.ElkOptions.Password.ContainsString() && logOptions.ElkOptions.Username.ContainsString())
                elasticsearchSinkOptions.ModifyConnectionSettings =
                    new Func<ConnectionConfiguration, ConnectionConfiguration>(
                        configuration =>
                        {
                            //configuration.BasicAuthentication("username", "password");
                            return configuration;
                        });

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Override("Microsoft", MinimumLogEventLevel(logOptions.ElkOptions.MinimumLogLevel))
                .Enrich.FromLogContext()
                .Enrich.WithElkEnrich(httpContextAccessor)
                .WriteTo.Elasticsearch(elasticsearchSinkOptions)
                .WriteTo.Console(restrictedToMinimumLevel:
                    MinimumLogEventLevel(logOptions.ElkOptions.MinimumConsoleLevel))
                .CreateLogger();
        }

        private static ElasticsearchSinkOptions CreateElkOptions(this ElkOptions optionsElkOptions, string serviceName)
        {
            ElasticsearchSinkOptions options =
                new ElasticsearchSinkOptions(optionsElkOptions.Endpoint);

            options.IndexFormat = $"log-{serviceName}-" + "{0:yyyy.MM.dd}";

            options.AutoRegisterTemplate = true;

            options.BatchPostingLimit = 50;

            options.QueueSizeLimit = 1000;

            options.BatchAction = ElasticOpType.Index;

            options.Period = TimeSpan.FromSeconds(5);

            options.ConnectionTimeout = TimeSpan.FromSeconds((double) 5);

            options.SingleEventSizePostingLimit = new long?();

            options.TemplateName = "serilog-events-template";

            options.InlineFields = false;

            options.MinimumLogEventLevel = MinimumLogEventLevel(optionsElkOptions.MinimumLogLevel);

            options.DeadLetterIndexName = $"log-{serviceName}-deadletter-" + "{0:yyyy.MM.dd}";

            options.EmitEventFailure = EmitEventFailureHandling.WriteToSelfLog;

            options.RegisterTemplateFailure = RegisterTemplateRecovery.IndexAnyway;

            options.BufferFileCountLimit = new int?(31);

            options.BufferFileSizeLimitBytes = new long?(104857600L);

            options.FormatStackTraceAsArray = false;

            options.AutoRegisterTemplateVersion = AutoRegisterTemplateVersion.ESv7;

            return options;
        }

        private static LogEventLevel MinimumLogEventLevel(LogCategories optionsElkOptions)
        {
            switch (optionsElkOptions)
            {
                case LogCategories.Information:
                    return LogEventLevel.Information;
                case LogCategories.Warning:
                    return LogEventLevel.Warning;
                case LogCategories.Error:
                    return LogEventLevel.Error;
                case LogCategories.Fatal:
                    return LogEventLevel.Fatal;
                default:
                    return LogEventLevel.Error;
            }
        }
    }
}