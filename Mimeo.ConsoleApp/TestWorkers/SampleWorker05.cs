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
    public class SampleWorker05
    {
        private readonly FragmentFactory _fragmentFactory;
        private readonly ImageFactoryLocal _imageFactory;
        private readonly HtmlTemplateService _templateService;
        private readonly IIndex<string, MailgunConfig> _configs;
        private readonly Func<MailgunConfig, MailgunApi> _mailgunApiFactory;

        public SampleWorker05(
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


            // // ### WARNING - LIVE
            var sendTo = "newsletter@mail.republicanpost.net";    // Smells like "MITM"
            // // ### WARNING - LIVE

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
                .SetLocalDirectory(@"C:\DEV\Mimeo\TestFileStorage\0005\")
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
                .ExtendedProperties[BasicTemplate01.CONTENTMODEL_TITLE] = "Bringing there's news you want right to your inbox";
            content.Add(new SingleBlock(_fragmentFactory.Image(companyLogo)));


            content.Add(new SingleBlock(_fragmentFactory.Html("<hr />")));
            content.Add(new SingleBlock(_fragmentFactory.Html("<h3>Global Plus - Your Resource For World &amp; National News</h3>")));

            var image1
                = _imageFactory
                    .GetImage("International.png")
                    .SetTitle("Economic Outlook")
                    .SetAlt("Economic Outlook")
                    .SetStyle("width", "100%");


            var article0 = new SingleBlock(_fragmentFactory.Image(image1));
            content.Add(article0);

            var article1 = new SingleBlock(
                _fragmentFactory.Html(
                    @"<p><h4><a href="""">
                        Economic Outlook for 2024:</a></h4>
                        The economic forecast suggests consumer spending will grow at a more muted pace. 
                        Spending is expected to become a modest drag. Household balance sheets and tight labor 
                        continue to support employment and income levels <em>(J.P. Morgan | Official Website)</em>.</p>"));
            content.Add(article1);


            var article2 = new SingleBlock(
                _fragmentFactory.Html(
                    @"<p><h4><a href=""https://www.timesofisrael.com/emirates-announces-5-million-to-beleaguered-unrwa-for-gaza-reconstruction/"">
                        Updates On The Israeli Campaign in Gaza</a></h4>
                        The United Arab Emirates has pledged $5 million to the UN Palestinian refugee agency 
                        (UNRWA) to aid in the Gaza Strip's reconstruction efforts <em>(The Times of Israel)</em>.</p>"));
            content.Add(article2);


            var article3 = new SingleBlock(
                _fragmentFactory.Html(
                    @"<p><h4><a href=""https://www.nytimes.com/2024/04/10/climate/epa-pfas-drinking-water.html"">
                        EPA Says No To Forever Chemicals</a></h4>
                        The EPA has taken significant steps to address the issue of PFAS in the U.S. New, 
                        first-ever federal limits on ""forever chemicals"" in drinking water, aim to protect communities 
                        from the adverse health effects associated with these substances.
                        <em>(New York Times)</em>.</p>"));
            content.Add(article3);



            content.Add(new SingleBlock(_fragmentFactory.Html("<hr />")));
            content.Add(new SingleBlock(_fragmentFactory.Html("<h3>Global Plus - Technology &amp; Startup Scenes</h3>")));

            var image2
                = _imageFactory
                    .GetImage("SoraVideo.png")
                    .SetTitle("Sora Produced Video")
                    .SetAlt("Sora Produced Video")
                    .SetStyle("width", "100%");

            var article4 = new SingleBlock(_fragmentFactory.Image(image2));
            content.Add(article4);

            var article5 = new SingleBlock(
                _fragmentFactory.Html(
                    @"<p><h4><a href=""https://startupsavant.com/startups-to-watch/tech"">Startups To Watch</a></h4>
                    Nuro stands out in Silicon Valley with autonomous delivery vehicles. Self-driving vehicles revolutionize the way goods are transported.
                    These Zero-These emission solutions may create millions of jobs. Funding from Google and SoftBank ""Vision Fund"".
                    <em>(Startupsavant.com)</em>.</p>"));
            content.Add(article5);

            var article6 = new SingleBlock(
                _fragmentFactory.Html(
                    @"<p><h4><a href=""https://www.technologyreview.com/2024/01/04/1086046/whats-next-for-ai-in-2024/"">Generative AI's New Video Move:</a></h4>
                    After revolutionizing image generation, AI is now moving into video production. New generative models are creating videos just a few seconds long but with impressive quality. This is finding applications in marketing, training, and more.
                    <em>(MIT Technology Review)</em>.</p>"));
            content.Add(article6);

            var article7 = new SingleBlock(
                _fragmentFactory.Html(
                    @"<p><h4><a href=""https://www.technologyreview.com/2024/02/15/1088401/openai-amazing-new-generative-ai-video-model-sora/"">OpenAI's Generative Video Model - Sora:</a></h4>
                    OpenAI has developed a generative video model named ""Sora"". Sora transforms short text descriptions into high-def film clips. 
                    Demonstrating a significant advancement in AI technology, Sora creates videos up to a minute long, showcasing scenes with impressive depth. 
                    Sora handles objects even that disappear from view, like a street sign reappearing after being hidden by passing traffic. 
                    Sora's still in the early stages, with OpenAI conducting safety testing and gathering feedback.
                    <em>(Technology Review)</em>.</p>"),
                    new StyleBag() { { "margin-bottom", "10px" } });

            content.Add(article7);


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

