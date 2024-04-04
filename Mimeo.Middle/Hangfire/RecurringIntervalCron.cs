using System.Collections.Generic;
using System.Linq;

namespace Mimeo.Middle.Hangfire
{
    public enum RecurringInterval
    {
        EveryMinute,
        Every5Minutes,
        Every10Minutes,
        Every15Minutes,
        Every30Minutes,
        EveryHour,
        Every2Hours,
        Every4Hours,
        Every8Hours,
        Every12Hours,
        EveryDayMidnight,
    }

    public class RecurringIntervalCron
    {
        public RecurringInterval Id { get; set; }
        public string Desc { get; set; }
        public string Cron { get; set; }

        public static RecurringIntervalCron Default 
            => Options.First(x => x.Id == RecurringInterval.Every5Minutes);

        public static readonly List<RecurringIntervalCron> Options = new List<RecurringIntervalCron>()
        {
            new RecurringIntervalCron
            {
                Id = RecurringInterval.EveryMinute,
                Desc = "every minute aka real-time",
                Cron = "* * * * *"
            },
            new RecurringIntervalCron
            {
                Id = RecurringInterval.Every5Minutes,
                Desc = "every 5 minutes",
                Cron = "*/5 * * * *"
            },
            new RecurringIntervalCron
            {
                Id = RecurringInterval.Every10Minutes,
                Desc = "every 15 minutes",
                Cron = "*/10 * * * *"
            },
            new RecurringIntervalCron
            {
                Id = RecurringInterval.Every15Minutes,
                Desc = "every 15 minutes",
                Cron = "*/15 * * * *"
            },
            new RecurringIntervalCron
            {
                Id = RecurringInterval.Every30Minutes,
                Desc = "every 30 minutes",
                Cron = "*/30 * * * *"
            },
            new RecurringIntervalCron
            {
                Id = RecurringInterval.EveryHour,
                Desc = "every 1 hour",
                Cron = "0 * * * *"
            },
            new RecurringIntervalCron
            {
                Id = RecurringInterval.Every2Hours,
                Desc = "every 2 hours",
                Cron = "0 * * * *"
            },
            new RecurringIntervalCron
            {
                Id = RecurringInterval.Every4Hours,
                Desc = "every 4 hours",
                Cron = "0 */4 * * *"
            },
            new RecurringIntervalCron
            {
                Id = RecurringInterval.Every8Hours,
                Desc = "every 8 hours",
                Cron = "0 */8 * * *"
            },
            new RecurringIntervalCron
            {
                Id = RecurringInterval.Every12Hours,
                Desc = "every 12 hours",
                Cron = "0 */12 * * *"
            },
            new RecurringIntervalCron
            {
                Id = RecurringInterval.EveryDayMidnight,
                Desc = "once a day at midnight",
                Cron = "0 0 * * *"
            },
        };
    }
}
