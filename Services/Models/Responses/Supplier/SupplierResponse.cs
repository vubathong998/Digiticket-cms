using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Models.Responses.Supplier
{
    public class ProfileData
    {
    }

    public class SupplierResponse
    {
        public string Id { get; set; }
        public int TenantId { get; set; }
        public string TenantCode { get; set; }
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
        public int TotalRows { get; set; }
        public int Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public string CreatedByName { get; set; }
        public DateTime LastEditedDate { get; set; }
        public int LastEditedBy { get; set; }
        public string LastEditedByName { get; set; }
        public ProfileData ProfileData { get; set; }
    }

    public class SupplierTenantResponse
    {
        public int TenantId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
    }

    public class SupplierGetFromIdResponse
    {
        public string Id { get; set; }
        public int TenantId { get; set; }
        public string TenantCode { get; set; }
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
        public int TotalRows { get; set; }
        public int Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public string CreatedByName { get; set; }
        public DateTime LastEditedDate { get; set; }
        public int LastEditedBy { get; set; }
        public string LastEditedByName { get; set; }
        public ProfileData ProfileData { get; set; }
    }

    public class SupplierGetByKeyResponse
    {
        public int TenantId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
    }
    
    public class SupplierGetByPageResponse
    {
        public int TenantId { get; set; }
        public string TenantCode { get; set; }
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
        public string SupplierBanks { get; set; }
        public int TotalRows { get; set; }
        public string Id { get; set; }
        public int Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public string CreatedByName { get; set; }
        public DateTime LastEditedDate { get; set; }
        public int LastEditedBy { get; set; }
        public string LastEditedByName { get; set; }
        public ProfileData ProfileData { get; set; }
    }

    public class SupplierBankResponse
    {
        public string SupplierId { get; set; }
        public string AccountName { get; set; }
        public string AccountNo { get; set; }
        public string BankName { get; set; }
        public string BranchName { get; set; }
        public int Id { get; set; }
        public int Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public string CreatedByName { get; set; }
        public DateTime LastEditedDate { get; set; }
        public int LastEditedBy { get; set; }
        public string LastEditedByName { get; set; }
        public ProfileData ProfileData { get; set; }
    }
}
