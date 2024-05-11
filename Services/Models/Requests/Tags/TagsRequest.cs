using DigiticketCMS.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Models.Requests.Tags
{
    public class TagsRequest
    {

    }
    public class ProductTagsGetByPageRequest : BaseRequest
    {

    }
    public class ProductTagsAddRequest
    {
        public int ParentId { get; set; }
        public Guid CategoryId { get; set; }
        public Guid DestinationId { get; set; }
        public string TextView { get; set; }
        public string TextLink { get; set; }
        public string Name { get; set; }
        public int Type { get; set; }
        public int Idx { get; set; }
    }
    public class ProductTagsUpdateIdxRequest
    {
        public int Id { get; set; }
        public int Idx { get; set; }
    }
    public class TagsGetListRequest
    {
        public int ParentId { get; set; }
        public string Key { get; set; }
        public Guid? CategoryId { get; set; }
        public Guid? DestinationId { get; set; }
        public int Type { get; set; }
    }
}
