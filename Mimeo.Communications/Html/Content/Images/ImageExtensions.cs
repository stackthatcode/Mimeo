namespace Mimeo.Communications.Html.Content.Images
{
    public static class ImageExtensions
    {
        public static List<ImageEnvelope> Merge(
                    this List<ImageEnvelope> list, List<ImageEnvelope> input)
        {
            var output = new List<ImageEnvelope>();
            output.AddRange(list);
            output.AddRange(input);
            return list.DistinctBy(x => x.UniqueId).ToList();
        }
    }
}
