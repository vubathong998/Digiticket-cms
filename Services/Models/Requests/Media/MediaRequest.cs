using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Services.Models.Requests.Media
{
    public class MediaRequest
    {
        public HttpPostedFileBase FileRequest { get; set; }
    }
    public class MediaUploadRequest
    {
        public decimal aspectRatioX { get; set; }
        public decimal aspectRatioY { get; set; }
        [Range(0.0, Double.MaxValue, ErrorMessage = "The field {0} must be greater than {1}.")]
        public decimal resizeWidth { get; set; }
        [Range(0.0, Double.MaxValue, ErrorMessage = "The field {0} must be greater than {1}.")]
        public decimal resizeHeight { get; set; }
    }
    public class MediaResizeRequest
    {
        public int originalId { get; set; }
        [Range(0.0, Double.MaxValue, ErrorMessage = "The field {0} must be greater than {1}.")]
        public decimal resizeWidth { get; set; }
        [Range(0.0, Double.MaxValue, ErrorMessage = "The field {0} must be greater than {1}.")]
        public decimal resizeHeight { get; set; }
    }
    public class MediaGetByPageRequest
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public bool OrderByDesc { get; set; }
        public string OrderBy { get; set; }
        public string KeyWord { get; set; }
        public MediaGetByPageRequest()
        {
            PageIndex = 1;
            PageSize = 10;
            OrderByDesc = true;
            OrderBy = "CreatedDate";
            KeyWord = "";
        }
    }
}
