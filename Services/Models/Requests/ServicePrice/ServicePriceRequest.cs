using DigiticketCMS.Services.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Models.Requests.ServicePrice
{
    public class ServicePriceRequest : BaseRequest
    {

    }
    public class ServicePriceImportServiceFromDigiPostRequest
    {
        public Guid ServicePriceId { get; set; }
        public Guid ProductId { get; set; }
    }
    public class ServicePriceGetByPageRequest : BaseRequest
    {
        public int ProductId { get; set; }
    }
    public class ServicePriceUpdateServicepriceFromDigipostRequest
    {
        public Guid ServicePriceId { get; set; }
        public Guid ProductId { get; set; }
    }
    public class ServicePriceUpdateListServicepriceFromDigipostRequest
    {
        public string ProductId { get; set; }
        public List<Guid> ServicePriceIds { get; set; }
    }
}
