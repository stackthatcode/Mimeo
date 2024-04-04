using System;
using System.Collections.Generic;
using System.IO;
using Mimeo.Blocks.Helpers;
using Mimeo.Blocks.Logging;
using Mimeo.Middle.Config;
using Mimeo.Middle.Email.Html;

namespace Mimeo.Middle.Email.Sending
{
    public class TestEmailService : IEmailService
    {
        private readonly string _outputDirectory;
        private readonly MimeoLogger _logger;

        public TestEmailService(MimeoAppSettings appSettings, MimeoLogger logger)
        {
            _outputDirectory = appSettings.TestEmailOutputDir ?? @"C:\Temp\MimeoTestEmail\";
            _logger = logger;
        }


        public void Send(string addressee, string subject, HtmlMessage message)
        {
            Send(new List<string> {addressee}, subject, message);
        }

        public void Send(List<string> addressees, string subject, HtmlMessage message)
        {
            foreach (var image in message.ImageReferences)
            {
                var imagePath = Path.Combine(_outputDirectory, image.FileName);

                if (!File.Exists(imagePath))
                {
                    File.WriteAllBytes(imagePath, image.Image);
                }
            }

            var destination =
                addressees.Count > 1
                    ? "(Multiple recipients)" : addressees.ToCommaDelimited();

            var fileName =
                $"{destination.LetterOrNumbersOnly()} - " +
                $"{subject.IsNullOrEmptyAlt("(Empty Subject)").LetterOrNumbersOnly()} - " +
                $"{DateTime.UtcNow.ToString().LetterOrNumbersOnly()}.html";

            var path = Path.Combine(_outputDirectory, fileName);
            _logger.Info($"Test Mode Enabled. Saving email message to {path}. Intended recipients: {destination}");

            File.WriteAllText(path, message.Html);
        }

    }
}

