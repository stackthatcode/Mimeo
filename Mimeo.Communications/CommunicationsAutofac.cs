using Autofac;
using Microsoft.Extensions.Configuration;
using Mimeo.Blocks.Http;
using Mimeo.Blocks.Logging;
using Mimeo.Blocks.Security;
using Mimeo.Communications.Config;
using Mimeo.Communications.Email.Delivery;
using Mimeo.Communications.Html;
using Mimeo.Communications.Html.Content.Fragments;
using Mimeo.Communications.Html.Content.Images;
using RazorLight;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;


namespace Mimeo.Communications
{
    public class CommunicationsAutofac
    {
        public static void Build(ContainerBuilder builder)
        {
            var loggingTemplate =
                "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} {RequestId} " +
                "[{Level:u3}] {Message}{NewLine}{Exception}";

            Log.Logger
                = new LoggerConfiguration()
                    .WriteTo.File(
                        @"C:\DEV\Mimeo\Logs\MimeoLog",
                        rollingInterval: RollingInterval.Day,
                        retainedFileCountLimit: 14,
                        outputTemplate: loggingTemplate)
                    .WriteTo.Console(theme: SystemConsoleTheme.None)
                    .SetMinimumLevel("Debug")
                    .Enrich.FromLogContext()
                    .CreateLogger();

            // Logging 
            // 
            builder.Register(c => new MimeoLogger(false))
                .As<MimeoLogger>()
                .InstancePerLifetimeScope();

            builder.RegisterType<ExecutorFactory>().InstancePerLifetimeScope();
            
            builder.RegisterType<ImageFactoryLocal>().InstancePerLifetimeScope();
            builder.RegisterType<HtmlTemplateService>().InstancePerLifetimeScope();
            builder.RegisterType<RazorLightEngineBuilder>().InstancePerLifetimeScope();
            builder.RegisterType<FragmentFactory>().InstancePerLifetimeScope();
            builder.RegisterType<HttpClient>().InstancePerLifetimeScope();

            builder.RegisterType<MailgunConfig>().InstancePerLifetimeScope();
            builder.RegisterType<MailgunConfigKeyService>().InstancePerLifetimeScope();
            builder.RegisterType<MailgunApi>().InstancePerLifetimeScope();

            // 0001
            //
            builder
                .Register(
                    x =>
                    {
                        var configService = new MailgunConfigKeyService();
                        var output = new MailgunConfig
                        {
                            //MailgunResource
                            //    = "https://api.mailgun.net/v3/sandbox1cfe721f7d774e249b23e53a9f251591.mailgun.org/messages",
                            MailgunResource
                                = "https://api.mailgun.net/v3/globalplus.news/messages",
                            MailgunApiKey = configService.GetApiKey(),
                            FromAddress = "info@globalplus.news",
                            ReplyToAddress = "info@globalplus.news",
                            PostmasterAddress = "info@globalplus.news",
                            EmailDomain = "globalplus.news",
                        };

                        return output;
                    })
                .Named<MailgunConfig>(MailgunConfigIds.Config0001);

        }
    }
}


