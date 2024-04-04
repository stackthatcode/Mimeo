namespace Mimeo.Blocks.Helpers
{
    public static class BoolExtensions
    {
        public static string ToYesNo(this bool input)
        {
            return input ? "YES" : "NO";
        }
        public static string ToYesNo(this bool? input)
        {
            return input.HasValue ? input.Value.ToYesNo() : "";
        }

        public static int ToInteger(this bool input)
        {
            return input ? 1 : 0;
        }
    }
}
