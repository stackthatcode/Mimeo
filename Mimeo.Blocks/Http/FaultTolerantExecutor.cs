using System;
using System.Threading.Tasks;
using Mimeo.Blocks.Logging;

namespace Mimeo.Blocks.Http
{
    [Obsolete("To be replaced by the Polly library")]
    public class FaultTolerantExecutor
    {
        private readonly MimeoLogger _logger;
        private readonly Throttler _throttler;

        public int MaxNumberOfAttempts { get; set; }
        public string ThrottlingKey { get; set; }
        public int ThrottlingDelay { get; set; }


        public FaultTolerantExecutor(MimeoLogger logger, Throttler throttler)
        {
            _logger = logger;
            _throttler = throttler;

            // These are the defaults - no retries, no delays
            //
            MaxNumberOfAttempts = 1;
            ThrottlingKey = Guid.NewGuid().ToString();
            ThrottlingDelay = 0;
        }


        public async Task<T> Run<T>(Func<Task<T>> task, DebugContext debugContext = null)
        {
            // Do Request and process the HTTP Status Codes
            //
            var startTime = DateTime.UtcNow;

            // Execute task
            //
            var result = await DoWorker(task, debugContext);
            
            // Log execution time
            //
            var executionTime = DateTime.UtcNow - startTime;
            _logger.Debug($"Call performance - {executionTime} ms");

            return result;
        }


        private async Task<T> DoWorker<T>(Func<Task<T>> task, DebugContext debugContext = null)
        {
            var numberOfAttempts = 1;

            while (true)
            {
                // Invoke the Throttler
                //
                _throttler.Process(ThrottlingKey, ThrottlingDelay);

                try
                {
                    numberOfAttempts++;
                    return await task();
                }
                catch (Exception ex)
                {
                    if (debugContext != null)
                    {
                        _logger.Error(ex, debugContext.ToString());
                    }
                    else
                    {
                        _logger.Error(ex);
                    }


                    if (numberOfAttempts >= MaxNumberOfAttempts)
                    {
                        _logger.Warn("Retry Limit has been exceeded... throwing exception");
                        throw;
                    }

                    _logger.Debug("Encountered an exception. Retrying invocation...");
                }
            }
        }
    }
}

