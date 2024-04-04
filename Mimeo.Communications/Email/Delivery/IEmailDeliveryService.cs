using Mimeo.Communications.Html.Content;
using Mimeo.Communications.Html.Content.Images;

namespace Mimeo.Communications.Email.Delivery
{
    public interface IEmailDeliveryService
    {
        Task<bool> Send(
            List<string> addressees, 
            string subject, 
            string message,
            List<ImageEnvelope> images = null, 
            List<Attachment> attachments = null);

        Task<bool> Send(
            string addressee, 
            string subject, 
            string message,
            List<ImageEnvelope> images = null,
            List<Attachment> attachments = null);
    }
}

