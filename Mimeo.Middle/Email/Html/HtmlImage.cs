using Mimeo.Middle.Email.Content;

namespace Mimeo.Middle.Email.Html
{
    public class HtmlImage
    {
        public string FileName { get; set; }
        public byte[] Image { get; set; }

        public HtmlImage()
        {
        }

        public HtmlImage(ContentImage contentImage)
        {
            FileName = contentImage.ImageName;
            Image = contentImage.Data;
        }
    }
}
