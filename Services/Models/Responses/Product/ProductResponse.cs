using Services.Models.Responses.Lib;
using Services.Models.Responses.Supplier;
using Services.Models.Responses.Tags;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Models.Responses.Product
{
    public class ProductResponse
    {
        public int LinkOption { get; set; }
        public int ProductDetail { get; set; }
        public string AvatarVideo { get; set; }
        public string CategoryId { get; set; }
        public int Idx { get; set; }
        public string CategoryName { get; set; }
        public string CategoryCode { get; set; }
        public string SupplierId { get; set; }
        public object SupplierName { get; set; }
        public int Type { get; set; }
        public string Name { get; set; }
        public string DigipostName { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public string PlaceId { get; set; }
        public string Place { get; set; }
        public string DestinationId { get; set; }
        public string DestinationName { get; set; }
        public string Avatar { get; set; }
        public List<BaseImg> Images { get; set; }
        public string Schedule { get; set; }
        public string DetailInfor { get; set; }
        public string Including { get; set; }
        public string Excluding { get; set; }
        public string Condition { get; set; }
        public string ExtraInfo { get; set; }
        public string Url { get; set; }
        public metaTag metaTags { get; set; }
        public string IntrOduction { get; set; }
        public bool? IsHot { get; set; }
        public bool? IsHome { get; set; }
        public List<LibTagViewResponse> TagsView { get; set; }
        public List<ProductTagsResponse> TagsHighlight { get; set; }
        public List<TagsGroupResponse> TagsGroup { get; set; }
        public List<ProductTagsResponse> Tags { get; set; }
        public bool OutDate { get; set; }
        public bool RequireDate { get; set; }
        public int MinOrderDate { get; set; }
        public int TicketLimit { get; set; }
        public int ShipType { get; set; }
        public decimal OrderConditions { get; set; }
        public string LanguageCode { get; set; }
        public string CurrencyCode { get; set; }
        public string Id { get; set; }
        public int Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public string CreatedByName { get; set; }
        public DateTime LastEditedDate { get; set; }
        public int LastEditedBy { get; set; }
        public string LastEditedByName { get; set; }
        public ProfileData ProfileData { get; set; }
        public string Note { get; set; }
        public bool IsPublic { get; set; }
        public int IdxHot { get; set; }
        public int IdxHome { get; set; }
    }
    public class metaTag
    {
        public string Titles { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
    }
    public class ProductGetListByTenantDigipostResponse
    {
        public string ProductOriginalId { get; set; }
        public string Name { get; set; }
        public int Version { get; set; }
        public int LastestVersion { get; set; }
        public int SyncStatus { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
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
}
