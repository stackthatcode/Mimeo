using System;
using System.Collections.Generic;
using System.Linq;

namespace Mimeo.Blocks.Helpers
{
    public static class LinqExtensions
    {
        public static Dictionary<K,V> ToDictionary<K, V>(this IList<V> input, Func<V, K> keyExtractor)
        {
            var output = new Dictionary<K, V>();
            foreach (var item in input)
            {
                output.Add(keyExtractor(item), item);
            }
            return output;
        }

        public static int NextIndexOf<T>(this IList<T> input, Func<T, bool> test, int startingIndex)
        {
            var index = startingIndex;
            while (index < input.Count)
            {
                if (test(input[index]))
                {
                    return index;
                }

                index++;
            }

            return -1;
        }

        public static List<T> ElementsFrom<T>(this IList<T> input, int startingIndex)
        {
            var output = new List<T>();
            var index = startingIndex;
            while (index < input.Count)
            {
                output.Add(input[index++]);
            }

            return output;
        }

        public static List<T> TakeAfter<T>(this IList<T> input, T element)
        {
            var output = new List<T>();
            var index = input.IndexOf(element) + 1;

            while (index < input.Count)
            {
                output.Add(input[index++]);
            }

            return output;
        }

        public static List<T> AddRangeFluent<T>(this List<T> input, IEnumerable<T> newRange)
        {
            if (newRange != null)
            {
                input.AddRange(newRange);
            }

            return input;
        }

        public static T SafeGetByIndex<T>(this List<T> input, int index) where T : class
        {
            return index < input.Count ? input[index] : null;
        }

        public static List<T> FluentAppend<T>(this IList<T> input, IEnumerable<T> append)
        {
            var output = new List<T>();
            output.AddRange(input);
            output.AddRange(append);
            return output;
        }
    }
}
