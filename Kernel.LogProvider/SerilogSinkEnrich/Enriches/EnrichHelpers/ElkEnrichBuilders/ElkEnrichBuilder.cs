using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Kernel.LogProvider.Helpers;
using Kernel.LogProvider.SerilogSinkEnrich.Enriches.EnrichHelpers.logEnrichModels;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Serilog.Events;
using UAParser;

namespace Kernel.LogProvider.SerilogSinkEnrich.Enriches.EnrichHelpers.ElkEnrichBuilders
{
    public class ElkEnrichBuilder : IElkEnrichBuilder
    {
        private readonly Exception _exception;
        private readonly HttpContext _httpContext;
        private readonly LogEvent _logEvent;
        private readonly ClaimsPrincipal _user;
        private DiagnoseLogModel _diagnoseLogModel;

        public ElkEnrichBuilder(LogEvent logEvent, IHttpContextAccessor httpContextAccessor)
        {
            _httpContext = httpContextAccessor.HttpContext;
            _user = httpContextAccessor.HttpContext?.User;
            _logEvent = logEvent;
            _exception = _logEvent.Exception
                         ?? httpContextAccessor.HttpContext?.Features?.Get<IExceptionHandlerPathFeature>()?.Error;
            Reset();
        }

        public void BasicBuilder()
        {
            _diagnoseLogModel.LogDateTime = _logEvent.Timestamp.ToString("g", new CultureInfo("fa-Ir"));
            _diagnoseLogModel.Timezone = TimeZoneInfo.Local.StandardName;
        }

        public void PayloadBuilder()
        {
            if (_logEvent.Properties.ContainsKey(EventLogPropertyNames.ActionPayload))
            {
                var seqValue = _logEvent.Properties.FirstOrDefault(f => f.Key == EventLogPropertyNames.ActionPayload)
                    .Value;
                if (seqValue is DictionaryValue seq)
                {
                    _diagnoseLogModel.Payloads =
                        seq.Elements
                            .ToDictionary(pair => pair.Key.ToString().Trim(),
                                pair => pair.Value.ToString().Trim());
                    return;
                }
            }

            _diagnoseLogModel.Payloads = new Dictionary<string, string>();
        }

        public void ApplicationBuilder()
        {
            _diagnoseLogModel.ApplicationModel = new ApplicationModel
            {
                Name = _logEvent.Properties.ContainsKey(EventLogPropertyNames.ApplicationName)
                    ? _logEvent.Properties[EventLogPropertyNames.ApplicationName].ToString()
                    : null,
                Version = _logEvent.Properties.ContainsKey(EventLogPropertyNames.ApplicationVersion)
                    ? _logEvent.Properties[EventLogPropertyNames.ApplicationVersion].ToString()
                    : null,
                Environment = _logEvent.Properties.ContainsKey(EventLogPropertyNames.ApplicationEnvironment)
                    ? _logEvent.Properties[EventLogPropertyNames.ApplicationEnvironment].ToString()
                    : null,
            };
            if (_httpContext?.Request != null)
            {
                _diagnoseLogModel.ApplicationModel.IsHttps = _httpContext.Request?.IsHttps.ToString();
                _diagnoseLogModel.ApplicationModel.Protocol = _httpContext.Request?.Protocol;
            }
        }

        public void ClientBuilder()
        {
            _diagnoseLogModel.Client = new ClientModel();
            if (_httpContext == null) return;
            _diagnoseLogModel.Client.User =
                _user?.Identity?.IsAuthenticated ?? false ? _user?.Identity?.Name : "Guest User";

            _diagnoseLogModel.Client.Address = _httpContext.Request?.GetDisplayUrl();

            _diagnoseLogModel.Client.Ip = _httpContext.Connection?.RemoteIpAddress?.ToString();

            _diagnoseLogModel.Client.Bytes = _httpContext.Request?.ContentLength ?? 0;

            _diagnoseLogModel.Client.Port = _httpContext.Connection?.RemotePort.ToString();
        }

