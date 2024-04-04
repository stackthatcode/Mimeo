using System;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Hangfire;
using Hangfire.Autofac;
using Hangfire.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Mimeo.Blocks.Logging;
using Mimeo.Middle;
using Mimeo.Middle.Config;
using Mimeo.Middle.Hangfire;
using Mimeo.Middle.Identity;
using Serilog;


namespace Mimeo.ConsoleApp
{
    public class Bootstrap
    {
        public static bool LoggerInitialized = false;

        public static IContainer ConfigureAndBuilderContainer(bool configureHangfire = false)
        {
            // Configuration fetch
            //
            var configBuilder = new ConfigurationBuilder()
                .AddJsonFile($"appsettings.{MimeoAppSettings.CurrentAspNetCoreEnvironment()}.json", optional: true)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();
            var configuration = configBuilder.Build();

            // Read the JSON configuration into Mimeo App Settings
            //
            var appSettings = MimeoAppSettings.ReadConfig(configuration);

            // Configure Serilog
            //
            if (LoggerInitialized == false)
            {
                Log.Logger
                    = new LoggerConfiguration()
                        .WriteTo.RollingFile(
                            appSettings.SerilogFilePath,
                            retainedFileCountLimit: 14,
                            outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} {CorrelationId} [{Level}] {Message}{NewLine}{Exception}")
                        .WriteTo.Console()
                        .SetMinimumLevel(appSettings.SerilogLogLevel)
                        .Enrich.FromLogContext()
                        .CreateLogger();

                LoggerInitialized = true;
            }


            // The .NET Core Services wire-up
            //
            var services
                = new ServiceCollection()
                    .AddMimeoIdentity()
                    .AddOptions()
                    .AddLogging();

            // Create the Autofac Container
            //
            var containerBuilder = new ContainerBuilder();
            MiddleAutofac.BuildAppSettings(containerBuilder, appSettings);
            MiddleAutofac.Build(containerBuilder, configuration);
            MiddleAutofac.BuildIdentityRegistrations(containerBuilder, configuration);

            // Important - this ties the .NET Core ServicesCollection from above into Autofac
            //
            containerBuilder.Populate(services);

            // ... add ConsoleApp-specific functionality
            //
            containerBuilder.RegisterType<AdminCommandExecutor>();
            containerBuilder.RegisterType<LoggedConsoleInput>();

            var container = containerBuilder.Build();

            if (configureHangfire)
            {
                HangfireShim.Configure(configuration["ConnectionStrings:DefaultConnection"]);
            }

            return container;
        }


        public static void Execute<T>(Action<T> action, string processName = "(unnamed)")
        {
            var container = ConfigureAndBuilderContainer();
            var serviceProvider = new AutofacServiceProvider(container);

            using (var scope = serviceProvider.LifetimeScope.BeginLifetimeScope())
            {
                var logger = scope.Resolve<MimeoLogger>();
                logger.SetCorrelationId();

                try
                {
                    logger.Info($"Starting process - {processName}");

                    // Performing the actual action
                    //
                    var service = scope.Resolve<T>();
                    action(service);
                }
                catch (Exception e)
                {
                    logger.Error(e);
                }
                finally
                {
                    logger.Info($"Ending process - {processName}");
                }
            }
        }


        public static void RunHangfireBackgroundJob()
        {
            // Configure and build Autofac Container
            //
            var container = ConfigureAndBuilderContainer(configureHangfire: true);

            // Configure Hangfire to run Background Job
            //
            LogProvider.SetCurrentLogProvider(new HangfireLogProvider());
            GlobalConfiguration.Configuration.UseAutofacActivator(container);

            var options = new BackgroundJobServerOptions()
            {
                SchedulePollingInterval = new TimeSpan(0, 0, 0, 1),
                WorkerCount = 10, 
            };

            using (var server = new BackgroundJobServer(options))
            {
                Console.WriteLine("Hangfire Server started. Press any key to exit...");
                Console.ReadKey();
                Console.WriteLine("Hangfire Server terminated." + Environment.NewLine);
            }
        }
    }
}

