using Mimeo.Communications.Html.Content.Images;

namespace Mimeo.Communications.Html.Content.Fragments
{
    public interface IFragment
    {
        List<ImageEnvelope> ImageRefs { get; }
        string ToHtml();
    }
}
