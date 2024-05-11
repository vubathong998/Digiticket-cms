using Infrastructure.Config;
using Services.Interfaces;
using Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Implement
{
    public class AccountServices : IAccountServices
    {
        private BaseServices _baseService;

        public AccountServices()
        {
            _baseService = new BaseServices(APIConfig.API_ACCOUNT_URL);
        }

        public BaseResponse<LoginResponse> Login(LoginRequest data)
        {
            var result = _baseService.PostApi<LoginResponse>("/api/account/login", data, null, true);
            return result;
        }

        public BaseResponse<LoginResponse> LoginTenant(LoginRequest data)
        {
            var result = _baseService.PostApi<LoginResponse>("/api/account/login-tenant", data, null, true);
            return result;
        }
        public BaseResponse<LoginResponse> LoginCms(LoginRequest data)
        {
            var result = _baseService.PostApi<LoginResponse>("/api/account/login-cms", data, null, true);
            return result;
        }

        public BaseResponse<ProfileResponse> GetProfile(string token)
        {
            var result = _baseService.GetApi<ProfileResponse>("/api/account/profile", null, token);
            return result;
        }

        public BaseResponse<List<string>> GetRole(string token)
        {
            var result = _baseService.GetApi<List<string>>("/api/account/role", null, token);
            return result;
        }

        public BaseResponse<List<string>> GetPermission(string token)
        {
            var result = _baseService.GetApi<List<string>>("/api/account/permission", null, token);
            return result;
        }

        public BaseResponse<LoginResponse> RefreshToken(string token)
        {
            var result = _baseService.GetApi<LoginResponse>("/api/account/token/refresh/" + token, null, null, true);
            return result;
        }

        public BaseResponse<string> VerifyToken(string token)
        {
            var result = _baseService.GetApi<string>("/api/account/token/verify", null, token, true);
            return result;
        }

        public BaseResponse<SearchUsersResponse> SearchUser(string data, string token)
        {
            var result = _baseService.GetApi<SearchUsersResponse>("/api/account/search?keyword=" + data, null, token);
            return result;
        }

        public BaseResponse<List<WorkgroupResponse>> GetListWorkgroup(string token)
        {
            var result = _baseService.GetApi<List<WorkgroupResponse>>("/api/account/workgroup/get-list", null, token);
            return result;
        }

        public BaseResponse<LoginResponse> SelectWorkgroup(string data, string token)
        {
            var result = _baseService.GetApi<LoginResponse>("/api/account/workgroup/select/" + data, null, token, true);
            return result;
        }

        public BaseResponse<string> Register(RegisterRequest data)
        {
            var result = _baseService.PostApi<string>("/api/account/register", data, null, true);
            return result;
        }
        public BaseResponse<string> RegisterCms(RegisterRequest data)
        {
            var result = _baseService.PostApi<string>("/api/account/register-cms", data, null, true);
            return result;
        }

        public BaseResponse<string> ConfirmAccount(ConfirmAccountRequest data)
        {
            var result = _baseService.PostApi<string>("/api/account/comfirm-account", data, null, true);
            return result;
        }

        public BaseResponse<string> ResendConfirmAccount(ResendConfirmAccountRequest data)
        {
            var result = _baseService.PostApi<string>("/api/account/resend-confirm-account", data, null, true);
            return result;
        }

        public BaseResponse<string> ForgotPassword(ForgotPasswordRequest data)
        {
            var result = _baseService.PostApi<string>("/api/account/forgot-password", data, null, true);
            return result;
        }

        public BaseResponse<string> ResetPassword(ResetPasswordRequest data)
        {
            var result = _baseService.PostApi<string>("/api/account/reset-password", data, null, true);
            return result;
        }

        #region Workgroup
        public BaseResponse<PagingResult<SearchUsersResponse>> WorkgroupSearchUser(string data, string token)
        {
            var result = _baseService.GetApi<PagingResult<SearchUsersResponse>>("/api/account/tenant/search-user?Keyword=" + data, null, token);
            return result;
        }

        public BaseResponse<PagingResult<WorkgroupGetUserResponse>> WorkgroupGetUser(WorkgroupGetUserRequest data, string token)
        {
            var result = _baseService.PostApi<PagingResult<WorkgroupGetUserResponse>>("/api/account/workgroup/get-user", data, token);
            return result;
        }

        public BaseResponse<string> WorkgroupAddUser(WorkgroupAddUserRequest data, string token)
        {
            var result = _baseService.PostApi<string>("/api/account/workgroup/add-user", data, token, true);
            return result;
        }

        public BaseResponse<string> WorkgroupRemoveUser(string data, string token)
        {
            var result = _baseService.DeleteApi<string>("/api/workgroup/user/" + data, null, token, true);
            return result;
        }

        public BaseResponse<List<UserPermission>> WorkgroupGetUserPermissions(int data, string token)
        {
            var result = _baseService.GetApi<List<UserPermission>>("/api/account/workgroup/get-user-permissions?UserId=" + data, null, token);
            return result;
        }

        public BaseResponse<string> WorkgroupUserEditPermission(EditPerUserRequest data, string token)
        {
            var result = _baseService.PutApi<string>("/api/workgroup/user/update-permissions/" + data.UserId, data, token, true);
            return result;
        }

        public BaseResponse<List<UserRole>> WorkgroupGetUserRoles(int data, string token)
        {
            var result = _baseService.GetApi<List<UserRole>>("/api/account/workgroup/get-user-roles?UserId=" + data, null, token);
            return result;
        }

        public BaseResponse<string> WorkgroupUpdateUserRoles(EditUpdateuserRolseRequest data, string token)
        {
            var result = _baseService.PutApi<string>("/api/account/workgroup/update-user-roles", data, token, true);
            return result;
        }
        #endregion

    }
}
