namespace Mimeo.Communications.Html.Content
{
    public class ExtendedProperties : Dictionary<string, string?>
    {
        public ExtendedProperties MaybeSetRange(ExtendedProperties? input)
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
