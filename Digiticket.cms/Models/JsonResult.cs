using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DigiticketCMS.Models
{
    public class JsonClientResult
    {
        public bool Success { get; set; }
        public object Data { get; set; }
        public string Message { get; set; }
        public string CallBack { get; set; }
    }

    public class JsonDataTable
    {
        public PagingInfo meta { get; set; }
        public object data { get; set; }
        public bool success { get; set; }
    }

    public class PagingInfo
    {
        /// <summary>
        /// page number
        /// </summary>
        public long page { get; set; }
        /// <summary>
        /// total pages
        /// </summary>
        public long pages { get; set; }
        /// <summary>
        /// page size
        /// </summary>
        public int perpage { get; set; }
        /// <summary>
        /// total items
        /// </summary>
        public long total { get; set; }
        /// <summary>
        /// asc - desc
        /// </summary>
        public string sort { get; set; }
        /// <summary>
        /// sort by field
        /// </summary>
        public string field { get; set; }
        public string keyword { get; set; }
    }
}