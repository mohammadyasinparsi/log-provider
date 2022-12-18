using System;
using Kernel.LogProvider.SerilogProvider;
using Kernel.LogProvider.SerilogProvider.Enum;
using Kernel.LogProvider.SerilogProvider.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Serilog;

namespace TestApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            _environment = env;
            _configuration = configuration;
        }

        private IWebHostEnvironment _environment;
        private IConfiguration _configuration;

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new OpenApiInfo {Title = "TestApi", Version = "v1"}); });

            services.AddHttpContextAccessor();

            services.AddSingleton(new LogOptions
            {
                ElkOptions = new ElkOptions(new Uri("http://172.31.34.5:9200"), minimumLogLevel: LogCategories.Error)
                ,
                ApplicationInfo = new ApplicationInfo("test-api", _environment.EnvironmentName)
            });
            services.AddSingleton<ILogProvider, LogProvider>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory factory)
        {
            app.UseSerilogRequestLogging();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                app.UseSwagger();

                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "TestApi v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}