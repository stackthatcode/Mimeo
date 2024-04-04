using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace Mimeo.Middle.Identity
{
    public class RoleClaimRequiredHandler : AuthorizationHandler<RoleClaimRequiredPolicy>
    {
        protected override Task HandleRequirementAsync(
                AuthorizationHandlerContext context, RoleClaimRequiredPolicy requirement)
        {
            if (!context.User.Claims.Any(x => x.Type == SecurityConfig.RoleClaim &&
                                              x.Value == requirement.RoleClaimValue))
            {
                context.Fail();
            }
            else
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
