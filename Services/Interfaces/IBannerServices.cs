using Services.Models;
using Services.Models.Requests.Banner;
using Services.Models.Responses.Banner;

namespace Services.Interfaces
{
    public interface IBannerServices
    {
        BaseResponse<PagingResult<BannerResponse>> GetByPage(BannerRequest data, string token);
        BaseResponse<string> Add(BannerAddRequest data, string token);
        BaseResponse<string> Edit(BannerEditRequest data, string token);
        BaseResponse<BannerResponse> GetById(int id, string token);
        BaseResponse<string> Delete(int id, string token);
        BaseResponse<string> ToggleView(int id, string token);
        BaseResponse<BannerTypeResponse> TypeGetById(int id, string token);
        BaseResponse<string> TypeEdit(BannerTypeEditRequest data, string token);
        BaseResponse<string> TypeAdd(BannerTypeAddRequest data, string token);
        BaseResponse<PagingResult<BannerTypeResponse>> TypeGetByPage(BannerTypeRequest model, string token);
        BaseResponse<string> TypeDelete(int id, string token);
    }
}
