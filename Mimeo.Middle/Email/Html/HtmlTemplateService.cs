using System.IO;
using Mimeo.Middle.Email.Content;
using RazorEngine;
using RazorEngine.Templating;

namespace Mimeo.Middle.Email.Html
{
    public class HtmlTemplateService
    {
        private const string ActionTemplateResource = "Mimeo.Middle.Email.Html.ActionTemplate.cshtml";
        private const string ActionTemplateKey = "ActionTemplate";

        private IRazorEngineService _templateService;


        public HtmlTemplateService Initialize()
        {
            if (_templateService == null)
            {
                using (var templateStream = GetType().Assembly.GetManifestResourceStream(ActionTemplateResource))
                using (var reader = new StreamReader(templateStream))
                {
                    var razorTemplate = reader.ReadToEnd();
                    Engine.Razor.Compile(razorTemplate, ActionTemplateKey, typeof(Message));
                }
            }

            return this;
        }

        public HtmlMessage Build(Message message, bool useEmbeddedImages)
        {
            dynamic viewBag = new DynamicViewBag();
            viewBag.UseEmbeddedImages = useEmbeddedImages;

            var output = new HtmlMessage();
            output.UsesEmbeddedImages = useEmbeddedImages;
            output.Html = Engine.Razor.Run(ActionTemplateKey, typeof(Message), message, (DynamicViewBag)viewBag);
            output.ImageReferences.Add(new HtmlImage(message.Logo));

            return output;
        }
    }
}

