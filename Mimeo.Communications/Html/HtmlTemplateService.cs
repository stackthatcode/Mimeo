using Mimeo.Communications.Html.Content;
using Mimeo.Communications.Html.Template;
using RazorLight;


namespace Mimeo.Communications.Html
{
    public class HtmlTemplateService
    {
        private readonly RazorLightEngine _engine;

        public HtmlTemplateService(RazorLightEngineBuilder engineBuilder)
        {
            _engine = engineBuilder
                .UseEmbeddedResourcesProject(typeof(HtmlTemplateService).Assembly)
                .UseMemoryCachingProvider()
                .Build();
        }

        public string GenerateHtml(ContentModel message, IMimeoTemplate template)
        {
            //var testResource = 
            // (Yes, I know that everything should be async. We'll get there after I find a suitable way to
            // ... invoke async stuff from synchronous code)
            //
            var htmlOutput = _engine.CompileRenderAsync(template.ResourceId, message).Result;
            var index = htmlOutput.IndexOf("<!DOCTYPE");
            return htmlOutput.Substring(index);
        }
    }
}

