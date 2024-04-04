using Microsoft.AspNetCore.Identity;

namespace Mimeo.Middle.Identity
{
    public class ApplicationUser : IdentityUser
    {
        public virtual Instance Instance { get; set; }
        public virtual long? InstanceId { get; set; }
        public virtual bool IsEnabled { get; set; }
    }
}
