using System.Drawing;
using System.Drawing.Imaging;
using Mimeo.Blocks.Helpers;
using Mimeo.Blocks.Security;


namespace Mimeo.Communications.Html.Content.Images
{
    public class ImageEnvelope
    {
        public string UniqueId { get; private set; } = Guid.NewGuid().ToString();

        public Bitmap ImageData { get; private set; }
        public ImageTransferMedium TransferMedium { get; set; } = ImageTransferMedium.CidEmbedded;
        public ImageFormat Base64EmbeddedFormat { get; set; } = ImageFormat.Jpeg;
        public string ExternalSrcUrl { get; set; }

        public string Src => TransferMedium switch
        {
            ImageTransferMedium.ExternalSrcUrl => ExternalSrcUrl,
            ImageTransferMedium.CidEmbedded => $"CID:{UniqueId}",

            // Hardcode for now, make configurable later
            //
            _ => $"data:image/jpeg;base64,{ImageData.ToBase64(ImageFormat.Jpeg)}",
        };

        public string Title { get; private set; }
        public string Alt { get; private set; }

        public StyleBag Style { get; private set; } = new StyleBag();


        public ImageEnvelope(
                Bitmap imageData,
                ImageTransferMedium transferMedium,
                string title = null,
                string alt = null)
        {
            ImageData = imageData;
            TransferMedium = transferMedium;
            Title = title;
            Alt = alt;
        }

        public ImageEnvelope SetTitle(string title)
        {
            Title = title;
            return this;
        }

        public ImageEnvelope SetAlt(string alt)
        {
            Alt = alt;
            return this;
        }

        public ImageEnvelope SetStyle(string property, string value)
        {
            Style[property] = value;
            return this;
        }

        public byte[] GetBytes(ImageFormat imageFormat)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                // Save the bitmap to the memory stream, specifying the format you want.
                //
                ImageData.Save(memoryStream, imageFormat); // Or any other format

                // Convert the memory stream to a byte array
                //
                byte[] byteArray = memoryStream.ToArray();

                return byteArray;
            }
        }

        public string ToHtml()
        {
            var output = string.Empty;
            output += $"<img src=\"{Src}\" alt=\"{Alt}\" title=\"{Title}\" style=\"{Style.ToHtml()}\" />";

            return output;
        }
    }
}

