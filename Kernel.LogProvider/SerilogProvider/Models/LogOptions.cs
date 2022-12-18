namespace Kernel.LogProvider.SerilogProvider.Models
{
    public class LogOptions
    {
        public ElkOptions ElkOptions { get; set; }

        public ApplicationInfo ApplicationInfo { get; set; }
    }
}