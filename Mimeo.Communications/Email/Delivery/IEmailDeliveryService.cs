using Mimeo.Communications.Html.Content;
using Mimeo.Communications.Html.Content.Images;

namespace Mimeo.Communications.Email.Delivery
{
    public interface IEmailDeliveryService
    {
        Task<bool> Send(
            string to,
            List<string> bccList,
            string subject, 
            string message,
            List<ImageEnvelope> images = null, 
            List<Attachment> attachments = null);
    }
}

