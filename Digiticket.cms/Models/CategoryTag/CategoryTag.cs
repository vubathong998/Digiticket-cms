using Services.Models.Responses.CategoryTag;
using Services.Models.Responses.Lib;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DigiticketCMS.Models.CategoryTag
{
    public class CategoryTagFromClient
    {
        public int ParentId { get; set; }
        public string ParentName { get; set; }
        public int Round { get; set; }
    }
    public class CategoryTagToGetByPageViewModel
    {
        public int ParentId { get; set; }
        public string ParentName { get; set; }
        public int Round { get; set; }
        //public List<SubCategoryResponse> Categories { get; set; }
        //public List<LibDestinationResponse> Destinations { get; set; }
        public CategoryTagToGetByPageViewModel()
        {
            ParentId = 0;
            ParentName = "";
            Round = 1;
        }
    }

    public class CategoryGetByPageViewModel : BaseViewModel
    {
        public int? ParentId { get; set; }
        public string ParentName { get; set; }
        public bool? IsHome { get; set; }
        public bool? IsHot { get; set; }
    }

    public class CategoryTagAddViewModel
    {
        public int TagParentId { get; set; }
        public int TagId { get; set; }
        public int? Idx { get; set; }
        public bool IsView { get; set; }
        public bool IsHome { get; set; }
        public bool IsHot { get; set; }
        public CategoryTagAddViewModel()
        {
            Idx = 0;
        }
    }

    public class CategoryTagEditRequestViewModel
    {
        [Required]
        public int Id { get; set; }
        public int Idx { get; set; }
        public bool IsView { get; set; }
    }

    public class CategoryTagDataToAddOrUpdateViewModal
    {
        public int ParentId { get; set; }
        public string ParentName { get; set; }
        public int Round { get; set; }
        public CategoryTagResponse CategoryTagResponse { get; set; }
        public CategoryTagDataToAddOrUpdateViewModal()
        {
            CategoryTagResponse = new CategoryTagResponse();
        }
    }
    public class CategoryTagUpdateHomeViewModel
    {
        public int Id { get; set; }
        public bool IsHome { get; set; }
        public int IdxHome { get; set; }
    }
    public class CategoryTagUpdateHotViewModel
    {
        public int Id { get; set; }
        public bool IsHot { get; set; }
        public int IdxHot { get; set; }
    }

    public class CategoryTagEditViewModel
    {
        public int Id { get; set; }
        public int Idx { get; set; }
        public bool IsView { get; set; }
    }
}