using System;
using System.Globalization;

namespace Mimeo.Blocks.Helpers
{
    public static class TimeFormatExtensions
    {
        public static string ToFormattedString(this TimeSpan ts)
        {
            return $"{ts.Hours:00}:{ts.Minutes:00}:{ts.Seconds:00}.{ts.Milliseconds/10:00}";
        }

        public static string ToStringWithMillis(this DateTime input)
        {
            return input.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);
        }

        public static string To12HourString(this TimeSpan? input)
        {
            return input == null ? null : new DateTime(input.Value.Ticks).ToString("h:mm tt");
        }

        public static TimeSpan? ToTimeSpan(this string time12HourFormat)
        {
            if (time12HourFormat.IsNullOrEmpty())
            {
                return null;
            }
            else
            {
                var parsedDateTime = DateTime.Parse($"1/1/2001 {time12HourFormat}");
                return new TimeSpan(parsedDateTime.Hour, parsedDateTime.Minute, 0);
            }
        }
    }
}
