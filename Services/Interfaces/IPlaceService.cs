using Newtonsoft.Json;
using Services.Models;
using Services.Models.Requests.Product;
using Services.Models.Responses.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IPlaceServices
    {
        BaseResponse<string> GetSSID(string token);
        BaseResponse<ProductPlaceSuggestResponse> Suggest(ProductPlaceSuggestRequest model, string token);
        BaseResponse<ProductPlaceDetailResponse> Detail(ProductPlaceDetailRequest model, string token);
    }
}
