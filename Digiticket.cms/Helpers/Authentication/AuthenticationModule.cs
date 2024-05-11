using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Principal;
using System.Web;

namespace DigiticketCMS.Helpers.Authentication
{
    public static class AuthenticationModule
    {
        public static JWTAuthenticationIdentity PopulateUserIdentity(JwtSecurityToken userPayloadToken)
        {
            string name = ((userPayloadToken)).Claims.FirstOrDefault(m => m.Type == "unique_name")?.Value;
            string userId = ((userPayloadToken)).Claims.FirstOrDefault(m => m.Type == "nameid")?.Value;
            string avatar = ((userPayloadToken)).Claims.FirstOrDefault(m => m.Type == "avatar")?.Value;
            string displayName = ((userPayloadToken)).Claims.FirstOrDefault(m => m.Type == "displayname")?.Value;
            string tenantId = ((userPayloadToken)).Claims.FirstOrDefault(m => m.Type == "tenantid")?.Value;
            string workGroupId = ((userPayloadToken)).Claims.FirstOrDefault(m => m.Type == "tenantwgid")?.Value;
            string permission = string.Join("~", ((userPayloadToken)).Claims.Where(m => m.Type.ToLower() == "permission").Select(x => x.Value).ToList());
            return new JWTAuthenticationIdentity(name)
            {
                UserId = Convert.ToInt32(userId),
                UserName = name,
                Avatar = avatar,
                DisplayName = displayName,
                TenantId = long.Parse(tenantId),
                WorkGroupId = long.Parse(workGroupId),
                Permission = permission
            };
        }
    }

    public class JWTAuthenticationIdentity : GenericIdentity
    {
        public string UserName { get; set; }
        public long UserId { get; set; }
        public string Avatar { get; set; }
        public string DisplayName { get; set; }
        public long TenantId { get; set; }
        public long WorkGroupId { get; set; }
        public string Permission { get; set; }
        public string AccessToken { get; set; }

        public JWTAuthenticationIdentity(string userName)
            : base(userName)
        {
            UserName = userName;
        }
    }
}