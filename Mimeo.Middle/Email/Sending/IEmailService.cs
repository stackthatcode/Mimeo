using System.Collections.Generic;
using Mimeo.Middle.Email.Html;

namespace Mimeo.Middle.Email.Sending
{
    public interface IEmailService
    {
        void Send(List<string> addressees, string subject, HtmlMessage message);
        void Send(string addressee, string subject, HtmlMessage message);
    }
}

