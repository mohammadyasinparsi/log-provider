using System.Collections.Generic;
using System.Text;
using Serilog.Core;
using Serilog.Events;

namespace Kernel.LogProvider.SerilogSinkEnrich.Enriches.EnrichHelpers.logEnrichModels
{
    /// <summary>
    /// URL fields provide support for complete or partial URLs, and supports the breaking down into scheme, domain, path, and so on.
    /// </summary>
    public class UrlModel
    {
        private const string ObjectName = "Url";

        /// <summary>
        /// Domain of the url, such as "www.elastic.co".
        /// In some cases a URL may refer to an IP and/or port directly, without a domain name.
        /// In this case, the IP address would go to the domain field.
        /// type: keyword
        /// example: www.elastic.co
        /// </summary>
        public string Domain { get; set; }

        /// <summary>
        /// If full URLs are important to your use case, they should be stored in url.full,
        /// whether this field is reconstructed or present in the event source.
        /// type: keyword
        /// example: https://www.elastic.co:443/search?q=elasticsearch#top
        /// </summary>
        public string FullAddress { get; set; }

        /// <summary>
        /// Path of the request, such as "/search".
        /// type: keyword
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// Port of the request, such as 443.
        /// type: long
        /// example: 443
        /// </summary>
        public long? Port { get; set; }

        /// <summary>
        /// The query field describes the query string of the request, such as "q=elasticsearch".
        /// The ? is excluded from the query string.
        /// If a URL contains no ?, there is no query field. If there is a ? but no query, the query field exists with an empty string.
        /// The exists query can be used to differentiate between the two cases.
        /// type: keyword
        /// </summary>
        public string Query { get; set; }

        /// <summary>
        /// Scheme of the request, such as "https".
        /// Note: The : is not part of the scheme.
        /// type: keyword
        /// example: https
        /// </summary>
        public string Scheme { get; set; }

        public void AddPropertiesToLogEventProperties(IDictionary<string, LogEventProperty> logEventProperties,
            ILogEventPropertyFactory propertyFactory)
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.AppendLine($"{nameof(Domain)} = {Domain}");
            stringBuilder.AppendLine(".....................................................................");
            stringBuilder.AppendLine($"{nameof(FullAddress)} = {FullAddress}");
            stringBuilder.AppendLine(".....................................................................");
            stringBuilder.AppendLine($"{nameof(Path)} = {Path}");
            stringBuilder.AppendLine(".....................................................................");
            stringBuilder.AppendLine($"{nameof(Port)} = {Port}");
            stringBuilder.AppendLine(".....................................................................");
            stringBuilder.AppendLine($"{nameof(Query)} = {Query}");
            stringBuilder.AppendLine(".....................................................................");
            stringBuilder.AppendLine($"{nameof(Scheme)} = {Scheme}");

            logEventProperties[$"{ObjectName}"] =
                propertyFactory.CreateProperty($"{ObjectName}", stringBuilder.ToString());
        }
    }
}