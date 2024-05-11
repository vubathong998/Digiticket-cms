using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DigiticketCMS.Models.Product
{
    public class ProductPlaceSuggestViewModel
    {
        [Required]
        public string Keyword { get; set; }
        [Required]
        public Guid Ssid { get; set; }
        public string LanguageCode { get; set; }
    }
    public class ProductPlaceDetailViewModel
    {
        [Required]
        public string PlaceId { get; set; }
        [Required]
        public Guid Ssid { get; set; }
        public string LanguageCode { get; set; }
    }
    public class ProductPlaceJSON_SSID
    {
        public Guid PlaceCookieName { get; set; }
    }
}