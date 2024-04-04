using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Mimeo.Blocks.Helpers;


namespace Mimeo.Middle.Config
{
    // Cache of application configuration settings
    //
    public class MimeoAppSettings
    {
        public readonly string ApplicationName = "Mimeo Quickstart";
        public readonly string ProtectedConfigEnvName = "MimeoQuickstartConfig";


        public string CurrentEnvironment { get; set; }
        public bool IsProduction => CurrentEnvironment == Environments.Production;

        public string Url { get; set; }
        public string SerilogFilePath { get; set; }
        public string SerilogLogLevel { get; set; }
        public string TestEmailOutputDir { get; set; }

        public bool UseTestEmailService => !TestEmailOutputDir.IsNullOrEmpty();
        public bool UseEmbeddedEmailImages => !UseTestEmailService;


        public static MimeoAppSettings ReadConfig(IConfiguration configuration)
        {
            var output = new MimeoAppSettings();

            // Get current environment...
            //
            output.CurrentEnvironment = CurrentAspNetCoreEnvironment();

            // JSON config file settings
            //
            output.Url = configuration["Mimeo:Url"];
            output.SerilogFilePath = configuration["Mimeo:SerilogFilePath"];
            output.SerilogLogLevel = configuration["Mimeo:SerilogLogLevel"];
            output.TestEmailOutputDir = configuration["Mimeo:TestEmailOutputDir"];

            return output;
        }


        public static string CurrentAspNetCoreEnvironment()
        {
            return Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        }
    }
}
