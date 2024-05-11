using Infrastructure.Config;
using Services.Interfaces;
using Services.Models.Requests.Product;
using Services.Models.Responses.Product;
using Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Implement
{
    public class PlaceServices : IPlaceServices
    {
        private BaseServices _baseService;
        public PlaceServices()
        {
            _baseService = new BaseServices(APIConfig.API_SERVICES_URL);
        }
        public BaseResponse<string> GetSSID(string token)
        {
            var result = _baseService.GetApi<string>("api/place/get-ssid",null ,token);
            return result;
        }
        public BaseResponse<ProductPlaceSuggestResponse> Suggest(ProductPlaceSuggestRequest model, string token)
        {
            var result = _baseService.GetApi<ProductPlaceSuggestResponse>("/api/place/suggest", model, token);
            return result;
        }
        public BaseResponse<ProductPlaceDetailResponse> Detail(ProductPlaceDetailRequest model, string token)
        {
            var result = _baseService.GetApi<ProductPlaceDetailResponse>("/api/place/detail", model, token);
            return result;
        }
    }
}
