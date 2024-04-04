using System;
using Hangfire;
using Mimeo.Middle.Instance;

namespace Mimeo.Middle.Hangfire
{
    public class JobScheduler
    {
        private readonly InstanceContext _instanceContext;
        private readonly JobMonitoringService _monitoringService;

        public JobScheduler(InstanceContext instanceContext, JobMonitoringService monitoringService)
        {
            _instanceContext = instanceContext;
            _monitoringService = monitoringService;
        }

        public void Schedule<T>(string uniqueIdentifier, string description, Action<T> action) where T: new()
        {
            var monitor = _monitoringService.ProvisionMonitor(uniqueIdentifier, description);
            if (monitor == null)
            {
                return;
            }

            var hangfireJobId 
                = BackgroundJob.Enqueue<JobRunner<T>>(
                    x => x.ExecuteAsync(_instanceContext.InstanceId, monitor.Id, action));
           
           _monitoringService.AssignHangfireJob(monitor.Id, hangfireJobId); 
        }
    }
}
