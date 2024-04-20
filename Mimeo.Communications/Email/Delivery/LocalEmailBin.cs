using Mimeo.Blocks.Helpers;
using Mimeo.Blocks.Logging;
using Mimeo.Communications.Html.Content;
using Mimeo.Communications.Html.Content.Images;

namespace Mimeo.Communications.Email.Delivery
{
    public class LocalEmailBin : IEmailDeliveryService
    {
        private readonly string _outputDirectory;
        private readonly MimeoLogger _logger;

        public LocalEmailBin(string localEmailBin, MimeoLogger logger)
        {
            //_outputDirectory = config.LocalEmailBin ?? @"C:\Temp\EmailBin\";
            _logger = logger;
        }


        public async Task<bool> Send(
                List<string> addressees,
                string subject,
                string message,
                List<Attachment> attachments = null)
        {
            // Since images won't be embedded, this test service will save the files and
            // ... reference them locally from the same location as the HTML file
            //
            if (!Directory.Exists(_outputDirectory))
            {
                Directory.CreateDirectory(_outputDirectory);
            }

            //foreach (var image in message.ImageReferences)
            //{
            //    var imagePath = Path.Combine(_outputDirectory, image.FileName);

            //    if (!File.Exists(imagePath))
            //    {
            //        await File.WriteAllBytesAsync(imagePath, image.Image);
            //    }
            //}

            var destination =
                addressees.Count > 1
                    ? "(Multiple recipients)" : addressees.ToCommaDelimited();

            var fileName =
                $"{destination.LettersOrNumbersOnly()} - " +
                $"{subject.IfEmptyAlt("(Empty Subject)").LettersOrNumbersOnly()} - " +
                $"{DateTime.UtcNow.ToString().LettersOrNumbersOnly()}.html";

            var path = Path.Combine(_outputDirectory, fileName);
            _logger.Info($"Test Mode Enabled. Saving email message to {path}. Intended recipients: {destination}");

            // await File.WriteAllTextAsync(path, message.ToHtml());

            return true;
        }


        public Task<bool> Send(List<string> addressees, string subject, string message, List<ImageEnvelope> images = null, List<Attachment> attachments = null)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Send(string addressee, string subject, string message, List<ImageEnvelope> images = null, List<Attachment> attachments = null)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Send(List<string> toList, List<string> bccList, string subject, string message, List<ImageEnvelope> images = null, List<Attachment> attachments = null)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Send(string to, List<string> bccList, string subject, string message, List<ImageEnvelope> images = null, List<Attachment> attachments = null)
        {
            throw new NotImplementedException();
        }
    }
}

