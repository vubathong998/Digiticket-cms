using Services.Implement;
using Services.Models;
using Services.Models.Responses.GroupService;
using Services.Models.Responses.Lib;
using Services.Models.Responses.Product;
using Services.Models.Responses.ServicePrice;
using Services.Models.Responses.Tags;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DigiticketCMS.Models.Product
{
    public class Product
    {

    }
    public class ProductDetailViewModel
    {
        public Guid ProductId { get; set; }
        public List<TagsGroupResponse> FullTagsGroup { get; set; }
        public ProductResponse ProductResponse { get; set; }
        public ProductDetailViewModel()
        {
            FullTagsGroup = new List<TagsGroupResponse>();
        }
    }
    public class ProductDetailTagsPartialViewModel
    {
        public List<LibTagViewResponse> LibTagViewResponse { get; set; }
        public List<ProductTagsResponse> TagsResponse { get; set; }    
        public ProductResponse ProductResponse { get; set; }
    }
    public class ProductGetByPageViewModel : BaseViewModel
    {
        public List<Filter> FilterViewModel { get; set; }
        public string CategoryId { get; set; }
        public string CategoryIdRange { get; set; }
        public string DestinationId { get; set; }
        public string StringIsPublic { get; set; }
        public bool? IsHot { get; set; }
        public bool? IsHome { get; set; }
        public int? Status { get; set; }
        public Guid? SupplierId { get; set; }
        public string CategoryIdStringArray { get; set; } 
        public string DestinationStringArray { get; set; } 
        public ProductGetByPageViewModel()
        {
            LanguageCode = String.Empty;
            CurrencyCode = String.Empty;
            CategoryId = String.Empty;
            DestinationId = String.Empty;
        }
    }
    public class Filter
    {
        public string Opt { get; set; }
        public string Name { get; set; }
        public string Opt1 { get; set; }
        public int Type { get; set; }
        public string Values { get; set; }
    }
    public class ProductImportProductFromDigipostViewModel
    {
        [Required]
        public Guid SupplierId { get; set; }
        [Required]
        public Guid ProductId { get; set; }
    }
    public class ProductUpdateMainInfoViewModel
    {
        [Required]
        public Guid Id { get; set; }
        //[Required]
        //public string Name { get; set; }
        [Required]
        public Guid CategoryId { get; set; }
        [Required]
        public Guid DestinationId { get; set; }
    }
    public class ProductUpdateIdxViewModel
    {
        [Required]
        public Guid Id { get; set; }
        [Required]
        public int Idx { get; set; }
    }
    public class ProductUpdateBaseInfoViewModel
    {
        [Required]
        public Guid Id { get; set; }
        //[Required]
        //public string Name { get; set; }
        public string PlaceId { get; set; }
        public string Place { get; set; }
        [AllowHtml]
        public string Description { get; set; }
        [AllowHtml]
        public string Schedule { get; set; }
        [AllowHtml]
        public string DetailInfor { get; set; }
        [AllowHtml]
        public string Including { get; set; }
        [AllowHtml]
        public string Excluding { get; set; }
        [AllowHtml]
        public string Condition { get; set; }
        [AllowHtml]
        public string ExtraInfo { get; set; }
        [AllowHtml]
        public string Note { get; set; }
    }
    public class ProductUpdateImageViewModel
    {
        public Guid Id { get; set; }
        public string Avatar { get; set; }
        public string AvatarVideo { get; set; }
        public List<BaseImgViewModel> Images { get; set; }
        public List<BaseImgViewModel> Videos { get; set; }
    }
    public class ProductUpdateSeoInfoViewModel
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

    #region update tag
    public class ProductUpdateTagsViewModel
    {
        public Guid Id { get; set; }
        public List<TagsViewViewModel> TagsView { get; set; }
        public List<TagViewModel> TagsHighlight { get; set; }
        public List<TagViewModel> Tags { get; set; }
    }
    public class TagViewModel
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

    public class TagsViewViewModel
    {
        public int Id { get; set; }
        public string Icon { get; set; }
        public string Detail { get; set; }
        public int Type { get; set; }
    }
    #endregion

    #region update config oder and status
    public class ProductUpdateConfigOderViewModel
    {
        [Required]
        public Guid Id { get; set; }
        public bool RequireDate { get; set; }
        public int MinOrderDate { get; set; }
        public int TicketLimit { get; set; }
        public int ShipType { get; set; }
        public int OrderConditions { get; set; }
    }
    public class ProductUpdateStatusViewModel
    {
        [Required]
        public Guid Id { get; set; }
        public int Status { get; set; }
    }
    #endregion

    #region update hot view model
    public class ProductUpdateHotViewModel
    {
        public Guid Id { get; set; }
        public bool IsHot { get; set; }
        public int IdxHot { get; set; }
    }
    public class ProductAddOrUpdateHotModalViewModel
    {
        public string Name { get; set; }
        public Guid? Id { get; set; }
        public bool IsHot { get; set; }
        public int? IdxHot { get; set; }
        public bool IsAdd { get; set; }
    }
    #endregion

    #region update home view model

    public class ProductUpdateHomeViewModel
    {
        [Required]
        public Guid Id { get; set; }
        public bool IsHome { get; set; }
        public int IdxHome { get; set; }
    }
    public class ProductAddOrUpdateHomeModalViewModel
    {
        public string Name { get; set; }
        public Guid? Id { get; set; }
        public bool IsHome { get; set; }
        public int? IdxHome { get; set; }
        public bool IsAdd { get; set; }
    }
    #endregion

    public class ProductUpdateTagsGroupViewModel
    {
        public string Id { get; set; }
        public List<TagsGroupViewModel> TagsGroup { get; set; }
    }

    public class TagsGroupViewModel
    {
        [Required]
        public long Id { get; set; }
        public string Name { get; set; }
        public List<TagsGroupViewModel> SubOptions { get; set; }
    }

    public class ProductGetListByTenantFromDigipostViewModel
    {
        public Guid Id { get; set; }
        public string Key { get; set; }
    }
}