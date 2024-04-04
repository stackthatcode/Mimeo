using System.Web;
using Mimeo.Communications.Html.Content.Images;


namespace Mimeo.Communications.Html.Content.Fragments
{
    public class FragmentFactory
    {
        private readonly Func<HttpClient> _httpClientFactory;

        public FragmentFactory(Func<HttpClient> httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public IFragment Html(string html)
        {
            return new HtmlFragment(html);
        }

        public IFragment Text(string text)
        {
            return new HtmlFragment(HttpUtility.HtmlEncode(text));
        }

        public IFragment Image(ImageEnvelope envelope, string href = null)
        {
            return new ImageFragment(envelope, href);
        }

        public IFragment EventualContent(string resourceUrl, Func<string, string> transformer = null)
        {
            return new EventualContentFragment(_httpClientFactory(), resourceUrl, transformer);
        }
    }
}

