using DigiticketCMS.Models;
using DigiticketCMS.Services.Models;
using Services.Models.Responses.Lib;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DigiticketCMS.Models.Banner
{
    public class BannerViewModel : BaseViewModel
    {
        public string CategoryId { get; set; }
        public string DestinationId { get; set; }
        public int? Status { get; set; }
        public string StringIsPublic { get; set; }
        public int? Type { get; set; }
    }
    public class BannerAddViewModel : BaseImgViewModel
    {
        public Guid CategoryId { get; set; }
        public Guid DestinationId { get; set; }
    }

    public class BannerEditViewModel : BaseImgViewModel
    {
        public int Id { get; set; }
    }

    public class BannerEditDateFromClientViewModel
    {
        public string DestinationName { get; set; }
        public string CategoryName { get; set; }
        public int Id { get; set; }

    }
    public class BannerShowViewModel
    {
        public List<SubCategoryResponse> Categories { get; set; }
        public List<LibDestinationResponse> Destinations { get; set; }
    }
    public class BannerTypeViewModel : BaseViewModel
    {
        public string CategoryId { get; set; }
        public string DestinationId { get; set; }
        public int? Status { get; set; }
    }
    public class BannerTypeAddViewModel
    {
        public Guid? CategoryId { get; set; }
        public Guid? DestinationId { get; set; }
        public string Name { get; set; }
    }
    public class BannerTypeEditViewModel
    {
        [Required]
        public int Id { get; set; }
        public string Name { get; set; }
    }
}