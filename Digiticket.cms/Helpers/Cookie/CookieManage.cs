using Infrastructure.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Utilities.Logs;
using Utilities.Security;

namespace DigiticketCMS.Helpers.Cookie
{
    public class CookieManage
    {
        public static void SetCookie(string key, string value, TimeSpan expires)
        {
            var encodedValue = Security.EncryptByAES(value, SecurityConfig.SECURE_SECRET_KEY_ENCRYPT, SecurityConfig.SECURE_SECRET_IV);

            if (HttpContext.Current.Request.Cookies[key] != null)
            {
                var cookieOld = HttpContext.Current.Request.Cookies[key];
                cookieOld.Expires = DateTime.Now.Add(expires);
                cookieOld.Value = encodedValue;
                HttpContext.Current.Response.Cookies.Add(cookieOld);
            }
            else
            {
                var cookieNew = new HttpCookie(key, encodedValue);
                cookieNew.Expires = DateTime.Now.Add(expires);
                HttpContext.Current.Response.Cookies.Add(cookieNew);
            }
        }
        public static string GetCookie(string key)
        {
            try
            {
                string value = string.Empty;
                HttpCookie cookie = HttpContext.Current.Request.Cookies[key];

                if (cookie != null)
                {
                    value = Security.DecryptByAES(cookie.Value, SecurityConfig.SECURE_SECRET_KEY_ENCRYPT, SecurityConfig.SECURE_SECRET_IV);
                }
                return value;
            }
            catch (Exception ex)
            {
                NLogManager.PublishException(ex);
                return string.Empty;
            }
        }
    }

    public static class CookieKey
    {
        public const string AreaCookie = "AreaCookie";
    }
}