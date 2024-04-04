using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace Mimeo.Middle.Identity
{
    public class RoleClaimRequiredPolicy : IAuthorizationRequirement
    {
        public readonly string RoleClaimValue;

        public RoleClaimRequiredPolicy(string roleClaimValue)
        {
            this.RoleClaimValue = roleClaimValue;
        }
    }
}
