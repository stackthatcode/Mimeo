using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;

namespace Mimeo.Blocks.Security
{
    public static class EncodingExtensions
    {
        public static string ToBase64(this string message)
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(message));
        }

        public static string ToBase64(this Bitmap bitmap, ImageFormat imageFormat)
        {
            using (var memoryStream = new MemoryStream())
            {
                // Save the bitmap into the memory stream in PNG format
                //
                bitmap.Save(memoryStream, imageFormat);

                // Convert the memory stream to a byte array
                //
                byte[] imageBytes = memoryStream.ToArray();

                // Convert the byte array to a Base64 string
                //
                return Convert.ToBase64String(imageBytes);
            }
        }

        public static string FromBase64(this string base64message)
        {
            var buffer = Convert.FromBase64String(base64message);
            return System.Text.Encoding.UTF8.GetString(buffer, 0, buffer.Length);
        }
    }
}

