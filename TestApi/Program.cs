using Kernel.LogProvider.Modules;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Kernel.LogProvider.Modules;
using Serilog;

namespace TestApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var hostBuilder =CreateHostBuilder(args);
            var host = hostBuilder.Build();
            
            host.Services.UseLogProvider();
            //Console.WriteLine();

            host.Run();
        }

        private static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); })
            .UseSerilog();
        }
    }
}