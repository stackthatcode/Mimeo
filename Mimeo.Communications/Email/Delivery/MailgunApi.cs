using System.Drawing.Imaging;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using Mimeo.Blocks.Helpers;
using Mimeo.Blocks.Http;
using Mimeo.Blocks.Logging;
using Mimeo.Blocks.Security;
using Mimeo.Communications.Config;
using Mimeo.Communications.Html.Content;
using Mimeo.Communications.Html.Content.Images;


namespace Mimeo.Communications.Email.Delivery
{
    public class MailgunApi : IEmailDeliveryService
    {
        private readonly MailgunConfig _config;
        private readonly MimeoLogger _logger;
        private readonly ExecutorFactory _executorFactory;

        private const string ServiceUrl = "https://api.mailgun.net/v3/";
        private const int ThrottlingDelayMs = 1000;
        private const int TimeoutMs = 30000;
        private const int MaxAttempts = 2;

        // TODO - replace with the Polly library
        //
        private FaultTolerantExecutor _executor;
        private HttpClient _httpClient;

        public MailgunApi(
                MailgunConfig config,
                MimeoLogger logger,
                ExecutorFactory executorFactory)
        {
            _config = config;
            _logger = logger;
            _executorFactory = executorFactory;
        }


        public void Initialize()
        {
            _executor = _executorFactory.Make(MaxAttempts, ThrottlingDelayMs, ServiceUrl);

            _httpClient
                = new HttpClient()
                {
                    BaseAddress = new Uri(ServiceUrl),
                    DefaultRequestHeaders =
                    {
                        Accept =
                        {
                            MediaTypeWithQualityHeaderValue.Parse("application/json")
                        },
                    }
                };

            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            // This is a hack to allow the use of a self-signed certificate
            // ... I suspect we don't want to do this (?)
            //
            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

            var byteArray = Encoding.ASCII.GetBytes(BasicCredsFactory());
            var base64EncodedCreds = Convert.ToBase64String(byteArray);

            _httpClient
                .DefaultRequestHeaders
                .Authorization = new AuthenticationHeaderValue("Basic", base64EncodedCreds);

            _httpClient.Timeout = new TimeSpan(0, 0, 0, TimeoutMs);
        }

        public string BasicCredsFactory()
        {
            var output = $"api:{_config.MailgunApiKey.ToInsecureString()}";
            return output;
        }


        public async Task<bool> Send(
                string to, 
                string subject, 
                string message,
                List<ImageEnvelope> images = null,
                List<Attachment> attachments = null)
        {
            return await Send(new List<string> { to }, subject, message, images, attachments);
        }

        public async Task<bool> Send(
                List<string> toList, 
                string subject, 
                string message, 
                List<ImageEnvelope> images = null,
                List<Attachment> attachments = null)
        {
            if (_httpClient == null)
            {
                Initialize();
            }

            var request = new MultipartFormDataContent();

            foreach (var to in toList)
            {
                request.Add(new StringContent(to), "to");
            }

            //request.Add(new StringContent(_config.PostmasterAddress), "bcc");
            //request.Add(new StringContent(_config.EmailDomain), "domain");

            request.Add(new StringContent(_config.FromAddress), "from");
            request.Add(new StringContent(_config.ReplyToAddress), "h:Reply-To");
            request.Add(new StringContent(subject), "subject");
            request.Add(new StringContent(message), "html");

            if (images != null)
            {
                foreach (var image in images.Where(x => x.TransferMedium == ImageTransferMedium.CidEmbedded))
                {
                    var imageContent = new ByteArrayContent(image.GetBytes(ImageFormat.Jpeg));
                    imageContent.Headers.ContentType = MediaTypeHeaderValue.Parse("image/jpeg");
                    request.Add(imageContent, "inline", image.UniqueId);
                }
            }

            if (attachments != null)
            {
                foreach (var attachment in attachments)
                {
                    var memoryStream
                        = new MemoryStream(attachment.Content, 0, attachment.Content.Length);

                    var fileContent = new StreamContent(memoryStream);
                    request.Add(fileContent, "attachment", attachment.FileName);
                }
            }

            var response = await _executor.Run(() => _httpClient.PostAsync(_config.MailgunResource, request));

            if (response.StatusCode == HttpStatusCode.OK)
            {
                _logger.Info($"Message sent to Mailgun - " +
                            $"recipients: {toList.ToCommaDelimited()} - " +
                            $"subject: {subject}");
                return true;
            }
            else
            {
                var contents = await response.Content.ReadAsStringAsync();
                var entry = $"Failed Mailgun call: {response.StatusCode} - {contents}";
                _logger.Error(entry);
                return false;
            }
        }

    }
}

