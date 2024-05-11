using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Extensions
{
    public static class IdentityExtensions
    {
        public static string GetClaims(this IIdentity identity, string key)
        {
            var claim = ((ClaimsIdentity)identity).FindFirst(key);
            return (claim != null) ? claim.Value : string.Empty;
        }
    }
}