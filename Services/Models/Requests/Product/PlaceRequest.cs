using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Text;
using System.Threading.Tasks;

namespace Services.Models.Requests.Product
{
    public class PlaceRequest
    {

    }
    public class ProductPlaceSuggestRequest
    {
        public string Keyword { get; set; }
        public Guid Ssid { get; set; }
        public string LanguageCode { get; set; }
    }
    public class ProductPlaceDetailRequest
    {
        public string PlaceId { get; set; }
        public Guid Ssid { get; set; }
        public string LanguageCode { get; set; }
    }
}
