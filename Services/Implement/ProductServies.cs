using Infrastructure.Config;
using Services.Interfaces;
using Services.Models.Requests.Supplier;
using Services.Models.Responses.Supplier;
using Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Services.Models.Responses.Product;
using Services.Models.Requests.Product;

namespace Services.Implement
{
    public class ProductServies : IProductServices
    {
        private BaseServices _baseService;
        public ProductServies()
        {
            _baseService = new BaseServices(APIConfig.API_SERVICES_URL);
        }
        public BaseResponse<PagingResult<ProductResponse>> GetByPage(ProductGetByPageRequest data, string token)
        {
            var result = _baseService.PostApi<PagingResult<ProductResponse>>("/api/product/get-by-page", data, token);
            return result;
        }
        public BaseResponse<List<ProductGetListByTenantDigipostResponse>> GetListByTenantDigipost(ProductGetListByTenantFromDigipostRequest model, string token)
        {
            var result = _baseService.GetApi<List<ProductGetListByTenantDigipostResponse>>($"/api/product/get-list-by-tenant-from-digipost/{model.Id}", model, token);
            return result;
        }
        public BaseResponse<string> ImportProductFromDigipost(ProductImportProductFromDigipostRequest model, string token)
        {
            var result = _baseService.PostApi<string>("/api/product/import-product-from-digipost", model, token);
            return result;
        }
        public BaseResponse<ProductResponse> ProductGetById(Guid id, string token)
        {
            var result = _baseService.GetApi<ProductResponse>($"/api/product/{id}", null, token);
            return result;
        }
        public BaseResponse<string> UpdateMainInfo(ProductUpdateMainInfoRequest model, string token)
        {
            var result = _baseService.PutApi<string>($"/api/product/{model.Id}/update-main-info", model, token);
            return result;
        }
        public BaseResponse<string> UpdateBaseInfo(ProductUpdateBaseInfoRequest model, string token)
        {
            var result = _baseService.PutApi<string>($"/api/product/{model.Id}/update-base-info", model, token);
            return result;
        }
        public BaseResponse<string> UpdateImage(ProductUpdateImageRequest model, string token)
        {
            var result = _baseService.PutApi<string>($"/api/product/{model.Id}/update-image", model, token);
            return result;
        }
        public BaseResponse<string> UpdateSeoInfo(ProductUpdateSeoInfoRequest model, string token)
        {
            var result = _baseService.PutApi<string>($"/api/product/{model.Id}/update-seo-info", model, token);
            return result;
        }
        public BaseResponse<string> UpdateTags(ProductUpdateTagsRequest model, string token)
        {
            var result = _baseService.PutApi<string>($"/api/product/{model.Id}/update-tags", model, token);
            return result;
        }
        public BaseResponse<string> UpdateTagsGroup(ProductUpdateTagsGroupRequest model, string token)
        {
            var result = _baseService.PutApi<string>($"/api/product/{model.Id}/update-tags-group", model, token);
            return result;
        }
        public BaseResponse<string> UpdateConfigOrder(ProductUpdateConfigOderRequest model, string token)
        {
            var result = _baseService.PutApi<string>($"/api/product/{model.Id}/update-config-order", model, token);
            return result;
        }
        public BaseResponse<string> UpdateStatus(ProductUpdateStatusRequest model, string token)
        {
            var result = _baseService.PutApi<string>($"/api/product/{model.Id}/update-status", model, token);
            return result;
        }
        public BaseResponse<string> UpdateIdx(ProductUpdateIdxRequest model, string token)
        {
            var result = _baseService.PutApi<string>($"/api/product/{model.Id}/update-idx", model, token);
            return result;
        }
        public BaseResponse<string> UpdatePublic(Guid id, string token)
        {
            var result = _baseService.PutApi<string>($"/api/product/{id}/update-public", id, token);
            return result;
        }
        public BaseResponse<string> UpdateUnPublic(Guid id, string token)
        {
            var result = _baseService.PutApi<string>($"/api/product/{id}/update-unpublic", id, token);
            return result;
        }
        public BaseResponse<string> UpdateHot(ProductUpdateHotRequest model, string token)
        {
            var result = _baseService.PutApi<string>($"/api/product/{model.Id}/update-hot", model, token);
            return result;
        }
        public BaseResponse<string> UpdateHome(ProductUpdateHomeRequest model, string token)
        {
            var result = _baseService.PutApi<string>($"/api/product/{model.Id}/update-home", model, token);
            return result;
        }
        public BaseResponse<string> UpdateQueryPrice(Guid id, string token)
        {
            var result = _baseService.GetApi<string>($"/api/product/update-query-price?ProductId={id}", null, token);
            return result;
        }
    }
}
