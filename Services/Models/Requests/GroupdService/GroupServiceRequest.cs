using DigiticketCMS.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Services.Models.Requests.GroupdService
{
    public class GroupServiceRequest : BaseRequest
    {
    }
    public class GroupServiceUpdateInfoRequest
    {
        public int Id { get; set; }
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
    public class GroupServiceUpdateImageRequest
    {
        public int Id { get; set; }
        public List<BaseImg> Images { get; set; }
    }
    public class GroupServiceUpdateNumberTicketRequest
    {
        public int Id { get; set; }
        public int MinTickets { get; set; }
        public int MaxTickets { get; set; }
    }
    public class GroupServiceUpdateGroupTimeRequest
    {
        public int Id { get; set; }
        public grouptTimeGrouptSerivceRequest GroupTime { get; set; }
    }
    public class grouptTimeGrouptSerivceRequest
    {
        public int TimeType { get; set; }
        public DateTime BeginDate { get; set; }
        public DateTime EndDate { get; set; }
        public int DateUnit { get; set; }
        public int PeriodTime { get; set; }
        public DateTime PeriodMark { get; set; }
    }

    public class GroupServiceUpdateGroupNumberRequest
    {
        public int Id { get; set; }
        public grouptGroupServiceRequest GroupNumbers { get; set; }
    }
    public class grouptGroupServiceRequest
    {
        public int Type { get; set; }
        public string Data { get; set; }
    }

    public class GroupServiceUpdateTagsRequest
    {
        public int Id { get; set; }
        public List<TagsGrouptServiceRequest> Tags { get; set; }
    }
    public class GroupServiceUpdateTagsProcessRequest
    {
        public int Id { get; set; }
        public List<TagsGrouptServiceRequest> TagsProcess { get; set; }
    }

    public class GroupServiceUpdateTagsViewRequest
    {
        public int Id { get; set; }
        public List<TagsGrouptServiceRequest> TagsView { get; set; }
    }
    public class TagsGrouptServiceRequest
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
}
