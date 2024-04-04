using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Mimeo.Blocks.AspNetCore
{
    public static class IQueryCollectionExtensions
    {
        public static IEnumerable<T> All<T>(
            this IQueryCollection collection,
            string key)
        {
            var values = new List<T>();

            if (collection.TryGetValue(key, out var results))
            {
                foreach (var s in results)
                {
                    try
                    {
                        var result = (T)Convert.ChangeType(s, typeof(T));
                        values.Add(result);
                    }
                    catch (Exception)
                    {
                        // conversion failed
                        // skip value
                    }
                }
            }

            // return an array with at least one
            return values;
        }

        public static T Get<T>(
            this IQueryCollection collection,
            string key,
            T @default = default,
            ParameterPick option = ParameterPick.First)
        {
            var values = All<T>(collection, key);
            var value = @default;

            if (values.Any())
            {
                value = option switch
                {
                    ParameterPick.First => values.FirstOrDefault(),
                    ParameterPick.Last => values.LastOrDefault(),
                    _ => value
                };
            }

            return value ?? @default;
        }
    }

    public enum ParameterPick
    {
        First,
        Last
    }
}
