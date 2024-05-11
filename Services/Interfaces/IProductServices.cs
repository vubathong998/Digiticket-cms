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

namespace Services.Interfaces
{
    public interface IProductServices
    {
        BaseResponse<PagingResult<ProductResponse>> GetByPage(ProductGetByPageRequest data, string token);
        BaseResponse<List<ProductGetListByTenantDigipostResponse>> GetListByTenantDigipost(ProductGetListByTenantFromDigipostRequest model, string token);
        BaseResponse<string> ImportProductFromDigipost(ProductImportProductFromDigipostRequest model, string token);
        BaseResponse<ProductResponse> ProductGetById(Guid id, string token);
        BaseResponse<string> UpdateMainInfo(ProductUpdateMainInfoRequest model, string token);
        BaseResponse<string> UpdateBaseInfo(ProductUpdateBaseInfoRequest model, string token);
        BaseResponse<string> UpdateImage(ProductUpdateImageRequest model, string token);
        BaseResponse<string> UpdateSeoInfo(ProductUpdateSeoInfoRequest model, string token);
        BaseResponse<string> UpdateTags(ProductUpdateTagsRequest model, string token);
        BaseResponse<string> UpdateTagsGroup(ProductUpdateTagsGroupRequest model, string token);
        BaseResponse<string> UpdateConfigOrder(ProductUpdateConfigOderRequest model, string token);
        BaseResponse<string> UpdateStatus(ProductUpdateStatusRequest model, string token);
        BaseResponse<string> UpdateIdx(ProductUpdateIdxRequest model, string token);
        BaseResponse<string> UpdatePublic(Guid id, string token);
        BaseResponse<string> UpdateUnPublic(Guid id, string token);
        BaseResponse<string> UpdateHot(ProductUpdateHotRequest model, string token);
        BaseResponse<string> UpdateHome(ProductUpdateHomeRequest model, string token);
        BaseResponse<string> UpdateQueryPrice(Guid id, string token);
    }
}
