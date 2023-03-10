using System;
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.WebUtilities;
using Kernel.Utilites.ValueCheck;

namespace Kernel.LogProvider.Helpers
{
    internal static class BufferingHelper
    {
        private const int DefaultBufferThreshold = 1024 * 30;

        public static HttpRequest EnableRewind(this HttpRequest request, int bufferThreshold = DefaultBufferThreshold,
            long? bufferLimit = null)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var body = request.Body;

            if (body.CanSeek) return request;
            var fileStream = new FileBufferingReadStream(body, bufferThreshold, bufferLimit,
                AspNetCoreTempDirectory.TempDirectoryFactory);
            request.Body = fileStream;
            request.HttpContext.Response.RegisterForDispose(fileStream);

            return request;
        }

        public static MultipartSection EnableRewind(this MultipartSection section,
            Action<IDisposable> registerForDispose,
            int bufferThreshold = DefaultBufferThreshold, long? bufferLimit = null)
        {
            if (section == null)
            {
                throw new ArgumentNullException(nameof(section));
            }

            if (registerForDispose == null)
            {
                throw new ArgumentNullException(nameof(registerForDispose));
            }

            var body = section.Body;
            if (body.CanSeek) return section;
            var fileStream = new FileBufferingReadStream(body, bufferThreshold, bufferLimit,
                AspNetCoreTempDirectory.TempDirectoryFactory);
            section.Body = fileStream;
            registerForDispose(fileStream);

            return section;
        }

        private static class AspNetCoreTempDirectory
        {
            private static string _tempDirectory;

            private static string TempDirectory
            {
                get
                {
                    if (_tempDirectory.ContainsString()) return _tempDirectory;
                    // Look for folders in the following order.
                    var temp =
                        Environment.GetEnvironmentVariable(
                            "ASPNETCORE_TEMP") ?? // ASPNETCORE_TEMP - User set temporary location.
                        Path.GetTempPath(); // Fall back.

                    if (!Directory.Exists(temp))
                    {
                        throw new DirectoryNotFoundException(temp);
                    }

                    _tempDirectory = temp;

                    return _tempDirectory;
                }
            }

            public static Func<string> TempDirectoryFactory => () => TempDirectory;
        }
    }
}