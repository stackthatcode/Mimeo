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
    public class SampleWorker03
    {
        private readonly FragmentFactory _fragmentFactory;
        private readonly ImageFactoryLocal _imageFactory;
        private readonly HtmlTemplateService _templateService;
        private readonly IIndex<string, MailgunConfig> _configs;
        private readonly Func<MailgunConfig, MailgunApi> _mailgunApiFactory;

        public SampleWorker03(
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


            // ### WARNING - LIVE
            //var sendTo = "newsletter@mail.republicanpost.net";    // Smells like "MITM"
            // ### WARNING - LIVE

            // TESTING ONLY
            var sendTo = "aleksjones@gmail.com";
            // TESTING ONLY

            var bccList = new List<string>();
            var subject = "Global Plus News (Subscriber News)";

            AsyncContext.Run(
                () => mailgun.Send(sendTo, bccList, subject, html, contentModel.ImageReferences));
        }


        private ContentModel BuildContent()
        {
            _imageFactory
                .SetLocalDirectory(@"C:\DEV\Mimeo\TestFileStorage\0003\")
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
                .ExtendedProperties[BasicTemplate01.CONTENTMODEL_TITLE] = "Bringing there news you want right to your inbox";
            content.Add(new SingleBlock(_fragmentFactory.Image(companyLogo)));


            content.Add(new SingleBlock(_fragmentFactory.Html("<hr />")));
            content.Add(new SingleBlock(_fragmentFactory.Html("<h3>Global Plus - Internet Security For People Like You</h3>")));

            var mitmImage
                = _imageFactory
                    .GetImage("MITM.png")
                    .SetTitle("Man In The Middle")
                    .SetAlt("Man In The Middle")
                    .SetStyle("max-width", "150px");

            var mitm = new DoubleBlock(
                _fragmentFactory.Html(
                    @"<h4>Man-in-the-Middle Attacks For Non-Geeks</h4>" +
                    @"<p style=""padding-right:20px;"">
                        <a href=""https://www.descope.com/learn/post/mitm-attack"">Man-in-the-Middle (MITM)</a>
                        attacks are a form of cyber eavesdropping where attackers intercept 
                        and possibly alter communications between two parties without their knowledge. 
                        This article covers the basics of MITM attacks, including how they're carried out through 
                        tactics like Wi-Fi eavesdropping and phishing.
                        It also delves into the dangers posed by these attacks, such as data theft and privacy breaches, 
                        and concludes highlighting strategies for detection and prevention <em>(Descope)</em>.
                    </p>"),
                    _fragmentFactory.Image(mitmImage, "https://www.descope.com/learn/post/mitm-attack"),
                    new StyleBag() { { "margin-bottom", "20px" } });

            content.Add(mitm);


            var advanceFeeScam
                = _imageFactory
                    .GetImage("NigerianPrince.png")
                    .SetTitle("Man In The Middle")
                    .SetAlt("Man In The Middle")
                    .SetStyle("max-width", "150px");

            content.Add(new DoubleBlock(
                _fragmentFactory.Html(
                    @"<h4>Unmasking Advance-fee Scams: Your Shield Against Fraud</h4>" +
                    @"<p style=""padding-right:20px;"">
                        <a href=""https://en.wikipedia.org/wiki/Advance-fee_scam"">Advance-fee scams</a> lure individuals with promises of wealth, only to drain their resources 
                        with upfront fees. This concise guide demystifies these schemes, highlighting the craftiness 
                        of fraudsters who vanish after collecting so-called taxes, fees, or bribes. 
                        Embrace empowerment through awareness, recognizing the red flags of such deceit. 
                        Knowledge is power—equip yourself to stand firm against these financial predators
                        <em>(Wikipedia)</em>.
                    </p>"),
                _fragmentFactory.Image(advanceFeeScam, "https://en.wikipedia.org/wiki/Advance-fee_scam"),
                new StyleBag() { { "margin-bottom", "20px" } }));


            var spanishPrisoner
                = _imageFactory
                    .GetImage("SpanishPrisoner.png")
                    .SetTitle("Man In The Middle")
                    .SetAlt("Man In The Middle")
                    .SetStyle("max-width", "150px");

            content.Add(new DoubleBlock(
                _fragmentFactory.Html(
                    @"<h4>The Spanish Prisoner Scam: A Cautionary Tale</h4>" +
                    @"<p style=""padding-right:20px;"">
                        <a href=""https://en.wikipedia.org/wiki/Advance-fee_scam"">The Spanish Prisoner Scam</a>
                        is a classic bait of trickery promising riches for aid, 
                        tracing back to the 18th century. It embodies advance-fee fraud's essence: 
                        pay now for a fortune that never materializes. By learning the intricacies of this 
                        age-old scam, you're not just exploring a piece of history but arming yourself against 
                        modern iterations. Protecting your wealth starts with the right knowledge​.
                        <em>(Wikipedia)</em>.
                    </p>"),
                _fragmentFactory.Image(spanishPrisoner, "https://en.wikipedia.org/wiki/Advance-fee_scam"),
                new StyleBag() { { "margin-bottom", "20px" } }));

            content.Add(mitm);

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

