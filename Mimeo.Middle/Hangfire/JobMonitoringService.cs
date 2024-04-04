using System;
using System.Linq;
using Hangfire;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Mimeo.Blocks.Logging;
using Mimeo.Blocks.Helpers;
using Mimeo.Middle.Instance;
using Mimeo.Middle.Instance.EF;

namespace Mimeo.Middle.Hangfire
{
    public class JobMonitoringService
    {
        private readonly InstanceContext _instanceContext;
        private readonly MimeoLogger _logger;

        private MimeoInstanceDbContext Entities => _instanceContext.InstanceDbContext;


        private long? _currentScopeMonitorId = null;
        public long CurrentScopeMonitorId => _currentScopeMonitorId.Value;


        public JobMonitoringService(InstanceContext instanceContext, MimeoLogger logger)
        {
            _instanceContext = instanceContext;
            _logger = logger;
        }

        public IDbContextTransaction BeginTransaction()
        {
            return Entities.Database.BeginTransaction();
        }

        private static readonly object ProvisioningLock = new object();

        public JobMonitors ProvisionMonitor(
                    string uniqueIdentifier, string description = null, string hangFireJobId = null)
        {
            lock (ProvisioningLock)
            {
                Cleanup();

                using (var transaction = BeginTransaction())
                {
                    if (IsJobRunning(uniqueIdentifier))
                    {
                        _logger.Info($"Failed to provision monitor for ({uniqueIdentifier}) {description}");
                        return null;
                    }

                    var newJob = new JobMonitors()
                    {
                        InstanceId = _instanceContext.InstanceId,
                        UniqueIdentifier = uniqueIdentifier,
                        Description = description,
                        HangFireJobId = hangFireJobId,
                        ReceivedKillSignal = false,
                        DateCreated = DateTime.UtcNow,
                        LastUpdated = DateTime.UtcNow,
                    };

                    Entities.JobMonitors.Add(newJob);
                    Entities.SaveChanges();

                    transaction.Commit();
                    return newJob;
                }
            }
        }

        public bool IsJobRunning(string uniqueIdentifier)
        {
            return Entities
                .JobMonitors
                .AsNoTracking()
                .Any(x => x.UniqueIdentifier == uniqueIdentifier);
        }

        public void AssignHangfireJob(long monitorId, string hangfireJobId)
        {
            var monitor = Entities.JobMonitors.FirstOrDefault(x => x.Id == monitorId);
            if (monitor != null)
            {
                monitor.HangFireJobId = hangfireJobId;
                Entities.SaveChanges();
            }
        }

        public JobMonitors RetrieveCurrentScopeMonitor()
        {
            return RetrieveMonitorByMonitorIdNoTracking(_currentScopeMonitorId.Value);
        }

        public JobMonitors RetrieveMonitorByMonitorIdNoTracking(long monitorId)
        {
            return Entities
                    .JobMonitors
                    .AsNoTracking()
                    .FirstOrDefault(x => x.Id == monitorId);
        }

        public JobMonitors RetrieveMonitorByUniqueIdNoTracking(string uniqueId)
        {
            return Entities
                .JobMonitors
                .AsNoTracking()
                .FirstOrDefault(x => x.UniqueIdentifier == uniqueId);
        }


        // Having some doubts about the location of this one
        //
        public void SetCurrentScopeMonitorId(long monitorId)
        {
            _currentScopeMonitorId = monitorId;
        }

        public bool DetectCurrentJobInterrupt()
        {
            if (!_currentScopeMonitorId.HasValue)
            {
                throw new Exception("Must invoke IdentifyCurrentJobType() before calling this method");
            }

            if (DetectInterruption(_currentScopeMonitorId.Value))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool DetectInterruption(long monitorId)
        {
            var monitor = RetrieveMonitorByMonitorIdNoTracking(monitorId);
            _logger.Info($"{monitor.Description} - Interruption Detected");
            return (monitor == null || monitor.ReceivedKillSignal);
        }

        public bool IsCorrupted(long monitorId)
        {
            var monitor = RetrieveMonitorByMonitorIdNoTracking(monitorId);
            return monitor != null && monitor.HangFireJobId.IsNullOrEmpty();
        }

        public void SendKillSignal()
        {
            foreach (var monitor in Entities.JobMonitors.ToList())
            {
                SendKillSignal(monitor.Id);
            }
        }

        private void SendKillSignal(long monitorId)
        {
            var monitor = Entities.JobMonitors.FirstOrDefault(x => x.Id == monitorId);
            if (monitor == null)
            {
                return;
            }

            monitor.ReceivedKillSignal = true;
            Entities.SaveChanges();

            _logger.Info($"{monitor.Description} - Kill Signal Received");
        }

        public void RemoveJobMonitor(long monitorId)
        {
            var monitor = Entities.JobMonitors.FirstOrDefault(x => x.Id == monitorId);
            Entities.JobMonitors.Remove(monitor);
            Entities.SaveChanges();
        }

        public void CleanupPostExecution(long finishedJobMonitor)
        {
            // First explicitly remove the Job Monitor
            //
            var monitor = Entities.JobMonitors.FirstOrDefault(x => x.Id == finishedJobMonitor);
            if (monitor != null)
            {
                RemoveJobMonitor(monitor.Id);
            }

            Cleanup();
        }

        public void Cleanup()
        {
            var monitors = Entities.JobMonitors.ToList();

            using (var connection = JobStorage.Current.GetConnection())
            {
                foreach (var monitor in monitors)
                {
                    // Remove any Job Monitor that is corrupted -- or doesn't have a valid Hangfire Job
                    //
                    if (monitor.HangFireJobId.IsNullOrEmpty())
                    {
                        RemoveJobMonitor(monitor.Id);
                        continue;
                    }

                    // Remove any Job Monitor for a Hangfire Job that is no longer Alive
                    //
                    var hangfireRecord = connection.GetJobData(monitor.HangFireJobId);
                    if (!hangfireRecord.IsAlive())
                    {
                        RemoveJobMonitor(monitor.Id);
                    }
                }
            }
        }
    }
}

