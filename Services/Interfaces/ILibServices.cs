using Services.Models.Requests.Product;
using Services.Models.Responses.Product;
using Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Services.Models.Responses.Lib;
using Services.Models.Requests.Lib;

namespace Services.Interfaces
{
    public interface ILibServices
    {
        #region category
        BaseResponse<List<SubCategoryResponse>> CategoryGetAll(string token);
        BaseResponse<LibCategoryResponse> CategoryGetById(Guid id, string token);
        BaseResponse<string> CategoryUpdate(LibCategoryUpdateRequest model, string token);
        #endregion
        #region destination
        BaseResponse<List<LibDestinationResponse>> DestinationGetAll(string token);
        BaseResponse<PagingResult<LibDestinationResponse>> DestinationGetByPage(LibDestinationRequest model, string token);
        BaseResponse<LibDestinationResponse> DestinationGetById(Guid Id, string token);
        BaseResponse<string> DestinationUpdate(LibDestinationUpdateRequest model, string token);
        #endregion
        BaseResponse<List<LibTagViewResponse>> TagView(string token);
    }
}
