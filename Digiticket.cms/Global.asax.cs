using AutoMapper;
using DigiticketCMS.Config;
using DigiticketCMS.Helpers.Authentication;
using DigiticketCMS.Helpers.Cookie;
using DigiticketCMS.Helpers.Language;
using DigiticketCMS.Models;
using Infrastructure.Config;
using Infrastructure.Globals;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Utilities.Logs;
using Services.Implement;
namespace DigiticketCMS
{
    public class MvcApplication : System.Web.HttpApplication
    {
        [Obsolete]
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AntiForgeryConfig.UniqueClaimTypeIdentifier = "Token";
            AutofacConfig.AutofacConfigContainer();

            Mapper.Initialize(cfg =>
            {
                cfg.AddProfile<ViewModelToRequest>();
                cfg.AddProfile<ResponseToViewModel>();
            });
        }
        protected void Application_AcquireRequestState(object sender, EventArgs e)
        {
            Globals.ClientId = TenantConfig.ClientId;
            Globals.ClientSecret = TenantConfig.ClientSecret;
            var culture = Request.Cookies[SystemConfig.System_CultureCookieName];
            if (culture != null)
                LanguageManage.SetLanguage(culture.Value);
            else
                LanguageManage.SetLanguage();
        }
        protected void Application_PostAuthenticateRequest(Object sender, EventArgs e)
        {
            try
            {
                if (!HttpContext.Current.User.Identity.IsAuthenticated)
                {
                    var cookieName = SystemConfig.System_AuthCookieName;
                    var authCookie = CookieManage.GetCookie(cookieName);
                    if (!string.IsNullOrEmpty(authCookie))
                    {
                        var accessData = JsonConvert.DeserializeObject<UserCookie>(authCookie);
                        var accService = new AccountServices();
                        if (accessData != null && accessData.ExpiresDate < DateTime.Now)
                        {
                            var result = accService.RefreshToken(accessData.RefreshToken);
                            if (result.Code > 0)
                            {
                                accessData = Mapper.Map<UserCookie>(result.Data);
                                accessData.ExpiresDate = DateTime.Now.AddSeconds(accessData.Expires);
                                CookieManage.SetCookie(cookieName, JsonConvert.SerializeObject(accessData), TimeSpan.FromSeconds(accessData.Expires + 3600));
                            }
                            else
                            {
                                Response.Cookies[cookieName].Value = string.Empty;
                                Response.Cookies[cookieName].Expires = DateTime.Now.AddMonths(-1);
                            }
                        }
                        else
                        {
                            var result = accService.VerifyToken(accessData.AccessToken);
                            if (result.Code > 0)
                            {
                                if (accessData != null)
                                {
                                    var principal = GetPrincipal(accessData);
                                    if (principal != null)
                                    {
                                        HttpContext.Current.User = principal;
                                    }
                                }
                            }
                            else
                            {
                                Response.Cookies[cookieName].Value = string.Empty;
                                Response.Cookies[cookieName].Expires = DateTime.Now.AddMonths(-1);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                NLogManager.PublishException(ex);
            }
        }

        private static ClaimsPrincipal GetPrincipal(UserCookie token)
        {
            try
            {
                var handler = new JwtSecurityTokenHandler();
                var jsonToken = handler.ReadToken(token.AccessToken) as JwtSecurityToken;
                var user = AuthenticationModule.PopulateUserIdentity(jsonToken);
                var claims = new List<Claim>()
                {
                    new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString() ?? "" ),
                    new Claim(ClaimTypes.Name, user.DisplayName ?? user.UserName),
                    new Claim("Token", token.AccessToken ?? ""),
                    new Claim("Avatar", user.Avatar ?? ""),
                    new Claim("Permission", user.Permission ?? "")
                };
                var identity = new ClaimsIdentity(claims, "Auth");
                return new ClaimsPrincipal(identity);
            }
            catch (Exception ex)
            {
                NLogManager.PublishException(ex);
                return null;
            }
        }
    }
}
