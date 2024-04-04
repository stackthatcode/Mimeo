using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using static System.Net.Mime.MediaTypeNames;

namespace Mimeo.Blocks.Helpers
{
    public static class StringExtensions
    {
        public static bool StartsWithAny(this string input, params string[] starts)
        {
            return starts.Any(input.StartsWith);
        }

        public static int IndexOfNth(this string str, string value, int nth = 0)
        {
            if (nth < 0)
                throw new ArgumentException("Can not find a negative index of substring in string. Must start with 0");

            int offset = str.IndexOf(value);
            for (int i = 0; i < nth; i++)
            {
                if (offset == -1) return -1;
                offset = str.IndexOf(value, offset + 1);
            }

            return offset;
        }

        public static string RemoveLeftZeroPadding(this string str)
        {
            return str.ToLongAlt(0).ToString();
        }


        public static string SafeSubstring(this string str, int startIndex, int length)
        {
            if (startIndex + 1 > str.Length)
            {
                return String.Empty;
            }

            var correctedLength =
                startIndex + 1 + length > str.Length
                    ? str.Length - startIndex
                    : length;

            return str.Substring(startIndex, correctedLength);
        }

        public static bool IsAllDigits(this string s)
        {
            if (s == null)
            {
                return false;
            }
            foreach (char c in s)
            {
                if (!char.IsDigit(c))
                    return false;
            }
            return true;
        }

        public static List<string> CharDebugDump(this string input)
        {
            return input.ToCharArray().Select(x => (short) x + " " + x).ToList();
        }

        public static string JoinBy(this IEnumerable<string> input, string separator)
        {
            return string.Join(separator, input);
        }

        public static List<string> SplitBy(this string input, string separator)
        {
            return input.Split(new[] { separator }, StringSplitOptions.None).ToList();
        }

        public static List<string> SplitByCharNonEmpty(this string input, char separator)
        {
            return input.Split(separator, StringSplitOptions.None)
                        .Select(x => x.Trim())
                        .Where(x => x != String.Empty)
                        .ToList();
        }

        public static string TruncateAfter(this string input, int length, string terminator = "")
        {
            if (input == null)
            {
                return null;
            }
            else
            {
                return input.Length <= length ? input : input.Substring(0, length) + terminator;
            }
        }

        public static string JoinByNewline(this IEnumerable<string> input)
        {
            return string.Join(Environment.NewLine, input);
        }

        public static string ToCommaDelimited(this IEnumerable<string> input)
        {
            return string.Join(",", input);
        }

        public static string ToDelimited(this IEnumerable<string> input, string delimeter)
        {
            return string.Join(delimeter, input);
        }

        public static string ToNewlineDelimited(this IEnumerable<string> input)
        {
            return string.Join(Environment.NewLine, input);
        }

        public static bool CaselessEquals(this string input, string other)
        {
            return string.Equals(input, other, StringComparison.OrdinalIgnoreCase);
        }


        public static string LettersOnly(this string input)
        {
            return new string((input ?? String.Empty).Where(Char.IsLetter).ToArray()).Trim();
        }

        public static string LettersOrNumbersOnly(this string input)
        {
            return new string((input?? String.Empty).Where(Char.IsLetterOrDigit).ToArray()).Trim();
        }

        public static string NumbersOnly(this string input)
        {
            return new string(input?.Where(Char.IsDigit).ToArray());
        }


        public static List<string> KeywordSanitizeAndSplit(this string terms)
        {
            var keywords
                = terms
                      ?.Trim()
                      .IfEmptyAlt("")
                      .Split(' ')
                      .Where(x => x.Trim() != "")
                      .ToList()
                  ?? new List<string>();

            return keywords;
        }

        public static List<string> FilterIntegers(this List<string> input)
        {
            return input.Where(x => !x.IsInteger()).ToList();
        }

        public static string EmailDomain(this string emailAddress)
        {
            return (emailAddress ?? "").Split('@').LastOrDefault();
        }


        public static string UrlDomain(this string url)
        {
            if (url.IsNullOrEmpty())
            {
                return String.Empty;
            }

            var myUri = new Uri(url);
            var host = myUri.Host;  // host is "www.contoso.com"
            return host.HasValue() ? host.Replace("www.", "", StringComparison.OrdinalIgnoreCase) : String.Empty;
        }

        public static bool IsCommonEmailDomain(this string domain)
        {
            return new[]
            {
                "gmail.com",
                "yahoo.com",
                "hotmail.com",
                "aol.com",
                "comcast.net"
            }.Contains(domain.ToLower());
        }


        public static string SummaryJoin(
                this List<string> input, int limit = 3, string separator = ",", string empty = "(none)")
        {
            if (input.Count == 0)
            {
                return empty;
            }

            var output = input.Take(limit).JoinBy(separator);
            var hidden = input.Count - 1;
            var plural = hidden > 1 ? "s" : "";

            return (input.Count > limit)
                ? $"{output} and {hidden} other{plural}"
                : output;
        }


        public static bool IsValidUsCanadaPhoneOrEmpty(this string input)
        {
            return input.IsNullOrEmpty() || input.IsValidUsCanadaPhone();
        }

        public static bool IsValidUsCanadaPhone(this string input)
        {
            const string matchPhonePattern = @"\(?\d{3}\)?-? *\d{3}-? *-?\d{4}";
            var regEx = new Regex(matchPhonePattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);
            var matches = regEx.Matches(input);
            return matches.Count > 0;
        }

        public static string PhoneFormat(this string input, bool? isUsCanada = null)
        {
            return
                (isUsCanada == true || !isUsCanada.HasValue && input.IsValidUsCanadaPhone())
                    ? input.PhoneFormatUsCanada()
                    : input;
        }

        public static string PhoneFormatUsCanada(this string input)
        {
            return input.HasValue()
                ? $"({input.NumbersOnly().SafeSubstring(0, 3)}) " +
                  $"{input.NumbersOnly().SafeSubstring(3, 3)}-" +
                  $"{input.NumbersOnly().SafeSubstring(6, 4)}".Trim()
                : String.Empty;
        }

        public static bool CoarselyTheSameAs(this string input, string comparison)
        {
            return input.IfEmptyAlt("").Trim() == comparison.IfEmptyAlt("").Trim();
        }
    }
}


