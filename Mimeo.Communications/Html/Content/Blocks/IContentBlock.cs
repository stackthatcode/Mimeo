using Mimeo.Communications.Html.Content.Images;

namespace Mimeo.Communications.Html.Content.Blocks
{
    public interface IContentBlock
    {
        List<ImageEnvelope> ImageRefs { get; }
        StyleBag Style { get; }
    }
}
