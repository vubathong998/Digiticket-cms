using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Models
{
    public class WorkgroupGetUserResponse
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public bool Gender { get; set; }
        public string DisplayName { get; set; }
        public string Email { get; set; }
        public int? PhoneNumber { get; set; }
        public string Avatar { get; set; }
        public string UserName { get; set; }
        public DateTime? birthday { get; set; }
        public bool IsOwner { get; set; }
        public bool IsActive { get; set; }
        public List<PermissionRequest> Permissions { get; set; }
        public List<RoleRequest> Roles { get; set; }
    }
    public class PermissionRequest
    {
        public int Id { get; set; }
        public int ParentId { get; set; }
        public string Name { get; set; }
        public string Descriptions { get; set; }
        public List<SubPermissionRequest> SubPermission { get; set; }
        public bool IsSelected { get; set; }
    }

    public class RoleRequest
    {
        public int Id { get; set; }
        public int TenantId { get; set; }
        public int WorkGroupId { get; set; }
        public bool IsSelected { get; set; }
        public string Name { get; set; }
        public object Permissions { get; set; }
    }

    public class SubPermissionRequest
    {
        public int Id { get; set; }
        public int ParentId { get; set; }
        public string Name { get; set; }
        public string Descriptions { get; set; }
        public List<object> SubPermission { get; set; }
        public bool IsSelected { get; set; }
    }

    public class UserMemberResponse
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Avatar { get; set; }
        public string Cover { get; set; }
        public bool IsActive { get; set; }
        public bool IsDelete { get; set; }
        public DateTime CreationTime { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
        public string DisplayName { get; set; }
        public string Birthday { get; set; }
        public string Address { get; set; }
        public bool Gender { get; set; }
        public bool EmailConfirmed { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public int TenantId { get; set; }
        public int Type { get; set; }
        public List<UserPermission> Permissions { get; set; }
    }

    public class UserRole
    {
        public int Id { get; set; }
        public int TenantId { get; set; }
        public int WorkGroupId { get; set; }
        public bool IsSelected { get; set; }
        public string Name { get; set; }
        public string Permissions { get; set; }
    }

    public class UserPermission
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Descriptions { get; set; }
        public List<UserPermission> SubPermission { get; set; }
        public bool IsSelected { get; set; }
    }
}
