using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Mimeo.Blocks.Helpers;

namespace Mimeo.Blocks.Http
{
    public class QueryStringBuilder
    {
        public readonly Dictionary<string, string> Dictionary = new Dictionary<string, string>();

        public QueryStringBuilder Add(string key, object value)
        {
            Dictionary[key] = value is DateTime ? ((DateTime)value).ToIso8601Utc() : value.ToString();
            return this;
        }

        public QueryStringBuilder Add(QueryStringBuilder builder)
        {
            foreach (var pair in builder.Dictionary)
            {
                Dictionary[pair.Key] = pair.Value;
            }
            return this;
        }

        public override string ToString()
        {
            var array =
                Dictionary.Select(
                        x => $"{HttpUtility.UrlEncode(x.Key)}={HttpUtility.UrlEncode(x.Value)}")
                    .ToArray();

            return string.Join("&", array);
        }
    }
}
