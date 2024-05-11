using Services.Models;
using Services.Models.Requests.Content;
using Services.Models.Responses.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IContentServices
    {
        #region Template component type
        BaseResponse<string> TemplateComponentTypeAdd(string name, string token);
        BaseResponse<List<TemplateComponentTypeResponse>> TemplateComponentTypeGetList(string key, string token);
        BaseResponse<TemplateComponentTypeResponse> TemplateComponentTypeGetById(int id, string token);
        BaseResponse<string> TemplateComponentTypeEdit(TemplateComponentTypeEditRequest model, string token);
        BaseResponse<string> TemplateComponentTypeDelete(int id, string token);
        #endregion

        #region template component
        BaseResponse<string> TemplateComponentAdd(TemplateComponentAddRequest model, string token);
        BaseResponse<PagingResult<TemplateComponentResponse>> TemplateComponentGetByPage(TemplateComponentGetByPageRequest model, string token);
        BaseResponse<List<TemplateComponentGetByTemplateResponse>> TemplateComponentGetByTemplate(int id, string token);
        BaseResponse<TemplateComponentResponse> TemplateComponentGetById(int id, string token);
        BaseResponse<string> TemplateComponentEdit(TemplateComponentEditRequest model, string token);
        BaseResponse<string> TemplateComponentDelete(int id, string token);
        #endregion

        #region Template
        BaseResponse<string> TemplateAdd(TemplateAddRequest model, string token);
        BaseResponse<PagingResult<TemplateResponse>> TemplateGetByPage(TemplateGetByPageRequest model, string token);
        BaseResponse<PagingResult<TemplateResponse>> TemplateGetByPageForSuggest(TemplateGetByPageRequest model, string token);
        BaseResponse<TemplateResponse> TemplateGetById(int id, string token);
        BaseResponse<string> TemplateEdit(TemplateEditRequest model, string token);
        BaseResponse<string> TemplateDelete(int id, string token);
        BaseResponse<string> TemplateMap(TemplateMapRequest model, string token);
        BaseResponse<string> TemplateMapEdit(TemplateMapEditRequest model, string token);
        BaseResponse<string> TemplateMapDelete(int id, string token);
        #endregion

        #region Page
        BaseResponse<string> PageAdd(PageAddRequest model, string token);
        BaseResponse<PagingResult<PageResponse>> PageGetByPage(PageGetByPageRequest model, string token);
        BaseResponse<PageResponse> PageGetById(Guid id, string token);
        BaseResponse<string> PageEdit(PageEditRequest model, string token);
        BaseResponse<string> PageDelete(Guid id, string token);
        BaseResponse<string> PageStatusEdit(PageUpdateStatusRequest model, string token);
        BaseResponse<string> PageURLTargetEdit(PageUpdateURLTargetRequest model, string token);
        #endregion

        #region Page component
        BaseResponse<List<PageComponentResponse>> PageComponentGetByPageId(Guid id, string token);
        BaseResponse<PageComponentResponse> PageComponentGetById(Guid id, string token);
        BaseResponse<string> PageComponentAdd(PageComponentAddRequest Model, string token);
        BaseResponse<string> PageComponentEdit(PageComponentEditRequest model, string token);
        BaseResponse<string> PageComponentEditIdx(PageComponentUpdateIdxRequest model, string token);
        BaseResponse<string> PageComponentEditStatus(PageComponentStatusEditRequest model, string token);
        #endregion

        #region Page component items
        BaseResponse<PagingResult<PageComponentItemsResponse>> PageComponentItemGetByPage(PageComponentItemsGetByPageRequest model, string token);
        BaseResponse<string> PageComponentItemAdd(PageComponentItemsAddRequest model, string token);
        BaseResponse<PageComponentItemsResponse> PageComponentItemsGetById(int id, string token);
        BaseResponse<string> PageComponentItemsEdit(PageComponentItemsEditRequest model, string token);
        BaseResponse<string> PageComponentItemsDelete(int id, string token);
        BaseResponse<string> PageComponentItemsEditIdx(PageComponentItemsUpdateIdxRequest model, string token);
        #endregion
    }
}
