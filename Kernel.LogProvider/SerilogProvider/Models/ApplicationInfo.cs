using Kernel.Utilites.ValueCheck;

namespace Kernel.LogProvider.SerilogProvider.Models
{
    public class ApplicationInfo
    {
        /// <summary>
        /// Name of current applications.
        /// Such as: name Api v1, name Api v1.0.0 and etc.
        /// </summary>
        public string ServiceName { get; }


        /// <summary>
        /// Current version of application in x.xy.xy format.
        /// Such as: 1.0.0, 1.15.8 and etc. 
        /// </summary>
        public string Version { get; }

        /// <summary>
        /// Executing environment name
        /// </summary>
        public string EnvironmentName { get; }

        public ApplicationInfo(string serviceName, string environmentName, string version = "1.0.0")
        {
            Precondition.NullCheck(serviceName, nameof(ServiceName));
            Precondition.NullCheck(version, nameof(Version));
            Precondition.NullCheck(environmentName, nameof(EnvironmentName));
            ServiceName = serviceName;
            EnvironmentName = environmentName;
            Version = version;
        }
    }
}