using DigiticketCMS.Services.Models;

namespace Services.Models.Requests.CategoryTag
{
    //public class CategoryTagRe
    //{
    //}
    public class CategoryTagGetByPageRequest : BaseRequest
    {

    }

    public class CategoryTagAddRequest
    {
        public int TagParentId { get; set; }
        public int TagId { get; set; }
        public int Idx { get; set; }
        public bool IsView { get; set; }
        public bool IsHome { get; set; }
        public bool IsHot { get; set; }
    }
    public class CategoryTagUpdateHomeRequest
    {
        public int Id { get; set; }
        public bool IsHome { get; set; }
        public int IdxHome { get; set; }
    }
    public class CategoryTagUpdateHotRequest
    {
        public int Id { get; set; }
        public bool IsHot { get; set; }
        public int IdxHot { get; set; }
    }

    public class CategoryTagEditRequest
    {
        public int Id { get; set; }
        public int Idx { get; set; }
        public bool IsView { get; set; }
    }
}
