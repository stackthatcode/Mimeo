using System.Security;
using static System.Net.WebRequestMethods;

namespace Mimeo.Communications.Config
{
    public class MailgunConfig
    {
        public string MailgunResource { get; set; }
        public SecureString MailgunApiKey { get; set; }

        public string FromAddress { get; set; } = string.Empty;
        public string ReplyToAddress { get; set; } = string.Empty;
        public string PostmasterAddress { get; set; }
        public string EmailDomain { get; set; }


        // Indeed this will be offloaded to a service that pulls said stuff from Azure Key Vault
        //
        //public static MailgunConfig ReadConfig(
        //        string mailgunResourceUri = null,
        //        string mailgunApiKey_Cipher = null,

        //        // This should allow a certain degree of flexibility
        //        //
        //        string fromAddress = null,
        //        string replyToAddress = null,
        //        string postmasterAddress = null)
        //{
        //    var output = new MailgunConfig();

        //    output.MailgunResource = mailgunResourceUri;
        //    output.MailgunApiKey = mailgunApiKey;
        //    output.MailgunApiKey_Cipher = mailgunApiKey_Cipher;

        //    output.FromAddress = fromAddress;
        //    output.ReplyToAddress = replyToAddress;
        //    output.PostmasterAddress = postmasterAddress;
            
        //    return output;
        //}
    }
}

