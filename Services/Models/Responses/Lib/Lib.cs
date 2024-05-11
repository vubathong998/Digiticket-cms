using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Models.Responses.Lib
{
    public class Lib
    {

    }
    #region category
    public class LibCategoryResponse
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public int Position { get; set; }
        public string Url { get; set; }
        public string IconPath { get; set; }
        public int Level { get; set; }
        public string ParentId { get; set; }
        public bool IsAvailable { get; set; }
        public bool IsView { get; set; }
        public object SubCategory { get; set; }
    }
    public class SubCategoryResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
        public string TagsDefault { get; set; }
        public string IconPath { get; set; }
        public int Level { get; set; }
        public Guid ParentId { get; set; }
        public bool IsAvailable { get; set; }
        public bool IsView { get; set; }
        public List<SubCategoryResponse> SubCategory { get; set; }
    }
    #endregion

    #region destination
    public class LibDestinationResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public List<BaseImg> Images { get; set; }
        public string PlaceId { get; set; }
        public string Place { get; set; }
        public int CountProduct { get; set; }
        public bool IsHome { get; set; }
        public int Status { get; set; }
        public int TotalRows { get; set; }
    }
    #endregion

    public class LibTagViewResponse
    {
        public int Id { get; set; }
        public string Icon { get; set; }
        public string Detail { get; set; }
        public int Type { get; set; }
    }
}
