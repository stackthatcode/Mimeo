using System;
using Hangfire.Logging;
using Mimeo.Blocks.Logging;
using Mimeo.Blocks.Helpers;

namespace Mimeo.Middle.Hangfire
{
    // Glue between the Push.Foundation Logger and HangFire
    // NOTE: Requires registration of LoggerSingleton delegate
    //
    public class HangfireLogger : ILog
    {
        private readonly MimeoLogger _logger;
        
        public HangfireLogger(MimeoLogger logger)
        {
            _logger = logger;
        }

        public bool Log(LogLevel logLevel, Func<string> messageFunc, Exception exception = null)
        {
            if (_logger == null)
            {
                return false;
            }
            if (messageFunc == null && exception == null)
            {
                return true;
            }


            if (logLevel == LogLevel.Trace)
            {
                _logger.Trace(messageFunc());
            }
            if (logLevel == LogLevel.Debug)
            {
                _logger.Debug(messageFunc());
            }
            if (logLevel == LogLevel.Info)
            {
                _logger.Info(messageFunc());
            }
            if (logLevel == LogLevel.Warn)
            {
                _logger.Warn(messageFunc());
            }
            if (logLevel == LogLevel.Error)
            {
                var message = messageFunc();
                if (!message.IsNullOrEmpty())
                {
                    _logger.Error(message);
                }
                _logger.Error(exception);
            }
            if (logLevel == LogLevel.Fatal)
            {
                _logger.Fatal(messageFunc());

            }
            return true;
        }
    }
}
