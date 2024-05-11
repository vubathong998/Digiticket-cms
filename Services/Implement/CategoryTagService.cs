using Infrastructure.Config;
using Services.Models.Requests.Content;
using Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Services.Models.Requests.CategoryTag;
using Services.Models.Responses.CategoryTag;
using Services.Interfaces;
using Services.Models.Requests.Tags;

namespace Services.Implement
{
    public class CategoryTagService : ICategoryTagService
    {
        #region Constructor
        private BaseServices _baseService;
        public CategoryTagService()
        {
            _baseService = new BaseServices(APIConfig.API_SERVICES_URL);
        }
        #endregion

        public BaseResponse<string> Add(CategoryTagAddRequest model, string token)
        {
            var result = _baseService.PostApi<string>("/api/categorytag", model, token, true);
            return result;
        }

        public BaseResponse<string> Edit(CategoryTagEditRequest model, string token)
        {
            var result = _baseService.PutApi<string>($"/api/categorytag/{model.Id}", model, token, true);
            return result;
        }

        public BaseResponse<string> Delete(int id, string token)
        {
            var result = _baseService.DeleteApi<string>($"/api/categorytag/{id}", null, token, true);
            return result;
        }

        public BaseResponse<CategoryTagResponse> GetById(int id, string token)
        {
            var result = _baseService.GetApi<CategoryTagResponse>($"/api/categorytag/{id}", null, token, true);
            return result;
        }

        public BaseResponse<PagingResult<CategoryTagResponse>> GetByPage(CategoryTagGetByPageRequest model, string token)
        {
            var result = _baseService.PostApi<PagingResult<CategoryTagResponse>>("api/categorytag/get-by-page", model, token, true);
            return result;
        }

        public BaseResponse<string> UpdateHome(CategoryTagUpdateHomeRequest model, string token)
        {
            var result = _baseService.PutApi<string>($"api/categorytag/{model.Id}/update-home", model, token, true);
            return result;
        }

        public BaseResponse<string> UpdateHot(CategoryTagUpdateHotRequest model, string token)
        {
            var result = _baseService.PutApi<string>($"api/categorytag/{model.Id}/update-hot", model, token, true);
            return result;
        }
    }
}
