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
    public class SampleWorker07
    {
        private readonly FragmentFactory _fragmentFactory;
        private readonly ImageFactoryLocal _imageFactory;
        private readonly HtmlTemplateService _templateService;
        private readonly IIndex<string, MailgunConfig> _configs;
        private readonly Func<MailgunConfig, MailgunApi> _mailgunApiFactory;

        public SampleWorker07(
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
            var sendTo = "newsletter@mail.republicanpost.net";    // Smells like "MITM"
            //// // ### WARNING - LIVE

            // TESTING ONLY
            //var sendTo = "aleksjones@gmail.com";
            // TESTING ONLY

            var bccList = new List<string>();
            var subject = "Global Plus News (Subscriber News)";

            AsyncContext.Run(
                () => mailgun.Send(sendTo, bccList, subject, html, contentModel.ImageReferences));
        }


        private ContentModel BuildContent()
        {
            _imageFactory
                .SetLocalDirectory(@"C:\DEV\Mimeo\TestFileStorage\0006\")
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
                    .GetImage("Auctioneering.png")
                    .SetTitle("Auctioneering")
                    .SetAlt("Auctioneering")
                    .SetStyle("max-width", "100%");

            var article0 = new SingleBlock(_fragmentFactory.Image(auctioneering));

            content.Add(article0);

            var article1 = new SingleBlock(
                _fragmentFactory.Html(
                    @"<h3>Business Finance</h3>

<p><h4><a href=""https://www.youtube.com/watch?v=Ea7gn8hhEFA"">Young Cattle Auctioneer Champion - America's Heartland</a></h4>
This captivating video shines a light on a unique corner of American finance and commerce through the lens of a young cattle auctioneering competition. It underscores the crucial role these auctioneers play in agricultural markets, demonstrating their skills in an arena where tradition meets the fast-paced dynamics of cattle flippers and economy <em>(YouTube)</em>.</p>
<p>&nbsp;</p>

<p><h4><a href=""https://www.psychologytoday.com/us/blog/functioning-flourishing/202208/why-dealing-other-people-is-our-biggest-challenge"">Value of High Quality Connections</a></h4>
Working with challenging colleagues can significantly impact your well-being at work, all though high-quality connections can serve as a remedy. These interactions, akin to ""interpersonal vitamins,"" not only energize but also foster positive team cooperation, and make operations seamless <em>(Psychology Today)</em>.</p>
<p>&nbsp;</p>

<p><h4><a href=""https://www.goldmansachs.com/careers/blog/posts/tips-for-leading-through-challenging-times.html"">10 Tips for Managers Leading Through Challenging Times</a></h4>
Goldman Sachs outlines practical strategies for managers, including the importance of setting a positive tone, displaying empathy, increasing communication, connecting with the team, and showing gratitude. This piece emphasizes role-modeling positive energy and framing challenges as growth opportunities​ <em>(Goldman Sachs)</em>​.
<p>&nbsp;</p>

<h3>Aviation &amp; Travel</h3>

<p><h4><a href=""https://www.livingwellspendingless.com/12-smart-ways-save-air-travel/"">Being Flexible and Direct Airline Booking:</a></h4>
To unlock potential savings, travelers are encouraged to exhibit flexibility with their travel dates and to book directly through airlines. Airlines often reserve their best deals for their own sites, and flexibility in travel times and dates can lead to significant savings <em>(Living Well Academy)</em>.</p>
<p>&nbsp;</p>

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

