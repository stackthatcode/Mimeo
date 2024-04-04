using System;
using Mimeo.Blocks.Logging;
using Mimeo.Middle.Instance;

namespace Mimeo.Middle.Hangfire
{
    public class JobRunner<T> where T : new()
    {
        private readonly T _worker;
        private readonly InstanceContext _instanceContext;
        private readonly JobMonitoringService _jobMonitoringService;
        private readonly RunLockRepository _runLockRepository;
        private readonly MimeoLogger _logger;



        // Obviously replace this with DI framework
        //
        public JobRunner(T worker,
                InstanceContext instanceContext,
                JobMonitoringService jobMonitoringService,
                RunLockRepository runLockRepository,
                MimeoLogger logger)
        {
            _worker = worker;
            _instanceContext = instanceContext;
            _jobMonitoringService = jobMonitoringService;
            _runLockRepository = runLockRepository;
            _logger = logger;
        }


        public async void ExecuteAsync(long instanceId, long jobMonitorId, Action<T> action)
        {
            await _instanceContext.InitializeAsync(instanceId);

            _jobMonitoringService.SetCurrentScopeMonitorId(jobMonitorId);

            var monitor = _jobMonitoringService.RetrieveCurrentScopeMonitor();

            try
            {
                if (!_runLockRepository.Acquire(monitor.UniqueIdentifier))
                {
                    return;
                }

                if (_jobMonitoringService.IsCorrupted(jobMonitorId))
                {
                    var msg = $"Job is missing or corrupted";
                    _logger.Info(msg);
                    _jobMonitoringService.CleanupPostExecution(jobMonitorId);
                    _runLockRepository.Free(instanceId.ToString());
                    return;
                }

                if (_jobMonitoringService.DetectInterruption(jobMonitorId))
                {
                    var msg = $"Job is missing or has received stop signal";
                    _logger.Info(msg);
                    _jobMonitoringService.CleanupPostExecution(jobMonitorId);
                    _runLockRepository.Free(instanceId.ToString());
                    return;
                }

                // Phew - we made it! Execute the requested task
                //
                _logger.Info($"{monitor.FullDescription()} - execution now starting");

                action(_worker);

                _jobMonitoringService.CleanupPostExecution(jobMonitorId);

                // *** IMPORTANT - do not refactor this to use-finally, else it will 
                // ... break concurrency locking
                //
                _runLockRepository.Free(instanceId.ToString());
            }
            catch (Exception ex)
            {
                _runLockRepository.Free(instanceId.ToString());

                // If this is One-Time Job, this will remove the Monitor now that the Job has failed
                //
                _jobMonitoringService.CleanupPostExecution(jobMonitorId);

                _logger.Info($"{monitor.FullDescription()} - encountered an error");
                _logger.Error(ex);
            }
            finally
            {
                _logger.Info($"{monitor.FullDescription()} - execution terminated");
            }
        }
    }
}
