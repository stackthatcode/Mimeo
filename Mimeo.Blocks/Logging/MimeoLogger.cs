using System;
using Sentry;
using Serilog;
using Serilog.Context;

namespace Mimeo.Blocks.Logging
{
    public class MimeoLogger
    {
        private Guid? _correlationId;
        private readonly bool _sentryEnabled;

        public MimeoLogger(bool sentryEnabled)
        {
            _sentryEnabled = sentryEnabled;
        }

        public virtual void SetCorrelationId(Guid? guid = null)
        {
            _correlationId = Guid.NewGuid();
        }

        public virtual void EnsureCorrelationId()
        {
            if (!_correlationId.HasValue)
            {
                _correlationId = Guid.NewGuid();
            }
        }


        public virtual void Trace(string message)
        {
            using (LogContext.PushProperty("CorrelationId", _correlationId))
            {
                
                Log.Verbose(message);
            }
        }

        public virtual void Debug(string message)
        {
            using (LogContext.PushProperty("CorrelationId", _correlationId))
            {
                Log.Debug(message);
            }
        }

        public virtual void Info(string message)
        {
            using (LogContext.PushProperty("CorrelationId", _correlationId))
            {
                Log.Information(message);
            }
        }

        public virtual void Warn(string message)
        {
            using (LogContext.PushProperty("CorrelationId", _correlationId))
            {
                Log.Warning(message);
            }
        }

        public virtual void Error(string message)
        {
            using (LogContext.PushProperty("CorrelationId", _correlationId))
            {
                Log.Error(message);

                if (_sentryEnabled)
                {
                    SentrySdk.CaptureMessage(message, SentryLevel.Error);
                }
            }
        }
        public virtual void Error(Exception ex, string message)
        {
            using (LogContext.PushProperty("CorrelationId", _correlationId))
            {
                Log.Error(message + Environment.NewLine + ex.FullStackTraceDump());

                if (_sentryEnabled)
                {
                    SentrySdk.CaptureException(ex);
                }
            }
        }
        public virtual void Error(Exception ex)
        {
            using (LogContext.PushProperty("CorrelationId", _correlationId))
            {
                Log.Error(ex.FullStackTraceDump());

                if (_sentryEnabled)
                {
                    SentrySdk.CaptureException(ex);
                }
            }
        }

        public virtual void Fatal(string message)
        {
            using (LogContext.PushProperty("CorrelationId", _correlationId))
            {
                Log.Fatal(message);

                if (_sentryEnabled)
                {
                    SentrySdk.CaptureMessage(message, SentryLevel.Fatal);
                }
            }
        }

        public virtual void Fatal(Exception ex)
        {
            using (LogContext.PushProperty("CorrelationId", _correlationId))
            {
                Log.Fatal(ex.FullStackTraceDump());

                if (_sentryEnabled)
                {
                    SentrySdk.CaptureException(ex);
                }
            }
        }
    }
}

