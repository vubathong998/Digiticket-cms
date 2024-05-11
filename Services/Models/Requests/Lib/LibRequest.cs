using DigiticketCMS.Services.Models;
using Services.Models.Responses.Lib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Models.Requests.Lib
{
    public class LibDestinationRequest
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public List<FilterRequest> Filter { get; set; }
        public string Keyword { get; set; }
        public string FieldName { get; set; }
        public string Orderby { get; set; }
        public bool? IsHome { get; set; }
        public LibDestinationRequest()
        {
            PageIndex = 1;
            PageSize = 10;
            FieldName = "name";
            Orderby = "desc";
            Filter = new List<FilterRequest>();
            Keyword = "";
        }
    }
    public class LibCategoryUpdateRequest
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Position { get; set; }
        public string Url { get; set; }
        public bool IsAvailable { get; set; }
        public bool IsView { get; set; }
        public string IconPath { get; set; }
    }

    public class LibDestinationUpdateRequest
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public List<BaseImg> Images { get; set; }
        public string PlaceId { get; set; }
        public string Place { get; set; }
        public int CountProduct { get; set; }
        public bool IsHome { get; set; }
        public int Status { get; set; }
    }
}
