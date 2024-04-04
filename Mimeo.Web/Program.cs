using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Mimeo.Blocks.Logging;
using Mimeo.Middle.Config;
using Serilog;

namespace Mimeo.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var appSettings = MimeoAppSettings.ReadConfig(Startup.BuildConfiguration());

            Log.Logger
                = new LoggerConfiguration()
                    .WriteTo.RollingFile(
                        appSettings.SerilogFilePath,
                        retainedFileCountLimit: 14,
                        outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} {RequestId} [{Level}] {Message}{NewLine}{Exception}")
                    .WriteTo.Console()
                    .SetMinimumLevel(appSettings.SerilogLogLevel)
                    .Enrich.FromLogContext()
                    .CreateLogger();

            CreateHostBuilder(args)
                .UseSerilog()
                .Build()
                .Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder
                        .UseIISIntegration()
                        .UseStartup<Startup>();
                });
    }
}

