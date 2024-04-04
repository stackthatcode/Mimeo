using System;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using Mimeo.Blocks.Helpers;

namespace Mimeo.Middle.Identity
{
    public static class Extensions
    {
        public static string ToFriendlyString(this IdentityResult input)
        {
            return input.Succeeded 
                ? "Succeeded!" 
                : "Failed!" + Environment.NewLine + input.Errors.Select(x => $"{x.Code} - {x.Description}").JoinByNewline();
        }
    }
}
