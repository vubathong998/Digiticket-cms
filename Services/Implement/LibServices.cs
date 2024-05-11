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
using Services.Models.Responses.Lib;
using Services.Models.Requests.Lib;

namespace Services.Implement
{
    public class LibServices : ILibServices
    {
        #region constructor
        private BaseServices _baseService;
        public LibServices()
        {
            _baseService = new BaseServices(APIConfig.API_SERVICES_URL);
        }
        #endregion

        #region category
        public BaseResponse<List<SubCategoryResponse>> CategoryGetAll(string token)
        {
            var result = _baseService.GetApi<List<SubCategoryResponse>>("/api/lib/category/get-all", null, token);
            return result;
        }
        public BaseResponse<LibCategoryResponse> CategoryGetById(Guid id, string token)
        {
            var result = _baseService.GetApi<LibCategoryResponse>("/api/lib/category/" + id, null, token);
            return result;
        }
        public BaseResponse<string> CategoryUpdate(LibCategoryUpdateRequest model, string token)
        {
            var result = _baseService.PutApi<string>("/api/lib/category/" + model.Id, model, token);
            return result;
        }

        #endregion

        #region destination
        public BaseResponse<List<LibDestinationResponse>> DestinationGetAll(string token)
        {
            var result = _baseService.GetApi<List<LibDestinationResponse>>("/api/lib/destination/get-all", null, token);
            return result;
        }
        public BaseResponse<PagingResult<LibDestinationResponse>> DestinationGetByPage(LibDestinationRequest model, string token)
        {
            var result = _baseService.PostApi<PagingResult<LibDestinationResponse>>("/api/lib/destination/get-by-page", model, token);
            return result;
        }
        public BaseResponse<LibDestinationResponse> DestinationGetById(Guid Id, string token)
        {
            var result = _baseService.GetApi<LibDestinationResponse>("/api/lib/destination/" + Id, null, token);
            return result;
        }
        public BaseResponse<string> DestinationUpdate(LibDestinationUpdateRequest model, string token)
        {
            var result = _baseService.PutApi<string>("/api/lib/destination/" + model.Id, model, token);
            return result;
        }
        #endregion

        public BaseResponse<List<LibTagViewResponse>> TagView(string token)
        {
            var result = _baseService.GetApi<List<LibTagViewResponse>>("/api/lib/tag-view/get-all", null, token);
            return result;
        }
    }
}
