using System.Configuration;

namespace Infrastructure.Config
{
    class AppConfig
    {
    }

    public class SystemConfig
    {
        public static string System_CultureCookieName
        {
            get
            {
                return ConfigurationManager.AppSettings["System:CultureCookieName"];
            }
        }

        public static string System_AuthCookieName
        {
            get
            {
                return ConfigurationManager.AppSettings["System:AuthCookieName"];
            }
        }

        public static string System_FEDomain
        {
            get
            {
                return ConfigurationManager.AppSettings["System:FEDomain"];
            }
        }

        public static string System_Domain
        {
            get
            {
                return ConfigurationManager.AppSettings["System:Domain"];
            }
        }

        public static string System_SSO
        {
            get
            {
                return ConfigurationManager.AppSettings["System:SSO"];
            }
        }

    }

    public class APIConfig
    {
        public static string API_ACCOUNT_URL
        {
            get
            {
                return ConfigurationManager.AppSettings["API_ACCOUNT_URL"];
            }
        }

        public static string API_SERVICES_URL
        {
            get
            {
                return ConfigurationManager.AppSettings["API_SERVICES_URL"];
            }
        }

        public static string API_CDN_URL
        {
            get
            {
                return ConfigurationManager.AppSettings["API_CDN_URL"];
            }
        }

        public static string LOAD_URL
        {
            get
            {
                return ConfigurationManager.AppSettings["LOAD_URL"];
            }
        }
    }

    public class SecurityConfig
    {
        public static string SECURE_SECRET_CONFIRM
        {
            get
            {
                return ConfigurationManager.AppSettings["SECURE_SECRET_CONFIRM"];
            }
        }

        public static string SECURE_SECRET_KEY_ENCRYPT
        {
            get
            {
                return ConfigurationManager.AppSettings["SECURE_SECRET_KEY_ENCRYPT"];
            }
        }

        public static string SECURE_SECRET_IV
        {
            get
            {
                return ConfigurationManager.AppSettings["SECURE_SECRET_IV"];
            }
        }
    }

    public class TenantConfig
    {
        public static string ClientId
        {
            get
            {
                return ConfigurationManager.AppSettings["ClientId"];
            }
        }

        public static string ClientSecret
        {
            get
            {
                return ConfigurationManager.AppSettings["ClientSecret"];
            }
        }
    }

    public class WorkgroupConfig
    {
        public static string WorkgroupDefault
        {
            get
            {
                return ConfigurationManager.AppSettings["WorkgroupDefault"];
            }
        }
    }

    public class PlaceInProductBaseInfoConfig
    {
        public static string CookieName
        {
            get
            {
                return ConfigurationManager.AppSettings["PlaceInProductBaseInfo:CookieName"];
            }
        }
    }

    public class APIMediaConfig
    {
        public static string APIMediaDefault
        {
            get
            {
                return ConfigurationManager.AppSettings["API_CDN_MEDIA_URL"];
            }
        }
        public static string APIMediaShowContentDefault
        {
            get
            {
                return ConfigurationManager.AppSettings["API_CDN_MEDIA_URL_SHOW_CONTENT"];
            }
        }
    }

    public class SystemVersion
    {
        public static string VersionCSS
        {
            get
            {
                return ConfigurationManager.AppSettings["Version_CSS"];
            }
        }
        public static string VersionJS
        {
            get
            {
                return ConfigurationManager.AppSettings["Version_JS"];
            }
        }
    }

    public class Permission
    {
        #region Product
        public static string Product
        {
            get
            {
                return ConfigurationManager.AppSettings["Permission_Products"];
            }
        }
        public static string ProductEditConfig
        {
            get
            {
                return ConfigurationManager.AppSettings["Permission_Products_EditConfig"];
            }
        }
        public static string ProductEditGroupService
        {
            get
            {
                return ConfigurationManager.AppSettings["Permission_Products_EditGroupService"];
            }
        }
        public static string ProductEditImage
        {
            get
            {
                return ConfigurationManager.AppSettings["Permission_Products_EditImage"];
            }
        }
        public static string ProductEditInfo
        {
            get
            {
                return ConfigurationManager.AppSettings["Permission_Products_EditInfo"];
            }
        }
        public static string ProductEditSEO
        {
            get
            {
                return ConfigurationManager.AppSettings["Permission_Products_EditSEO"];
            }
        }
        public static string ProductEditStatus
        {
            get
            {
                return ConfigurationManager.AppSettings["Permission_Products_EditStatus"];
            }
        }
        public static string ProductEditIdx
        {
            get
            {
                return ConfigurationManager.AppSettings["Permission_Products_EditIdx"];
            }
        }
        public static string ProductEditTags
        {
            get
            {
                return ConfigurationManager.AppSettings["Permission_Products_EditTags"];
            }
        }
        public static string ProductEditTagsGroup
        {
            get
            {
                return ConfigurationManager.AppSettings["Permission_Products_EditTagsGroup"];
            }
        }
        public static string ProductImportServicePrice
        {
            get
            {
                return ConfigurationManager.AppSettings["Permission_Products_ImportServicePrice"];
            }
        }
        public static string ProductPublicProduct
        {
            get
            {
                return ConfigurationManager.AppSettings["Permission_Products_PublicProduct"];
            }
        }
        public static string ProductViewConfig
        {
            get
            {
                return ConfigurationManager.AppSettings["Permission_Products_ViewConfig"];
            }
        }
        public static string ProductViewImage
        {
            get
            {
                return ConfigurationManager.AppSettings["Permission_Products_ViewImage"];
            }
        }
        public static string ProductViewInfo
        {
            get
            {
                return ConfigurationManager.AppSettings["Permission_Products_ViewInfo"];
            }
        }
        public static string ProductViewSEO
        {
            get
            {
                return ConfigurationManager.AppSettings["Permission_Products_ViewSEO"];
            }
        }
        public static string ProductViewService
        {
            get
            {
                return ConfigurationManager.AppSettings["Permission_Products_ViewService"];
            }
        }
        public static string ProductViewStatus
        {
            get
            {
                return ConfigurationManager.AppSettings["Permission_Products_ViewStatus"];
            }
        }
        public static string ProductViewTags
        {
            get
            {
                return ConfigurationManager.AppSettings["Permission_Products_ViewTags"];
            }
        }
        public static string ProductViewTagsGroup
        {
            get
            {
                return ConfigurationManager.AppSettings["Permission_Products_ViewTagsGroup"];
            }
        }
        public static string ProductUpdateQueryPrice
        {
            get
            {
                return ConfigurationManager.AppSettings["Permission_Products_PublicProduct"];
            }
        }
        #endregion

