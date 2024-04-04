using Mimeo.Blocks.Helpers;
using Mimeo.Communications.Html.Content.Images;

namespace Mimeo.Communications.Html.Content.Fragments
{
    public class ImageFragment : IFragment
    {
        private readonly ImageEnvelope _imageEnvelope;
        private readonly string _href;
        private readonly string _target;

        public ImageFragment(
                ImageEnvelope imageEnvelope, 
                string href = null, 
                string target = "_blank")
        {
            _imageEnvelope = imageEnvelope;
            _href = href;
            _target = target;
        }

        public List<ImageEnvelope> ImageRefs => new List<ImageEnvelope>() { _imageEnvelope };

        public string ToHtml()
        {
            var output = String.Empty;

            if (_href.HasValue())
            {
                output += $"<a href=\"{_href}\" target=\"{_target}\" border=\"0\">";
            }

            output += this._imageEnvelope.ToHtml();

            if (_href.HasValue())
            {
                output += $"</a>";
            }

            return output;
        }
    }
}

