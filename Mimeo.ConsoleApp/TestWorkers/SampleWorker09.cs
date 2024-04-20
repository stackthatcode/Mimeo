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
    public class SampleWorker09
    {
        private readonly FragmentFactory _fragmentFactory;
        private readonly ImageFactoryLocal _imageFactory;
        private readonly HtmlTemplateService _templateService;
        private readonly IIndex<string, MailgunConfig> _configs;
        private readonly Func<MailgunConfig, MailgunApi> _mailgunApiFactory;

        public SampleWorker09(
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
            var subject = "Global Plus News (Subscriber News) - [RESEND]";

            AsyncContext.Run(
                () => mailgun.Send(sendTo, bccList, subject, html, contentModel.ImageReferences));
        }


        private ContentModel BuildContent()
        {
            _imageFactory
                .SetLocalDirectory(@"C:\DEV\Mimeo\TestFileStorage\0009\")
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


            content.Add(new SingleBlock(_fragmentFactory.Html("<hr />")));

            content.Add(new SingleBlock(
                _fragmentFactory.Html("<h3>Global Plus - Your Resource For World &amp; National News</h3>")));

            //""Auctioneering.png""

            var auctioneering
                = _imageFactory
                    .GetImage("LordSaviourJesus.png")
                    .SetTitle("Birth of the Son, Lord Saviour Jesus")
                    .SetAlt("Birth of the Son, Lord Saviour Jesus")
                    .SetStyle("max-width", "100%");

            var article0 = new SingleBlock(_fragmentFactory.Image(auctioneering));

            content.Add(article0);

            var article1 = new SingleBlock(
                _fragmentFactory.Html(
@"<p style=""font-size:0.75em; margin-top:0px;"">(Birth of the Lord and Saviour, Son of God - example of serious work created by Microsoft Paint)</p>

<h3>The Journey of Microsoft Windows and Microsoft Paint</h3>

<p>In this edition, we celebrate the remarkable journey of Microsoft Windows, spotlighting both the OS's sweeping advancements and the evolution of one of its most iconic applications: Microsoft Paint. Not only has Windows evolved, but MS Paint, too, has undergone a significant transformation.</p>

<p>From its inception as a basic drawing tool to its sophisticated functionality, MS Paint has grown. These changes mirror the broader technological strides made by Windows. We'll trace the path from Paint's humble beginnings, through significant updates. Now we find new features, like 3D design and AI-assisted tools, augmenting current state-of-the-art capabilities.</p> 

<p><h4><a href=""https://en.wikipedia.org/wiki/Microsoft_Paint"">Wikipedia - Microsoft Paint</a></h4>
This is a comprehensive overview of MS Paint's evolution, highlighting significant updates from Windows 7 through to Windows 11. Enhancements include the introduction of artistic brushes and a ribbon interface in Windows 7, the debut of Paint 3D in Windows 10, and recent additions like a new interface with layers in Windows 11.</p>

<p><h4><a href=""https://www.microsoft.com/en-us/windows/features?trkng=Gg023-009874-sbf4433=233"">Microsoft.com - Draw, Create, and Edit with Paint</a></h4>
This Microsoft page details the latest features of Paint in Windows 11, emphasizing user-friendly tools like AI-powered background removal, layer management, and a variety of brushes and drawing tools. This is the product of Paint's evolution over the years.</p>

<p><h4><a href="""">SlashGear - The Transformation of Microsoft Windows From 1985 To 2022</a></h4>
Although focused on Windows as a whole, this article provides context on how Paint has been part of the broader evolution of the Windows operating system, from its early days bundled with Windows 1.0 through to its integration into the latest Windows versions.</p>

<p>&nbsp;</p>
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

