using Services.Models.Responses.Supplier;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Models.Responses.Tags
{
    public class TagsResponse
    {

    }

    public class ProductTagsForViewResponse : ProductTagsResponse
    {
        public int TypeResponse { get; set; }   
    }
    public class ProductTagsResponse
    {
        public int ParentId { get; set; }
        public string ParentName { get; set; }
        public Guid? CategoryId { get; set; }
        public string CategoryName { get; set; }
        public Guid? DestinationId { get; set; }
        public string DestinationName { get; set; }
        public string TextView { get; set; }
        public string TextLink { get; set; }
        public string Name { get; set; }
        public int Type { get; set; }
        public int Idx { get; set; }
        public string LanguageCode { get; set; }
        public string CurrencyCode { get; set; }
        public int Id { get; set; }
        public int Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public string CreatedByName { get; set; }
        public DateTime LastEditedDate { get; set; }
        public int LastEditedBy { get; set; }
        public string LastEditedByName { get; set; }
        public ProfileData ProfileData { get; set; }
    }
    public class TagsGroupResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<TagsGroupResponse> SubOptions { get; set; }
    }
    public class GetListTagsAndSubTagsCache
    {
        public List<ProductTagsResponse> ParentTags { get; set; }
        public List<ProductTagsResponse> SubTags { get; set; }
        public GetListTagsAndSubTagsCache()
        {
            ParentTags = new List<ProductTagsResponse>();
            SubTags = new List<ProductTagsResponse>();
        }
    }
    public class GetListTagsCache
    {
        public List<ProductTagsResponse> Tags { get; set; }
        public GetListTagsCache()
        {
            Tags = new List<ProductTagsResponse>();
        }
    }
}
