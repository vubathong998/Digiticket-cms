using Services.Models.Requests.CategoryTag;
using Services.Models.Responses.CategoryTag;
using Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Services.Models.Requests.Tags;

namespace Services.Interfaces
{
    public interface ICategoryTagService
    {
        BaseResponse<string> Add(CategoryTagAddRequest model, string token);
        BaseResponse<string> Edit(CategoryTagEditRequest model, string token);
        BaseResponse<string> Delete(int id, string token);
        BaseResponse<CategoryTagResponse> GetById(int id, string token);
        BaseResponse<PagingResult<CategoryTagResponse>> GetByPage(CategoryTagGetByPageRequest model, string token);
        BaseResponse<string> UpdateHome(CategoryTagUpdateHomeRequest model, string token);
        BaseResponse<string> UpdateHot(CategoryTagUpdateHotRequest model, string token);

    }
}
