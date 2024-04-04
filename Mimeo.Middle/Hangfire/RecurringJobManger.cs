using System;
using System.Linq;
using Hangfire;
using Mimeo.Blocks.Logging;


namespace Mimeo.Middle.Hangfire
{
    public class RecurringJobManger
    {
        private readonly MimeoLogger _logger;

        public RecurringJobManger(MimeoLogger logger)
        {
            _logger = logger;
        }

        public void Start<T>(string uniqueIdentifier, string desc, string cron, Action<T> action) where T : new()
        {
            RecurringJob.AddOrUpdate<JobScheduler>(
                uniqueIdentifier,
                x => x.Schedule<T>(uniqueIdentifier, desc, action),
                cron,
                TimeZoneInfo.Utc);

            _logger.Info($"Recurring Job {uniqueIdentifier} scheduled to run {cron}");
        }

        public bool IsActive(string uniqueIdentifier)
        {
            using (var connection = JobStorage.Current.GetConnection())
            {
                var entries = connection.GetAllEntriesFromHash("recurring-job:" + uniqueIdentifier);
                if (entries == null)
                {
                    return false;
                }
                else
                {
                    return entries.Any();
                }
            }
        }

        public void Kill(string uniqueIdentifier)
        {
            RecurringJob.RemoveIfExists(uniqueIdentifier);
            _logger.Info($"Recurring Job {uniqueIdentifier} has been killed");
        }
    }
}

