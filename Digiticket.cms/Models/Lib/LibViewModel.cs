using Services;
using Services.Models.Responses.Lib;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DigiticketCMS.Models.Lib
{
    public class LibViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public List<SubCategoryResponse> Category { get; set; }
        public List<LibDestinationResponse> Destination { get; set; }
    }
    #region category
    public class LibCategoryUpdateViewModel
    {
        [Required]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Position { get; set; }
        public string Url { get; set; }
        public bool IsAvailable { get; set; }
        public bool IsView { get; set; }
        public string IconPath { get; set; }
    }
    public class LibCategoryModalViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

    }
    #endregion

    #region destination
    public class LibDestinationViewModel : BaseViewModel
    {
        public bool? IsHome { get; set; }
    }

    public class LibDestinationUpdateViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public List<BaseImgViewModel> Images { get; set; }
        public string PlaceId { get; set; }
        public string Place { get; set; }
        public int CountProduct { get; set; }
        public bool IsHome { get; set; }
        public int Status { get; set; }
    }
    #endregion
}