using AutoMapper;
using DigiticketCMS.Helpers;
using DigiticketCMS.Models;
using DigiticketCMS.Models.CategoryTag;
using DigiticketCMS.Services.Models;
using Infrastructure.Config;
using Infrastructure.Extensions;
using Newtonsoft.Json.Serialization;
using Services.Implement;
using Services.Interfaces;
using Services.Models.Requests.CategoryTag;
using Services.Models.Requests.Tags;
using Services.Models.Responses.CategoryTag;
using Services.Models.Responses.Tags;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;
using System.Web.Mvc;

namespace DigiticketCMS.Controllers
{
    public class CategoryTagController : BaseController
    {
        #region constructor
        private ICategoryTagService _categoryTag;
        public CategoryTagController(ICategoryTagService iCategoryTagService)
        {
            _categoryTag = iCategoryTagService;
        }
        #endregion
        // GET: CategoryTag
        public ActionResult CategoryTag(CategoryTagFromClient model)
        {
            if (User.CheckPermission(Permission.CategoryTag))
            {
                var resultToClient = new CategoryTagToGetByPageViewModel();
                if (model.ParentId != 0)
                {
                    resultToClient.ParentId = model.ParentId;
                    resultToClient.ParentName = model.ParentName;
                    resultToClient.Round = model.Round + 1;
                }
                else
                {
                    resultToClient.ParentId = 0;
                    resultToClient.Round = 1;
                }
                return View(resultToClient);
            }
            return RedirectToAction("Index", "Home");
        }

