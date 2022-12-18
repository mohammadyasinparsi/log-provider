using System.IO;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace Kernel.LogProvider.Helpers
{
    public static class HttpContextExtensions
    {
        // https://devblogs.microsoft.com/aspnet/re-reading-asp-net-core-request-bodies-with-enablebuffering/
        public static string HttpRequestBodyToStringAsync(this HttpRequest httpRequest)
        {
            if (!httpRequest.Body.CanSeek)
            {
                httpRequest.EnableRewind();
            }

            httpRequest.Body.Position = 0;
            var reader = new StreamReader(httpRequest.Body, Encoding.UTF8);

            var body = reader.ReadToEndAsync()
                .ConfigureAwait(false)
                .GetAwaiter()
                .GetResult();
            httpRequest.Body.Position = 0;

            return body;
        }
    }
}