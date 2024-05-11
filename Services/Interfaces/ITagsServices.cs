using Services.Models;
using Services.Models.Requests.Tags;
using Services.Models.Responses.Tags;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface ITagsServices
    {
        BaseResponse<PagingResult<ProductTagsResponse>> GetByPage(ProductTagsGetByPageRequest model, string token);
        BaseResponse<int> Add(ProductTagsAddRequest model, string token);
        BaseResponse<string> UpdateIdx(ProductTagsUpdateIdxRequest model, string token);
        BaseResponse<string> Delete(int id, string token);
        BaseResponse<ProductTagsResponse> GetById(int id, string token);
        BaseResponse<List<ProductTagsResponse>> GetList(TagsGetListRequest model, string token);
        GetListTagsAndSubTagsCache GetListTagsAndSubTagsCache(int typeParent, string token);
        GetListTagsCache GetListTagsCache(int typeParent, string token);
    }
}
