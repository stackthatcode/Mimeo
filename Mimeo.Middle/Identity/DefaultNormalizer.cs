using Microsoft.AspNetCore.Identity;

namespace Mimeo.Middle.Identity
{
    public class DefaultNormalizer : ILookupNormalizer
    {
        public string NormalizeName(string name)
        {
            return name;
        }

        public string NormalizeEmail(string email)
        {
            return email;
        }
    }
}
