namespace Kernel.LogProvider.SerilogSinkEnrich.Enriches.EnrichHelpers.EventPropertyBuilder
{
    public class LogEventPropertyDirectoryDirector
    {
        public void Construct(ILogEventPropertyBuilder builder)
        {
            builder.BuildDiagnoseLog();
            builder.BuildLogEventPropertyDirectory();
        }
    }
}