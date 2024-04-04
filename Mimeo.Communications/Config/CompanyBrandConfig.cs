using Mimeo.Communications.Html.Content.Images;

namespace Mimeo.Communications.Config
{
    public class CompanyBrandConfig
    {
        public string CompanyName { get; private set; }
        public string CompanyWebsiteUrl { get; private set; }
        public ImageEnvelope CompanyLogo { get; private set; }

        public CompanyBrandConfig(string companyName, string websiteUrl, ImageEnvelope logo)
        {
            CompanyName = companyName;
            CompanyWebsiteUrl = websiteUrl;
            CompanyLogo = logo;
        }
    }
}