        public void ErrorBuilder()
        {
            _diagnoseLogModel.Errors = new ErrorModel
            {
                ActionSeverityTitle = _logEvent.Properties.ContainsKey(EventLogPropertyNames.ActionSeverityTitle)
                    ? _logEvent.Properties[EventLogPropertyNames.ActionSeverityTitle].ToString()
                    : null,
                Name = _logEvent.Properties.ContainsKey(EventLogPropertyNames.LogName)
                    ? _logEvent.Properties[EventLogPropertyNames.LogName].ToString()
                    : ""
            };
            if (_exception == null) return;
            _diagnoseLogModel.Errors.Message = _exception?.Message;
            _diagnoseLogModel.Errors.StackTrace = GetStackTraceAndInnerExceptions();
            _diagnoseLogModel.Errors.ExceptionType = _exception?.GetType().Name;
        }


        public void HttpBuilder()
        {
            if (_httpContext?.Request != null)
            {
                _diagnoseLogModel.Http = new HttpModel
                {
                    Request = new HttpRequestModel
                    {
                        Method = _httpContext.Request.Method,
                        ContentEncoding = _httpContext.Request.Headers?["Accept-Encoding"],
                        IsAuthenticated = _httpContext.User?.Identity?.IsAuthenticated,
                        ContentType = _httpContext.Request.ContentType,
                        Headers = _httpContext.Request.Headers?
                            .Where(w => w.Key != "Cookie")
                            .ToDictionary(pair => pair.Key, pair => pair.Value.ToString()),

                        Files = _httpContext.Request?.HasFormContentType ?? false
                            ? _httpContext.Request.Form?.Files?
                                .Select(f => f.FileName).ToList()
                            : null,
                        Form = _httpContext.Request?.HasFormContentType ?? false
                            ? _httpContext.Request?.Form?
                                .ToDictionary(pair => pair.Key, pair => pair.Value.ToString())
                            : null,
                        Body = new HttpBodyModel
                        {
                            ContentLength = _httpContext.Request?.ContentLength,
                            Content = _httpContext.Request.HttpRequestBodyToStringAsync()
                        },
                    },
                    Response = new HttpResponseModel
                    {
                        StatusCode = _httpContext.Response?.StatusCode ?? 0,
                    }
                };
                _diagnoseLogModel.Http.Request.Cookies = new Dictionary<string, string>();
                foreach (var requestCookies in _httpContext.Request?.Cookies)
                {
                    var cookieValue = _httpContext.Request?.Cookies[requestCookies.Key];
                    _diagnoseLogModel.Http.Request.Cookies.Add(
                        new KeyValuePair<string, string>(requestCookies.Key, cookieValue));
                }

                return;
            }

            _diagnoseLogModel.Http = new HttpModel();
        }

        public void UrlBuilder()
        {
            _diagnoseLogModel.Url = new UrlModel();
            if (_httpContext?.Request == null) return;
            _diagnoseLogModel.Url.FullAddress = _httpContext.Request.GetDisplayUrl();
            _diagnoseLogModel.Url.Path = _httpContext.Request.Path.Value;
            _diagnoseLogModel.Url.Scheme = _httpContext.Request.Scheme;
            _diagnoseLogModel.Url.Query = _httpContext.Request.QueryString.Value;
            _diagnoseLogModel.Url.Domain = _httpContext.Request.Host.Value;
            _diagnoseLogModel.Url.Port = _httpContext.Request.Host.Port;
        }

        public void UserBuilder()
        {
            _diagnoseLogModel.User = new UserModel();
            if (_user == null) return;
            _diagnoseLogModel.User.UserId = _user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            _diagnoseLogModel.User.Name = _user.FindFirst(ClaimTypes.Name)?.Value;
            _diagnoseLogModel.User.Email = _user.FindFirst(ClaimTypes.Email)?.Value;
            _diagnoseLogModel.User.Claims = _user.Claims != null
                ? _user.Claims.ToDictionary(pair => pair.Type, pair => pair.Value)
                : new Dictionary<string, string>();
        }

