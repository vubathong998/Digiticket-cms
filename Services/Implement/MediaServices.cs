using Infrastructure.Config;
using Services.Interfaces;
using Services.Models;
using Services.Models.Requests.Media;
using Services.Models.Responses.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Implement
{
    public class MediaServices : IMediaServices
    {
        private BaseServices _baseService;
        public MediaServices()
        {
            _baseService = new BaseServices(APIMediaConfig.APIMediaDefault);
        }
        #region cdn media
        public BaseResponse<PagingResultv2<MediaGetByPageResponse>> GetByPage(MediaGetByPageRequest model, string token)
        {
            var result = _baseService.Get<PagingResultv2<MediaGetByPageResponse>>("api/admin/digiticket-get-by-page-media-root", model, token);
            return result;
        }
        public BaseResponse<List<MediaResponse>> UploadMedia(MediaRequest request, MediaUploadRequest data, string token)
        {
            var result = _baseService.UploadMedia("api/upload/digiticket-original-images", data, request.FileRequest);
            return result;
        }
        public BaseResponse<List<MediaGetByRootResponse>> GetByRoot(decimal Id, string token)
        {
            var result = _baseService.Get<List<MediaGetByRootResponse>>($"/api/admin/digiticket-get-list-media-by-root?RootId={Id}", null,  token);
            return result;
        }
        public BaseResponse<List<MediaResponse>> ResizeMedia(MediaResizeRequest data, string token)
        {
            var result = _baseService.PostQuery<List<MediaResponse>>("api/upload/digiticket-original-images-resize", data, token);
            return result;
        }
        #endregion
    }
}
