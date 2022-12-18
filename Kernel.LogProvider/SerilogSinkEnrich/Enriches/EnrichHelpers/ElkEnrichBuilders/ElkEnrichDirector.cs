namespace Kernel.LogProvider.SerilogSinkEnrich.Enriches.EnrichHelpers.ElkEnrichBuilders
{
    public class ElkEnrichDirector
    {
        public void Construct(IElkEnrichBuilder builder)
        {
            builder.BasicBuilder();
            builder.PayloadBuilder();
            builder.ApplicationBuilder();
            builder.ClientBuilder();
            builder.ErrorBuilder();
            builder.HttpBuilder();
            builder.UrlBuilder();
            builder.UserBuilder();
            builder.UserAgentBuilder();
        }
    }
}