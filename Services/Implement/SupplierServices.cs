using Infrastructure.Config;
using Services.Interfaces;
using Services.Models;
using Services.Models.Requests.Supplier;
using Services.Models.Responses.Supplier;
using System;
using System.Collections.Generic;

namespace Services.Implement
{
    public class SupplierServices : ISupplierServices
    {
        private BaseServices _baseService;
        public SupplierServices()
        {
            _baseService = new BaseServices(APIConfig.API_SERVICES_URL);
        }

        #region supplier
        public BaseResponse<PagingResult<SupplierResponse>> GetByPage(SupplierGetByPageRequest data, string token)
        {
            var result = _baseService.PostApi<PagingResult<SupplierResponse>>("/api/supplier/get-by-page", data, token);
            return result;
        }

        public BaseResponse<SupplierResponse> GetById(Guid id, string token)
        {
            var result = _baseService.GetApi<SupplierResponse>($"/api/supplier/{id}", null, token);
            return result;
        }

        public BaseResponse<List<SupplierTenantResponse>> SupplierTenantGetByKey(string key, string token)
        {
            var result = _baseService.GetApi<List<SupplierTenantResponse>>($"/api/supplier/supplier-tenant/get-by-key?Key={key}", null, token);
            return result;
        }

        public BaseResponse<string> Add(SupplierRequest data, string token)
        {
            var result = _baseService.PostApi<string>("/api/supplier", data, token, true);
            return result;
        }

        public BaseResponse<string> UpdateTenant(SupplierUpdateTenantRequest model, string token)
        {
            var result = _baseService.PutApi<string>($"/api/supplier/{model.Id}/update-tenant", model, token, true);
            return result;
        }

        public BaseResponse<string> UpdateBaseInfo(SupplierRequest model, string token)
        {
            var result = _baseService.PutApi<string>($"/api/supplier/{model.Id}/update-base-info", model, token, true);
            return result;
        }

        public BaseResponse<string> UpdateBookingInfo(SupplierRequest model, string token)
        {
            var result = _baseService.PutApi<string>($"/api/supplier/{model.Id}/update-booking-info", model, token, true);
            return result;
        }
        #endregion

        #region supplier bank
        public BaseResponse<List<SupplierBankResponse>> GetSupplierBank(Guid id, string token)
        {
            var result = _baseService.GetApi<List<SupplierBankResponse>>($"/api/supplier/{id}/supplier-bank", null, token);
            return result;
        }

        public BaseResponse<string> CreateSupplierBank(SupplierBankRequest model, string token)
        {
            var result = _baseService.PostApi<string>($"/api/supplier/{model.SupplierId}/supplier-bank", model, token, true);
            return result;
        }
        public BaseResponse<string> UpdateSupplierBank(SupplierBankRequest model, string token)
        {
            var result = _baseService.PutApi<string>($"/api/supplier/{model.SupplierId}/supplier-bank/{model.Id}", model, token, true);
            return result;
        }
        public BaseResponse<string> DeleteSupplierBank(SupplierBankRequest model, string token)
        {
            var result = _baseService.DeleteApi<string>($"/api/supplier/{model.SupplierId}/supplier-bank/{model.Id}", model, token, true);
            return result;
        }
        #endregion
    }
}
