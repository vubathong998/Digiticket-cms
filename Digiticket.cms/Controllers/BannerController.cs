using AutoMapper;
using DigiticketCMS.Helpers;
using DigiticketCMS.Models;
using DigiticketCMS.Models.Banner;
using DigiticketCMS.Services.Models;
using Infrastructure.Config;
using Infrastructure.Extensions;
using Services.Interfaces;
using Services.Models.Requests.Banner;
using Services.Models.Responses.Banner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace DigiticketCMS.Controllers
{
    [Authorize]
    public class BannerController : BaseController
    {
        #region constructor
        private ILibServices _libServices;
        private IBannerServices _bannerServices;

        public BannerController(ILibServices libServices,
                                IBannerServices bannerServices)
        {
            _libServices = libServices;
            _bannerServices = bannerServices;
        }
        #endregion

        #region get by page
        [HttpGet]
        public ActionResult Banner()
        {
            if (User.CheckPermission(Permission.Setting))
            {
                var model = new BannerLibResponse();
                model.Categories = _libServices.CategoryGetAll(User.Identity.GetClaims("token")).Data;
                model.Destinations = _libServices.DestinationGetAll(User.Identity.GetClaims("token")).Data;
                return View(model);
            }
            return RedirectToAction("Index", "Home");
        }
        [HttpGet]
        public JsonResult GetByPage(BannerViewModel model)
        {
            if (User.CheckPermission(Permission.SettingView))
            {
                var request = Mapper.Map<BannerRequest>(model);
                request.PageIndex = model.page;
                request.PageSize = model.perpage;
                request.FieldName = model.field;
                request.Orderby = model.sort;

                if (model.CategoryId.IsTrueOrFalse())
                {
                    request.Filter.Add(new FilterRequest()
                    {
                        Opt = FilterOptionOpt.AND,
                        Name = "categoryId",
                        Opt1 = FilterOptionOpt1.EqualTo,
                        Type = FilterOptionType.String,
                        Values = model.CategoryId
                    });
                }
                if (model.DestinationId.IsTrueOrFalse())
                {
                    request.Filter.Add(new FilterRequest()
                    {
                        Opt = FilterOptionOpt.AND,
                        Name = "destinationId",
                        Opt1 = FilterOptionOpt1.EqualTo,
                        Type = FilterOptionType.String,
                        Values = model.DestinationId
                    });
                }
                if (model.Status == 1 || model.Status == 0)
                {
                    request.Filter.Add(new FilterRequest()
                    {
                        Opt = FilterOptionOpt.AND,
                        Name = "status",
                        Opt1 = FilterOptionOpt1.EqualTo,
                        Type = FilterOptionType.Number,
                        Values = model.Status.ToString()
                    });
                }
                if (model.Type != null)
                {
                    request.Filter.Add(new FilterRequest()
                    {
                        Opt = FilterOptionOpt.AND,
                        Name = "type",
                        Opt1 = FilterOptionOpt1.EqualTo,
                        Type = FilterOptionType.Number,
                        Values = model.Type.ToString()
                    });
                }
                if (model.key.IsTrueOrFalse()) request.Keyword = model.key;
                var result = _bannerServices.GetByPage(request, User.Identity.GetClaims("token"));
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
                data = new List<BannerResponse>()
            }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region add or edit
        public ActionResult BannerAddOrEditModal(int? id)
        {
            if (User.CheckPermission(Permission.SettingEdit))
            {
                if (!id.IsTrueOrFalse())
                {
                    var result = new BannerResponse();
                    result.Categories = _libServices.CategoryGetAll(User.Identity.GetClaims("token")).Data;
                    result.Destinations = _libServices.DestinationGetAll(User.Identity.GetClaims("token")).Data;
                    return PartialView("Partial/_bannerAddOfEditModal", result);
                }
                else
                {
                    if (User.CheckPermission(Permission.SettingEdit))
                    {
                        var result = _bannerServices.GetById((int)id, User.Identity.GetClaims("token"));
                        if (result.IsOK())
                        {
                            return PartialView("Partial/_bannerAddOfEditModal", result.Data);
                        }
                        return PartialView("~/Views/Shared/_ErrorViewModal.cshtml", new ErrorViewModel() { Title = result.Errors.GetResultError() });
                    }
                    return PartialView("~/Views/Shared/_ErrorViewModal.cshtml", new ErrorViewModel()
                    {
                        Title = "You do not have permission!"
                    });
                }
            }
            return PartialView("~/Views/Shared/_ErrorViewModal.cshtml", new ErrorViewModel()
            {
                Title = "You do not have permission!"
            });
        }

        [HttpPost]
        public JsonResult BannerAdd(BannerAddViewModel model)
        {
            if (User.CheckPermission(Permission.SettingEdit))
            {
                if (ModelState.IsValid)
                {
                    var request = Mapper.Map<BannerAddRequest>(model);
                    var result = _bannerServices.Add(request, User.Identity.GetClaims("token"));
                    if (result.IsOK())
                    {
                        return Json(new JsonClientResult
                        {
                            Success = true,
                            CallBack = "Banner.onRendAfterUpdateBanner('Thêm mới thành công');"
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
                CallBack = $"Common.onError('You do not have permission!');"
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult BannerEdit(BannerEditViewModel model)
        {
            if (User.CheckPermission(Permission.SettingEdit))
            {
                if (ModelState.IsValid)
                {
                    var request = Mapper.Map<BannerEditRequest>(model);
                    var result = _bannerServices.Edit(request, User.Identity.GetClaims("token"));
                    if (result.IsOK())
                    {
                        return Json(new JsonClientResult
                        {
                            Success = true,
                            CallBack = "Banner.onRendAfterUpdateBanner('Sửa thành thành công');"
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
                CallBack = $"swal.fire('Oop!', `You do not have permission`, 'error');"
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult BannerDelete(int id)
        {
            if (User.CheckPermission(Permission.SettingEdit))
            {
                if (id > 0)
                {
                    var result = _bannerServices.Delete(id, User.Identity.GetClaims("token"));
                    if (result.IsOK())
                    {
                        return Json(new JsonClientResult
                        {
                            Success = true,
                            CallBack = "Banner.onRendAfterUpdateBanner('Xóa thành thành công');"
                        }, JsonRequestBehavior.AllowGet);
                    }
                    return Json(new JsonClientResult
                    {
                        CallBack = result.Errors.CB_UpdateErrorResult()
                    }, JsonRequestBehavior.AllowGet);
                }
                return Json(new JsonClientResult
                {
                    CallBack = $"Common.onError(`Không thành công. id is invalid. id = {id}`);"
                }, JsonRequestBehavior.AllowGet);
            }
            return Json(new JsonClientResult
            {
                CallBack = $"swal.fire('Oop!', `You do not have permission`, 'error');"
            }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult BannerToggleView(int id)
        {
            if (User.CheckPermission(Permission.SettingEdit))
            {
                if (id >= 0)
                {
                    var result = _bannerServices.ToggleView(id, User.Identity.GetClaims("token"));
                    if (result.IsOK())
                    {
                        return Json(new JsonClientResult()
                        {
                            Success = true,
                            CallBack = "Banner.onRendAfterUpdateBanner('Toggle thành thành công');"
                        }, JsonRequestBehavior.AllowGet);
                    }
                    return Json(new JsonClientResult
                    {
                        CallBack = result.Errors.CB_UpdateErrorResult()
                    }, JsonRequestBehavior.AllowGet);
                }
                return Json(new JsonClientResult
                {
                    CallBack = $"Common.onError(`Không thành công. id is invalid. id = {id}`);"
                }, JsonRequestBehavior.AllowGet);
            }
            return Json(new JsonClientResult
            {
                CallBack = $"swal.fire('Oop!', `You do not have permission`, 'error');"
            }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region type get by page
        [HttpGet]
        public ActionResult BannerType()
        {
            if (User.CheckPermission(Permission.Setting))
            {
                var model = new BannerLibResponse();
                model.Categories = _libServices.CategoryGetAll(User.Identity.GetClaims("token")).Data;
                model.Destinations = _libServices.DestinationGetAll(User.Identity.GetClaims("token")).Data;
                return View(model);
            }
            return RedirectToAction("Index", "Home");
        }
        [HttpGet]
        public JsonResult GetByPageType(BannerTypeViewModel model)
        {
            if (User.CheckPermission(Permission.SettingView))
            {
                var request = Mapper.Map<BannerTypeRequest>(model);
                request.PageIndex = model.page;
                request.PageSize = model.perpage;
                request.FieldName = model.field;
                request.Orderby = model.sort;

                if (model.CategoryId.IsTrueOrFalseButGuidEmpty())
                {
                    request.Filter.Add(new FilterRequest()
                    {
                        Opt = FilterOptionOpt.AND,
                        Name = "categoryId",
                        Opt1 = FilterOptionOpt1.EqualTo,
                        Type = FilterOptionType.String,
                        Values = model.CategoryId
                    });
                }
                if (model.DestinationId.IsTrueOrFalseButGuidEmpty())
                {
                    request.Filter.Add(new FilterRequest()
                    {
                        Opt = FilterOptionOpt.AND,
                        Name = "destinationId",
                        Opt1 = FilterOptionOpt1.EqualTo,
                        Type = FilterOptionType.String,
                        Values = model.DestinationId
                    });
                }
                if (model.Status.IsTrueOrFalse())
                {
                    request.Filter.Add(new FilterRequest()
                    {
                        Opt = FilterOptionOpt.AND,
                        Name = "status",
                        Opt1 = FilterOptionOpt1.EqualTo,
                        Type = FilterOptionType.Number,
                        Values = model.Status.ToString()
                    });
                }
                if (model.key == null) model.key = "";
                if (model.key.CheckForKey())
                {
                    request.Keyword = model.key;
                    var result = _bannerServices.TypeGetByPage(request, User.Identity.GetClaims("token"));
                    if (result.IsOK())
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
            }
            return Json(new JsonDataTable
            {
                meta = null,
                data = new List<BannerResponse>()
            }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region add or edit type
        public ActionResult BannerTypeAddOrEditModal(int? id)
        {

            if (User.CheckPermission(Permission.SettingEdit))
            {
                if (!id.IsTrueOrFalse())
                {
                    var result = new BannerTypeResponse();
                    result.Categories = _libServices.CategoryGetAll(User.Identity.GetClaims("token")).Data;
                    result.Destinations = _libServices.DestinationGetAll(User.Identity.GetClaims("token")).Data;
                    return PartialView("Partial/_bannerTypeAddOfEditModal", result);
                }
                else
                {
                    if (User.CheckPermission(Permission.SettingEdit))
                    {
                        var result = _bannerServices.TypeGetById((int)id, User.Identity.GetClaims("token"));
                        if (result.IsOK())
                        {
                            return PartialView("Partial/_bannerTypeAddOfEditModal", result.Data);
                        }
                        return PartialView("~/Views/Shared/_ErrorViewModal.cshtml", new ErrorViewModel() { Title = result.Errors.GetResultError() });
                    }
                    return PartialView("~/Views/Shared/_ErrorViewModal.cshtml", new ErrorViewModel() { Title = "You do not have permission" });
                }
            }
            return PartialView("~/Views/Shared/_ErrorViewModal.cshtml", new ErrorViewModel()
            {
                Title = "You do not have permission!"
            });
        }
        [HttpPost]
        public JsonResult BannerTypeAdd(BannerTypeAddViewModel model)
        {

            if (User.CheckPermission(Permission.SettingEdit))
            {
                if (ModelState.IsValid)
                {
                    var request = Mapper.Map<BannerTypeAddRequest>(model);
                    var result = _bannerServices.TypeAdd(request, User.Identity.GetClaims("token"));
                    if (result.IsOK())
                    {
                        return Json(new JsonClientResult
                        {
                            Success = true,
                            CallBack = "Banner.onRendAfterBannerType('Thêm mới thành công');"
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
                CallBack = $"Common.onError('You do not have permission!');"
            }, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public JsonResult BannerTypeEdit(BannerTypeEditViewModel model)
        {
            if (User.CheckPermission(Permission.SettingEdit))
            {
                if (ModelState.IsValid)
                {
                    var request = Mapper.Map<BannerTypeEditRequest>(model);
                    var result = _bannerServices.TypeEdit(request, User.Identity.GetClaims("token"));
                    if (result.IsOK())
                    {
                        return Json(new JsonClientResult
                        {
                            Success = true,
                            CallBack = "Banner.onRendAfterBannerType('Sửa thành thành công');"
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
                CallBack = $"Common.onError('You do not have permission!');"
            }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult BannerTypeDelete(int id)
        {
            if (User.CheckPermission(Permission.SettingEdit))
            {
                if (id > 0)
                {
                    var result = _bannerServices.TypeDelete(id, User.Identity.GetClaims("token"));
                    if (result.IsOK())
                    {
                        return Json(new JsonClientResult
                        {
                            Success = true,
                            CallBack = "Banner.onRendAfterBannerType('Xóa thành thành công');"
                        }, JsonRequestBehavior.AllowGet);
                    }
                    return Json(new JsonClientResult
                    {
                        CallBack = result.Errors.CB_UpdateErrorResult()
                    }, JsonRequestBehavior.AllowGet);
                }
                return Json(new JsonClientResult
                {
                    CallBack = $"Common.onError(`Không thành công. id is not valid. id = {id}`);"
                }, JsonRequestBehavior.AllowGet);
            }
            return Json(new JsonClientResult
            {
                CallBack = $"swal.fire('Oop!', `You do not have permission`, 'error');"
            }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region suggest banner type
        public ActionResult SuggestBannerType(string categoryId, string destinationId, string key = "")
        {
            if (key.CheckForKey())
            {
                var request = new BannerTypeRequest()
                {
                    Keyword = key,
                    PageIndex = 1,
                    PageSize = 20,
                    Orderby = "createddate",
                    FieldName = "desc"
                };
                if (categoryId.IsTrueOrFalseButGuidEmpty())
                {
                    request.Filter.Add(new FilterRequest()
                    {
                        Opt = FilterOptionOpt.AND,
                        Name = "categoryId",
                        Opt1 = FilterOptionOpt1.EqualTo,
                        Type = FilterOptionType.String,
                        Values = categoryId
                    });
                }
                if (destinationId.IsTrueOrFalseButGuidEmpty())
                {
                    request.Filter.Add(new FilterRequest()
                    {
                        Opt = FilterOptionOpt.AND,
                        Name = "destinationId",
                        Opt1 = FilterOptionOpt1.EqualTo,
                        Type = FilterOptionType.String,
                        Values = destinationId
                    });
                }
                var result = _bannerServices.TypeGetByPage(request, User.Identity.GetClaims("token"));
                if (result.IsOK())
                {
                    return Json(new JsonClientResult()
                    {
                        Success = true,
                        Data = result.Data.Result,
                    }, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(new JsonClientResult()
            {
                Success = true,
                Data = new List<BannerTypeRequest>()
            }, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}