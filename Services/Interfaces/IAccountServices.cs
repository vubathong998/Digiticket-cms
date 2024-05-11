using Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IAccountServices
    {
        BaseResponse<LoginResponse> Login(LoginRequest data);
        BaseResponse<LoginResponse> LoginTenant(LoginRequest data);
        BaseResponse<LoginResponse> LoginCms(LoginRequest data);
        BaseResponse<ProfileResponse> GetProfile(string token);
        BaseResponse<List<string>> GetRole(string token);
        BaseResponse<List<string>> GetPermission(string token);
        BaseResponse<LoginResponse> RefreshToken(string token);
        BaseResponse<string> VerifyToken(string token);
        BaseResponse<SearchUsersResponse> SearchUser(string data, string token);
        BaseResponse<List<WorkgroupResponse>> GetListWorkgroup(string token);
        BaseResponse<LoginResponse> SelectWorkgroup(string data, string token);

        BaseResponse<string> Register(RegisterRequest data);
        BaseResponse<string> RegisterCms(RegisterRequest data);
        BaseResponse<string> ConfirmAccount(ConfirmAccountRequest data);
        BaseResponse<string> ResendConfirmAccount(ResendConfirmAccountRequest data);
        BaseResponse<string> ForgotPassword(ForgotPasswordRequest data);
        BaseResponse<string> ResetPassword(ResetPasswordRequest data);

        #region Workgroup
        BaseResponse<PagingResult<SearchUsersResponse>> WorkgroupSearchUser(string data, string token);
        BaseResponse<PagingResult<WorkgroupGetUserResponse>> WorkgroupGetUser(WorkgroupGetUserRequest data, string token);
        BaseResponse<string> WorkgroupAddUser(WorkgroupAddUserRequest data, string token);
        BaseResponse<string> WorkgroupRemoveUser(string data, string token);
        BaseResponse<List<UserPermission>> WorkgroupGetUserPermissions(int data, string token);
        BaseResponse<string> WorkgroupUserEditPermission(EditPerUserRequest data, string token);
        BaseResponse<List<UserRole>> WorkgroupGetUserRoles(int data, string token);
        BaseResponse<string> WorkgroupUpdateUserRoles(EditUpdateuserRolseRequest data, string token);
        #endregion

    }
}
