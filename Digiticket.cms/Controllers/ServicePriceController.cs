using AutoMapper;
using DigiticketCMS.Helpers;
using DigiticketCMS.Models;
using DigiticketCMS.Models.ServicePrice;
using DigiticketCMS.Services.Models;
using Infrastructure.Config;
using Infrastructure.Extensions;
using KellermanSoftware.CompareNetObjects;
using Newtonsoft.Json;
using Services.Interfaces;
using Services.Models.Requests.ServicePrice;
using Services.Models.Responses.Product;
using Services.Models.Responses.ServicePrice;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace DigiticketCMS.Controllers
{
    [Authorize]
    public class ServicePriceController : BaseController
    {

        #region constructor
        private IServicepriceServices _servicepriceServices;

        public ServicePriceController(IServicepriceServices servicepriceServices)
        {
            _servicepriceServices = servicepriceServices;
        }
        #endregion

        #region get by page service price
        [HttpGet]
        public ActionResult GetByPageServicePrice(ServicePriceGetByPageViewModel model)
        {
            if (User.CheckPermission(Permission.ProductViewService))
            {
                var request = new ServicePriceGetByPageRequest()
                {
                    PageIndex = model.page,
                    PageSize = model.perpage,
                    FieldName = model.field,
                    Orderby = model.sort,
                    Keyword = !string.IsNullOrEmpty(model.key) && model.key != "null" ? model.key : "",
                    Filter = new List<FilterRequest>()
                };
                if (model.ProductId != null && model.ProductId != Guid.Empty)
                {
                    request.Filter.Add(new FilterRequest()
                    {
                        Opt = FilterOptionOpt.AND,
                        Opt1 = FilterOptionOpt1.EqualTo,
                        Name = "productId",
                        Values = model.ProductId.ToString(),
                        Type = FilterOptionType.String
                    });
                }
                var result = _servicepriceServices.GetByPage(request, User.Identity.GetClaims("token"));
                if (result.IsOK() && !result.Data.Result.IsNullOrEmpty())
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
        #endregion

        #region import service price modal
        public ActionResult ImportServicePriceModal(Guid productId)
        {
            if (User.CheckPermission(Permission.ProductImportServicePrice))
            {
                if (productId != null)
                {
                    var result = _servicepriceServices.GetListByProductFromDigipost(productId, User.Identity.GetClaims("token"));
                    if (result.IsOK())
                    {
                        return PartialView("Partial/_ImportServicePriceFromDigipostModal", result.Data);
                    }
                }
                return PartialView("Partial/_ImportServicePriceFromDigipostModal", new ServicePriceResponse() { });
            }
            return PartialView("~/Views/Shared/_ErrorViewModal.cshtml", new ErrorViewModel()
            {
                Title = "You do not have permission",
            });
        }
        #endregion

        #region importList
        [HttpPost]
        public JsonResult ServicePriceImportListServicePriceFromDigipost(ServicePriceUpdateListServicepriceFromDigipostViewModel model)
        {
            if (User.CheckPermission(Permission.ProductImportServicePrice))
            {
                if (ModelState.IsValid)
                {
                    var request = Mapper.Map<ServicePriceUpdateListServicepriceFromDigipostRequest>(model);
                    var result = _servicepriceServices.ImportListServicePriceFromDigipost(request, User.Identity.GetClaims("token"));
                    if (result.IsOK())
                    {
                        return Json(new JsonClientResult()
                        {
                            Success = true,
                            CallBack = $"servicePrice.onRendAfterUpdateServicePriceFromDigipost('Import giai đoạn giá từ Digipost thành công!');"
                        }, JsonRequestBehavior.AllowGet);
                    }
                    return Json(new JsonClientResult()
                    {
                        CallBack = result.Errors.CB_UpdateErrorResult()
                    }, JsonRequestBehavior.AllowGet);
                }
                return Json(new JsonClientResult()
                {
                    CallBack = $"Common.onError(`{GetModelStateError()}`);"
                }, JsonRequestBehavior.AllowGet);
            }
            return Json(new JsonClientResult
            {
                CallBack = $"Common.onError('You do not have permission!');"
            }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Update serviceprice from digipost
        [HttpPost]
        public JsonResult UpdateListServicepriceFromDigipost(ServicePriceUpdateListServicepriceFromDigipostViewModel model)
        {
            if (User.CheckPermission(Permission.ProductImportServicePrice))
            {
                if (ModelState.IsValid)
                {
                    var request = Mapper.Map<ServicePriceUpdateListServicepriceFromDigipostRequest>(model);
                    var result = _servicepriceServices.UpdateListServicePriceFromDigipost(request, User.Identity.GetClaims("token"));
                    if (result.IsOK())
                    {
                        var keepModal__str = model.KeepModal ? "true" : "false";
                        return Json(new JsonClientResult()
                        {
                            Success = true,
                            CallBack = $"servicePrice.onRendAfterUpdateServicePriceFromDigipost('Cập nhật giai đoạn giá từ Digipost thành công!', {keepModal__str});"
                        }, JsonRequestBehavior.AllowGet);
                    }
                    return Json(new JsonClientResult()
                    {
                        CallBack = result.Errors.CB_UpdateErrorResult()
                    }, JsonRequestBehavior.AllowGet);
                }
                return Json(new JsonClientResult()
                {
                    CallBack = $"Common.onError(`{GetModelStateError()}`);"
                }, JsonRequestBehavior.AllowGet);
            }
            return Json(new JsonClientResult
            {
                CallBack = $"Common.onError('You do not have permission!');"
            }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region service price detail 
        public ActionResult ServicePriceDetail(Guid id)
        {
            if (User.CheckPermission(Permission.ProductViewService))
            {
                if (id != null && id != Guid.Empty)
                {
                    var result = _servicepriceServices.GetFromId(id, User.Identity.GetClaims("token"));
                    if (result.IsOK())
                    {
                        return View(result.Data);
                    }
                    return View("~/Views/Shared/Error.cshtml");
                }
            }
            return View("Index", "Home");
        }
        public ActionResult ServicePriceDetailModal(Guid id)
        {
            if (User.CheckPermission(Permission.ProductViewService))
            {
                if (id != null && id != Guid.Empty)
                {
                    var result = _servicepriceServices.GetFromId(id, User.Identity.GetClaims("token"));
                    if (result.IsOK())
                    {
                        var resultToView = Mapper.Map<ServicePriceViewToViewModel>(result.Data);
                        if (resultToView.PricesTime != null)
                        {
                            if (!resultToView.PricesTime.ListDayOfWeek.IsNullOrEmpty())
                            {
                                resultToView.PricesTime.ListDayOfWeek.Add(resultToView.PricesTime.ListDayOfWeek[0]);
                                resultToView.PricesTime.ListDayOfWeek.RemoveAt(0);
                            }
                        }
                        return PartialView("Partial/ServicePriceDetailModal", resultToView);
                    }
                }
                return PartialView("Partial/ServicePriceDetailModal", new ServicePriceViewToViewModel() { });
            }
            return PartialView("~/Views/Shared/_ErrorViewModal.cshtml", new ErrorViewModel()
            {
                Title = "You do not have permission!"
            });
        }
        #endregion

        #region compare
        [HttpPost]
        public ActionResult ServicePriceCompare(ServicePriceCompareViewModel model)
        {
            CompareLogic compareLogic = new CompareLogic();
            compareLogic.Config.IgnoreConcreteTypes = true;
            compareLogic.Config.AttributesToIgnore.Add(typeof(CompareIgnoreAttribute));
            if (model.NewServicePrice.ListPrices.Any(l => l.GroupServiceName == null))
            {
                compareLogic.Config.MembersToIgnore.Add("GroupServiceName");
            }

            compareLogic.Config.MaxDifferences = 100;
            var result = new ServicePriceCompareResultToViewViewModal()
            {
                OldServicePrice = model.OldServiceprice,
                NewServicePrice = model.NewServicePrice,
                CompareResult = new ServicePriceCompareResult()
                {
                    All = compareLogic.Compare(model.OldServiceprice, model.NewServicePrice),
                    ListPrices = compareLogic.Compare(model.OldServiceprice.ListPrices, model.NewServicePrice.ListPrices),
                    PricesTime = compareLogic.Compare(model.OldServiceprice.PricesTime, model.NewServicePrice.PricesTime)
                }
            };

            return Json(new JsonClientResult
            {
                Success = true,
                Data = RenderViewToString("~/Views/ServicePrice/Partial/_CompareServicePrice.cshtml", result),
                Message = JsonConvert.SerializeObject(result)
            }, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}