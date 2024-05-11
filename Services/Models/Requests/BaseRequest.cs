using Services.Models.Requests.Product;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;

namespace DigiticketCMS.Services.Models
{
    public class BaseRequest
    {
        public int LinkOption { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public string FieldName { get; set; }
        public string Orderby { get; set; }
        public string Keyword { get; set; }
        public string Url { get; set; }
        public List<FilterRequest> Filter { get; set; }
        public string LanguageCode { get; set; }
        public string CurrencyCode { get; set; }
        public BaseRequest()
        {
            PageIndex = 1;
            PageSize = 10;
            FieldName = "";
            Orderby = ""; // asc | desc
            Keyword = "";
            LanguageCode = string.Empty;
            CurrencyCode = string.Empty;
            Filter = new List<FilterRequest>();
        }
    }
    public class FilterRequest
    {
        public string Opt { get; set; }
        public string Name { get; set; }
        public string Opt1 { get; set; }
        public int Type { get; set; }
        public string Values { get; set; }
    }
    public static class FilterOptionOpt
    {
        public static string AND = "AND";
        public static string OR = "OR";
    }

    public static class FilterOptionOpt1
    {
        public static string EqualTo = "=";
        public static string NotEqualTo = "<>";
        public static string GreaterThan = ">";
        public static string LessThan = "<";
        public static string GreaterThanOrEqualTo = ">=";
        public static string LessThanOrEqualTo = "<=";
        public static string In = "IN";
    }

    public static class FilterOptionType
    {
        public static int Number = 1;
        public static int String = 2;
        public static int Json = 3;
        public static int Datetime = 4;
        public static int Guid = 5;
        public static int Dic = 6;
        public static int List = 7;
        public static int Boolean = 8;
        public static int Date = 9;
    }
}
