using AutoMapper;
using DigiticketCMS.Helpers;
using DigiticketCMS.Models;
using DigiticketCMS.Models.Users;
using Infrastructure.Config;
using Infrastructure.Extensions;
using Services.Interfaces;
using Services.Models;
using Services.Models.Responses.Product;
using Services.Models.Responses.Supplier;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace DigiticketCMS.Controllers
{
    [Authorize]
    public class UsersController : BaseController
    {

        #region constructor
        private IAccountServices _accountServices;
        public UsersController(IAccountServices accountServices)
        {
            _accountServices = accountServices;
        }
        #endregion

        #region get by page
        public ActionResult Users()
        {
            if (User.CheckPermission(Permission.SysWorkGroup))
            {
                return View();
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public ActionResult UsersGetByPage(WorkgroupGetUserViewModel model)
        {
            if (User.CheckPermission(Permission.SysWorkGroup))
            {
                var request = Mapper.Map<WorkgroupGetUserRequest>(model);
                request.PageIndex = model.page;
                request.PageSize = model.perpage;
                request.FieldName = model.field;
                request.Orderby = model.sort;
                if (!string.IsNullOrEmpty(model.key))
                {
                    request.Keyword = model.key;
                }
                var result = _accountServices.WorkgroupGetUser(request, User.Identity.GetClaims("token"));
                if (result.IsOK() && !result.Data.Result.IsNullOrEmpty())
                {
                    long totalItems = result.Data.Total;
                    var totalPages = Math.Ceiling((double)totalItems / model.perpage);
                    return Json(new JsonDataTable
                    {
                        success = true,
                        meta = new PagingInfo
                        {
                            page = model.page,
                            pages = (long)totalPages,
                            perpage = model.perpage,
                            total = totalItems,
                            field = model.field
                        },
                        data = result.Data.Result
                    }, JsonRequestBehavior.AllowGet);
                }
                else if (result.Code == 401)
                {
                    Response.Cookies[SystemConfig.System_AuthCookieName].Value = string.Empty;
                    Response.Cookies[SystemConfig.System_AuthCookieName].Expires = DateTime.Now.AddMonths(-1);
                    return Json(new
                    {
                        page = "login page"
                    }, JsonRequestBehavior.AllowGet);
                }
                return Json(new JsonDataTable
                {
                    meta = new PagingInfo()
                    {
                        page = model.page,
                        perpage = model.perpage,
                    },
                    data = new List<ProductResponse>()
                }, JsonRequestBehavior.AllowGet);
            }
            return Json(new JsonDataTable
            {
                meta = null,
                data = new List<SupplierResponse>()
            }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Modal edit
        public ActionResult UsersAddModal()
        {
            if (User.CheckPermission(Permission.SysWorkGroup))
            {
                return PartialView("Partial/_UsersAddModal");
            }
            return PartialView("Views/Shared/_ErrorViewModal.cshtml", new ErrorViewModel()
            {
                Title = "You do not have permission"
            });
        }

        public ActionResult UsersEditModal(int id, string name)
        {
            if (User.CheckPermission(Permission.SysWorkGroup))
            {
                if (id != 0)
                {
                    var result = _accountServices.WorkgroupGetUserRoles(id, User.Identity.GetClaims("Token"));
                    if (result.IsOK())
                    {
                        var finalResult = new UsersEditToViewModel()
                        {
                            Id = id,
                            Name = name,
                            Data = result.Data,
                        };
                        return PartialView("Partial/_UsersEditModal", finalResult);
                    }
                    return PartialView("~/Views/Shared/_ErrorViewModal.cshtml", new ErrorViewModel()
                    {
                        Title = ""
                    });
                }
                return PartialView("~/Views/Shared/_ErrorViewModal.cshtml", new ErrorViewModel()
                {
                    Title = "Id cannot be empty!",
                });
            }
            return PartialView("Views/Shared/_ErrorViewModal.cshtml", new ErrorViewModel()
            {
                Title = "You do not have permission"
            });
        }
        #endregion

        #region edit
        [HttpPost]
        public JsonResult UsersEdit(EditUpdateuserRolseViewModel model)
        {
            if (ModelState.IsValid)
            {
                var request = new EditUpdateuserRolseRequest()
                {
                    RoleIds = model.RoleIds.Split('~').Select(u => Int32.Parse(u)).ToList(),
                    UserId = model.UserId
                };
                var result = _accountServices.WorkgroupUpdateUserRoles(request, User.Identity.GetClaims("token"));
                if (result.IsOK())
                {
                    return Json(new JsonClientResult()
                    {
                        Success = true,
                        CallBack = "users.onRendAfterUpdate();"
                    });
                }
                return Json(new JsonClientResult()
                {
                    CallBack = $"Common_another.showErr();"
                });
            }
            return Json(new JsonClientResult()
            {
                CallBack = $"Common_another.showErr(`{GetModelStateError()}`);"
            });
        }
        #endregion

        #region look user roles
        public ActionResult GetUserPermissionModal(GetUserRolseFromClientViewModel model)
        {
            if (model.Id != 0)
            {
                if (User.CheckPermission(Permission.SysWorkGroup))
                {
                    var result = _accountServices.WorkgroupGetUserPermissions(model.Id, User.Identity.GetClaims("token"));
                    if (result.IsOK())
                    {
                        var dataToClient = new GetUserRolesViewModel()
                        {
                            Name = model.Name,
                            UserPermission = result.Data
                        };
                        return PartialView("Partial/_GetUserPermissionModal", dataToClient);
                    }
                    return PartialView("~/Views/Shared/_ErrorViewModal.cshtml", new ErrorViewModel()
                    {
                        Title = result.Errors.GetResultError()
                    });
                }
                return PartialView("~/Views/Shared/_ErrorViewModal.cshtml", new ErrorViewModel()
                {
                    Title = "You do not have permission",
                });
            }
            return PartialView("~/Views/Shared/_ErrorViewModal.cshtml", new ErrorViewModel()
            {
                Title = $"Id cannot be null. Id = {model.Id}",
            });
        }
        #endregion

        #region add
        public JsonResult UsersSuggestAdd(string key = "")
        {
            if (User.CheckPermission(Permission.SysWorkGroup))
            {
                var resultToView = new List<SearchUsersResponse>();
                if (key.CheckForKey())
                {
                    var result = _accountServices.WorkgroupSearchUser(key, User.Identity.GetClaims("token"));
                    if(result.IsOK() && result.Data != null)
                    {
                        resultToView = result.Data.Result.ToList();
                    }
                }
                return Json(new JsonClientResult
                {
                    Success = true,
                    Data = resultToView
                }, JsonRequestBehavior.AllowGet);
            }
            return Json(new JsonClientResult()
            {
                Message = "You do not have permission"
            });
        }

        [HttpPost]
        public JsonResult UsersAdd(WorkgroupAddUserViewModel model)
        {
            if (User.CheckPermission(Permission.SysWorkGroup))
            {
                var request = Mapper.Map<WorkgroupAddUserRequest>(model);
                var result = _accountServices.WorkgroupAddUser(request, User.Identity.GetClaims("token"));
                if (result.IsOK())
                {
                    return Json(new JsonClientResult()
                    {
                        Success = true,
                        CallBack = "users.onRendAfterAdd();"
                    });
                }
                return Json(new JsonClientResult()
                {
                    CallBack = $"Common_another.showErr();"
                });
            }
            return Json(new JsonClientResult()
            {
                Message = "You do not have permission"
            });
        }
        #endregion
    }
}