using DigiticketCMS.Services.Models;
using Services.Models;
using Services.Models.Requests.GroupdService;
using Services.Models.Responses.GroupService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IGroupServiceService
    {
        BaseResponse<PagingResult<GroupServiceResponse>> GetByPage(GroupServiceRequest data, string token);
        BaseResponse<GroupServiceResponse> GetFromId(int id, string token);
        BaseResponse<string> UpdateInfo(GroupServiceUpdateInfoRequest model, string token);
        BaseResponse<string> UpdateImage(GroupServiceUpdateImageRequest model, string token);
        BaseResponse<string> UpdateNumberTicket(GroupServiceUpdateNumberTicketRequest model, string token);
        BaseResponse<string> UpdateGroupTime(GroupServiceUpdateGroupTimeRequest model, string token);
        BaseResponse<string> UpdateGroupNumber(GroupServiceUpdateGroupNumberRequest model, string token);
        BaseResponse<string> UpdateTags(GroupServiceUpdateTagsRequest model, string token);
        BaseResponse<string> UpdateTagsProcess(GroupServiceUpdateTagsProcessRequest model, string token);
        BaseResponse<string> UpdateTagsView(GroupServiceUpdateTagsViewRequest model, string token);
    }
}
