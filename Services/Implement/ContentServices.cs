using Infrastructure.Config;
using Services.Models.Requests.GroupdService;
using Services.Models.Responses.GroupService;
using Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Services.Models.Responses.Content;
using Services.Models.Requests.Content;
using Services.Interfaces;
using Utilities.Caching;
using Infrastructure.Extensions;

namespace Services.Implement
{
    public class ContentServices : IContentServices
    {
        #region Constructor
        private BaseServices _baseService;
        public ContentServices()
        {
            _baseService = new BaseServices(APIConfig.API_SERVICES_URL);
        }
        #endregion

        #region Template component type
        public BaseResponse<string> TemplateComponentTypeAdd(string name, string token)
        {
            var result = _baseService.PostApi<string>("/api/content/template-component-type", new { Name = name }, token, true);
            return result;
        }
        #endregion
        #region Template componenet
        public BaseResponse<List<TemplateComponentTypeResponse>> TemplateComponentTypeGetList(string key, string token)
        {
            var result = _baseService.GetApi<List<TemplateComponentTypeResponse>>($"/api/content/template-component-type/get-list?Key={key}", null, token, true);
            return result;
        }
        public BaseResponse<TemplateComponentTypeResponse> TemplateComponentTypeGetById(int id, string token)
        {
            var result = _baseService.GetApi<TemplateComponentTypeResponse>($"/api/content/template-component-type/{id}", null, token, true);
            return result;
        }
        public BaseResponse<string> TemplateComponentTypeEdit(TemplateComponentTypeEditRequest model, string token)
        {
            var result = _baseService.PutApi<string>($"/api/content/template-component-type/{model.Id}", model, token, true);
            return result;
        }
        public BaseResponse<string> TemplateComponentTypeDelete(int id, string token)
        {
            var result = _baseService.DeleteApi<string>($"/api/content/template-component-type/{id}", null, token, true);
            return result;
        }
        #endregion

        #region Template component
        public BaseResponse<string> TemplateComponentAdd(TemplateComponentAddRequest model, string token)
        {
            var result = _baseService.PostApi<string>("/api/content/template-component", model, token, true);
            return result;
        }
        public BaseResponse<PagingResult<TemplateComponentResponse>> TemplateComponentGetByPage(TemplateComponentGetByPageRequest model, string token)
        {
            var result = _baseService.PostApi<PagingResult<TemplateComponentResponse>>("/api/content/template-component/get-by-page", model, token, true);
            return result;
        }
        public BaseResponse<List<TemplateComponentGetByTemplateResponse>> TemplateComponentGetByTemplate(int id, string token)
        {
            var result = _baseService.GetApi<List<TemplateComponentGetByTemplateResponse>>($"/api/content/template-component/get-by-template/{id}", null, token, true);
            return result;
        }
        public BaseResponse<TemplateComponentResponse> TemplateComponentGetById(int id, string token)
        {
            var result = _baseService.GetApi<TemplateComponentResponse>($"/api/content/template-component/{id}", null, token, true);
            return result;
        }
        public BaseResponse<string> TemplateComponentEdit(TemplateComponentEditRequest model, string token)
        {
            var result = _baseService.PutApi<string>($"/api/content/template-component/{model.Id}", model, token, true);
            return result;
        }
        public BaseResponse<string> TemplateComponentDelete(int id, string token)
        {
            var result = _baseService.DeleteApi<string>($"/api/content/template-component/{id}", null, token, true);
            return result;
        }
        #endregion

        #region Template
        public BaseResponse<string> TemplateAdd(TemplateAddRequest model, string token)
        {
            var result = _baseService.PostApi<string>("/api/content/template", model, token, true);
            return result;
        }
        public BaseResponse<PagingResult<TemplateResponse>> TemplateGetByPage(TemplateGetByPageRequest model, string token)
        {
            var result = _baseService.PostApi<PagingResult<TemplateResponse>>("/api/content/template/get-by-page", model, token, true);
            return result;
        }
        public BaseResponse<PagingResult<TemplateResponse>> TemplateGetByPageForSuggest(TemplateGetByPageRequest model, string token)
        {
            var keyToken = CacheNameConfig.PageSuggest;
            var result = new BaseResponse<PagingResult<TemplateResponse>>();
            if (model.Keyword == "")
            {
                var cache = new PagingResult<TemplateResponse>();
                cache = CacheUtility.GetValue(keyToken) as PagingResult<TemplateResponse>;
                if (cache != null && !cache.Result.IsNullOrEmpty())
                {
                    result.Code = 200;
                    result.Message = "Success";
                    result.Data = cache;
                }
            }
            result = _baseService.PostApi<PagingResult<TemplateResponse>>("/api/content/template/get-by-page", model, token, true);
            CacheUtility.Add(keyToken, result.Data, DateTimeOffset.Now.AddMinutes(10));
            return result;
        }
        public BaseResponse<TemplateResponse> TemplateGetById(int id, string token)
        {
            var result = _baseService.GetApi<TemplateResponse>($"/api/content/template/{id}", null, token, true);
            return result;
        }
        public BaseResponse<string> TemplateEdit(TemplateEditRequest model, string token)
        {
            var result = _baseService.PutApi<string>($"/api/content/template/{model.Id}", model, token, true);
            return result;
        }
        public BaseResponse<string> TemplateDelete(int id, string token)
        {
            var result = _baseService.DeleteApi<string>($"/api/content/template/{id}", null, token, true);
            return result;
        }
        public BaseResponse<string> TemplateMap(TemplateMapRequest model, string token)
        {
            var result = _baseService.PostApi<string>("/api/content/template-map", model, token, true);
            return result;
        }
        public BaseResponse<string> TemplateMapEdit(TemplateMapEditRequest model, string token)
        {
            var result = _baseService.PutApi<string>($"/api/content/template-map/{model.Id}", model, token, true);
            return result;
        }
        public BaseResponse<string> TemplateMapDelete(int id, string token)
        {
            var result = _baseService.DeleteApi<string>($"/api/content/template-map/{id}", null, token, true);
            return result;
        }
        #endregion

