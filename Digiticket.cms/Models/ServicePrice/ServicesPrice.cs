using KellermanSoftware.CompareNetObjects;
using Services;
using Services.Models.Responses.ServicePrice;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DigiticketCMS.Models.ServicePrice
{
    public class ServicesPrice
    {
    }
    public class ServicePriceImportServiceFromDigiPostViewModel
    {
        [Required]
        public Guid ServicePriceId { get; set; }
        [Required]
        public Guid ProductId { get; set; }
    }
    public class ServicePriceGetByPageViewModel : BaseViewModel
    {
        public Guid? ProductId { get; set; }
    }
    public class ServicePriceUpdateServicepriceFromDigipostViewModel
    {
        [Required]
        public Guid ServicePriceId { get; set; }
        [Required]
        public Guid ProductId { get; set; }
    }
    public class ServicePriceUpdateListServicepriceFromDigipostViewModel
    {
        public string ProductId { get; set; }
        public List<Guid> ServicePriceIds { get; set; }
        public bool KeepModal { get; set; }
        public ServicePriceUpdateListServicepriceFromDigipostViewModel ()
        {
            KeepModal = false;
        }
    }
    public class ServicePriceViewToViewModel
    {
        public string PriceConfigId { get; set; }
        public string Name { get; set; }
        public PricesTime PricesTime { get; set; }
        public List<ListPriceViewModel> ListPrices { get; set; }
        public object Images { get; set; }
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
    public class ListPriceViewModel
    {
        public string GroupServiceId { get; set; }
        public string GroupServiceName { get; set; }
        public string ProfileType { get; set; }
        public List<BaseImg> Images { get; set; }
        public double? OriginalPrice { get; set; }
        public double? CostPrice { get; set; }
        public double RecommendPrice { get; set; }
        public int? PriceType { get; set; }
        public int CommissionUnit { get; set; }
        public double? CommissionValue { get; set; }
        public double? SalePrice { get; set; }
        public bool SupplierSetEndUsedPrice { get; set; }
        public string EndUsedPriceType { get; set; }
        public string EndUsedCommissionUnit { get; set; }
        public string EndUsedCommissionValue { get; set; }
        public decimal? EndUsedPrice { get; set; }
        public ProfileData ProfileData { get; set; }
    }

    public class ServicePriceCompareViewModel
    {
        public ServicePriceResponse NewServicePrice { get; set; }
        public ServicePriceResponse OldServiceprice { get; set; }
    }
    public class ServicePriceCompareResultToViewViewModal
    {
        public ServicePriceCompareResult CompareResult { get; set; }
        public ServicePriceResponse OldServicePrice { get; set; }
        public ServicePriceResponse NewServicePrice { get; set; }
        public ServicePriceCompareResultToViewViewModal()
        {
            CompareResult = new ServicePriceCompareResult();
            OldServicePrice = new ServicePriceResponse();
            NewServicePrice = new ServicePriceResponse();
        }
    }
    public class ServicePriceCompareResult
    {
        public ComparisonResult All { get; set; }
        public ComparisonResult PricesTime { get; set; }
        public ComparisonResult ListPrices { get; set; }
    }
}