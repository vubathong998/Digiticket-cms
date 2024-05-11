using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DigiticketCMS.Models.ViewModel
{
    public class WorkgroupViewModel
    {
    }

    public class WorkgroupUserEditPerViewModel
    {
        public long UserId { get; set; }
        public string PerIds { get; set; }
    }

    public class WorkgroupUserEditRoleViewModel
    {
        public long UserId { get; set; }
        public List<string> Roles { get; set; }
    }
}