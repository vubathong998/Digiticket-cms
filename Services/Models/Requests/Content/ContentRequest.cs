using DigiticketCMS.Services.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Services.Models.Requests.Content
{
    public class ContentRequest
    {
    }

    #region Template component type
    public class TemplateComponentTypeEditRequest
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
    #endregion

    #region Template component
    public class TemplateComponentAddRequest
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string ViewName { get; set; }
        public string ViewImage { get; set; }
        public int Type { get; set; }
        public string DefaultHtmlClass { get; set; }
        public string DefaultHtmlId { get; set; }
        public List<string> DefaultLinkStyles { get; set; }
        public List<string> DefaultLinkScripts { get; set; }
    }

    public class TemplateComponentGetByPageRequest : BaseRequest
    {
        //public string Type { get; set; }
    }

    public class TemplateComponentEditRequest
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ViewImage { get; set; }
        public string DefaultHtmlClass { get; set; }
        public string DefaultHtmlId { get; set; }
        public List<string> DefaultLinkStyles { get; set; }
        public List<string> DefaultLinkScripts { get; set; }
    }

    #endregion

    #region Template
    public class TemplateAddRequest
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string ViewName { get; set; }
    }

    public class TemplateGetByPageRequest : BaseRequest
    {
    }

    public class TemplateEditRequest
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }

    public class TemplateMapRequest
    {
        public int TemplateId { get; set; }
        public int TemplateComponentId { get; set; }
        public int Idx { get; set; }
    }

    public class TemplateMapEditRequest
    {
        public int Id { get; set; }
        public int Idx { get; set; }
    }
    #endregion

    #region Page
    public class PageAddRequest
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
        public List<PageTagRequest> CategoryTags { get; set; }
        public List<PageTagRequest> Tags { get; set; }
        public string MetaTitle { get; set; }
        public string MetaDescription { get; set; }
        public string MetaImage { get; set; }
    }

    public class PageTagRequest
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

    public class PageGetByPageRequest : BaseRequest
    {
        public int Status { get; set; }
        public int? TemplateId { get; set; }

    }

    public class PageEditRequest
    {
        public string Id { get; set; }
        public string Avatar { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string HtmlClass { get; set; }
        public string HtmlId { get; set; }
        public int Type { get; set; }
        public List<PageTagRequest> CategoryTags { get; set; }
        public List<PageTagRequest> Tags { get; set; }
        public List<string> LinkStyles { get; set; }
        public List<string> LinkScripts { get; set; }
        public string CustomData { get; set; }
        public bool IsAuthenticated { get; set; }
        public string MetaTitle { get; set; }
        public string MetaDescription { get; set; }
        public string MetaImage { get; set; }
    }

    public class PageUpdateStatusRequest
    {
        public string Id { get; set; }
        public int Status { get; set; }
    }

    public class PageUpdateURLTargetRequest
    {
        public Guid Id { get; set; }
        public string URLTarget { get; set; }
        public int TypeTarget { get; set; }
    }
    #endregion

    #region Page component
    public class PageComponentAddRequest
    {
        public string PageId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int TemplateComponentId { get; set; }
        public int Idx { get; set; }
        public string HtmlContent { get; set; }
        public string HtmlClass { get; set; }
        public string HtmlId { get; set; }
        public List<string> LinkStyles { get; set; }
        public List<string> LinkScripts { get; set; }
        public Guid CategoryId { get; set; }
        public Guid DestinationId { get; set; }
    }

    public class PageComponentEditRequest
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Idx { get; set; }
        public string HtmlContent { get; set; }
        public string HtmlClass { get; set; }
        public string HtmlId { get; set; }
        public string MetaTitle { get; set; }
        public string MetaDescription { get; set; }
        public string MetaImage { get; set; }
        public List<string> LinkStyles { get; set; }
        public List<string> LinkScripts { get; set; }

    }

    public class PageComponentStatusEditRequest
    {
        public Guid Id { get; set; }
        public int Status { get; set; }
    }

    public class PageComponentUpdateIdxRequest
    {
        public List<PageComponentUpdateIdxArrayRequest> Components { get; set; }
    }

    public class PageComponentUpdateIdxArrayRequest
    {
        public Guid Id { get; set; }
        public int Idx { get; set; }
    }
    #endregion

    #region Page component items
    public class PageComponentItemsGetByPageRequest : BaseRequest
    {

    }

    public class PageComponentItemsAddRequest : BaseImg
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string PageComponentId { get; set; }
        public string ProductId { get; set; }
        public string ObjectId { get; set; }
        public string Image { get; set; }
    }

    public class PageComponentItemsEditRequest : BaseImg
    {
        public int Id { get; set; }
        public string ProductId { get; set; }
        public string Image { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }
        public string ObjectId { get; set; }
    }

    public class PageComponentItemsUpdateIdxRequest
    {
        public List<PageComponentItemUpdateIdxArrayRequest> ComponentItems { get; set; }
    }

    public class PageComponentItemUpdateIdxArrayRequest
    {
        public int Id { get; set; }
        public int Idx { get; set; }
    }
    #endregion
}
