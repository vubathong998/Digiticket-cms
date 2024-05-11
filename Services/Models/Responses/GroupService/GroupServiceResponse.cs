using Services.Models.Responses.Tags;
using System;
using System.Collections.Generic;

namespace Services.Models.Responses.GroupService
{
    public class GroupServiceResponse
    {
        public string GroupServiceId { get; set; }
        public int LinkOption { get; set; }
        public string ProductId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string DigipostName { get; set; }
        public string Description { get; set; }
        public List<Detail> Detail { get; set; }
        public List<BaseImg> Images { get; set; }
        public string Condition { get; set; }
        public int MinTickets { get; set; }
        public int MaxTickets { get; set; }
        public string Including { get; set; }
        public string Excluding { get; set; }
        public string Schedule { get; set; }
        public string Infor { get; set; }
        public string Note { get; set; }
        public object Amenities { get; set; }
        public groupTimeServiceResponse GroupTime { get; set; }
        public object GroupNumbers { get; set; }
        public List<ProductTagsResponse> Tags { get; set; }
        public List<ProductTagsResponse> TagsProcess { get; set; }
        public List<ProductTagsResponse> TagsView { get; set; }
        public int Type { get; set; }
        public int TotalRows { get; set; }
        public string LanguageCode { get; set; }
        public string CurrencyCode { get; set; }
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
    public class statusGroupServiceResponse
    {
        public int ParentId { get; set; }
        public object ParentName { get; set; }
        public object CategoryId { get; set; }
        public object CategoryName { get; set; }
        public object DestinationId { get; set; }
        public object DestinationName { get; set; }
        public object TextView { get; set; }
        public object TextLink { get; set; }
        public object Name { get; set; }
        public int Type { get; set; }
        public int Idx { get; set; }
        public string LanguageCode { get; set; }
        public string CurrencyCode { get; set; }
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
    public class groupNumbersGroupServiceResponse
    {
        public int Type { get; set; }
        public string Data { get; set; }
    }
    public class groupTimeServiceResponse
    {
        public int TimeType { get; set; }
        public DateTime BeginDate { get; set; }
        public DateTime EndDate { get; set; }
        public int DateUnit { get; set; }
        public int PeriodTime { get; set; }
        public DateTime PeriodMark { get; set; }
    }
    public class Detail
    {
        public string ServiceId { get; set; }
        public string ServiceName { get; set; }
        public int LimitUse { get; set; }
        public bool IsLimit { get; set; }
        public int Quantity { get; set; }
        public int UnitId { get; set; }
        public string Unit { get; set; }
    }
    public class ProfileData
    {
    }

}
