using System.Linq;
using System.Security.Claims;
using Hangfire.Dashboard;
using Mimeo.Middle.Identity;

namespace Mimeo.Web
{
    public class HangfireAuthorizationFilter : IDashboardAuthorizationFilter
    {
        private readonly string _role;

        public HangfireAuthorizationFilter(string role)
        {
            _role = role;
        }

        public bool Authorize(DashboardContext context)
        {
            var identity = context.GetHttpContext()?.User?.Identity as ClaimsIdentity;
            if (identity?.Claims == null)
            {
                return false;
            }

            return (identity.Claims.Any(x => x.Type == SecurityConfig.RoleClaim &&
                                             x.Value == SecurityConfig.AdminRole));
        }
    }
}