        public void UserAgentBuilder()
        {
            _diagnoseLogModel.UserAgent = new UserAgentModel();
            // https://github.com/ua-parser/uap-csharp
            if (_httpContext?.Request?.Headers != null)
            {
                var clientInfo = _httpContext.Request.Headers != null
                    ? Parser.GetDefault().Parse(_httpContext.Request.Headers["User-Agent"])
                    : null;
                _diagnoseLogModel.UserAgent.Original =
                    _httpContext.Request.Headers?["User-Agent"];
                if (clientInfo != null)
                {
                    _diagnoseLogModel.UserAgent.IsMobileDevice = MobileHelper.IsMobile(clientInfo.Device.Family);
                    _diagnoseLogModel.UserAgent.DeviceName = clientInfo.Device.Family;
                    _diagnoseLogModel.UserAgent.DeviceManufacturer = clientInfo.Device.Brand;
                    _diagnoseLogModel.UserAgent.Name = clientInfo.UA.Family; // e.g : Mobile Safari
                    _diagnoseLogModel.UserAgent.Platform = clientInfo.OS.Family; // e.g : IOS
                    _diagnoseLogModel.UserAgent.IsCrawler = clientInfo.Device.IsSpider;
                    _diagnoseLogModel.UserAgent.Type = clientInfo.Device.Family; // e.g : Iphone
                    _diagnoseLogModel.UserAgent.Version = clientInfo.OS.ToString(); // e.g : 4.3.2.1
                }
            }
        }

        public DiagnoseLogModel GetModel()
        {
            var resultModel = _diagnoseLogModel;
            Reset();
            return resultModel;
        }

        public void Reset()
        {
            _diagnoseLogModel = new DiagnoseLogModel();
        }

        private string GetStackTraceAndInnerExceptions()
        {
            if (_exception == null) return string.Empty;

            var sb = new StringBuilder();

            var stackFrame = new StackTrace(_exception, true).GetFrame(0);
            sb.AppendLine($"Type: {_exception.GetType()}");
            sb.AppendLine(".....................................................................");
            sb.AppendLine($"Source: {_exception.TargetSite?.DeclaringType?.AssemblyQualifiedName}");
            sb.AppendLine(".....................................................................");
            sb.AppendLine($"Message: {_exception.Message}");
            sb.AppendLine(".....................................................................");
            sb.AppendLine($"Trace: {_exception.StackTrace}");
            sb.AppendLine(".....................................................................");
            sb.AppendLine($"Location: {stackFrame?.GetFileName()}");
            sb.AppendLine(".....................................................................");
            sb.AppendLine(
                $"Method: {stackFrame?.GetMethod()} (Line: {stackFrame?.GetFileLineNumber().ToString()}, Column: {stackFrame?.GetFileColumnNumber().ToString()})");
            sb.AppendLine(".....................................................................");
            var innerException = _exception.InnerException;
            var j = 0;
            while (innerException != null)
            {
                var innerStackFrame = new StackTrace(innerException, true).GetFrame(0);
                sb.AppendLine($"****************** Inner Exception {j:D2} Begins ******************");
                sb.AppendLine($"Type: {innerException.GetType()}");
                sb.AppendLine(".....................................................................");
                sb.AppendLine($"Source: {innerException.TargetSite?.DeclaringType?.AssemblyQualifiedName}");
                sb.AppendLine(".....................................................................");
                sb.AppendLine($"Message: {innerException.Message}");
                sb.AppendLine(".....................................................................");
                sb.AppendLine($"Location: {innerStackFrame?.GetFileName()}");
                sb.AppendLine(".....................................................................");
                sb.AppendLine(
                    $"Method: {innerStackFrame?.GetMethod()} (Line: {innerStackFrame?.GetFileLineNumber().ToString()}, Column: {innerStackFrame?.GetFileColumnNumber().ToString()})");
                sb.AppendLine($"****************** Inner Exception {j:D2} Ends ******************");

                innerException = innerException.InnerException;
                j++;
            }

            return sb.ToString();
        }
    }
}