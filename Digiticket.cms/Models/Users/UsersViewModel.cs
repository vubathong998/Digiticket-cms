using Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DigiticketCMS.Models.Users
{
    public class UsersViewModel
    {
    }

    public class WorkgroupGetUserViewModel : BaseViewModel
    {

    }

    public class UsersEditToViewModel
    {
        public string Name { get; set; }
        public int Id { get; set; }
        public List<UserRole> Data { get; set; }
    }
    public class EditUpdateuserRolseViewModel
    {
        public long UserId { get; set; }
        public string RoleIds { get; set; }
    }
    public class WorkgroupAddUserViewModel
    {
        public List<int> Ids { get; set; }
    }
    public class GetUserRolesViewModel
    {
        public string Name { get; set; }
        public List<UserPermission> UserPermission { get; set; }
    }
    public class GetUserRolseFromClientViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }    
    }
}