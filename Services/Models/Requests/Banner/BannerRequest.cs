using DigiticketCMS.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Models.Requests.Banner
{
    public class BannerRequest : BaseRequest
    {
    }

    public class BannerAddRequest : BaseImg
    {
        public Guid CategoryId { get; set; }
        public Guid DestinationId { get; set; }
    }

    public class BannerEditRequest : BaseImg
    {
        public int Id { get; set; }
    }

    public class BannerTypeRequest : BaseRequest
    {

    }

    public class BannerTypeAddRequest
    {
        public Guid? CategoryId { get; set; }
        public Guid? DestinationId { get; set; }
        public string Name { get; set; }
    }
    public class BannerTypeEditRequest
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
    
}
