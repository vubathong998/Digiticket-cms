using Infrastructure.Config;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;

namespace DigiticketCMS.Helpers.Language
{
    public class LanguageManage
    {
        public static void SetLanguage(string lang = null)
        {
            try
            {
                var language = GetLanguage(lang);

                var cultureInfo = new CultureInfo(language.LanguageCode);
                cultureInfo.DateTimeFormat.ShortDatePattern = language.DateFormat;
                cultureInfo.DateTimeFormat.FullDateTimePattern = language.DateTimeFormat;
                cultureInfo.DateTimeFormat.LongTimePattern = "";

                Thread.CurrentThread.CurrentUICulture = cultureInfo;
                Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(cultureInfo.Name);

                HttpCookie langCookie = new HttpCookie(SystemConfig.System_CultureCookieName, language.LanguageClientCode);
                langCookie.Expires = DateTime.Now.AddYears(1);
                HttpContext.Current.Response.Cookies.Add(langCookie);
            }
            catch (Exception) { }
        }

        public static Languages GetLanguage(string langCode)
        {
            var lang = new Languages();
            if (langCode == null)
                lang = LanguagesProvider.GetListLanguages().FirstOrDefault(x => x.LanguageClientCode.Contains(LanguagesProvider._defaultLang));
            else
                lang = LanguagesProvider.GetListLanguages().FirstOrDefault(x => x.LanguageClientCode.Contains(langCode));
            return lang;
        }

        //public static string GetClientResources()
        //{
        //    try
        //    {
        //        ResourceSet resourceSet = ClientResources.ResourceManager.GetResourceSet(CultureInfo.CurrentUICulture, true, true);
        //        var sbInitial = "{";
        //        var sb = new StringBuilder(sbInitial);
        //        var resEnum = resourceSet.GetEnumerator();
        //        while (resEnum.MoveNext())
        //        {
        //            if (sb.ToString() != sbInitial)
        //            {
        //                sb.Append(",");
        //            }
        //            sb.Append("\"" + resEnum.Key + "\":\"" + resEnum.Value.ToString().Replace("\r\n", "").Replace("\"", "\\\"") + "\"");
        //        }
        //        sb.Append("}");
        //        return sb.ToString();
        //    }
        //    catch (Exception ex)
        //    {
        //        NLogManager.PublishException(ex);
        //        return "{}";
        //    }
        //}

        public static string GetCurrentLanguageCode()
        {
            HttpCookie langCookie = HttpContext.Current.Request.Cookies[SystemConfig.System_CultureCookieName];
            return langCookie.Value;
        }
    }

    public class LanguagesProvider
    {
        public static string _defaultLang = "vi";
        public static List<Languages> GetListLanguages()
        {
            return new List<Languages> {
                new Languages {
                    LanguageFullName = "Tiếng việt",
                    LanguageCode = "vi-VN",
                    LanguageClientCode = "vi",
                    DateFormat = "dd/MM/yyyy",
                    DateTimeFormat = "dd/MM/yyyy hh:mm",
                    FlagPath = SystemConfig.System_Domain + "/Content/media/flags/220-vietnam.svg"
                },
                new Languages {
                    LanguageFullName = "English",
                    LanguageCode = "en-US",
                    LanguageClientCode = "en",
                    DateFormat = "MM/dd/yyyy",
                    DateTimeFormat = "MM/dd/yyyy hh:mm",
                    FlagPath = SystemConfig.System_Domain + "/Content/media/flags/226-united-states.svg"
                }
            };
        }
    }
    public class Languages
    {
        public string LanguageFullName { get; set; }
        public string LanguageCode { get; set; }
        public string LanguageClientCode { get; set; }
        public string DateFormat { get; set; }
        public string DateTimeFormat { get; set; }
        public string FlagPath { get; set; }
    }
}