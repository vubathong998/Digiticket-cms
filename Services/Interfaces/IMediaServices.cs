using Services.Models;
using Services.Models.Requests.Media;
using Services.Models.Responses.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IMediaServices
    {
        BaseResponse<PagingResultv2<MediaGetByPageResponse>> GetByPage(MediaGetByPageRequest model, string token);
        BaseResponse<List<MediaResponse>> UploadMedia(MediaRequest request, MediaUploadRequest data, string token);
        BaseResponse<List<MediaGetByRootResponse>> GetByRoot(decimal Id, string token);
        BaseResponse<List<MediaResponse>> ResizeMedia(MediaResizeRequest Id, string token);
    }
}
