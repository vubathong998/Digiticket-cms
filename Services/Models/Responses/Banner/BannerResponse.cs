using Services.Models.Responses.Lib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Models.Responses.Banner
{
    public class BannerResponse : BannerLibResponse
    {
        public string CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string DestinationId { get; set; }
        public string DestinationName { get; set; }
        public string TypeName { get; set; }
        public int TotalRows { get; set; }
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
    public class ProfileData
    {
    }

    public class BannerTypeResponse : BannerLibResponse
    {
        public Guid? CategoryId { get; set; }
        public string CategoryName { get; set; }
        public Guid? DestinationId { get; set; }
        public string DestinationName { get; set; }
        public int TotalRows { get; set; }
        public int Id { get; set; }
        public int Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public string CreatedByName { get; set; }
        public DateTime LastEditedDate { get; set; }
        public int LastEditedBy { get; set; }
        public string LastEditedByName { get; set; }
        public ProfileBannerTypeData ProfileData { get; set; }
    }
    public class ProfileBannerTypeData
    {
    }

    public class BannerLibResponse : BaseImg
    {
        public List<SubCategoryResponse> Categories { get; set; }
        public List<LibDestinationResponse> Destinations { get; set; }
    }
}
