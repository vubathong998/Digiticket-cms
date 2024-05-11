using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Models.Responses.Content
{
    public class ContentResponse
    {
    }

    #region Component type
    public class TemplateComponentTypeResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int SystemCode { get; set; }
        public object SystemName { get; set; }
    }
    #endregion

    #region Template Component
    public class TemplateComponentResponse
    {
        public string TypeName { get; set; }
        public int SystemCode { get; set; }
        public string SystemName { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ViewName { get; set; }
        public string ViewImage { get; set; }
        public int Type { get; set; }
        public string DefaultHtmlClass { get; set; }
        public string DefaultHtmlId { get; set; }
        public List<string> DefaultLinkStyles { get; set; }
        public List<string> DefaultLinkScripts { get; set; }
        public int Id { get; set; }
        public int Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public string CreatedByName { get; set; }
        public DateTime LastEditedDate { get; set; }
        public int LastEditedBy { get; set; }
        public string LastEditedByName { get; set; }
        public TemplateComponentGetByPageProfileDataResponse ProfileData { get; set; }
    }
    public class TemplateComponentGetByPageProfileDataResponse
    {
    }

    public class TemplateComponentGetByTemplateResponse
    {
        public int MapId { get; set; }
        public int Idx { get; set; }
        public string TypeName { get; set; }
        public int SystemCode { get; set; }
        public object SystemName { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ViewName { get; set; }
        public string ViewImage { get; set; }
        public int Type { get; set; }
        public object DefaultHtmlClass { get; set; }
        public object DefaultHtmlId { get; set; }
        public List<object> DefaultLinkStyles { get; set; }
        public List<object> DefaultLinkScripts { get; set; }
        public int Id { get; set; }
        public int Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public string CreatedByName { get; set; }
        public DateTime LastEditedDate { get; set; }
        public int LastEditedBy { get; set; }
        public string LastEditedByName { get; set; }
        public TemplateComponentGetByTemplateProfileDataResponse ProfileData { get; set; }
    }
    public class TemplateComponentGetByTemplateProfileDataResponse
    {
    }
    #endregion

    #region Template
    public class TemplateResponse
    {
        public int SystemCode { get; set; }
        public object SystemName { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ViewName { get; set; }
        public int Id { get; set; }
        public int Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public string CreatedByName { get; set; }
        public DateTime LastEditedDate { get; set; }
        public int LastEditedBy { get; set; }
        public string LastEditedByName { get; set; }
        public TemplateProfileDateResponse ProfileData { get; set; }
    }
    public class TemplateProfileDateResponse
    {
    }
    #endregion

    #region Page
    public class PageResponse
    {
        public string ViewImage { get; set; }
        public string TemplateName { get; set; }
        public string TemplateDescription { get; set; }
        public string TemplateViewName { get; set; }
        public string URL { get; set; }
        public string MetaTitle { get; set; }
        public string MetaDescription { get; set; }
        public string MetaImage { get; set; }
        public int Type { get; set; }
        public string Avatar { get; set; }
        public List<PageTagResponse> Tags { get; set; }
        public List<PageTagResponse> CategoryTags { get; set; }
        public int SystemCode { get; set; }
        public string SystemName { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int TemplateId { get; set; }
        public string HtmlClass { get; set; }
        public string HtmlId { get; set; }
        public List<string> LinkStyles { get; set; }
        public List<string> LinkScripts { get; set; }
        public string CustomData { get; set; }
        public bool IsAuthenticated { get; set; }
        public Guid Id { get; set; }
        public int Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public string CreatedByName { get; set; }
        public DateTime LastEditedDate { get; set; }
        public int LastEditedBy { get; set; }
        public string LastEditedByName { get; set; }
        public PageProfileDataResponse ProfileData { get; set; }
    }
    public class PageProfileDataResponse { }

    public class PageTagResponse
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
    #endregion

    #region Page component
    public class PageComponentResponse
    {
        public Guid? DestinationId { get; set; }
        public Guid? CategoryId { get; set; }
        public string TemplateComponentName { get; set; }
        public string TemplateComponentDescription { get; set; }
        public string ViewName { get; set; }
        public string ViewImage { get; set; }
        public int Type { get; set; }
        public string TypeName { get; set; }
        public int SystemCode { get; set; }
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
        public string CustomData { get; set; }
        public Guid Id { get; set; }
        public int Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public string CreatedByName { get; set; }
        public DateTime LastEditedDate { get; set; }
        public int LastEditedBy { get; set; }
        public string LastEditedByName { get; set; }
        public PageComponentProfileResponse ProfileData { get; set; }
    }
    public class PageComponentProfileResponse { }
    #endregion

    #region Page component items
    public class PageComponentItemsResponse : BaseImg
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string PageComponentId { get; set; }
        public string PageComponentName { get; set; }
        public string ObjectId { get; set; }
        public string ObjectName { get; set; }
        public string ObjectDescription { get; set; }
        public string ObjectAvatar { get; set; }
        public string ObjectURL { get; set; }
        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public string Image { get; set; }
        public int TotalRows { get; set; }
        public int Id { get; set; }
        public int Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public string CreatedByName { get; set; }
        public DateTime LastEditedDate { get; set; }
        public int LastEditedBy { get; set; }
        public string LastEditedByName { get; set; }
        public PageComponentItemsProfileDataResponse ProfileData { get; set; }
    }
    public class PageComponentItemsProfileDataResponse { }
    #endregion
}
