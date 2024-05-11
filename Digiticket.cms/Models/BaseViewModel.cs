using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DigiticketCMS.Models
{
    public class BaseViewModel
    {
        public int page { get; set; }
        public int perpage { get; set; }
        public string field { get; set; }
        public string sort { get; set; }
        public string key { get; set; }
        public string LanguageCode { get; set; }
        public string CurrencyCode { get; set; }
        public List<FilterViewModel> filter { get; set; }
        public BaseViewModel()
        {
            page = 1;
            perpage = 10;
            field = string.Empty;
            sort = string.Empty; // asc | desc
            key = string.Empty;
            LanguageCode = string.Empty;
            CurrencyCode = string.Empty;
        }
    }
    public class FilterViewModel
    {
        public string Opt { get; set; }
        public string Name { get; set; }
        public string Opt1 { get; set; }
        public int Type { get; set; }
        public string Values { get; set; }
    }

    public class ErrorViewModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
    }

    public class BaseImgViewModel
    {
        public int LinkOption { get; set; }
        public string Name { get; set; }
        public string Link { get; set; }
        public string Url { get; set; }
        public string Alt { get; set; }
        public int Type { get; set; }
        public int? Idx { get; set; }
        public BaseImgViewModel()
        {
            Name = string.Empty;
            Link = string.Empty;
            Url = string.Empty;
            Alt = string.Empty;
            Type = 0;
            Idx = 0;
        }
    }
}