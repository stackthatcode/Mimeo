using Mimeo.Communications.Html.Content.Images;

namespace Mimeo.Communications.Html.Content.Fragments
{
    public class HtmlFragment : IFragment
    {
        private readonly string _htmlText;

        public HtmlFragment(string htmlText)
        {
            _htmlText = htmlText;
        }

        public List<ImageEnvelope> ImageRefs => new List<ImageEnvelope>();
        
        public string ToHtml()
        {
            return _htmlText;
        }
    }
}
