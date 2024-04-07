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


namespace Mimeo.ConsoleApp
{
    public class SampleWorker01
    {
        private readonly FragmentFactory _fragmentFactory;
        private readonly ImageFactoryLocal _imageFactory;
        private readonly HtmlTemplateService _templateService;
        private readonly IIndex<string, MailgunConfig> _configs;
        private readonly Func<MailgunConfig, MailgunApi> _mailgunApiFactory;

        public SampleWorker01(
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
            AsyncContext.Run(
                () => mailgun.Send("info@logicautomated.com", "Hello", html, contentModel.ImageReferences));
        }


        private ContentModel BuildContent()
        {
            _imageFactory
                .SetLocalDirectory(@"C:\DEV\Mimeo\TestFileStorage\0001\")
                .SetDefaultTransferMedium(ImageTransferMedium.CidEmbedded);
            
            var content = new List<IContentBlock>();

            var companyLogo
                = _imageFactory
                    .GetImage("GlobalPlusBlackLogo.png")
                    .SetTitle("Global Plus New - Media Company")
                    .SetAlt("Global Plus New - Media Company")
                    .SetStyle("max-width", "100%");


            var contentModel = new ContentModel();

            contentModel.ExtendedProperties[BasicTemplate01.CONTENTMODEL_TITLE]
                = "Bringing they news you want right to your inbox";

            content.Add(new SingleBlock(_fragmentFactory.Image(companyLogo)));
            content.Add(new SingleBlock(_fragmentFactory.Html("<hr />")));
            content.Add(new SingleBlock(_fragmentFactory.Html("<h4>Global Plus - Family &amp; Parenting</h4>")));

            var shoppingImage
                = _imageFactory
                    .GetImage("Shopping.png")
                    .SetTitle("Customers finding the stuff they like")
                    .SetAlt("Customers finding the stuff they like")
                    .SetStyle("max-width", "150px");

            var shopping = new DoubleBlock(
                _fragmentFactory.Html(
                    @"<p style=""padding-right:20px;"">Offline shopping with self-checkouts has been increasingly favored for its speed and efficiency, 
                    contributing significantly to a positive customer experience. 
                    <a href=""https://www.shopify.com/retail/trend-watch-the-death-of-the-checkout-line"">A report by Shopify</a> highlights that
                    providing more checkout options, including self-checkouts, not only boosts customer satisfaction 
                    but also reduces wait times, leading to more sales and a higher Customer Lifetime Value (CLV) (Shopify).</p>"),
                _fragmentFactory.Image(shoppingImage,
                    "https://www.shopify.com/retail/trend-watch-the-death-of-the-checkout-line"),
                new StyleBag() {{"margin-bottom", "20px"}});
            content.Add(shopping);

            var seatSafety
                = _imageFactory
                    .GetImage("SeatSafety.png")
                    .SetTitle("Transport for NSW")
                    .SetAlt("Transport for NSW")
                    .SetStyle("max-width", "200px");

            content.Add(new DoubleBlock(
                _fragmentFactory.Html(
                    @"<p style=""padding-right:20px;"">
                        Having an adult sit on another's lap in a vehicle is highly unsafe for several reasons. 
                        Seat belts are designed to protect individual occupants based on standard seating positions, 
                        and having someone sit on your lap disrupts this safety mechanism. 
                        <a href=""https://www.transport.nsw.gov.au/roadsafety/topics-tips/seatbelts"">
                            The primary function of a seatbelt is to decelerate the passenger at the same rate as the vehicle during a crash,
                        </a>
                        distribute the force of impact over the body's stronger areas (such as the pelvis and chest),
                        and prevent the occupant from hitting interior parts of the vehicle or being ejected 
                        (Transport for NSW)​.</p>"
            ),
            _fragmentFactory.Image(seatSafety, "https://www.transport.nsw.gov.au/roadsafety/topics-tips/seatbelts")
            ));


            var hillaryHome
                = _imageFactory
                    .GetImage("HillaryHome.png")
                    .SetTitle("Home Grown Hillary")
                    .SetAlt("Home Grown Hillary")
                    .SetStyle("max-width", "200px");

            content.Add(new DoubleBlock(
                _fragmentFactory.Html(
                    @"<p style=""padding-right:20px;"">
                        <a href=""https://homegrownhillary.com/save-money-on-groceries/"">Meal Planning with AI:</a>
                        Utilize AI tools like chatbots for meal planning.
                        By inputting your available ingredients, dietary preferences, and meal types you enjoy, 
                        you can get meal plans and shopping lists that help you use what you have and buy only what you need​. 
                        Being conscientious about using leftovers and prioritizing eating what you have can save money. 
                        Creative cooking to use up ingredients before they go bad prevents wasting money on food that's thrown away  
                        (Homegrown Hillary)​.</p>"
                ),
                _fragmentFactory.Image(hillaryHome, "https://homegrownhillary.com/save-money-on-groceries/")
            ));


            // Utilize AI tools like chatbots for meal planning. By inputting your available ingredients, dietary preferences, and meal types you enjoy, you can get meal plans and shopping lists that help you use what you have and buy only what you need​(Homegrown Hillary)​.

            //var officeImage
            //    = _imageFactory
            //        .GetImage("Office1.jpg")
            //        .SetTitle("Working Away")
            //        .SetAlt("Pushing the thresholds of all known human productivity limits");
            //content.Add(new SingleBlock(_fragmentFactory.Image(officeImage)));

            //var emoji
            //    = _imageFactory
            //        .GetImage("Emoji.png")
            //        .SetTitle("Email Marketing Made Global")
            //        .SetAlt("Company Logo - can you not observe the ferocity??")
            //        .SetStyle("max-width", "15px");

            //var series = new List<IFragment>()
            //{
            //    _fragmentFactory.Html("<p>This is a test if more content &amp; "),
            //    _fragmentFactory.Image(emoji),
            //    _fragmentFactory.Text(" that's right &amp; <h1>"),
            //    _fragmentFactory.Html("I keep typing etc etc.</p>"),
            //};
            //content.Add(new SingleBlock(series));


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
                <p>Thank you for choosing Global Plus News. Together, let's redefine the way we stay informed about the world.</p>" 
                //@"<p style=\"word-wrap:break-word;\">"
                //+ @".".ToBase64() + "</p>"
                ));

            return contentModel;
        }
    }
}

