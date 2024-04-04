namespace Mimeo.Blocks.Helpers
{
    public static class HelperMethods
    {

        public static string IfNotNullEmpty(this string input, string output)
        {
            return !input.IsNullOrEmpty() ? output : "";
        }

        public static string WrapInParagraph(this string input)
        {
            return $"<p>{input}</p>";
        }

        public static string AppendOnNonEmpty(this string input, string addition)
        {
            return !input.IsNullOrEmpty() ? input + addition : input;
        }

        public static string PrependOnNonEmpty(this string input, string prepend)
        {
            return !input.IsNullOrEmpty() ? prepend + input : input;
        }

        public static string SafeTrim(this string input)
        {
            return (input ?? "").Trim();
        }

        public static bool IsNullOrEmpty(this string input)
        {
            return string.IsNullOrEmpty(input);
        }

        public static string IfEmptyAlt(this string input, string alternative = StringConsts.None)
        {
            return string.IsNullOrEmpty(input) ? alternative : input;
        }

        public static bool HasValue(this string input)
        {
            return !string.IsNullOrEmpty(input);
        }


        public static bool HasNonZeroValue(this decimal? input)
        {
            return input.HasValue && input.Value != 0m;
        }


        public static bool HasNumericValue(this string input)
        {
            return !string.IsNullOrEmpty(input) && input.NumbersOnly().Length > 0;
        }

        public static string Truncate(this string value, int maxLength)
        {
            if (string.IsNullOrEmpty(value)) return value;
            return value.Length <= maxLength ? value : value.Substring(0, maxLength);
        }
    }
}

