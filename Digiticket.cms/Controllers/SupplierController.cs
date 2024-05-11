using AutoMapper;
using DigiticketCMS.Helpers;
using DigiticketCMS.Models;
using DigiticketCMS.Models.Supplier;
using DigiticketCMS.Services.Models;
using Infrastructure.Config;
using Infrastructure.Extensions;
using Services.Implement;
using Services.Interfaces;
using Services.Models.Requests.Supplier;
using Services.Models.Responses.Supplier;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace DigiticketCMS.Controllers
{
    [Authorize]
    public class SupplierController : BaseController
    {
        #region constructor
        private ISupplierServices _supplierServices;
        private ILibServices _libServices;
        public SupplierController(ISupplierServices supplierServices,
                                 ILibServices libServices)
        {
            _supplierServices = supplierServices;
            _libServices = libServices;
        }
        #endregion

        #region get by page
        public ActionResult Supplier()
        {
            if (User.CheckPermission(Permission.Suppliers))
            {
                return View();
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public ActionResult SupplierGetByPage(Supplier model)
        {
            if (User.CheckPermission(Permission.SuppliersView))
            {
                var request = new SupplierGetByPageRequest()
                {
                    PageIndex = model.page,
                    PageSize = model.perpage,
                    FieldName = model.field,
                    Orderby = model.sort,
                    Keyword = string.IsNullOrEmpty(model.key) ? "" : model.key
                };
                var result = _supplierServices.GetByPage(request, User.Identity.GetClaims("token"));
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
                            total = totalItems
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
                    data = new List<SupplierResponse>()
                }, JsonRequestBehavior.AllowGet);
            }
            return Json(new JsonDataTable
            {
                meta = null,
                data = new List<SupplierResponse>()
            }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region add
        [HttpGet]
        public ActionResult Add()
        {
            if (User.CheckPermission(Permission.SuppliersAdd))
            {
                return View();
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public JsonResult Add(SupplierCreateViewModel model)
        {
            if (User.CheckPermission(Permission.SuppliersAdd))
            {
                if (ModelState.IsValid)
                {
                    var request = Mapper.Map<SupplierRequest>(model);
                    var result = _supplierServices.Add(request, User.Identity.GetClaims("token"));
                    if (result.IsOK())
                    {
                        return Json(new JsonClientResult
                        {
                            Success = true,
                            CallBack = $"CreateOrUpdate.onRendAfterAdd(`{result.Data}`);"
                        }, JsonRequestBehavior.AllowGet);
                    }
                    return Json(new JsonClientResult
                    {
                        CallBack = result.Errors.CB_UpdateErrorResult()
                    }, JsonRequestBehavior.AllowGet);
                }
                return Json(new JsonClientResult
                {
                    Success = false,
                    Message = GetModelStateError(),
                    CallBack = $"Common.onError('Không thêm được! Có lỗi xảy ra! {GetModelStateError()}')"
                }, JsonRequestBehavior.AllowGet);
            }
            return Json(new JsonClientResult
            {
                Success = false,
                Message = GetModelStateError(),
                CallBack = $" swal.fire('Oops!', 'You have no permission for this action!', 'error');"
            }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region detail 
        [HttpGet]
        public ActionResult SupplierDetail(Guid id)
        {
            if (User.CheckPermission(Permission.Suppliers))
            {
                if (id != Guid.Empty)
                {
                    var result = _supplierServices.GetById(id, User.Identity.GetClaims("token"));
                    if (result.IsOK())
                    {
                        var model = Mapper.Map<SupplierViewModel>(result.Data);
                        model.Categories = _libServices.CategoryGetAll(User.Identity.GetClaims("token")).Data;
                        model.Destinations = _libServices.DestinationGetAll(User.Identity.GetClaims("token")).Data;
                        return View(model);
                    }
                }
            }
            return RedirectToAction("Index", "Home");
        }
        #endregion

        #region update tenant
        [HttpGet]
        public ActionResult SupplierUpdatenantModal(SupplierUpdateTenantViewModel model)
        {
            if (User.CheckPermission(Permission.SuppliersImportProduct))
            {
                if (ModelState.IsValid)
                {
                    var result = Mapper.Map<SupplierUpdateTenantRequest>(model);
                    return PartialView("Partial/_SupplierUpdateTenantModal", result);
                }
            }
            return View("~/Views/Shared/Error.cshtml");
        }

        [HttpPost]
        public JsonResult SupplierUpdateTenant(SupplierUpdateTenantViewModel model)
        {
            if (ModelState.IsValid)
            {
                var request = Mapper.Map<SupplierUpdateTenantRequest>(model);
                var result = _supplierServices.UpdateTenant(request, User.Identity.GetClaims("token"));
                if (result.IsOK())
                {
                    return Json(new JsonClientResult
                    {
                        CallBack = "SupplierDetail.onRendAfterGetListByTenantFromDigipost();",
                        Success = true
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

        [HttpGet]
        public JsonResult SuggestTenantByKey(string key = "")
        {
            var result = new List<SupplierTenantResponse>();
            if (key.CheckForKey())
            {
                var model = _supplierServices.SupplierTenantGetByKey(key, User.Identity.GetClaims("token"));
                if (model.IsOK() && model.Data != null)
                {
                    result = model.Data.ToList();
                }
            }
            return Json(new JsonClientResult
            {
                Success = true,
                Data = result
            }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region update base info
        public JsonResult SupplierUpdateBaseInfo(SupplierUpdateBaseInfoViewModel model)
        {
            if (User.CheckPermission(Permission.SuppliersEdit))
            {
                if (ModelState.IsValid)
                {
                    var request = Mapper.Map<SupplierRequest>(model);
                    var result = _supplierServices.UpdateBaseInfo(request, User.Identity.GetClaims("token"));
                    if (result.IsOK())
                    {
                        return Json(new JsonClientResult
                        {
                            CallBack = $"SupplierDetail.onRendAfterUpdateName(`{model.Name}`);",
                            Success = true
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
        #endregion

        #region update booking info
        public JsonResult SupplierUpdateBookingInfo(SupplierUpdateBookingInfoViewModel model)
        {
            if (User.CheckPermission(Permission.SuppliersEdit))
            {
                if (ModelState.IsValid)
                {
                    var request = Mapper.Map<SupplierRequest>(model);
                    var result = _supplierServices.UpdateBookingInfo(request, User.Identity.GetClaims("token"));
                    if (result.IsOK())
                    {
                        return Json(new JsonClientResult
                        {
                            CallBack = "Common.onSuccess();",
                            Success = true
                        }, JsonRequestBehavior.AllowGet);
                    }
                    return Json(new JsonClientResult
                    {
                        CallBack = result.Errors.CB_UpdateErrorResult()
                    }, JsonRequestBehavior.AllowGet);
                }
                return Json(new JsonClientResult
                {
                    CallBack = $"swal.fire('Oop!', `Đã có lỗi xảy ra: {GetModelStateError()}`, 'error');"
                }, JsonRequestBehavior.AllowGet);
            }
            return Json(new JsonClientResult
            {
                CallBack = $"swal.fire('Error!', `You have no permission!`, 'error');"
            }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region get bank
        public ActionResult SupplierGetBank(Guid id)
        {
            if (User.CheckPermission(Permission.SuppliersView))
            {
                if (id != Guid.Empty)
                {
                    var result = _supplierServices.GetSupplierBank(id, User.Identity.GetClaims("token"));
                    return Json(new JsonDataTable
                    {
                        data = result.Data,
                    }, JsonRequestBehavior.AllowGet);
                }
                return Json(new JsonDataTable
                {
                    meta = null,
                    data = new List<SupplierBankResponse>()
                }, JsonRequestBehavior.AllowGet);
            }
            return Json(new JsonDataTable
            {
                meta = null,
                data = new List<SupplierBankResponse>()
            }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region create, update and delete bank
        [HttpGet]
        public ActionResult SupplierAddOrUpdateBankModal(SupplierUpdateBankRequest model)
        {
            if (ModelState.IsValid)
            {
                var HTML = RenderViewToString("Partial/_SupplierAddOrUpdateBankModal", model);
                return Json(new JsonClientResult
                {
                    Success = true,
                    Data = HTML
                }, JsonRequestBehavior.AllowGet);
            }
            return Json(new JsonClientResult
            {
                Message = GetModelStateError(),
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SupplierCreateOrUpdateBank(SupplierCreateBankViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.Id == null)
                {
                    if (User.CheckPermission(Permission.SuppliersAdd))
                    {
                        var request = Mapper.Map<SupplierBankRequest>(model);
                        var result = _supplierServices.CreateSupplierBank(request, User.Identity.GetClaims("token"));
                        if (result.IsOK())
                        {
                            return Json(new JsonClientResult
                            {
                                Success = true,
                                CallBack = "SupplierDetail.onRendAfterUpdateSupplierBank();"
                            }, JsonRequestBehavior.AllowGet);
                        }
                        return Json(new JsonClientResult
                        {
                            CallBack = result.Errors.CB_UpdateErrorResult()
                        }, JsonRequestBehavior.AllowGet);
                    }
                    return Json(new JsonClientResult
                    {
                        CallBack = $"Common.onError('You do not have permission!');"
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    if (User.CheckPermission(Permission.SuppliersEdit))
                    {
                        var request = Mapper.Map<SupplierBankRequest>(model);
                        var result = _supplierServices.UpdateSupplierBank(request, User.Identity.GetClaims("token"));
                        if (result.IsOK())
                        {
                            return Json(new JsonClientResult
                            {
                                Success = true,
                                CallBack = "Common.onSuccess();"
                            }, JsonRequestBehavior.AllowGet);
                        }
                        return Json(new JsonClientResult
                        {
                            CallBack = result.Errors.CB_UpdateErrorResult()
                        }, JsonRequestBehavior.AllowGet);
                    }
                    return Json(new JsonClientResult
                    {
                        CallBack = $"Common.onError('You do not have permission!');"
                    }, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(new JsonClientResult
            {
                CallBack = $"Common.onError(`{GetModelStateError()}`);"
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult SupplierDeleteBank(SupplierDeleteBankViewModel model)
        {
            if (User.CheckPermission(Permission.SuppliersEdit))
            {
                if (ModelState.IsValid)
                {
                    var request = Mapper.Map<SupplierBankRequest>(model);
                    var result = _supplierServices.DeleteSupplierBank(request, User.Identity.GetClaims("token"));
                    if (result.IsOK())
                    {
                        return Json(new JsonClientResult
                        {
                            Success = true,
                            CallBack = "SupplierDetail.onRendAfterUpdateSupplierBank();"
                        }, JsonRequestBehavior.AllowGet);
                    }
                    return Json(new JsonClientResult
                    {
                        CallBack = result.Errors.CB_UpdateErrorResult()
                    }, JsonRequestBehavior.AllowGet);
                }
                return Json(new JsonClientResult
                {
                    CallBack = $"Common.onError('Không thêm được! Có lỗi xảy ra! {GetModelStateError()}')"
                }, JsonRequestBehavior.AllowGet);
            }
            return Json(new JsonClientResult
            {
                CallBack = $"Common.onError('You do not have permission!');"
            }, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}