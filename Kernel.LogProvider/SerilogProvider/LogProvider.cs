using System;
using System.Collections.Generic;
using Kernel.LogProvider.Helpers;
using Kernel.LogProvider.SerilogProvider.Enum;
using Kernel.LogProvider.SerilogProvider.Models;
using Kernel.Utilites.ValueCheck;
using Serilog.Context;

namespace Kernel.LogProvider.SerilogProvider
{
    public class LogProvider : ILogProvider
    {
        protected readonly ApplicationInfo ApplicationInformation;

        public LogProvider(LogOptions logOptions)
        {
            Precondition.NullCheck(logOptions, nameof(LogOptions));
            Precondition.NullCheck(logOptions.ApplicationInfo, nameof(ApplicationInfo));
            ApplicationInformation = logOptions.ApplicationInfo;
        }

        public void LogInformation(string name = "", Exception exception = null,
            IDictionary<string, string> payload = null)
        {
            Log(LogCategories.Information, name, exception, payload);
        }

        public void LogWarning(string name = "", Exception exception = null, IDictionary<string, string> payload = null)
        {
            Log(LogCategories.Warning, name, exception, payload);
        }

        public void LogError(string name = "", Exception exception = null, IDictionary<string, string> payload = null)
        {
            Log(LogCategories.Error, name, exception, payload);
        }

        public void LogFetal(string name = "", Exception exception = null, IDictionary<string, string> payload = null)
        {
            Log(LogCategories.Fatal, name, exception, payload);
        }

        #region + Log

        protected virtual void Log(LogCategories category, string name = "", Exception exception = null,
            IDictionary<string, string> payload = null)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentNullException(nameof(name));

            try
            {
                using (LogContext.PushProperty(EventLogPropertyNames.ApplicationName,
                           ApplicationInformation.ServiceName))
                using (LogContext.PushProperty(EventLogPropertyNames.ApplicationVersion,
                           ApplicationInformation.Version))
                using (LogContext.PushProperty(EventLogPropertyNames.ApplicationEnvironment,
                           ApplicationInformation.EnvironmentName))
                using (LogContext.PushProperty(EventLogPropertyNames.LogName, name))
                using (LogContext.PushProperty(EventLogPropertyNames.ActionSeverityTitle, category.ToString()))


                using (LogContext.PushProperty(EventLogPropertyNames.ActionPayload, payload))
                {
                    switch (category)
                    {
                        case LogCategories.Information:
                            if (exception != null)
                                Serilog.Log.Logger.Information(exception, name);
                            else
                                Serilog.Log.Logger.Information(name);
                            break;
                        case LogCategories.Warning:
                            if (exception != null)
                                Serilog.Log.Logger.Warning(exception, name);
                            else
                                Serilog.Log.Logger.Warning(name);
                            break;
                        case LogCategories.Error:
                            if (exception != null)
                                Serilog.Log.Logger.Error(exception, name);
                            else
                                Serilog.Log.Logger.Error(name);
                            break;
                        case LogCategories.Fatal:
                            if (exception != null)
                                Serilog.Log.Logger.Fatal(exception, name);
                            else
                                Serilog.Log.Logger.Fatal(name);
                            break;
                        default:
                            break;
                    }
                }
            }
            catch (Exception e)
            {
                LogError("It's fucked up, logger itself has exception.", e);
            }
        }

        #endregion
    }
}