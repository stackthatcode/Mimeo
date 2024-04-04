using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Mimeo.Middle.Identity
{
    public class ApplicationUserValidator : IUserValidator<IdentityUser>
    {
        public Task<IdentityResult> ValidateAsync(UserManager<IdentityUser> manager, IdentityUser user)
        {
            return new Task<IdentityResult>(() => new IdentityResult());
        }
    }
}

