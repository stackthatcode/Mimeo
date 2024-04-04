using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Mimeo.Blocks.Helpers
{
    public static class NumberHelpers
    {
        public static bool IsDecimal(this string input, int? min = null, int? max = null)
        {
            decimal output;
            var result = decimal.TryParse(input, out output);

            if (!result)
            {
                return false;
            }
            if (min.HasValue && output < min.Value)
            {
                return false;
            }
            if (max.HasValue && output > max.Value)
            {
                return false;
            }
            return true;
        }

        public static bool IsInteger(this string input)
        {
            int dummy;
            return Int32.TryParse(input, out dummy);
        }

        public static int? ToNullableInteger(this string input)
        {
            return input.IsInteger() ? int.Parse(input) : (int?)null;
        }
        
        public static decimal ToDecimal(this string input)
        {
            return decimal.Parse(input);
        }

        public static decimal? ToDecimalNullable(this string input, decimal? altValue = null)
        {
            decimal output;
            return decimal.TryParse(input, out output) ? output : altValue;
        }

        public static decimal ToDecimalAlt(this string input, decimal alternate)
        {
            return input.IsDecimal() ? decimal.Parse(input) : alternate;
        }


        public static int ToInteger(this string input)
        {
            return int.Parse(input);
        }
        
        public static int ToIntegerDurable(this string input, int altValue = 0)
        {
            int output;
            return int.TryParse(input, out output) ? output : altValue;
        }

        public static int ToIntegerAlt(this string input, int altValue)
        {
            return input.IsInteger() ? int.Parse(input) : altValue;
        }


        public static bool IsLong(this string input)
        {
            long dummy;
            return long.TryParse(input, out dummy);
        }

        public static long ToLong(this string input)
        {
            return long.Parse(input);
        }

        public static List<long> ToLongFromCommaDelimited(this string input)
        {
            return input.SplitByCharNonEmpty(',').Select(x => x.Trim().ToLong()).ToList();
        }


        public static long ToLongAlt(this string input, long altValue)
        {
            return input.IsLong() ? long.Parse(input) : altValue;
        }

        public static long? ToLongNullableAlt(this string input, long? altValue = null)
        {
            return input.IsLong() ? long.Parse(input) : altValue;
        }
        
        public static List<string> ToStringFromCommaDelimited(this string input)
        {
            return input.SplitByCharNonEmpty(',').Select(x => x.Trim()).ToList();
        }
    }
}

