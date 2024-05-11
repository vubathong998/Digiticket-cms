using Infrastructure.Config;
using Services.Models.Requests.Supplier;
using Services.Models.Responses.Supplier;
using Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Services.Models.Responses.ServicePrice;
using Services.Models.Requests.ServicePrice;
using Services.Interfaces;

namespace Services.Implement
{
    public class ServicePriceServices : IServicepriceServices
    {
        private BaseServices _baseService;
        public ServicePriceServices()
        {
            _baseService = new BaseServices(APIConfig.API_SERVICES_URL);
        }
        public BaseResponse<PagingResult<ServicePriceResponse>> GetByPage(ServicePriceGetByPageRequest data, string token)
        {
            var result = _baseService.PostApi<PagingResult<ServicePriceResponse>>("/api/serviceprice/get-by-page", data, token);
            return result;
        }
        public BaseResponse<List<ServicePriceResponse>> GetListByProductFromDigipost(Guid id, string token)
        {
            var result = _baseService.GetApi<List<ServicePriceResponse>>("/api/serviceprice/get-list-by-product-from-digipost/" + id, null, token);
            return result;
        }
        public BaseResponse<string> ImportServicePriceFromDigipost(ServicePriceImportServiceFromDigiPostRequest model, string token)
        {
            var result = _baseService.PostApi<string>("/api/serviceprice/import-serviceprice-from-digipost", model, token);
            return result;
        }
        public BaseResponse<string> ImportListServicePriceFromDigipost(ServicePriceUpdateListServicepriceFromDigipostRequest model, string token)
        {
            var result = _baseService.PostApi<string>("/api/serviceprice/import-list-serviceprice-from-digipost", model, token);
            return result;
        }
        public BaseResponse<string> UpdateServicePriceFromDigipost(ServicePriceUpdateServicepriceFromDigipostRequest model, string token)
        {
            var result = _baseService.PutApi<string>("/api/serviceprice/update-serviceprice-from-digipost", model, token);
            return result;
        }
        public BaseResponse<string> UpdateListServicePriceFromDigipost(ServicePriceUpdateListServicepriceFromDigipostRequest model, string token)
        {
            var result = _baseService.PutApi<string>("/api/serviceprice/update-list-serviceprice-from-digipost", model, token);
            return result;
        }
        public BaseResponse<ServicePriceResponse> GetFromId(Guid id, string token)
        {
            var result = _baseService.GetApi<ServicePriceResponse>($"/api/serviceprice/{id}", null, token);
            return result;
        }

    }
}
