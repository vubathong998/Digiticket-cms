using DigiticketCMS.Services.Models;
using Infrastructure.Config;
using Services.Interfaces;
using Services.Models;
using Services.Models.Requests.GroupdService;
using Services.Models.Responses.GroupService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Implement
{
    public class GroupServiceService : IGroupServiceService
    {
        private BaseServices _baseService;

        public GroupServiceService()
        {
            _baseService = new BaseServices(APIConfig.API_SERVICES_URL);
        }
        public BaseResponse<PagingResult<GroupServiceResponse>> GetByPage(GroupServiceRequest data, string token)
        {
            var result = _baseService.PostApi<PagingResult<GroupServiceResponse>>("api/groupservice/get-by-page", data, token, true);
            return result;
        }
        public BaseResponse<GroupServiceResponse> GetFromId(int id, string token)
        {
            var result = _baseService.GetApi<GroupServiceResponse>($"/api/groupservice/{id}", null, token, true);
            return result;
        }
        public BaseResponse<string> UpdateInfo(GroupServiceUpdateInfoRequest model, string token)
        {
            var result = _baseService.PutApi<string>($"/api/groupservice/{model.Id}/update-info", model, token, true);
            return result;
        }
        public BaseResponse<string> UpdateImage(GroupServiceUpdateImageRequest model, string token)
        {
            var result = _baseService.PutApi<string>($"/api/groupservice/{model.Id}/update-image", model, token, true);
            return result;
        }
        public BaseResponse<string> UpdateNumberTicket(GroupServiceUpdateNumberTicketRequest model, string token)
        {
            var result = _baseService.PutApi<string>($"/api/groupservice/{model.Id}/update-number-ticket", model, token, true);
            return result;
        }
        public BaseResponse<string> UpdateGroupTime(GroupServiceUpdateGroupTimeRequest model, string token)
        {
            var result = _baseService.PutApi<string>($"/api/groupservice/{model.Id}/update-group-time", model, token, true);
            return result;
        }
        public BaseResponse<string> UpdateGroupNumber(GroupServiceUpdateGroupNumberRequest model, string token)
        {
            var result = _baseService.PutApi<string>($"/api/groupservice/{model.Id}/update-group-number", model, token, true);
            return result;
        }
        public BaseResponse<string> UpdateTags(GroupServiceUpdateTagsRequest model, string token)
        {
            var result = _baseService.PutApi<string>($"/api/groupservice/{model.Id}/update-tags", model, token, true);
            return result;
        }
        public BaseResponse<string> UpdateTagsProcess(GroupServiceUpdateTagsProcessRequest model, string token)
        {
            var result = _baseService.PutApi<string>($"/api/groupservice/{model.Id}/update-tags-process", model, token, true);
            return result;
        }
        public BaseResponse<string> UpdateTagsView(GroupServiceUpdateTagsViewRequest model, string token)
        {
            var result = _baseService.PutApi<string>($"/api/groupservice/{model.Id}/update-tags-view", model, token, true);
            return result;
        }
    }
}
