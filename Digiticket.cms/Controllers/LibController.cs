using AutoMapper;
using DigiticketCMS.Helpers;
using DigiticketCMS.Models;
using DigiticketCMS.Models.Lib;
using DigiticketCMS.Models.Product;
using DigiticketCMS.Services.Models;
using Infrastructure.Config;
using Infrastructure.Extensions;
using Services.Implement;
using Services.Interfaces;
using Services.Models.Requests.Lib;
using Services.Models.Requests.Product;
using Services.Models.Responses.Banner;
using Services.Models.Responses.Lib;
using Services.Models.Responses.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace DigiticketCMS.Controllers
{
    [Authorize]
    public class LibController : BaseController
    {
        #region constructor
        private ILibServices _libServices;
        public LibController(ILibServices libServices)
        {
            _libServices = libServices;
        }
        #endregion

        #region category
        public ActionResult LibCategory()
        {
            if (User.CheckPermission(Permission.SettingView))
            {
                var categoryResponse = _libServices.CategoryGetAll(User.Identity.GetClaims("token"));
                if (categoryResponse.IsOK())
                {
                    return View(categoryResponse.Data);
                }
                return View(new List<SubCategoryResponse>());
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public ActionResult LibCategoryUpdateModal(Guid id)
        {
            if (User.CheckPermission(Permission.SettingEdit))
            {
                if (ModelState.IsValid)
                {
                    var result = _libServices.CategoryGetById(id, User.Identity.GetClaims("token"));
                    if (result.IsOK())
                    {
                        return PartialView("Partial/_CategoryUpdateModal", result.Data);
                    }
                    return PartialView("~/Views/Shared/_ErrorViewModal.cshtml", new ErrorViewModel()
                    {
                        Title = "Đã có lỗi xảy ra",
                        Description = result.Errors.GetResultError()
                    });
                }
                return PartialView("~/Views/Shared/_ErrorViewModal.cshtml", new ErrorViewModel() { Title = GetModelStateError() });
            }
            return PartialView("~/Views/Shared/_ErrorViewModal.cshtml", new ErrorViewModel() { Title = "You do not permission!" });
        }

        [HttpPost]
        public JsonResult LibCategoryUpdate(LibCategoryUpdateViewModel model)
        {
            if (User.CheckPermission(Permission.SettingEdit))
            {
                if (ModelState.IsValid)
                {
                    if (model.Id != Guid.Empty)
                    {
                        var request = Mapper.Map<LibCategoryUpdateRequest>(model);
                        var result = _libServices.CategoryUpdate(request, User.Identity.GetClaims("token"));
                        if (result.IsOK())
                        {
                            return Json(new JsonClientResult()
                            {
                                Success = true,
                                CallBack = $"category.onRendAfterUpdate(`{model.Name}`);"
                            });
                        }
                        return Json(new JsonClientResult()
                        {
                            CallBack = $"Common_another.showErr(``)"
                        });
                    }
                    return Json(new JsonClientResult()
                    {
                        CallBack = "Common_another.showErr(`id can not null`)"
                    }, JsonRequestBehavior.AllowGet);
                }
                return Json(new JsonClientResult()
                {
                    CallBack = $"Common_another.showErr(`{GetModelStateError()}`)"
                }, JsonRequestBehavior.AllowGet);
            }
            return Json(new JsonClientResult()
            {
                CallBack = $"Common_another.showErr(`You do not have permission`)"
            }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region destination 
        public ActionResult LibDestination()
        {
            return View();
        }

        public JsonResult LibDestinationGetByPage(LibDestinationViewModel model)
        {
            if (User.CheckPermission(Permission.SettingView))
            {
                var request = new LibDestinationRequest();
                request.PageIndex = model.page;
                request.PageSize = model.perpage;
                request.FieldName = model.field;
                request.Orderby = model.sort;
                if (model.key.IsTrueOrFalse())
                {
                    request.Keyword = model.key;
                }
                if (model.IsHome != null)
                {
                    request.Filter.Add(new FilterRequest()
                    {
                        Name = "isHome",
                        Values = model.IsHome.ToString(),
                        Opt = FilterOptionOpt.AND,
                        Opt1 = FilterOptionOpt1.EqualTo,
                        Type = FilterOptionType.Boolean
                    });
                }
                var result = _libServices.DestinationGetByPage(request, User.Identity.GetClaims("token"));
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
            }
            return Json(new JsonDataTable
            {
                meta = new PagingInfo()
                {
                    page = model.page,
                    perpage = model.perpage,
                },
                data = new List<LibDestinationResponse>()
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult LibDestinationUpdateModal(Guid id)
        {
            if (User.CheckPermission(Permission.SettingEdit))
            {
                if (id != Guid.Empty)
                {
                    var result = _libServices.DestinationGetById(id, User.Identity.GetClaims("token"));
                    if (result.IsOK())
                    {
                        return PartialView("Partial/_DestinationUpdateModal", result.Data);
                    }
                    return PartialView("~/Views/Shared/_ErrorViewModal.cshtml", new ErrorViewModel() { Title = result.Errors.GetResultError() });
                }
                return PartialView("~/Views/Shared/_ErrorViewModal.cshtml", new ErrorViewModel()
                {
                    Title = $"Id can not be empty.",
                    Description = $"Id: {id}"
                });
            }
            return PartialView("~/Views/Shared/_ErrorViewModal.cshtml", new ErrorViewModel()
            {
                Title = "You do not have permission"
            });
        }

        [HttpPost]
        public JsonResult LibDestinationUpdate(LibDestinationUpdateViewModel model)
        {
            if (User.CheckPermission(Permission.SettingEdit))
            {
                if (ModelState.IsValid)
                {
                    var request = Mapper.Map<LibDestinationUpdateRequest>(model);
                    var result = _libServices.DestinationUpdate(request, User.Identity.GetClaims("token"));
                    if (result.IsOK())
                    {
                        return Json(new JsonClientResult()
                        {
                            Success = true,
                            CallBack = "destination.onRendAfterUpdate();"
                        });
                    }
                    return Json(new JsonClientResult()
                    {
                        CallBack = $"Common_another.showErr(`{result.Errors.GetResultError()}`);"
                    });
                }
                return Json(new JsonClientResult()
                {
                    CallBack = $"Common_another.showErr(`{GetModelStateError()}`);"
                });
            }
            return Json(new JsonClientResult()
            {
                CallBack = "You do not have permission"
            });
        }
        #endregion
    }
}