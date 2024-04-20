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
    public class SampleWorker08
    {
        private readonly FragmentFactory _fragmentFactory;
        private readonly ImageFactoryLocal _imageFactory;
        private readonly HtmlTemplateService _templateService;
        private readonly IIndex<string, MailgunConfig> _configs;
        private readonly Func<MailgunConfig, MailgunApi> _mailgunApiFactory;

        public SampleWorker08(
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
            // var sendTo = "newsletter@mail.republicanpost.net";    // Smells like "MITM"
            //// // ### WARNING - LIVE

            // TESTING ONLY
            var sendTo = "aleksjones@gmail.com";
            // TESTING ONLY

            var bccList = new List<string>();
            var subject = "Global Plus News (Subscriber News) - Entertainment";

            AsyncContext.Run(
                () => mailgun.Send(sendTo, bccList, subject, html, contentModel.ImageReferences));
        }


        private ContentModel BuildContent()
        {
            _imageFactory
                .SetLocalDirectory(@"C:\DEV\Mimeo\TestFileStorage\0008\")
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
                .ExtendedProperties[BasicTemplate01.CONTENTMODEL_TITLE] = "Bringing these news you want right to your inbox";
            content.Add(new SingleBlock(_fragmentFactory.Image(companyLogo)));


            content.Add(new SingleBlock(_fragmentFactory.Html("<hr />")));

            //content.Add(new SingleBlock(_fragmentFactory.Html("<h3>Global Plus - Your Resource For World &amp; National News</h3>")));

            //""Auctioneering.png""

            var auctioneering
                = _imageFactory
                    .GetImage("Quiddage.png")
                    .SetTitle("Quiddage")
                    .SetAlt("Quiddage")
                    .SetStyle("max-width", "100%");

            var article0 = new SingleBlock(_fragmentFactory.Image(auctioneering));

            content.Add(article0);

            var article1 = new SingleBlock(
                _fragmentFactory.Html(
@"<h3>Global Plus News: Eclectic Sports</h3>

<p><strong><a href=""https://sportsfoundation.org/unique-sports-list/"">Slackline Yoga:</a></strong>
This sport combines the art of balancing on a slackline with the physical and mental discipline of yoga. Practitioners perform yoga poses on a narrow, slightly elastic band suspended above the ground <em>(SportsFoundation)</em>.</p>

<p><strong><a href=""https://sportsfoundation.org/unique-sports-list/"">Underwater Hockey (Octopush):</a></strong>
Invented in 1954 by British Navy officers, this sport involves two teams of six players who use short sticks to maneuver a puck across the bottom of a swimming pool into the opponents' goal <em>(SportsFoundation)</em>.</p>

<p><strong><a href=""https://sportsfoundation.org/unique-sports-list/"">Quidditch:</a></strong>
Inspired by the Harry Potter series, this mixed-gender sport involves elements of rugby, dodgeball, and tag. Players run with a broomstick between their legs as they attempt to score points through various means <em>(SportsFoundation)</em>.</p>

<p><strong><a href=""https://stacker.com/sports/25-unique-sports-around-world"">Kabaddi:</a></strong>
This contact team sport, popular in South Asia, involves offensive players, known as ""raiders"", attempting to tag as many opponents as possible on the opposing team's half before returning to their side without being tackled <em>(Stacker)</em>.</p>

<p><strong><a href=""https://stacker.com/sports/25-unique-sports-around-world"">Kabaddi:</a></strong>
Originating in Japan, this sport is essentially a competitive snowball fight where two teams of seven players each use 90 premade snowballs to eliminate opponents by hitting them <em>(Stacker)</em>.</p>

<p><strong><a href=""https://americancowboy.com/cowboys-archive/history-bull-riding-pbr/"">Professional Bull Riding History</a></strong> An look at global reach of PBR events, including the significant payouts to riders and stock contractors. It also covers moving World Finals to Fort Worth, Texas, and how bull riding is considered the fastest-growing sport in the U.S <em>(American Cowboy)</em>.</p>
"));

            content.Add(article1);

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

