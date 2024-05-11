using DigiticketCMS.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DigiticketCMS.Models.Media
{
    public class MediaViewModel
    {
        //public MediaUploadRequest UploadRequest { get; set; }
        public HttpPostedFileBase FileRequest { get; set; }
    }
    public class MediaUploadViewModel
    {
        //[Required]
        //[Range(0.1, Double.MaxValue, ErrorMessage = "The field {0} must be greater than {1}.")]
        public decimal aspectRatioX { get; set; }
        //[Required]
        //[Range(0.1, Double.MaxValue, ErrorMessage = "The field {0} must be greater than {1}.")]
        public decimal aspectRatioY { get; set; }
        [Required]
        [Range(0.0, Double.MaxValue, ErrorMessage = "The field {0} must be greater than {1}.")]
        public decimal resizeWidth { get; set; }
        [Required]
        [Range(0.0, Double.MaxValue, ErrorMessage = "The field {0} must be greater than {1}.")]
        public decimal resizeHeight { get; set; }
    }
    public class MediaResizeViewModel
    {
        [Required]
        public int originalId { get; set; }
        [Required]
        [Range(0.0, Double.MaxValue, ErrorMessage = "The field {0} must be greater than {1}.")]
        public decimal resizeWidth { get; set; }
        [Required]
        [Range(0.0, Double.MaxValue, ErrorMessage = "The field {0} must be greater than {1}.")]
        public decimal resizeHeight { get; set; }
    }
    public class MediaGetByPageViewModel : BaseViewModel
    {
        
    }
}