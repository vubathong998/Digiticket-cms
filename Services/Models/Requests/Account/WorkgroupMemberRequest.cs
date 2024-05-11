using DigiticketCMS.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Models
{
    public class WorkgroupGetUserRequest : BaseRequest
    {
    }
    public class WorkgroupAddUserRequest
    {
        public List<int> Ids { get; set; }
    }

    public class EditPerUserRequest
    {
        public long UserId { get; set; }
        public List<string> PermissionIds { get; set; }
    }

    public class EditUpdateuserRolseRequest
    {
        public long UserId { get; set; }
        public List<int> RoleIds { get; set; }
    }
}
