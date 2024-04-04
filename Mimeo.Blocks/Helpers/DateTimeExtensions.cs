using System;
using System.Globalization;

namespace Mimeo.Blocks.Helpers
{
    public static class DateTimeExtensions
    {
        public static DateTime StartOfWeek(this DateTime dt, DayOfWeek startOfWeek)
        {
            int diff = dt.DayOfWeek - startOfWeek;
            if (diff < 0)
            {
                diff += 7;
            }
            return dt.AddDays(-1 * diff).Date;
        }

        public static string ToStringAlt(this TimeSpan? timeSpan, string alternative = "(empty)")
        {
            return timeSpan.HasValue ? timeSpan.Value.ToString() : alternative;
        }

        public static string ToMonthName(this int month)
        {
            return CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(month);
        }

        public static string ToShortMonthName(this int month)
        {
            return CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(month);
        }

        public static int Quarter(this DateTime input)
        {
            return input.Month.Quarter();
        }

        public static int Quarter(this int month)
        {
            if (month >= 1 && month <= 3)
                return 1;
            if (month >= 4 && month <= 6)
                return 2;
            if (month >= 7 && month <= 9)
                return 3;
            if (month >= 10 && month <= 12)
                return 4;
            throw new ArgumentException();
        }

        // This presumes that weeks start with Monday.
        // Week 1 is the 1st week of the year with a Thursday in it.
        public static int WeekOfYearIso8601(this DateTime time)
        {
            // Seriously cheat.  If its Monday, Tuesday or Wednesday, then it'll 
            // be the same week# as whatever Thursday, Friday or Saturday are,
            // and we always get those right
            DayOfWeek day = CultureInfo.InvariantCulture.Calendar.GetDayOfWeek(time);
            if (day >= DayOfWeek.Monday && day <= DayOfWeek.Wednesday)
            {
                time = time.AddDays(3);
            }

            // Return the week of our adjusted day
            return CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(time, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Sunday);
        }

        public static DateTime DateOnly(this DateTime input)
        {
            return new DateTime(input.Year, input.Month, input.Day);
        }



        public static DateTime FirstOfMonth(this DateTime input)
        {
            return new DateTime(input.Year, input.Month, 1);
        }
        public static DateTime LastOfMonth(this DateTime input)
        {
            return input.FirstOfMonth().AddMonths(1).AddDays(-1);
        }

        public static int QuarterNumber(this DateTime input)
        {
            return (input.Month - 1) / 3 + 1;
        }

        public static DateTime FirstOfQuarter(this DateTime input)
        {
            return new DateTime(input.Year, (input.QuarterNumber() - 1) * 3 + 1, 1);
        }

        public static DateTime LastOfQuarter(this DateTime input)
        {
            return input.FirstOfQuarter().AddMonths(3).AddDays(-1);
        }

        public static DateTime FirstOfYear(this DateTime input)
        {
            return new DateTime(input.Year, 1, 1);
        }

        public static DateTime LastOfYear(this DateTime input)
        {
            return input.FirstOfYear().AddYears(1).AddDays(-1);
        }

        public static string ToIso8601Utc(this DateTime input)
        {
            return input.ToString("s", CultureInfo.InvariantCulture) + "-00:00";
        }

        public static DateTime? ToDateTimeNullable(this string input)
        {
            DateTime output;
            return DateTime.TryParse(input, out output) ? output : (DateTime ?)null;
        }
    }
}
