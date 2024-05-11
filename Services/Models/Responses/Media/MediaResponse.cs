using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Models.Responses.Media
{
    public class MediaGetByPageResponse
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public string Url { get; set; }
        public string Type { get; set; }
        public double AspectRatioX { get; set; }
        public double AspectRatioY { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
        public int Parent { get; set; }
        public DateTime CreatedDate { get; set; }
        public int TotalCount { get; set; }
    }
    public class MediaResponse
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public string Thumb { get; set; }
    }
    public class MediaGetByRootResponse
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public string Url { get; set; }
        public string Type { get; set; }
        public decimal AspectRatioX { get; set; }
        public decimal AspectRatioY { get; set; }
        public decimal Width { get; set; }
        public decimal Height { get; set; }
        public int Parent { get; set; }
        public DateTime CreatedDate { get; set; }
        public int TotalCount { get; set; }
    }
}
