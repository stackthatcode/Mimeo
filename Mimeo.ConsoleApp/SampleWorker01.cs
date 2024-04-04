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
            AsyncContext.Run(() => mailgun.Send("aleksjones@gmail.com", "Hello", html, contentModel.ImageReferences));
        }


        private ContentModel BuildContent()
        {
            _imageFactory
                .SetLocalDirectory(@"C:\DEV\Mimeo\TestFileStorage")
                .SetDefaultTransferMedium(ImageTransferMedium.CidEmbedded);
            
            var content = new List<IContentBlock>();

            var companyLogo
                = _imageFactory
                    .GetImage("CompanyLogo.jpg")
                    .SetTitle("Email Marketing Made Global")
                    .SetAlt("Company Logo - can you not observe the ferocity??");


            var contentModel = new ContentModel();

            contentModel.ExtendedProperties[BasicTemplate01.CONTENTMODEL_TITLE]
                = "This is test of extended properties";

            content.Add(new SingleBlock(_fragmentFactory.Image(companyLogo)));

            content.Add(new SingleBlock(_fragmentFactory.Html(
                @"<p>Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut 
                    labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco 
                    laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in 
                    voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat 
                    non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.</p>")));

            //var chatGptImage
            //    = _imageFactory
            //        .GetImage("ChatGptNews.png")
            //        .SetTitle("AI/ML Advancements")
            //        .SetAlt("New developments in AI technology");

            //content.Add(new DoubleBlock(
            //    _fragmentFactory.Html(
            //        @"<p style='padding-right:15px;'>
            //            So, how should we expect to take care of these test items???</p>"),
            //    _fragmentFactory.Image(chatGptImage)));

            //content.Add(new SingleBlock(
            //    _fragmentFactory.Html(
            //        @"<p>Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut 
            //            labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco 
            //            laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in 
            //            voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat 
            //            non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.</p>"),
                    
            //        new StyleBag() { { "background-color","#f4f4f4" }}));

            //var chatGptImage2
            //    = _imageFactory
            //        .GetImage("ChatGptNews.png")
            //        .SetTitle("AI/ML Advancements")
            //        .SetAlt("New developments in AI technology");

            //content.Add(new DoubleBlock(
            //    _fragmentFactory.Html(
            //        @"<p style='padding-right:15px;'>
            //            So, how should we expect to take care of these test items???</p>"),
            //    _fragmentFactory.Image(chatGptImage2),
            //    new StyleBag() { { "background-color", "#e4e4e4" } }));

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
                @"<p>Specially vetted verbiage lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut 
                labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco 
                laboris nisi ut aliquip ex ea commodo consequat.</p>" +
                "<p style=\"word-wrap:break-word;\">"
                + @"Specially vetted verbiage lorem ipsum dolor sit amet, consectetur adipiscing elit, 
                    sed do eiusmod tempor incididunt ut \r\n                
                    labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco \r\n                
                    laboris nisi ut aliquip ex ea commodo consequat.".ToBase64() + "</p>"));

            return contentModel;
        }
    }
}

