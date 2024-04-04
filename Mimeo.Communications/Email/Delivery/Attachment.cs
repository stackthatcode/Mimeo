namespace Mimeo.Communications.Email.Delivery
{
    public class Attachment
    {
        public string FileName { get; set; }
        public byte[] Content { get; set; }

        public Attachment(string fileName, byte[] content)
        {
            FileName = fileName;
            Content = content;
        }
    }
}
