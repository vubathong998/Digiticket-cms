using DigiticketCMS.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Models.Requests.Supplier
{
    public class SupplierGetByPageRequest : BaseRequest
    {
        
    }

    public class SupplierRequest
    {
        public Guid Id { get; set; }
        public int TenantId { get; set; }
        public string TenantCode { get; set; }
        public string Name { get; set; }
        //public string Code { get; set; }
        public string ShortDescription { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        public string Title { get; set; }
        public string ContactName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Hotline { get; set; }
        public string BookingEmail { get; set; }
        public string CompanyName { get; set; }
        public string CompanyAddress { get; set; }
        public string CompanyTaxCode { get; set; }
        public string CompanyRepresentative { get; set; }
    }

    public class SupplierBankRequest
    {
        public int Id { get; set; }
        public string SupplierId { get; set; }
        public string AccountName { get; set; }
        public string AccountNo { get; set; }
        public string BankName { get; set; }
        public string BranchName { get; set; }
    }

    public class SupplierCreateRequest
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public string ShortDescription { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        public string Title { get; set; }
        public string ContactName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Hotline { get; set; }
        public string BookingEmail { get; set; }
        public string CompanyName { get; set; }
        public string CompanyAddress { get; set; }
        public string CompanyTaxCode { get; set; }
        public string CompanyRepresentative { get; set; }
    }
    public class SupplierUpdateBaseInfoRequest
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string ShortDescription { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        public string Title { get; set; }
        public string ContactName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string CompanyName { get; set; }
        public string CompanyAddress { get; set; }
        public string CompanyTaxCode { get; set; }
        public string CompanyRepresentative { get; set; }
    }
    public class SupplierUpdateTenantRequest
    {
        public string Id { get; set; }
        public int TenantId { get; set; }
        public string TenantCode { get; set; }
        public string Name { get; set; }
    }
    public class SupplierUpdateBookingInfoRequest
    {
        public string Id { get; set; }
        public string Hotline { get; set; }
        public string BookingEmail { get; set; }
    }

    public class SupplierCreateBankRequest
    {
        public string SupplierId { get; set; }
        public string AccountName { get; set; }
        public string AccountNo { get; set; }
        public string BankName { get; set; }
        public string BranchName { get; set; }
    }
    public class SupplierUpdateBankRequest : SupplierCreateBankRequest
    {
        public int? Id { get; set; }
    }
    public class SupplierDeleteBankRequest
    {
        public int Id { get; set; }
        public string SupplierId { get; set; }
    }
}