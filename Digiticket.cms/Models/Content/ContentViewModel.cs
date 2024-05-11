using Services.Models.Responses.Content;
using Services.Models.Responses.Lib;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace DigiticketCMS.Models.Content
{
    public class ContentViewModel
    {
    }
    #region Template component type
    public class TemplateComponentTypeEditViewModel
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
    }
    #endregion

    #region Template component
    public class TemplateComponentAddViewModel
    {
        public string ViewImage { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ViewName { get; set; }
        public int Type { get; set; }
        public string DefaultHtmlClass { get; set; }
        public string DefaultHtmlId { get; set; }
        public List<string> DefaultLinkStyles { get; set; }
        public List<string> DefaultLinkScripts { get; set; }
    }
    public class TemplateComponentGetByPageViewModel : BaseViewModel
    {
        public int? Type { get; set; }
    }

    public class TemplateComponentEditViewModel
    {
        [Required]
        public int Id { get; set; }
        public string Name { get; set; }
        public string ViewImage { get; set; }
        public string Description { get; set; }
        public string DefaultHtmlClass { get; set; }
        public string DefaultHtmlId { get; set; }
        public List<string> DefaultLinkStyles { get; set; }
        public List<string> DefaultLinkScripts { get; set; }
    }
    #endregion

    #region Template
    public class TemplateAddViewModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string ViewName { get; set; }
    }

    public class TemplateGetByPageViewModel : BaseViewModel
    {
    }
    public class TemplateEditViewModel
    {
        [Required]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }

    public class TemplateMapViewModel
    {
        [Required]
        public int TemplateId { get; set; }
        [Required]
        public int TemplateComponentId { get; set; }
        public int Idx { get; set; }
    }

    public class TemplateMapEditViewModel
    {
        [Required]
        public int Id { get; set; }
        public int Idx { get; set; }
    }

    public class TemplateTemplateMapModelToViewViewModal
    {
        public int Id { get; set; }
        public string ViewImage { get; set; }
    }
    #endregion

    #region Page
    public class PageAddViewModel
    {
        public string Name { get; set; }
        public string Avatar { get; set; }
        public string Description { get; set; }
        public int TemplateId { get; set; }
        public string HtmlClass { get; set; }
        public string HtmlId { get; set; }
        public List<string> LinkStyles { get; set; }
        public List<string> LinkScripts { get; set; }
        public string CustomData { get; set; }
        public bool IsAuthenticated { get; set; }
        public int TypeURL { get; set; }
        public int Type { get; set; }
        public List<PageTagViewModel> CategoryTags { get; set; }
        public List<PageTagViewModel> Tags { get; set; }
        public string MetaTitle { get; set; }
        public string MetaDescription { get; set; }
        public string MetaImage { get; set; }
    }

    public class PageTagViewModel
    {
        public int Id { get; set; }
        public int ParentId { get; set; }
        public string CategoryId { get; set; }
        public string DestinationId { get; set; }
        public string TextView { get; set; }
        public string TextLink { get; set; }
        public string Name { get; set; }
        public int Type { get; set; }
        public int Idx { get; set; }
    }

    public class PageGetByPageViewModel : BaseViewModel
    {
        public int? TemplateId {  get; set; }
        public int Status { get; set; }
    }

    public class PageEditViewModel
    {
        [Required]
        public string Id { get; set; }
        public string Avatar { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string HtmlClass { get; set; }
        public string HtmlId { get; set; }
        public int Type { get; set; }
        public List<PageTagViewModel> CategoryTags { get; set; }
        public List<PageTagViewModel> Tags { get; set; }
        public List<string> LinkStyles { get; set; }
        public List<string> LinkScripts { get; set; }
        public string CustomData { get; set; }
        public bool IsAuthenticated { get; set; }
        public string MetaTitle { get; set; }
        public string MetaDescription { get; set; }
        public string MetaImage { get; set; }
    }

    public class PageUpdateStatusViewModel
    {
        [Required]
        public string Id { get; set; }
        public int Status { get; set; }
    }

    public class PageUpdateURLTargetViewModel
    {
        [Required]
        public Guid Id { get; set; }
        public string URLTarget { get; set; }
        public int TypeTarget { get; set; }
    }
    #endregion

    #region Page component
    public class PageComponentDetailToViewViewModel
    {
        public PageComponentResponse PageComponentResponse { get; set; }
        public string CategoriesResponse { get; set; }
    }
    public class PageComponentAddViewModel
    {
        public string PageId { get; set; }
        public string Name { get; set; }
        [AllowHtml]
        public string Description { get; set; }
        public int TemplateComponentId { get; set; }
        public int Idx { get; set; }
        [AllowHtml]
        public string HtmlContent { get; set; }
        public string HtmlClass { get; set; }
        public string HtmlId { get; set; }
        public List<string> LinkStyles { get; set; }
        public List<string> LinkScripts { get; set; }
        public Guid CategoryId { get; set; }
        public Guid DestinationId { get; set; }
    }

    public class PageComponentEditViewModel
    {
        [Required]
        public string Id { get; set; }
        public string Name { get; set; }
        [AllowHtml]
        public string Description { get; set; }
        public int Idx { get; set; }
        [AllowHtml]
        public string HtmlContent { get; set; }
        public string HtmlClass { get; set; }
        public string HtmlId { get; set; }
        public string MetaTitle { get; set; }
        public string MetaDescription { get; set; }
        public string MetaImage { get; set; }
        public List<string> LinkStyles { get; set; }
        public List<string> LinkScripts { get; set; }
    }

    public class ToViewPageComponentAddOrEditViewModel
    {

        public Guid? Id { get; set; }
        public Guid? PageId { get; set; }
        public string ViewImage { get; set; }
        public List<SubCategoryResponse> Category { get; set; }
        public List<LibDestinationResponse> Destination { get; set; }
        public List<TemplateComponentGetByTemplateResponse> TemplateComponent { get; set; }
        public PageComponentResponse PageComponentResponse { get; set; }
        public bool IsError { get; set; }
        public string MessageErr { get; set; }
    }

    public class PageComponentStatusEditViewModel
    {
        public Guid Id { get; set; }
        public int Status { get; set; }
    }

    public class PageComponentUpdateIdxViewModel
    {
        public List<PageComponentUpdateIdxArrayViewModel> Components { get; set; }
    }

    public class PageComponentUpdateIdxArrayViewModel
    {
        public Guid Id { get; set; }
        public int Idx { get; set; }
    }
    #endregion

    #region Page component items
    public class PageComponentItemsGetByPageViewModel : BaseViewModel
    {
        public Guid PageComponentId { get; set; }
        public int? Type { get; set; }
    }

    public class PageComponentItemsAddViewModel : BaseImgViewModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string PageComponentId { get; set; }
        public string ProductId { get; set; }
        public string ObjectId { get; set; }
        public string Image { get; set; }
    }

    public class PageComponentItemsEditViewModel : BaseImgViewModel
    {
        public int Id { get; set; }
        public string ProductId { get; set; }
        public string Image { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ObjectId { get; set; }
    }

    public class ToViewPageComponentItemsAddOrEditViewModel
    {
        public Guid? PageComponentId { get; set; }
        public int Type { get; set; }
        public PageComponentItemsResponse PageComponentItemsResponse { get; set; }
    }
    public class PageComponentItemsUpdateIdxViewModel
    {
        public List<PageComponentItemUpdateIdxArrayViewModel> ComponentItems { get; set; }
    }
    public class PageComponentItemUpdateIdxArrayViewModel
    {
        public int Id { get; set; }
        public int Idx { get; set; }
    }
    #endregion



    public class PageTagResponseButLikeCategoryModel : PageTagResponse
    {
        public int TagId { get; set; }
    }
}