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
    public class SampleWorker15
    {
        private readonly FragmentFactory _fragmentFactory;
        private readonly ImageFactoryLocal _imageFactory;
        private readonly HtmlTemplateService _templateService;
        private readonly IIndex<string, MailgunConfig> _configs;
        private readonly Func<MailgunConfig, MailgunApi> _mailgunApiFactory;

        public SampleWorker15(
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
            //var sendTo = @"newsletter@mail.republicanpost.net";    // Smells like "MITM"
            //var sendTo = @"unsubscribe-AQ2WSTeEe7-tuU9SJ3SMR-zdaZd@win.donaldjtrump.com";
            //// // ### WARNING - LIVE

            // TESTING ONLY
            var sendTo = "aleksjones@gmail.com";
            // TESTING ONLY

            var bccList = new List<string>();
            var subject = "Global Plus News (Subscriber News) - [Emissions Reduction]";

            AsyncContext.Run(
                () => mailgun.Send(sendTo, bccList, subject, html, contentModel.ImageReferences));
        }


        private ContentModel BuildContent()
        {
            _imageFactory
                .SetLocalDirectory(@"C:\DEV\Mimeo\TestFileStorage\0015\")
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
                .ExtendedProperties[BasicTemplate01.CONTENTMODEL_TITLE]
                    = "Bringing their news you want right to your inbox";
            content.Add(new SingleBlock(_fragmentFactory.Image(companyLogo)));



            content.Add(new SingleBlock(_fragmentFactory.Html("<hr />")));

            content.Add(new SingleBlock(
                _fragmentFactory.Html("<h3>Global Plus - Your Resource For World &amp; National News</h3>")));


            var article3 = new SingleBlock(
                _fragmentFactory.Html($@"<div style=""font-size:17px; line-height:30px;"">

<p>Get ready to dream big because, the future of flying is going to be awesome—and super clean!</p>

<p>For a long time, big countries and <strong>OPEC</strong> have controlled oil, tell us how much we going pay for fuel.</p>

<p>This affected air travel in a bad way.</p>

<p>This made things tough for people everywhere, especially the U.S.!</p>

<p>But (and it's a big <u>but</u>) here’s the exciting news: we can change this story.</p>

<p>Thanks to advances in <em>drone technology</em>, we're using electrification to fight aviation gas prices.</p>

<p>With electrification, we don’t pollute our air at all.</p>

<p>We can finally <u>kick the habit</u>, and say goodbye to dominance from OPEC and other big countries.</p>

<p>Isn't that great???</p>

<p>And guess what? Every day, these technologies are getting better, smarter, and cheaper.</p>

<p>Staggering geniuses like Elon Musk spent years flighting to make Lithium batteries ready for real world user.
We need to be thankful for Elon.
</p>

<p>Let's get excited about making our skies and our planet healthier.</p>

<p>We’re creating cool new jobs and showing the world how innovative we can be.</p>

<p>Plus, we can be the bosses of our own energy use, not letting other countries decide things for us.</p>

<p><u>We</u> are the bosses of our <u>future progress</u>.</p>

<p>Let’s fly into the future with clean skies and cool tech.</p>

<p>Are you with me? Let's do this and make our world a better place, <u>one flight at a time</u>!</p>

<p><strong>Keep dreaming and flying high</strong></p>

<h3><a href=""https://act.earthjustice.org/"">Learn More at Earth Justice</a></h3>
</div>
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