        public ActionResult CategoryTagGetByPage(CategoryGetByPageViewModel model)
        {
            if (User.CheckPermission(Permission.CategoryTagView))
            {
                if (model.key == null) model.key = "";
                if (model.key.CheckForKey())
                {
                    var request = Mapper.Map<CategoryTagGetByPageRequest>(model);
                    request.PageIndex = model.page;
                    request.PageSize = model.perpage;
                    request.FieldName = model.field;
                    request.Filter = new List<FilterRequest>();
                    request.Orderby = model.sort;
                    if (model.key != null) request.Keyword = model.key;
                    if (model.ParentId != null)
                    {
                        request.Filter.Add(new FilterRequest()
                        {
                            Opt = FilterOptionOpt.AND,
                            Name = "TagParentId",
                            Opt1 = FilterOptionOpt1.EqualTo,
                            Type = FilterOptionType.Number,
                            Values = model.ParentId.ToString()
                        });
                    }

                    if (model.IsHome != null)
                    {
                        request.Filter.Add(new FilterRequest()
                        {
                            Opt = FilterOptionOpt.AND,
                            Name = "IsHome",
                            Opt1 = FilterOptionOpt1.EqualTo,
                            Type = FilterOptionType.String,
                            Values = model.IsHome.ToString()
                        });
                    }
                    if (model.IsHot != null)
                    {
                        request.Filter.Add(new FilterRequest()
                        {
                            Opt = FilterOptionOpt.AND,
                            Name = "IsHot",
                            Opt1 = FilterOptionOpt1.EqualTo,
                            Type = FilterOptionType.String,
                            Values = model.IsHot.ToString()
                        });
                    }

                    var result = _categoryTag.GetByPage(request, User.Identity.GetClaims("token"));
                    if (result.IsOK())
                    {
                        long totalItems = result.Data.Total;
                        var totalPages = Math.Ceiling((double)totalItems / model.perpage);
                        return Json(new JsonDataTable
                        {
                            meta = new PagingInfo
                            {
                                page = model.page,
                                pages = (long)totalPages,
                                perpage = model.perpage,
                                total = totalItems,
                                field = model.field
                            },
                            data = result.Data.Result,
                            success = true
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
                    data = new List<CategoryTagResponse>()
                }, JsonRequestBehavior.AllowGet);
            }
            return Json(new JsonDataTable { });
        }

        public ActionResult CategoryTagAddOrUpdateModal(int? id, int? parentId, string ParentName, int? round)
        {
            if (id == 0 || id == null)
            {
                if (User.CheckPermission(Permission.CategoryTagAdd))
                {
                    return PartialView("~/Views/CategoryTag/Partial/CategoryTagAddOrUpdateModal.cshtml", new CategoryTagDataToAddOrUpdateViewModal()
                    {
                        ParentId = (int)parentId,
                        ParentName = ParentName,
                        Round = (int)round
                    });
                }
            }
            else
            {
                if (User.CheckPermission(Permission.CategoryTagEdit))
                {
                    var resultToClient = new CategoryTagDataToAddOrUpdateViewModal();
                    var result = _categoryTag.GetById((int)id, User.Identity.GetClaims("token"));
                    if (result.IsOK())
                    {
                        resultToClient.CategoryTagResponse = result.Data;
                        return PartialView("~/Views/CategoryTag/Partial/CategoryTagAddOrUpdateModal.cshtml", resultToClient);
                    }
                }
            }
            return PartialView("");
        }

        public JsonResult CategoryTagAdd(CategoryTagAddViewModel model)
        {
            if (User.CheckPermission(Permission.CategoryTagAdd))
            {
                if (ModelState.IsValid)
                {
                    var request = Mapper.Map<CategoryTagAddRequest>(model);
                    var result = _categoryTag.Add(request, User.Identity.GetClaims("token"));
                    if (result.IsOK())
                    {
                        return Json(new JsonClientResult
                        {
                            Success = true,
                            CallBack = "categoryTag.onRendAfterUpdated('Thêm mới thành công');"
                        }, JsonRequestBehavior.AllowGet);
                    }
                    return Json(new JsonClientResult
                    {
                        CallBack = result.Errors.CB_UpdateErrorResult()
                    }, JsonRequestBehavior.AllowGet);
                }
                return Json(new JsonClientResult
                {
                    CallBack = $"Common.onError(`{GetModelStateError()}`);"
                }, JsonRequestBehavior.AllowGet);
            }
            return Json(new JsonClientResult
            {
                CallBack = $"Common.onError(`You do not have permission`);"
            }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult CategoryTagEdit(CategoryTagEditRequestViewModel model)
        {
            if (User.CheckPermission(Permission.CategoryTagEdit))
            {
                if (ModelState.IsValid)
                {
                    var request = Mapper.Map<CategoryTagEditRequest>(model);
                    var result = _categoryTag.Edit(request, User.Identity.GetClaims("token"));
                    if (result.IsOK())
                    {
                        return Json(new JsonClientResult
                        {
                            Success = true,
                            CallBack = "categoryTag.onRendAfterUpdated('');"
                        }, JsonRequestBehavior.AllowGet);
                    }
                    return Json(new JsonClientResult
                    {
                        CallBack = result.Errors.CB_UpdateErrorResult()
                    }, JsonRequestBehavior.AllowGet);
                }
                return Json(new JsonClientResult
                {
                    CallBack = $"Common.onError(`{GetModelStateError()}`);"
                }, JsonRequestBehavior.AllowGet);
            }
            return Json(new JsonClientResult
            {
                CallBack = $"Common.onError(`You do not have permission`);"
            }, JsonRequestBehavior.AllowGet);
        }

        #region IsHome
        public ActionResult CategoryTagIsHome(CategoryTagFromClient model)
        {
            if (User.CheckPermission(Permission.CategoryTag))
            {
                var resultToClient = new CategoryTagToGetByPageViewModel();
                if (model.ParentId != 0)
                {
                    resultToClient.ParentId = model.ParentId;
                    resultToClient.ParentName = model.ParentName;
                    resultToClient.Round = model.Round + 1;
                }
                else
                {
                    resultToClient.ParentId = 0;
                    resultToClient.Round = 1;
                }
                return View(resultToClient);
            }
            return RedirectToAction("Index", "Home");
        }
        public ActionResult CategoryTagIsHomeAddOrUpdateModal()
        {

            if (User.CheckPermission(Permission.CategoryTagAdd))
            {
                return PartialView("~/Views/CategoryTag/Partial/IsHomeAdd.cshtml");
            }
            return Json(new ErrorViewModel());
        }
        public JsonResult CategoryTagUpdateHome(CategoryTagUpdateHomeViewModel model)
        {
            if (User.CheckPermission(Permission.CategoryTagEdit))
            {
                if (ModelState.IsValid)
                {
                    var request = Mapper.Map<CategoryTagUpdateHomeRequest>(model);
                    var result = _categoryTag.UpdateHome(request, User.Identity.GetClaims("token"));
                    if (result.IsOK())
                    {
                        var mess = "";
                        if(model.IsHome)
                        {
                            mess = "Thêm mới chuyên mục is home thành công!";
                        }
                        else
                        {
                            mess = "Bỏ chuyên mục is home thành công!";
                        }
                        return Json(new JsonClientResult()
                        {
                            Success = true,
                            CallBack = $"categoryTag.onRendAfterUpdated(`{mess}`);"
                        }, JsonRequestBehavior.AllowGet);
                    }
                    return Json(new JsonClientResult
                    {
                        CallBack = result.Errors.CB_UpdateErrorResult()
                    }, JsonRequestBehavior.AllowGet);
                }
                return Json(new JsonClientResult()
                {
                    CallBack = $"Common.onError(`{GetModelStateError()}`, true)"
                }, JsonRequestBehavior.AllowGet);
            }
            return Json(new ErrorViewModel(), JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region is hot
        public ActionResult CategoryTagIsHot(CategoryTagFromClient model)
        {
            if (User.CheckPermission(Permission.CategoryTag))
            {
                var resultToClient = new CategoryTagToGetByPageViewModel();
                if (model.ParentId != 0)
                {
                    resultToClient.ParentId = model.ParentId;
                    resultToClient.ParentName = model.ParentName;
                    resultToClient.Round = model.Round + 1;
                }
                else
                {
                    resultToClient.ParentId = 0;
                    resultToClient.Round = 1;
                }
                return View(resultToClient);
            }
            return RedirectToAction("Index", "Home");
        }
        public ActionResult CategoryTagIsHotAddOrUpdateModal()
        {

            if (User.CheckPermission(Permission.CategoryTagAdd))
            {
                return PartialView("~/Views/CategoryTag/Partial/IsHotAdd.cshtml");
            }
            return Json(new ErrorViewModel());
        }
        public JsonResult CategoryTagUpdateHot(CategoryTagUpdateHotViewModel model)
        {
            if (User.CheckPermission(Permission.CategoryTagEdit))
            {
                if (ModelState.IsValid)
                {
                    var request = Mapper.Map<CategoryTagUpdateHotRequest>(model);
                    var result = _categoryTag.UpdateHot(request, User.Identity.GetClaims("token"));
                    if (result.IsOK())
                    {
                        var mess = "";
                        if (model.IsHot)
                        {
                            mess = "Thêm mới chuyên mục is hot thành công!";
                        }
                        else
                        {
                            mess = "Bỏ chuyên mục is hot thành công!";
                        }
                        return Json(new JsonClientResult()
                        {
                            Success = true,
                            CallBack = $"categoryTag.onRendAfterUpdated(`{mess}`);"
                        }, JsonRequestBehavior.AllowGet);
                    }
                    return Json(new JsonClientResult
                    {
                        CallBack = result.Errors.CB_UpdateErrorResult()
                    }, JsonRequestBehavior.AllowGet);
                }
                return Json(new JsonClientResult()
                {
                    CallBack = $"Common.onError(`{GetModelStateError()}`, true)"
                }, JsonRequestBehavior.AllowGet);
            }
            return Json(new ErrorViewModel(), JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}