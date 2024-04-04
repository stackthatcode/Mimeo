using Mimeo.Middle.Config;

// *** Warning - if this namespace is relocated, the ResourceName constant will have to be updated
//

namespace Mimeo.Middle.Email.Content
{
    public class MessageBuilder
    {
        private readonly MimeoAppSettings _appSettings;

        private const string LogoImageName = "DefaultLogo.png";
        private const string LogoResource = "Mimeo.Middle.Email.Content.DefaultLogo.png";

        private Message _model;

        public MessageBuilder(MimeoAppSettings appSettings)
        {
            _appSettings = appSettings;
            this.Clear();   
        }

        public void Clear()
        {
            _model = new Message();
            _model.AppSettings = _appSettings;
        }

        public MessageBuilder AddHtmlTextBlock(string content)
        {
            _model.ContentBlocks.Add(new HtmlBlock() { Content = content });
            return this;
        }

        public MessageBuilder AddParagraph(string content)
        {
            _model.ContentBlocks.Add(new HtmlBlock() { Content = $"<p>{content}</p>" });
            return this;
        }

        public MessageBuilder AddActionButton(string url, string buttonText)
        {
            _model.ContentBlocks.Add(
                new ActionBlock()
                {
                    Url = url,
                    ButtonText = buttonText
                });
            return this;
        }

        public MessageBuilder AddLogo()
        {
            var logo = new ContentImage();
            logo.ImageName = LogoImageName;

            using (var templateStream = GetType().Assembly.GetManifestResourceStream(LogoResource))
            {
                byte[] ba = new byte[templateStream.Length];
                templateStream.Read(ba, 0, ba.Length);

                logo.Data = ba;
            }

            _model.Logo = logo;
            return this;
        }


        public Message Output()
        {
            return this._model;
        }
    }
}
