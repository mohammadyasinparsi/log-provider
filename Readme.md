#Log Provider
##How to Use
###Register Services
####1- Register IHttpContextAccessor.
####2- Register <b5>ElkOptions<b/> as Singleton.
####3- Register <b>ILogProvider<b/> as Singleton\n
````
public void ConfigureServices(IServiceCollection services)
{
       services.AddHttpContextAccessor();

       services.AddSingleton(new LogOptions
       {
           ElkOptions = new ElkOptions(new Uri(elastic urls), minimumLogEventLevel: LogCategories.Error
           )
           {
               Username = "username",
               Password = "password"
           },
           ApplicationInfo = new ApplicationInfo("Test", _environment.EnvironmentName)
       });
       services.AddSingleton<IRabaniLogProvider, RabaniLogProvider>();
}
````

####4- Install <b>Serilog.AspNetCore<b> from nuget
####5- Use Serilog in <b>IHostBuilder<b/> in Program.cs
````
 private static IHostBuilder CreateHostBuilder(string[] args)
{
      return Host.CreateDefaultBuilder(args)
          .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); })
      .UseSerilog();
}
````
####6- Use <b>UseLogProvider<b/> in Program.cs
````
public static void Main(string[] args)
{
     var hostBuilder =CreateHostBuilder(args);
     
     var host = hostBuilder.Build();
     
     host.Services.UseLogProvider();

     host.Run();
}
````
