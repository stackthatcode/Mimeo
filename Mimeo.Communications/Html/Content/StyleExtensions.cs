using Mimeo.Blocks.Helpers;

namespace Mimeo.Communications.Html.Content
{
    public static class StyleExtensions
    {
        public static string ToHtml(this Dictionary<string, string?> input)
        {
            return input
                .Where(x => x.Value.HasValue())
                .Select(x => $"{x.Key}:{x.Value}")
                .ToDelimited("; ");
        }
    }
}