        #region Page
        public BaseResponse<string> PageAdd(PageAddRequest model, string token)
        {
            var result = _baseService.PostApi<string>("/api/content/page", model, token, true);
            return result;
        }
        public BaseResponse<PagingResult<PageResponse>> PageGetByPage(PageGetByPageRequest model, string token)
        {
            var result = _baseService.PostApi<PagingResult<PageResponse>>("/api/content/page/get-by-page", model, token, true);
            return result;
        }
        public BaseResponse<PageResponse> PageGetById(Guid id, string token)
        {
            var result = _baseService.GetApi<PageResponse>($"/api/content/page/{id}", null, token, true);
            return result;
        }
        public BaseResponse<string> PageEdit(PageEditRequest model, string token)
        {
            var result = _baseService.PutApi<string>($"/api/content/page/{model.Id}", model, token, true);
            return result;
        }
        public BaseResponse<string> PageDelete(Guid id, string token)
        {
            var result = _baseService.DeleteApi<string>($"/api/content/page/{id}", null, token, true);
            return result;
        }
        public BaseResponse<string> PageStatusEdit(PageUpdateStatusRequest model, string token)
        {
            var result = _baseService.PutApi<string>($"/api/content/page/{model.Id}/update-status", model, token, true);
            return result;
        }
        public BaseResponse<string> PageURLTargetEdit(PageUpdateURLTargetRequest model, string token)
        {
            var result = _baseService.PutApi<string>($"/api/content/page/{model.Id}/update-url-target", model, token, true);
            return result;
        }
        #endregion

        #region Page component
        public BaseResponse<string> PageComponentAdd(PageComponentAddRequest Model, string token)
        {
            var result = _baseService.PostApi<string>("/api/content/page-component", Model, token, true);
            return result;
        }

        public BaseResponse<List<PageComponentResponse>> PageComponentGetByPageId(Guid id, string token)
        {
            var result = _baseService.GetApi<List<PageComponentResponse>>($"/api/content/page-component/get-by-page-id/{id}", null, token, true);
            return result;
        }

        public BaseResponse<PageComponentResponse> PageComponentGetById(Guid id, string token)
        {
            var result = _baseService.GetApi<PageComponentResponse>($"/api/content/page-component/{id}", null, token, true);
            return result;
        }

        public BaseResponse<string> PageComponentEdit(PageComponentEditRequest model, string token)
        {
            var result = _baseService.PutApi<string>($"/api/content/page-component/{model.Id}", model, token, true);
            return result;
        }

        public BaseResponse<string> PageComponentEditIdx(PageComponentUpdateIdxRequest model, string token)
        {
            var result = _baseService.PutApi<string>("/api/content/page-component/update-idx", model, token, true);
            return result;
        }

        public BaseResponse<string> PageComponentEditStatus(PageComponentStatusEditRequest model, string token)
        {
            var result = _baseService.PutApi<string>($"/api/content/page-component/{model.Id}/update-status", model, token, true);
            return result;
        }
        #endregion

        #region Page component items
        public BaseResponse<PagingResult<PageComponentItemsResponse>> PageComponentItemGetByPage(PageComponentItemsGetByPageRequest model, string token)
        {
            var result = _baseService.PostApi<PagingResult<PageComponentItemsResponse>>("/api/content/page-component/items/get-by-page", model, token, true);
            return result;
        }
        public BaseResponse<string> PageComponentItemAdd(PageComponentItemsAddRequest model, string token)
        {
            var result = _baseService.PostApi<string>("/api/content/page-component/items", model, token, true);
            return result;
        }
        public BaseResponse<PageComponentItemsResponse> PageComponentItemsGetById(int id, string token)
        {
            var result = _baseService.GetApi<PageComponentItemsResponse>($"/api/content/page-component/items/{id}", null, token, true);
            return result;
        }
        public BaseResponse<string> PageComponentItemsEdit(PageComponentItemsEditRequest model, string token)
        {
            var result = _baseService.PutApi<string>($"/api/content/page-component/items/{model.Id}", model, token, true);
            return result;
        }
        public BaseResponse<string> PageComponentItemsDelete(int id, string token)
        {
            var result = _baseService.DeleteApi<string>($"/api/content/page-component/items/{id}", null, token, true);
            return result;
        }

        public BaseResponse<string> PageComponentItemsEditIdx(PageComponentItemsUpdateIdxRequest model, string token)
        {
            var result = _baseService.PutApi<string>("api/content/page-component/items/update-idx", model, token, true);
            return result;
        }

        #endregion
    }
}
