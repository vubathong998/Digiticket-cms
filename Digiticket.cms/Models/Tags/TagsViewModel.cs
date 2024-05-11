using Services.Models;
using Services.Models.Responses.Lib;
using Services.Models.Responses.Product;
using Services.Models.Responses.Tags;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DigiticketCMS.Models.Tags
{
    public class TagsViewModel : BaseViewModel
    {
    }
    public class ProductTagsGetByPageViewModel : BaseViewModel
    {
        public int ParentId { get; set; }
        public string ParentName { get; set; }
        public string CategoryId { get; set; }
        public string DestinationId { get; set; }
        public int? Type { get; set; }   
        public ProductTagsGetByPageViewModel()
        {
            ParentId = 0;
            ParentName = "";
        }
    }
    public class ProductTagsAddModalViewModel
    {
        public string Keyfilter { get; set; }
        public int ParentId { get; set; }
        public string ParentName { get; set; }
        public string FilterName { get; set; }
        public string CategoryId { get; set; }
        public string DestinationId { get; set; }
        public string CategoryName { get; set; }
        public string DestinationName { get; set; }
        public int Type { get; set; }
        public List<SubCategoryResponse> Categories { get; set; }
        public List<LibDestinationResponse> Destinations { get; set; }
        public BaseResponse<PagingResult<ProductTagsResponse>> GetByPage { get; set; }
    }
    public class TagsAddModalViewModel
    {
        public int ParentId { get; set; }
        public string CategoryId { get; set; }
        public string DestinationId { get; set; }
        public string CategoryName { get; set; }
        public string DestinationName { get; set; }
        public string ParentName { get; set; }
        public int Type { get; set; }

        public List<SubCategoryResponse> Categories { get; set; }
        public List<LibDestinationResponse> Destinations { get; set; }
    }
    public class ProductInfoToView
    {
        public List<SubCategoryResponse> Categories { get; set; }
        public List<LibDestinationResponse> Destinations { get; set; }
    }
    public class ProductTagsAddViewModel
    {
        public int ParentId { get; set; }
        [Required]
        public Guid CategoryId { get; set; }
        [Required]
        public Guid DestinationId { get; set; }
        [Required]
        public string Name { get; set; }
        public string TextView { get; set; }
        public string TextLink { get; set; }
        public int Type { get; set; }
        public int Idx { get; set; }
        public int TypeResponse { get; set; }
        public ProductTagsAddViewModel()
        {
            TypeResponse = 1;
        }
    }
    public class ProductTagsUpdateIdxViewModel
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public int Idx { get; set; }
    }
    //public class ProductTagsGetListViewModel
    //{
    //    [Required]
    //    public int parentId { get; set; }
    //    public string key { get; set; }
    //    [Required]
    //    public Guid categoryId { get; set; }
    //    [Required]
    //    public Guid destinationId { get; set; }
    //}
    //public class JustHasId
    //{
    //    public int id { get; set; }
    //    public int idx { get; set; }
    //    public string name { get; set; }
    //}
}