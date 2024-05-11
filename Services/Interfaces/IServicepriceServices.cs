using Services.Models;
using Services.Models.Requests.ServicePrice;
using Services.Models.Responses.ServicePrice;
using System;
using System.Collections.Generic;

namespace Services.Interfaces
{
    public interface IServicepriceServices
    {
        BaseResponse<PagingResult<ServicePriceResponse>> GetByPage(ServicePriceGetByPageRequest model, string token);
        BaseResponse<List<ServicePriceResponse>> GetListByProductFromDigipost(Guid id, string token);
        BaseResponse<string> ImportServicePriceFromDigipost(ServicePriceImportServiceFromDigiPostRequest model, string token);
        BaseResponse<string> ImportListServicePriceFromDigipost(ServicePriceUpdateListServicepriceFromDigipostRequest model, string token);
        BaseResponse<string> UpdateServicePriceFromDigipost(ServicePriceUpdateServicepriceFromDigipostRequest model, string token);
        BaseResponse<string> UpdateListServicePriceFromDigipost(ServicePriceUpdateListServicepriceFromDigipostRequest model, string token);
        BaseResponse<ServicePriceResponse> GetFromId(Guid id, string token);
    }
}
