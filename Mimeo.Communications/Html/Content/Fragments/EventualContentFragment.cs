using Mimeo.Communications.Html.Content.Images;

namespace Mimeo.Communications.Html.Content.Fragments
{
    public class EventualContentFragment : IFragment
    {
        private readonly HttpClient _httpClient;
        private readonly string _resourceUrl;
        private readonly Func<string, string> _transformer;

        public EventualContentFragment(
                HttpClient httpClient, 
                string resourceUrl, 
                Func<string, string> transformer)
        {
            _httpClient = httpClient;
            _resourceUrl = resourceUrl;
            _transformer = transformer;
        }

        public List<ImageEnvelope> ImageRefs => new List<ImageEnvelope>();

        public string ToHtml()
        {
            throw new NotImplementedException();
        }

    }
}
