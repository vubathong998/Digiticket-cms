using Infrastructure.Config;
using Services.Interfaces;
using Services.Models;
using Services.Models.Requests.Banner;
using Services.Models.Requests.ServicePrice;
using Services.Models.Responses.Banner;
using Services.Models.Responses.ServicePrice;

namespace Services.Implement
{
    public class BannerServices : IBannerServices
    {
        private BaseServices _baseService;
        public BannerServices()
        {
            _baseService = new BaseServices(APIConfig.API_SERVICES_URL);
        }
        public BaseResponse<PagingResult<BannerResponse>> GetByPage(BannerRequest data, string token)
        {
            var result = _baseService.PostApi<PagingResult<BannerResponse>>("/api/banner/get-by-page", data, token);
            return result;
        }
        public BaseResponse<string> Add(BannerAddRequest data, string token)
        {
            var result = _baseService.PostApi<string>($"/api/banner", data, token, true);
            return result;
        }
        public BaseResponse<string> Edit(BannerEditRequest data, string token)
        {
            var result = _baseService.PutApi<string>($"/api/banner/{data.Id}", data, token, true);
            return result;
        }
        public BaseResponse<BannerResponse> GetById(int id, string token)
        {
            var result = _baseService.GetApi<BannerResponse>($"/api/banner/{id}", null, token, true);
            return result;
        }
        public BaseResponse<string> Delete(int id, string token)
        {
            var result = _baseService.DeleteApi<string>($"/api/banner/{id}", null, token, true);
            return result;
        }
        public BaseResponse<string> ToggleView(int id, string token)
        {
            var result = _baseService.PutApi<string>($"/api/banner/{id}/toggle-view", new { id = id }, token, true);
            return result;
        }

        public BaseResponse<PagingResult<BannerTypeResponse>> TypeGetByPage(BannerTypeRequest model, string token)
        {
            var result = _baseService.PostApi<PagingResult<BannerTypeResponse>>("/api/bannertype/get-by-page", model, token, true);
            return result;
        }
        public BaseResponse<string> TypeAdd(BannerTypeAddRequest data, string token)
        {
            var result = _baseService.PostApi<string>($"/api/bannertype", data, token, true);
            return result;
        }
        public BaseResponse<string> TypeEdit(BannerTypeEditRequest data, string token)
        {
            var result = _baseService.PutApi<string>($"/api/bannertype/{data.Id}", data, token, true);
            return result;
        }
        public BaseResponse<BannerTypeResponse> TypeGetById(int id, string token)
        {
            var result = _baseService.GetApi<BannerTypeResponse>($"/api/bannertype/{id}", null, token, true);
            return result;
        }
        public BaseResponse<string> TypeDelete(int id, string token)
        {
            var result = _baseService.DeleteApi<string>($"/api/bannertype/{id}", null, token, true);
            return result;
        }
    }
}
