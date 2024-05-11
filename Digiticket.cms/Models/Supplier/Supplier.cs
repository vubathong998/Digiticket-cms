using Services.Models.Responses.Lib;
using Services.Models.Responses.Supplier;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DigiticketCMS.Models.Supplier
{
    public class Supplier : BaseViewModel
    {

    }

    public class SupplierGetByPageViewModel
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }

    public class SupplierCreateViewModel
    {
        [Required]
        public string Name { get; set; }
        //[Required]
        //public string Code { get; set; }
        public string ShortDescription { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string ContactName { get; set; }
        [Required]
        public string Phone { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Hotline { get; set; }
        [Required]
        public string BookingEmail { get; set; }
        public string CompanyName { get; set; }
        public string CompanyAddress { get; set; }
        public string CompanyTaxCode { get; set; }
        public string CompanyRepresentative { get; set; }
    }

    public class SupplierUpdateBaseInfoViewModel
    {
        [Required]
        public string Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string ShortDescription { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string ContactName { get; set; }
        [Required]
        public string Phone { get; set; }
        [Required]
        public string Email { get; set; }
        public string CompanyName { get; set; }
        public string CompanyAddress { get; set; }
        public string CompanyTaxCode { get; set; }
        public string CompanyRepresentative { get; set; }
    }

    public class SupplierUpdateTenantViewModel
    {
        [Required]
        public string Id { get; set; }
        public string Name { get; set; }
        public int TenantId { get; set; }
        public string TenantCode { get; set; }
    }

    public class SupplierUpdateBookingInfoViewModel
    {
        [Required]
        public string Id { get; set; }
        [Required]
        public string Hotline { get; set; }
        [Required]
        public string BookingEmail { get; set; }
    }

    public class SupplierCreateBankViewModel
    {
        [Required]
        public string SupplierId { get; set; }
        public int? Id { get; set; }
        [Required]
        public string AccountName { get; set; }
        [Required]
        public string AccountNo { get; set; }
        [Required]
        public string BankName { get; set; }
        [Required]
        public string BranchName { get; set; }
    }

    public class SupplierDeleteBankViewModel
    {
        [Required]
        public string Id { get; set; }
        [Required]
        public string SupplierId { get; set; }
    }

    public class SupplierGetFromIdViewToHtmlViewModel
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
        public string LookingFor { get; set; }
    }
    
    public class SupplierViewModel
    {
        public List<SubCategoryResponse> Categories { get; set; }
        public List<LibDestinationResponse> Destinations { get; set; }
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
        public string LookingFor { get; set; }
    }
}