using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Models.Responses.CategoryTag
{
    public class CategoryTagResponse
    {
        public int TagParentId { get; set; }
        public string ParentName { get; set; }
        public int TagId { get; set; }
        public string TextView { get; set; }
        public string TextLink { get; set; }
        public string Name { get; set; }
        public int Idx { get; set; }
        public int IdxHome { get; set; }
        public int IdxHot { get; set; }
        public bool IsView { get; set; }
        public bool IsHome { get; set; }
        public bool IsHot { get; set; }
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
    public class ProfileData { }
}
