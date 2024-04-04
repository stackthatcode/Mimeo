//using Mimeo.Blocks.Logging;
//using Mimeo.Communications.Email.Content;
//using Mimeo.Communications.Email.Html;

//namespace Mimeo.Communications.Email.Delivery
//{
//    public class MicrosoftGraphApi : IEmailDeliveryService
//    {
//        private readonly GraphServiceClient _graphServiceClient;
//        private readonly MimeoLogger _logger;

//        public MicrosoftGraphApi(GraphServiceClient graphServiceClient, MimeoLogger logger)
//        {
//            _graphServiceClient = graphServiceClient;
//            _logger = logger;
//        }


//        public async Task<bool> Send(
//                List<string> addressees, string subject, HtmlContent message,
//                List<Attachment> attachments = null)
//        {
//            var currentUser = await _graphServiceClient.Me.Request().GetAsync();

//            var graphMessage = new Message();
//            graphMessage.ToRecipients =
//                addressees
//                    .Select(x => new Recipient()
//                    {
//                        EmailAddress = new EmailAddress
//                        {
//                            Address = x
//                        }
//                    }).AsList();
//            graphMessage.Subject = subject;
//            graphMessage.Body = new ItemBody()
//            {
//                ContentType = BodyType.Html,
//                Content = message.Html,
//            };
//            graphMessage.From =
//                new Recipient()
//                {
//                    EmailAddress = new EmailAddress
//                    {
//                        Address = currentUser.Mail
//                    }
//                };
//            graphMessage.ReplyTo = new List<Recipient>()
//                {
//                    new Recipient()
//                    {
//                        EmailAddress = new EmailAddress
//                        {
//                            Address = currentUser.Mail
//                        }
//                    }
//                };

//            graphMessage.Attachments = new MessageAttachmentsCollectionPage();

//            if (attachments != null)
//            {
//                attachments.ForEach(x =>
//                {
//                    graphMessage.Attachments.Add(new FileAttachment
//                    {
//                        Name = x.FileName,
//                        ContentBytes = x.Content,
//                    });
//                });
//            }

//            if (message.ImageReferences != null)
//            {
//                //message.ImageReferences.ForEach(x =>
//                //{
//                //    graphMessage.Attachments.Add(new FileAttachment
//                //    {
//                //        Name = x.FileName,
//                //        ContentId = x.FileName,
//                //        ContentBytes = x.Image,
//                //        ODataType = "#microsoft.graph.fileAttachment",
//                //        ContentType = "image/jpeg",
//                //        IsInline = true,
//                //    });
//                //});
//            }

//            var request = _graphServiceClient.Me.SendMail(graphMessage);
//            await request.Request().PostAsync();
//            return true;
//        }

//        public async Task<bool> Send(
//                string addressee, string subject, HtmlContent message,
//                List<Attachment> attachments = null)
//        {
//            return await Send(new List<string> { addressee }, subject, message, attachments);
//        }
//    }
//}

