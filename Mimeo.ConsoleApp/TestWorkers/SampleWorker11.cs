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
    public class SampleWorker11
    {
        private readonly FragmentFactory _fragmentFactory;
        private readonly ImageFactoryLocal _imageFactory;
        private readonly HtmlTemplateService _templateService;
        private readonly IIndex<string, MailgunConfig> _configs;
        private readonly Func<MailgunConfig, MailgunApi> _mailgunApiFactory;

        public SampleWorker11(
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
            var subject = "Global Plus News (Subscriber News) - [American Culture]";

            AsyncContext.Run(
                () => mailgun.Send(sendTo, bccList, subject, html, contentModel.ImageReferences));
        }


        private ContentModel BuildContent()
        {
            _imageFactory
                .SetLocalDirectory(@"C:\DEV\Mimeo\TestFileStorage\0011\")
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

            content.Add(new SingleBlock(
                _fragmentFactory.Html("<h3>Global Plus - Your Resource For World &amp; National News</h3>")));



            var dtcImage = _imageFactory
                    .GetImage("DTCs.png")
                    .SetTitle("The impact of Digital Transformation Companies")
                    .SetAlt("The impact of Digital Transformation Companies")
                    .SetStyle("max-width", "100%");

            var article2 = new SingleBlock(_fragmentFactory.Image(dtcImage));
            content.Add(article2);

            var article3 = new SingleBlock(
                _fragmentFactory.Html(@"
<p><h4>Digitial Transformation Companies vs Enterprise Back End Devs</h4>

When choosing between hiring digital transformation companies (DTCs) and enterprise back-end software developers, look at the <em>scope</em> of your project. DTCs like Cognizant and Deloitte offer comprehensive services for wide-ranging digital overhauls. They are ideal for enterprises seeking to integrate tech across all operations.
<br /> <br />

Contrast that with enterprise back-end developers. They focus on specific technical needs like server-side development and database management. They're suitable for projects where there's a need for deep technical expertise. 
<a href=""https://www.eweek.com/it-management/digital-transformation-companies/"">(eWEEK)</a>
<a href=""https://www.hackerrank.com/blog/back-end-development-trends/"">(HackerRank)</a>.
</p>
"));
            content.Add(article3);




            var asianBuns
                = _imageFactory
                    .GetImage("HotAsianBuns.png")
                    .SetTitle("Lovers of Hot Asian Buns Rejoice")
                    .SetAlt("Lovers of Hot Asian Buns Rejoice")
                    .SetStyle("max-width", "100%");

            var article5 = new SingleBlock(_fragmentFactory.Image(asianBuns, "https://www.theinfatuation.com/chicago/reviews/chiu-quon-bakery-1"));
            content.Add(article5);
            var article6 = new SingleBlock(
                _fragmentFactory.Html(
@"<p><h4>Local Dining Blitz - Illinois, Chicago</a></h4>

For those looking for outstanding Asian buns in Chicago, <a href=""https://www.theinfatuation.com/chicago/reviews/chiu-quon-bakery-1"">Chiu Quon Bakery</a> in Chinatown offers a traditional choice with its barbecue pork buns (cha siu bao). These buns are beloved for their soft exteriors and deliciously balanced salty-sweet pork filling. This bakery, established in 1986, offers high-quality traditional goods at affordable prices <em>(The Infatuation)</em>.<br /><br />

A few miles away, <em>Au Cheval</em> elevates the humble burger into a culinary experience. Get rich, flavorful burgers in a hip, bustling atmosphere. Meanwhile, at <em>Alinea</em> in Lincoln Park, the dining experience is transformed into an avant-garde culinary adventure, where Chef <em>Grant Achatz</em> presents multi-sensory dishes that are great artistry, with subtle taste.</p>
"));
            content.Add(article6);




            var taylorSwift
                = _imageFactory
                    .GetImage("TaylorSwift.png")
                    .SetTitle("Taylor Swift - CUSTOM OUTFITS")
                    .SetAlt("Taylor Swift - CUSTOM OUTFITS")
                    .SetStyle("max-width", "100%");
            var article0 = new SingleBlock(_fragmentFactory.Image(taylorSwift, "https://www.voguehk.com/en/article/fashion/taylor-swift-eras-tour-looks/"));
            content.Add(article0);
            var article1 = new SingleBlock(
                _fragmentFactory.Html(
                    @"<p><h4>Taylor Swift's Eras Tour</h4>
Taylor Swift's tour showcases an extravagant fashion display with each performance, 
mirroring the thematic elements of her music eras. Notably, 
her stage outfits include a variety of custom-designed bodysuits and gowns 
by renowned designers like Versace and Roberto Cavalli. 
Each concert not just a musical event but a high-fashion spectacle as well 
<a href=""https://www.voguehk.com/en/article/fashion/taylor-swift-eras-tour-looks/"">(Vogue Hong Kong)</a>.</p>
<p></p>
"));
            content.Add(article1);




            //DTCs.png


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

