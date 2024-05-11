using System;
using System.Collections.Generic;

namespace Services.Models.Responses.ServicePrice
{
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class CompareIgnoreAttribute : Attribute
    {
    }
    public class ServicePriceResponse
    {
        [CompareIgnore]
        public string PriceConfigId { get; set; }
        public string Name { get; set; }
        public PricesTime PricesTime { get; set; }
        public List<ListPriceResponse> ListPrices { get; set; }
        [CompareIgnore]
        public string DigipostName { get; set; }
        [CompareIgnore]
        public bool IsChanged { get; set; }
        [CompareIgnore]
        public object Images { get; set; }
        [CompareIgnore]
        public string Id { get; set; }
        [CompareIgnore]
        public bool IsExist { get; set; }
        [CompareIgnore]
        public int Status { get; set; }
        [CompareIgnore]
        public DateTime CreatedDate { get; set; }
        [CompareIgnore]
        public int CreatedBy { get; set; }
        [CompareIgnore]
        public string CreatedByName { get; set; }
        [CompareIgnore]
        public DateTime LastEditedDate { get; set; }
        [CompareIgnore]
        public int LastEditedBy { get; set; }
        [CompareIgnore]
        public string LastEditedByName { get; set; }
        [CompareIgnore]
        public ProfileData ProfileData { get; set; }
    }
    public class PricesTime
    {
        public int TimeType { get; set; }
        public DateTime BeginDate { get; set; }
        public DateTime EndDate { get; set; }
        public List<int> ListDayOfWeek { get; set; }
        public List<DateTime> ListDate { get; set; }
        public object ListTime { get; set; }
        public List<DateTime> ListDateOff { get; set; }
        public PricesTime()
        {
            ListDayOfWeek = new List<int>();    
        }
    }
    public class ListPriceResponse
    {
        public string GroupServiceId { get; set; }
        public string GroupServiceName { get; set; }
        [CompareIgnore]
        public string ProfileType { get; set; }
        [CompareIgnore]
        public List<BaseImg> Images { get; set; }
        [CompareIgnore]
        public double? OriginalPrice { get; set; }
        public double? CostPrice { get; set; }
        [CompareIgnore]
        public double? RecommendPrice { get; set; }
        [CompareIgnore]
        public int? PriceType { get; set; }
        public int? CommissionUnit { get; set; }
        public double? CommissionValue { get; set; }
        public double? SalePrice { get; set; }
        [CompareIgnore]
        public bool SupplierSetEndUsedPrice { get; set; }
        [CompareIgnore]
        public string EndUsedPriceType { get; set; }
        [CompareIgnore]
        public string EndUsedCommissionUnit { get; set; }
        [CompareIgnore]
        public string EndUsedCommissionValue { get; set; }
        public decimal? EndUsedPrice { get; set; }
        [CompareIgnore]
        public ProfileData ProfileData { get; set; }
    }
    public class ProfileData
    {
    }
}
