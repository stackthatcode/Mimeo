using Autofac.Features.Indexed;
using Mimeo.Blocks.Security;
using Mimeo.Communications.Config;
using Mimeo.Communications.Email.Delivery;
using Mimeo.Communications.Html;
using Mimeo.Communications.Html.Content;
using Mimeo.Communications.Html.Content.Blocks;
using Mimeo.Communications.Html.Content.Fragments;
using Mimeo.Communications.Html.Content.Images;
using Mimeo.Communications.Html.Template;
using Nito.AsyncEx;
using System.Security.Cryptography.Xml;


namespace Mimeo.ConsoleApp.TestWorkers
{
    public class SampleWorker13
    {
        private readonly FragmentFactory _fragmentFactory;
        private readonly ImageFactoryLocal _imageFactory;
        private readonly HtmlTemplateService _templateService;
        private readonly IIndex<string, MailgunConfig> _configs;
        private readonly Func<MailgunConfig, MailgunApi> _mailgunApiFactory;

        public SampleWorker13(
                HtmlTemplateService templateService,
                ImageFactoryLocal imageFactory,
                FragmentFactory fragmentFactory,
                IIndex<string, MailgunConfig> configs,
                Func<MailgunConfig, MailgunApi> mailgunApiFactory)
        {
            _templateService = templateService;
            _imageFactory = imageFactory;
            _fragmentFactory = fragmentFactory;
            _configs = configs;
            _mailgunApiFactory = mailgunApiFactory;
        }


        public void TestEmailRun()
        {
            var contentModel = BuildContent();
            var html = _templateService.GenerateHtml(contentModel, new BasicTemplate01());
            File.WriteAllText(@"C:\DEV\Mimeo\TestOutput\TestEmail.html", html);

            var config = _configs[MailgunConfigIds.Config0001];
            var mailgun = _mailgunApiFactory(config);


            //// // ### WARNING - LIVE
            //var sendTo = @"newsletter@mail.republicanpost.net";    // Smells like "MITM"var sendTo = @"unsubscribe-AQ2WSTeEe7-tuU9SJ3SMR-zdaZd@win.donaldjtrump.com";
            //// // ### WARNING - LIVE

            // TESTING ONLY
            var sendTo = "aleksjones@gmail.com";
            // TESTING ONLY

            var bccList = new List<string>();
            var subject = "Global Plus News (Subscriber News) - [Technology and Culture]";

            AsyncContext.Run(
                () => mailgun.Send(sendTo, bccList, subject, html, contentModel.ImageReferences));
        }


