using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Mimeo.Blocks.Logging;

namespace Mimeo.Blocks.Http
{
    public class Throttler
    {
        private readonly MimeoLogger _logger;

        private static readonly
            IDictionary<string, DateTime> 
                LastExecutionTime = new ConcurrentDictionary<string, DateTime>();

        public Throttler(MimeoLogger logger)
        {
            _logger = logger;
        }

        public void Process(string host, int timeBetweenCallsMs = 0)
        {
            if (LastExecutionTime.ContainsKey(host))
            {
                var lastExecutionTime = LastExecutionTime[host];

                var timeSinceLastExecution = DateTime.UtcNow - lastExecutionTime;
                var throttlingDelay = new TimeSpan(0, 0, 0, 0, timeBetweenCallsMs);

                if (timeSinceLastExecution < throttlingDelay)
                {
                    var remainingTimeToDelay = throttlingDelay - timeSinceLastExecution;

                    if (_logger != null)
                    {
                        _logger.Debug($"Intentional delay before next call: {remainingTimeToDelay} ms");
                    }

                    Task.Delay(2000).Wait(remainingTimeToDelay);
                }
            }

            LastExecutionTime[host] = DateTime.UtcNow;
        }
    }
}
