using Hangfire.Logging;
using Mimeo.Blocks.Logging;

namespace Mimeo.Middle.Hangfire
{
    public class HangfireLogProvider : ILogProvider
    {
        private static MimeoLogger _loggerInstance;

        public static void RegisterInstance(MimeoLogger loggerInstance)
        {
            _loggerInstance = loggerInstance;
        }

        public ILog GetLogger(string name)
        {
            return new HangfireLogger(_loggerInstance);
        }
    }
}
