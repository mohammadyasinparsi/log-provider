using Kernel.LogProvider.SerilogSinkEnrich.Enriches.EnrichHelpers.logEnrichModels;

namespace Kernel.LogProvider.SerilogSinkEnrich.Enriches.EnrichHelpers.ElkEnrichBuilders
{
    public interface IElkEnrichBuilder
    {
        void BasicBuilder();
        void PayloadBuilder();
        void ApplicationBuilder();
        void ClientBuilder();
        void ErrorBuilder();
        void HttpBuilder();
        void UrlBuilder();
        void UserBuilder();
        void UserAgentBuilder();
        void Reset();
        DiagnoseLogModel GetModel();
    }
}