        #region MyRegion
        public static string Setting
        {
            get
            {
                return ConfigurationManager.AppSettings["Permission_Setting"];
            }
        }
        public static string SettingEdit
        {
            get
            {
                return ConfigurationManager.AppSettings["Permission_Setting_Edit"];
            }
        }
        public static string SettingView
        {
            get
            {
                return ConfigurationManager.AppSettings["Permission_Setting_View"];
            }
        }
        #endregion

        #region Supplier 
        public static string Suppliers
        {
            get
            {
                return ConfigurationManager.AppSettings["Permission_Suppliers"];
            }
        }
        public static string SuppliersAdd
        {
            get
            {
                return ConfigurationManager.AppSettings["Permission_Suppliers_Add"];
            }
        }
        public static string SuppliersEdit
        {
            get
            {
                return ConfigurationManager.AppSettings["Permission_Suppliers_Edit"];
            }
        }
        public static string SuppliersImportProduct
        {
            get
            {
                return ConfigurationManager.AppSettings["Permission_Suppliers_ImportProduct"];
            }
        }
        public static string SuppliersView
        {
            get
            {
                return ConfigurationManager.AppSettings["Permission_Suppliers_View"];
            }
        }
        #endregion

        #region SysWorkGroup
        public static string SysWorkGroup
        {
            get
            {
                return ConfigurationManager.AppSettings["Permission_SysWorkGroup"];
            }
        }
        #endregion

        #region Tags
        public static string Tags
        {
            get
            {
                return ConfigurationManager.AppSettings["Permission_Tags"];
            }
        }
        public static string TagsAdd
        {
            get
            {
                return ConfigurationManager.AppSettings["Permission_Tags_Add"];
            }
        }
        public static string TagsEdit
        {
            get
            {
                return ConfigurationManager.AppSettings["Permission_Tags_Edit"];
            }
        }
        public static string TagsView
        {
            get
            {
                return ConfigurationManager.AppSettings["Permission_Tags_View"];
            }
        }
        #endregion

        #region LandingPage
        public static string LandingPage
        {
            get
            {
                return ConfigurationManager.AppSettings["Permission_LandingPage"];
            }
        }
        public static string LandingPageView
        {
            get
            {
                return ConfigurationManager.AppSettings["Permission_LandingPage_View"];
            }
        }
        public static string LandingPageAdd
        {
            get
            {
                return ConfigurationManager.AppSettings["Permission_LandingPage_Add"];
            }
        }
        public static string LandingPageEdit
        {
            get
            {
                return ConfigurationManager.AppSettings["Permission_LandingPage_Edit"];
            }
        }
        #endregion

        #region Category Tag
        public static string CategoryTag
        {
            get
            {
                return ConfigurationManager.AppSettings["Permission_CategoryTag"];
            }
        }
        public static string CategoryTagView
        {
            get
            {
                return ConfigurationManager.AppSettings["Permission_CategoryTag_View"];
            }
        }
        public static string CategoryTagAdd
        {
            get
            {
                return ConfigurationManager.AppSettings["Permission_CategoryTag_Add"];
            }
        }
        public static string CategoryTagEdit
        {
            get
            {
                return ConfigurationManager.AppSettings["Permission_CategoryTag_Edit"];
            }
        }
        #endregion
    }

    #region tokeToken
    public class CacheNameConfig
    {
        public static string PageSuggest
        {
            get
            {
                return ConfigurationManager.AppSettings["PageSuggest"];
            }
        }
        public static string GroupServiceTags
        {
            get
            {
                return ConfigurationManager.AppSettings["GroupServiceTags"];
            }
        }
        public static string GetListTags
        {
            get
            {
                return ConfigurationManager.AppSettings["GetListTags"];
            }
        }
    }
    #endregion
}
