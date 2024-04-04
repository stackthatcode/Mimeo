using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Serilog;

namespace Mimeo.Blocks.Logging
{
    public static class LogLevelExtension
    {
        private static readonly Dictionary<string, LogLevel> 
            _storage = new Dictionary<string, LogLevel>
                {
                    {"NONE", LogLevel.None},
                    {"TRACE", LogLevel.Trace},
                    {"DEBUG", LogLevel.Debug},
                    {"INFORMATION", LogLevel.Information},
                    {"WARNING", LogLevel.Warning},
                    {"ERROR", LogLevel.Error},
                    {"FATAL", LogLevel.Critical},
                };

        public static LogLevel ToLogLevel(this string input)
        {
            return _storage.ContainsKey(input.ToUpper()) ? _storage[input.ToUpper()] : LogLevel.None;
        }

        public static LoggerConfiguration SetMinimumLevel(this LoggerConfiguration configuration, string logLevel)
        {
            var logLevelStandard = logLevel.ToUpper();

            if (logLevel == "FATAL")
            {
                return configuration.MinimumLevel.Fatal();
            }
            if (logLevel == "ERROR")
            {
                return configuration.MinimumLevel.Error();
            }
            if (logLevel == "WARN")
            {
                return configuration.MinimumLevel.Warning();
            }
            if (logLevel == "INFO")
            {
                return configuration.MinimumLevel.Information();
            }
            if (logLevel == "DEBUG")
            {
                return configuration.MinimumLevel.Debug();
            }
            if (logLevel == "TRACE")
            {
                return configuration.MinimumLevel.Verbose();
            }

            return configuration;
        }
    }
}

