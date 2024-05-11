using Services.Models.Requests.GroupdService;
using Services.Models.Responses.GroupService;
using Services.Models.Responses.Product;
using Services.Models.Responses.Tags;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DigiticketCMS.Models.GroupService
{
    public class GroupServiceViewModel : BaseViewModel
    {
        public Guid ProductId { get; set; }
    }
    public class GroupServiceUpdateInfoViewModel
    {
        [Required]
        public int Id { get; set; }
        //[Required]
        //public string Name { get; set; }
        [AllowHtml]
        public string Description { get; set; }
        [AllowHtml]
        public string Condition { get; set; }
        [AllowHtml]
        public string Including { get; set; }
        [AllowHtml]
        public string Excluding { get; set; }
        [AllowHtml]
        public string Schedule { get; set; }
        [AllowHtml]
        public string Infor { get; set; }
        [AllowHtml]
        public string Note { get; set; }
    }
    public class GroupServiceUpdateImageViewModel
    {
        [Required]
        public int Id { get; set; }
        public List<BaseImgViewModel> Images { get; set; }
        public GroupServiceUpdateImageViewModel()
        {
            Images = new List<BaseImgViewModel> { };
        }
    }
    public class GroupServiceUpdateNumberTicketViewModel
    {
        [Required]
        public int Id { get; set; }
        public int MinTickets { get; set; }
        public int MaxTickets { get; set; }
    }
    public class GroupServiceUpdateGroupTimeViewModel
    {
        [Required]
        public int Id { get; set; }
        public grouptTimeGrouptSerivceViewModel GroupTime { get; set; }
    }
    public class grouptTimeGrouptSerivceViewModel
    {
        public int TimeType { get; set; }
        public DateTime BeginDate { get; set; }
        public DateTime EndDate { get; set; }
        public int DateUnit { get; set; }
        public int PeriodTime { get; set; }
        public DateTime PeriodMark { get; set; }
    }

    public class GroupServiceUpdateGroupNumberVIewModel
    {
        [Required]
        public int Id { get; set; }
        public grouptGroupServiceViewModel GroupNumbers { get; set; }
    }
    public class grouptGroupServiceViewModel
    {
        public int Type { get; set; }
        public string Data { get; set; }
    }

    public class GroupServiceUpdateTagsViewModel
    {
        [Required]
        public int Id { get; set; }
        public List<TagsGrouptServiceViewModel> Tags { get; set; }
    }
    public class GroupServiceUpdateTagsProcessViewModel
    {
        [Required]
        public int Id { get; set; }
        public List<TagsGrouptServiceViewModel> TagsProcess { get; set; }
    }


    public class GroupServiceUpdateTagsViewViewModel
    {
        public int Id { get; set; }
        public List<TagsGrouptServiceViewModel> TagsView { get; set; }
    }

    public class TagsGrouptServiceViewModel
    {
        [Required]
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

    public class GroupServiceResponseViewModel
    {
        public GroupServiceResponse GroupServiceResponse { get; set; }
        public List<ProductTagsResponse> AllParentTags { get; set; }
        public List<ProductTagsResponse> AllSubTags { get; set; }
        public GroupServiceResponseViewModel()
        {
            AllSubTags = new List<ProductTagsResponse> { };
        }
    }


}
