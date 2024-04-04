using System.Drawing;

namespace Mimeo.Communications.Html.Content.Images
{
    public class ImageFactoryLocal
    {
        public string LocalDirectory { get; set; } = ".";
        public ImageTransferMedium DefaultTransferMedium { get; set; } = ImageTransferMedium.Base64Embedded;

        public ImageFactoryLocal SetLocalDirectory(string directory)
        {
            if (!Directory.Exists(directory))
            {
                throw new Exception("Invalid directory: " + directory);
            }
            LocalDirectory = directory;
            return this;
        }

        public ImageFactoryLocal SetDefaultTransferMedium(ImageTransferMedium transferMedium)
        {
            DefaultTransferMedium = transferMedium;
            return this;
        }

        // Yes, I'd like a layer of abstraction around Bitmap, but that can wait
        //
        public ImageEnvelope GetImage(string fileName)
        {
            var path = Path.Combine(LocalDirectory, fileName);
            var output = new Bitmap(path);
            return new ImageEnvelope(output, DefaultTransferMedium);
        }
    }
}

