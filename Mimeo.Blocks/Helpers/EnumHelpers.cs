using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Mimeo.Blocks.Helpers
{
    public static class EnumHelpers
    {
        public static string EnumToString<T>(this int input) where T : struct, IConvertible
        {
            return Enum.GetName(typeof(T), (int)input);
        }

        public static T StringToEnum<T>(this string input) where T : struct, IConvertible
        {
            return Enum.Parse<T>(input);
        }


        public static string GetEnumDescription(Enum value)
        {
            var fi = value.GetType().GetField(value.ToString());
            var attributes = fi.GetCustomAttributes(typeof(DescriptionAttribute), false) as DescriptionAttribute[];

            if (attributes != null && attributes.Any())
            {
                return attributes.First().Description;
            }

            return value.ToString();
        }
    }
}
