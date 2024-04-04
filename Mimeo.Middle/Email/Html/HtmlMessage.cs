using System.Collections.Generic;

namespace Mimeo.Middle.Email.Html
{
    public class HtmlMessage
    {
        public bool UsesEmbeddedImages { get; set; }
        public string Html { get; set; }
        public List<HtmlImage> ImageReferences { get; set; }

        public HtmlMessage()
        {
            ImageReferences = new List<HtmlImage>();
        }
    }
}
