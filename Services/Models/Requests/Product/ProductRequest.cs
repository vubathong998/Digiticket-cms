using DigiticketCMS.Services.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Services.Models.Requests.Product
{
    public class ProductRequest
    {

    }
    public class ProductGetByPageRequest : BaseRequest
    {
        public string Sort { get; set; }
    }

    public class ProductImportProductFromDigipostRequest
    {
        public Guid SupplierId { get; set; }
        public Guid ProductId { get; set; }
    }
    public class ProductUpdateMainInfoRequest
    {
        public Guid Id { get; set; }
        //public string Name { get; set; }
        public Guid CategoryId { get; set; }
        public Guid DestinationId { get; set; }
    }
    public class ProductUpdateBaseInfoRequest
    {
        public Guid Id { get; set; }
        //public string Name { get; set; }
        [AllowHtml]
        public string Description { get; set; }
        public Guid PlaceId { get; set; }
        public string Place { get; set; }
        [AllowHtml]
        public string Schedule { get; set; }
        [AllowHtml]
        public string DetailInfor { get; set; }
        [AllowHtml]
        public string Including { get; set; }
        [AllowHtml]
        public string Excluding { get; set; }
        public string Condition { get; set; }
        public string ExtraInfo { get; set; }
        public string Note { get; set; }
    }
    public class ProductUpdateImageRequest
    {
        public Guid Id { get; set; }
        public string Avatar { get; set; }
        public string AvatarVideo { get; set; }
        public List<BaseImg> Images { get; set; }
    }
    public class ProductUpdateIdxRequest
    {
        public Guid Id { get; set; }
        public int Idx { get; set; }
    }
    public class ProductUpdateSeoInfoRequest
    {

        [Required]
        public Guid Id { get; set; }
        [Required]
        public string MetaTitles { get; set; }
        public string MetaDescription { get; set; }
        public string MetaImage { get; set; }
        [AllowHtml]
        public string IntrOduction { get; set; }
    }

    public class ProductGetListByTenantFromDigipostRequest
    {
        public Guid Id { get; set; }
        public string Key { get; set; }
    }

    #region update tag
    public class ProductUpdateTagsRequest
    {
        public Guid Id { get; set; }
        public List<TagsViewRequest> TagsView { get; set; }
        public List<TagRequest> TagsHighlight { get; set; }
        public List<TagRequest> Tags { get; set; }
    }
    public class TagRequest
    {
        public int Id { get; set; }
        public int ParentId { get; set; }
        public Guid CategoryId { get; set; }
        public Guid DestinationId { get; set; }
        public string TextView { get; set; }
        public string TextLink { get; set; }
        public string Name { get; set; }
        public int Type { get; set; }
        public int Idx { get; set; }
    }

    public class TagsViewRequest
    {
        public int Id { get; set; }
        public string Icon { get; set; }
        public string Detail { get; set; }
        public int Type { get; set; }
    }
    #endregion

    public class ProductUpdateConfigOderRequest
    {
        public Guid Id { get; set; }
        public bool RequireDate { get; set; }
        public int MinOrderDate { get; set; }
        public int TicketLimit { get; set; }
        public int ShipType { get; set; }
        public int OrderConditions { get; set; }
    }

    #region update hot 
    public class ProductUpdateStatusRequest
    {
        public Guid Id { get; set; }
        public int Status { get; set; }
    }

    public class ProductUpdateHotRequest
    {
        public Guid Id { get; set; }
        public bool IsHot { get; set; }
        public int IdxHot { get; set; }
    }
    #endregion

    #region update home
    public class ProductUpdateHomeRequest
    {
        public Guid Id { get; set; }
        public bool IsHome { get; set; }
        public int IdxHome { get; set; }
    }
    public class ProductAddOrUpdateHomeModalRequest
    {
        public string Name { get; set; }
        public Guid? Id { get; set; }
        public bool IsHome { get; set; }
        public int? IdxHome { get; set; }
        public bool IsAdd { get; set; }
    }
    #endregion

    #region update tagGroup
    public class ProductUpdateTagsGroupRequest
    {
        public string Id { get; set; }
        public List<TagsGroupViewRequest> TagsGroup { get; set; }
    }
    public class TagsGroupViewRequest
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public List<TagsGroupViewRequest> SubOptions { get; set; }
    }
    #endregion
}
