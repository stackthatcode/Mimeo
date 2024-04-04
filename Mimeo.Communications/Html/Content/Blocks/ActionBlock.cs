using Mimeo.Communications.Html.Content.Images;

namespace Mimeo.Communications.Html.Content.Blocks
{
    public class ActionBlock : IContentBlock
    {
        public List<ImageEnvelope> ImageRefs => new List<ImageEnvelope>();
        public StyleBag Style { get; set; } = new StyleBag();

        public string Url { get; set; }
        public string ButtonText { get; set; }
    }
}

