using Mimeo.Blocks.Helpers;
using Mimeo.Communications.Html.Content.Images;


namespace Mimeo.Communications.Html.Content.Fragments
{
    public static class FragmentExtensions
    {
        public static string ToHtml(this IList<IFragment> fragments)
        {
            return fragments.Select(x => x.ToHtml()).JoinBy(" ");
        }

        public static IList<IFragment> FluentAdd(
                this IList<IFragment> fragments, IFragment fragment)
        {
            fragments.Add(fragment);
            return fragments;
        }

        public static List<IFragment> ToListOfFragments(this IFragment fragment)
        {
            return new List<IFragment>() { fragment };
        }

        public static List<ImageEnvelope> ImageRefs(this IList<IFragment> fragments)
        {
            return fragments
                .SelectMany(x => x.ImageRefs)
                .DistinctBy(x => x.UniqueId)
                .ToList();
        }
    }
}
