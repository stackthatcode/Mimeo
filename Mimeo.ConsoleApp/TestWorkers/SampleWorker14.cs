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
    public class SampleWorker14
    {
        private readonly FragmentFactory _fragmentFactory;
        private readonly ImageFactoryLocal _imageFactory;
        private readonly HtmlTemplateService _templateService;
        private readonly IIndex<string, MailgunConfig> _configs;
        private readonly Func<MailgunConfig, MailgunApi> _mailgunApiFactory;

        public SampleWorker14(
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
            var subject = "Global Plus News (Subscriber News) - [Tech And Business] (TEST-PUBLISH)";

            AsyncContext.Run(
                () => mailgun.Send(sendTo, bccList, subject, html, contentModel.ImageReferences));
        }


        private ContentModel BuildContent()
        {
            _imageFactory
                .SetLocalDirectory(@"C:\DEV\Mimeo\TestFileStorage\0014\")
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
                .ExtendedProperties[BasicTemplate01.CONTENTMODEL_TITLE] = "Bringing their news you want right to your inbox";
            content.Add(new SingleBlock(_fragmentFactory.Image(companyLogo)));



            content.Add(new SingleBlock(_fragmentFactory.Html("<hr />")));

            content.Add(new SingleBlock(
                _fragmentFactory.Html("<h3>Global Plus - Your Resource For World &amp; National News</h3>")));


            var url1 = "https://mileiq.com/blog/5-places-where-to-find-a-business-partner";
            var exclusiveCoverage
                = _imageFactory
                    .GetImage("ExclusiveCoverage.png")
                    .SetTitle("Covering Young Entrepreneurs")
                    .SetAlt("Covering Young Entrepreneurs")
                    .SetStyle("max-width", "100%");

            content.Add(new SingleBlock(_fragmentFactory.Image(exclusiveCoverage, url1)));

            var article3 = new SingleBlock(
                _fragmentFactory.Html($@"
<p><h4><a href=""{url1}"">COVERAGE - Growing Youth Entreepeneurs</a></h4>
As a young entrepreneur, finding a partner, means looking for someone who not only shares your passion but also <em>complements</em> your skills. 
You might find greatness among people you already know, like former classmates or coworkers. 
Events and Websites should help you connect, with potential partners. 
Above all, make sure you both share the same goals and work ethics to ensure a partnership that lasts.</p>
"));
            content.Add(article3);



            var url2 = "https://www.politico.com/news/2024/04/17/china-lobbying-tiktok-congress-00152819";
            var tiktac
                = _imageFactory
                    .GetImage("ShouZiChew.png")
                    .SetTitle("Shou Zi Chew (2)")
                    .SetAlt("Shou Zi Chew (2)")
                    .SetStyle("max-width", "100%");

            content.Add(new SingleBlock(_fragmentFactory.Image(tiktac, url2)));
            var article4 = new SingleBlock(
                _fragmentFactory.Html($@"
    <p><h4><a href=""{url2}"">China Lobby To BAN TikTok</a></h4>
    China has been actively lobbying U.S. Congress to prevent a ban on TikTok, spending millions. 
    The lobbying includes going directly to lawmakers, trying to influence policy regarding the app's operation in the U.S. 
    For more details, you can visit the <a href=""{url2}"">full article page</a>.</p>
"));
            content.Add(article4);





            //            var url3 = "https://www.politico.com/news/2024/04/17/china-lobbying-tiktok-congress-00152819";
            //            var tiktac
            //                = _imageFactory
            //                    .GetImage("ShouZiChew.png")
            //                    .SetTitle("Shou Zi Chew (2)")
            //                    .SetAlt("Shou Zi Chew (2)")
            //                    .SetStyle("max-width", "100%");

            //            content.Add(new SingleBlock(_fragmentFactory.Image(tiktac, url2)));
            //            var article5 = new SingleBlock(
            //                _fragmentFactory.Html($@"
            //        <p><h4><a href=""{url2}"">Seismic Safeguards: Modern Techniques for Durable Designs</a></h4>
            //        Architects use damping systems like shock absorbers and seismic invisibility cloaks, to enhance building resilience against earthquakes. 
            //        These methods, materials like concrete and steel, help structures absorb and reduce seismic energy effectively. 
            //        <br /><br />        
            //        For more insighting, you can read about the latest in earthquake-resistant designs on 
            //        <a href=""{url3}"">BigRentz</a> and <a href="""">Rethinking The Future</a> 
            //"));
            //            content.Add(article4);



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

