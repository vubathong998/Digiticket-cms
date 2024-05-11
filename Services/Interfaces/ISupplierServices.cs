using Services.Models;
using Services.Models.Requests.Supplier;
using Services.Models.Responses.Supplier;
using System;
using System.Collections.Generic;

namespace Services.Interfaces
{
    public interface ISupplierServices
    {
        #region supplier
        BaseResponse<PagingResult<SupplierResponse>> GetByPage(SupplierGetByPageRequest data, string token);
        BaseResponse<SupplierResponse> GetById(Guid id, string token);
        BaseResponse<List<SupplierTenantResponse>> SupplierTenantGetByKey(string key, string token);
        BaseResponse<string> Add(SupplierRequest data, string token);
        BaseResponse<string> UpdateTenant(SupplierUpdateTenantRequest model, string token);
        BaseResponse<string> UpdateBaseInfo(SupplierRequest model, string token);
        BaseResponse<string> UpdateBookingInfo(SupplierRequest model, string token);
        #endregion

        #region supplier bank
        BaseResponse<List<SupplierBankResponse>> GetSupplierBank(Guid id, string token);
        BaseResponse<string> CreateSupplierBank(SupplierBankRequest model, string token);
        BaseResponse<string> UpdateSupplierBank(SupplierBankRequest model, string token);
        BaseResponse<string> DeleteSupplierBank(SupplierBankRequest model, string token);
        #endregion
    }
}