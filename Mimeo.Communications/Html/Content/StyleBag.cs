using Mimeo.Blocks.Helpers;

namespace Mimeo.Communications.Html.Content
{
    public class StyleBag : Dictionary<string, string?>
    {
        public string ToHtml()
        {
            return this
                .Where(x => x.Value.HasValue())
                .Select(x => $"{x.Key}:{x.Value}")
                .ToDelimited("; ");
        }

        public StyleBag MaybeSetRange(StyleBag? input)
        {
            if (input == null)
            {
                return this;
            }

            foreach (var keyValuePair in input)
            {
                this[keyValuePair.Key] = keyValuePair.Value;
            }
            return this;
        }
    }
}
