using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Models
{
    public class BaseResponse<T>
    {
        public int Code { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public ICollection<ApiError> Errors { get; set; }

        public bool IsOK()
        {
            if (Code >= 200 && Code <= 206)
                return true;
            return false;
        }
    }

    public class ApiError
    {
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Code { get; set; }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Message { get; set; }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public object MoreInfo { get; set; }
    }

    public class PagingResult<T>
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public long Total { get; set; }
        public IEnumerable<T> Result { get; set; }

        public PagingResult(int pageNumber, int pageSize, long total, IEnumerable<T> result) : this(pageNumber, pageSize)
        {
            Total = total;
            Result = result;
        }

        public PagingResult(int pageNumber, int pageSize)
        {
            PageIndex = pageNumber;
            PageSize = pageSize;
            Total = 0;
            Result = null;
        }

        public PagingResult()
        {

        }
    }

    public class PagingResultv2<T>
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public long TotalCount { get; set; }
        public IEnumerable<T> Result { get; set; }

        public PagingResultv2(int pageNumber, int pageSize, long total, IEnumerable<T> result) : this(pageNumber, pageSize)
        {
            TotalCount = total;
            Result = result;
        }

        public PagingResultv2(int pageNumber, int pageSize)
        {
            PageIndex = pageNumber;
            PageSize = pageSize;
            TotalCount = 0;
            Result = null;
        }

        public bool IsNullOrEmpty()
        {
            throw new NotImplementedException();
        }

        public PagingResultv2()
        {

        }
    }

    public class BaseEntityResponse<T>
    {
        public T Id { get; set; }
        public int Status { get; set; }
        public bool Active { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CreatedDateUnixTime { get; set; }
        public long CreatedBy { get; set; }
        public string CreatedByName { get; set; }
        public DateTime LastEditedDate { get; set; }
        public int LastEditedDateUnixTime { get; set; }
        public long LastEditedBy { get; set; }
        public string LastEditedByName { get; set; }
        public int TotalRows { get; set; }
    }
}
