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
    public class SampleWorker02
    {
        private readonly FragmentFactory _fragmentFactory;
        private readonly ImageFactoryLocal _imageFactory;
        private readonly HtmlTemplateService _templateService;
        private readonly IIndex<string, MailgunConfig> _configs;
        private readonly Func<MailgunConfig, MailgunApi> _mailgunApiFactory;

        public SampleWorker02(
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
            //var sendTo = "newsletter@mail.republicanpost.net";
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
                .SetLocalDirectory(@"C:\DEV\Mimeo\TestFileStorage\0002\")
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
                    = "Bringing thee news you want right to your inbox";

            content.Add(new SingleBlock(_fragmentFactory.Image(companyLogo)));




            content.Add(new SingleBlock(_fragmentFactory.Html("<hr />")));
            content.Add(new SingleBlock(_fragmentFactory.Html("<h3>Global Plus - Garden &amp; Home</h3>")));

            var frostDamageOnMagnolia
                = _imageFactory
                    .GetImage("FrostDamageOnMagnolia.png")
                    .SetTitle("Frost Damage On Magnolia")
                    .SetAlt("Frost Damage On Magnolia")
                    .SetStyle("max-width", "150px");

            var robinSnow = new DoubleBlock(
                _fragmentFactory.Html(
                    @"<h4>Mindful Gardening</h4>" +
                    @"<p style=""padding-right:20px;"">
                        Early spring flowers demonstrate remarkable resilience to unpredictable weather, including snowfalls 
                        like the 'Robin Snow' seen in Appalachia. These blooms, despite their hardiness, 
                        <a href=""https://yardandgarden.extension.iastate.edu/how-to/cold-and-freeze-damage-garden-plants"">
                        they can still suffer from freezing temperatures</a>, 
                        highlighting the need for mindful gardening practices to protect these early risers.
                    <em>(Yard and Garden)</em>.</p>"),
                _fragmentFactory.Image(frostDamageOnMagnolia,
                    "https://yardandgarden.extension.iastate.edu/how-to/cold-and-freeze-damage-garden-plants"),
                new StyleBag() { { "margin-bottom", "20px" } });
            content.Add(robinSnow);

            var grass
                = _imageFactory
                    .GetImage("grass.jpeg")
                    .SetTitle("Grass That Has Been Cut Too Short")
                    .SetAlt("Grass That Has Been Cut Too Short")
                    .SetStyle("max-width", "150px");

            var shopping = new DoubleBlock(
                _fragmentFactory.Html(
                    @"<h4>Regulated Watering</h4>" +
                    @"<p>
                    Accidentally mowing your lawn too short can stress the grass and expose it to a host of problems, 
                    including reduced photosynthesis, vulnerability to pests, and drought. 
                    However, one step you can take to help your lawn recover is 
                    <a href=""https://lawncarepro.co.uk/cutting-grass-too-short/"">water gently but regularly</a>.
                    This prevents the stems and roots from drying out due to excessive sun exposure <em>(Lawn Care Pro)</em>.
                    </p>"),
                _fragmentFactory.Image(grass,
                    "https://yardandgarden.extension.iastate.edu/how-to/cold-and-freeze-damage-garden-plants"),
                new StyleBag() { { "margin-bottom", "20px" } });
            content.Add(shopping);




            content.Add(new SingleBlock(_fragmentFactory.Html("<hr />")));
            content.Add(new SingleBlock(_fragmentFactory.Html("<h3>Global Plus - Tech Time</h3>")));

            var deepFakeImg
                = _imageFactory
                    .GetImage("DeepFakes.png")
                    .SetTitle("Deepfakes AI/ML/LLMs")
                    .SetAlt("Deepfakes AI/ML/LLMs")
                    .SetStyle("max-width", "150px");

            var deepfakes = new DoubleBlock(
                _fragmentFactory.Html(
                    @"<h4>Can Undetectable Deepfakes Be Avoided</h4>" +

                    @"<p style=""padding-right:20px;"">
                        Tell-tale signs of generative AI images are disappearing as the technology improves, 
                        and experts are scrambling for new methods to counter disinformation.

                        As deepfake technology advances, the challenge of distinguishing between real and 
                        artificially generated content intensifies.

                        <a href=""https://www.theguardian.com/technology/2024/apr/08/time-is-running-out-can-a-future-of-undetectable-deepfakes-be-avoided"">
                        This Article in the Guardian</a> explores the implications in depth. 

                    <em>(The Guardian)</em>.</p>"),
                _fragmentFactory.Image(deepFakeImg,
                    "https://www.theguardian.com/technology/2024/apr/08/time-is-running-out-can-a-future-of-undetectable-deepfakes-be-avoided"),
                new StyleBag() { { "margin-bottom", "20px" } });
            content.Add(deepfakes);


            var freelancerImg
                = _imageFactory
                    .GetImage("Freelancers.png")
                    .SetTitle("Feast And Famine Cycles")
                    .SetAlt("Feast And Famine Cycles")
                    .SetStyle("max-width", "150px");

            var freelancer = new DoubleBlock(
                _fragmentFactory.Html(
                    @"<h4>Freelance Growing Business</h4>

                    <p style=""padding-right:20px;"">
                        Freelancers love the stability and depth of engagement that comes with extended commitments. 
                        Building and maintaining strong relationships enables freelancers to become integral, trusted partners.
                        The transition from short-term gigs to 
                         <a href=""https://blog.xolo.io/freelancers-guide-to-finding-long-term-clients"">
                            getting projects 6 months and beyond</a> is an important turning point
                        in career growth.</a>

                    <em>(Xolo)</em>.</p>"),
                _fragmentFactory.Image(freelancerImg, "https://blog.xolo.io/freelancers-guide-to-finding-long-term-clients"),
                new StyleBag() { { "margin-bottom", "20px" } });
            content.Add(freelancer);



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