        private ContentModel BuildContent()
        {
            _imageFactory
                .SetLocalDirectory(@"C:\DEV\Mimeo\TestFileStorage\0013\")
                .SetDefaultTransferMedium(ImageTransferMedium.CidEmbedded);
            //.SetDefaultTransferMedium(ImageTransferMedium.Base64Embedded);

            var content = new List<IContentBlock>();

            var companyLogo
                = _imageFactory
                    .GetImage("GlobalPlusBlackLogo.png")
                    .SetTitle("Global Plus New - Media Company")
                    .SetAlt("Global Plus New - Media Company")
                    .SetStyle("max-width", "100%");

            var contentModel = new ContentModel();
            contentModel
                .ExtendedProperties[BasicTemplate01.CONTENTMODEL_TITLE] = "Bringing thee news you want right to your inbox";
            content.Add(new SingleBlock(_fragmentFactory.Image(companyLogo)));


            var url = "https://twitter.com/tonyzzhao/status/1780263497584230432";
            var robot
                = _imageFactory
                    .GetImage("Robot.png")
                    .SetTitle("That's YOUR Robot")
                    .SetAlt("That's YOUR Robot")
                    .SetStyle("max-width", "100%");

            content.Add(new SingleBlock(_fragmentFactory.Image(robot, url)));

            content.Add(new SingleBlock(_fragmentFactory.Html("<hr />")));

            content.Add(new SingleBlock(
                _fragmentFactory.Html("<h3>Global Plus - Your Resource For World &amp; National News</h3>")));


            var article3 = new SingleBlock(
                _fragmentFactory.Html(@"
    <h1 style=""color: #333; text-align: center;"">🌟🤖 Unleash the Future with Aloha Unleashed! 🤖🌟</h1>
    <p style=""font-size: 16px; color: #555;"">
        Hey there Superstar!
    </p>
    <p style=""font-size: 16px; color: #555;"">
        Are you ready to dive into the world of robots like never before? 🚀 Let me introduce you to the most incredible, jaw-dropping, and ultra-cool robot on the planet: <strong>Aloha Unleashed!</strong> This isn’t just any robot—it’s your new best friend powered by the super-smart brains at Google DeepMind! 🧠💡
    </p>
    <p style=""font-size: 16px; color: #555;"">
        Imagine having a robot that can learn, play, and explore the world with you. Whether it's solving puzzles, playing games, or even helping out with homework, Aloha Unleashed can do it all. It's like having a superhero sidekick right in your living room! 🦸🦸‍♂️
    </p>
    <h2 style=""color: #333;"">Why is Aloha Unleashed so awesome?</h2>
    <ul style=""font-size: 16px; color: #555;"">
        <li><strong>Super Smart:</strong> Trained by the brilliant minds at Google DeepMind, this robot learns faster than a cheetah runs!</li>
        <li><strong>Always Learning:</strong> Just like you, Aloha Unleashed loves to learn new things every day. The more you play, the smarter it gets!</li>
        <li><strong>Fun and Friendly:</strong> With Aloha Unleashed, every day is an adventure. It’s designed to be your perfect robotic buddy who never gets tired of playing.</li>
    </ul>
    <p style=""font-size: 16px; color: #555;"">
        So, are you ready to meet your future best friend? Get ready for endless fun and a peek into the future of technology with your very own Aloha Unleashed. It’s more than just a robot—it’s a gateway to new discoveries and endless possibilities! 🌌✨
    </p>
    <p style=""font-size: 16px; color: #555;"">
        Don’t miss out on the most thrilling adventure of your life. Ask your parents about Aloha Unleashed today and step into the future! Let the excitement begin! 🎉👾
    </p>
    <p style=""text-align: center; font-size: 16px; color: #555;"">
        See you in the future,<br>
        The Aloha Unleashed Team
    </p>

    <h3><a href=""https://twitter.com/tonyzzhao/status/1780263497584230432"">CLICK HERE TO SEE GENIUS IN ACTION</a></h3>

"));
            content.Add(article3);



            contentModel.AddContent(content);

            contentModel.Footer = new SingleBlock(_fragmentFactory.Html(
                @"<p>Global News Plus is technology-digital news aggregation company serving niche needs of our customers 
                with curated news feeds provided to our subscribers sourced from both, internal and external news providers.
                </p>
                <p>At Global Plus News, our commitment goes beyond merely aggregating news; we're dedicated to enhancing your understanding of the world. 
                By leveraging cutting-edge technology and insightful analytics, 
                we tailor our news delivery to match the specific interests and demands of our discerning audience. 
                Our curated feeds ensure that you stay informed with the most relevant and impactful stories, 
                spanning a broad array of categories from technology to global events, without the overwhelm of unnecessary information. 
                Our team meticulously selects content from a diverse range of reputable internal and external sources, 
                ensuring you receive not just news, but a comprehensive worldview.
                </p>
                <p>
                As subscribers to Global Plus News, you are at the center of everything we do. Your informed perspective is our greatest achievement, and we're constantly innovating to serve you better. 
                Trust us to keep you ahead of the curve, with access to exclusive insights and a seamless reading experience tailored just for you.
                </p>
                <p>Thank you for choosing Global Plus News. Together, let's redefine the way we stay informed about the world. 
                    Contact info@globalplus.new if you'd like to change your online subscription settings.</p>"
                //@"<p style=\"word-wrap:break-word;\">"
                //+ @".".ToBase64() + "</p>"
                ));

            return contentModel;
        }
    }
}

