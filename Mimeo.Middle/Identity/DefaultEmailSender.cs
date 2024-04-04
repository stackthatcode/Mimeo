using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.UI.Services;
using Mimeo.Middle.Email.Content;
using Mimeo.Middle.Email.Html;
using Mimeo.Middle.Email.Sending;

namespace Mimeo.Middle.Identity
{
    public class DefaultEmailSender : IEmailSender
    {
        private readonly MessageBuilder _messageBuilder;
        private readonly HtmlTemplateService _templateService;
        private readonly IEmailService _emailService;

        public DefaultEmailSender(
                MessageBuilder messageBuilder,
                HtmlTemplateService templateService,
                IEmailService emailService)
        {
            _messageBuilder = messageBuilder;
            _templateService = templateService;
            _emailService = emailService;
        }


        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var message
                = _messageBuilder
                    .AddLogo()
                    .AddHtmlTextBlock(htmlMessage)
                    .Output();

            var htmlMessagePayload
                = _templateService
                    .Initialize()
                    .Build(message, false);

            _emailService.Send(email, subject, htmlMessagePayload);
        }
    }
}
