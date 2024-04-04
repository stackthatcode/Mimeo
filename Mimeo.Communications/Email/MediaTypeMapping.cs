namespace Mimeo.Communications.Email
{
    public static class MediaTypeMapping
    {
        public static string MediaTypeFromFileName(this string fileName)
        {
            var extension = System.IO.Path.GetExtension(fileName);

            if (extension.ToUpper() == "JPG" || extension.ToUpper() == "JPEG")
            {
                return "image/jpeg";
            }
            if (extension.ToUpper() == "PNG")
            {
                return "image/png";
            }
            if (extension.ToUpper() == "GIF")
            {
                return "image/gif";
            }

            throw new NotImplementedException();
        }
    }
}
