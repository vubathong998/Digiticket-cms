using AutoMapper;
using DigiticketCMS.Config;
using DigiticketCMS.Helpers;
using DigiticketCMS.Models;
using DigiticketCMS.Models.GroupService;
using DigiticketCMS.Services.Models;
using Infrastructure.Config;
using Infrastructure.Extensions;
using Newtonsoft.Json.Linq;
using Services.Interfaces;
using Services.Models.Requests.GroupdService;
using Services.Models.Requests.Tags;
using Services.Models.Responses.Product;
using Services.Models.Responses.Tags;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Xml.Linq;

namespace DigiticketCMS.Controllers
{
    [Authorize]
    public class GroupServiceController : BaseController
    {
        #region constructor
        private IGroupServiceService _groupServiceService;
        private ITagsServices _tagsServices;
        public GroupServiceController(
                                 IGroupServiceService groupServiceService,
                                 ITagsServices tagsServices)
        {
            _groupServiceService = groupServiceService;
            _tagsServices = tagsServices;
        }
        #endregion

        #region page
        public ActionResult GroupService()
        {
            return View();
        }
        #endregion

        #region get by page 
        [HttpGet]
        public ActionResult GetByPageGroupService(GroupServiceViewModel model)
        {
            if (User.CheckPermission(Permission.ProductViewService))
            {
                var request = new GroupServiceRequest()
                {
                    PageIndex = model.page,
                    PageSize = model.perpage,
                    FieldName = model.field,
                    Orderby = model.sort,
                    Filter = new List<FilterRequest>()
                };
                if (!string.IsNullOrEmpty(model.key)) request.Keyword = model.key;
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
                var result = _groupServiceService.GetByPage(request, User.Identity.GetClaims("token"));
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

        #region detail
        public ActionResult GroupServiceDetailModal(int id)
        {
            if (User.CheckPermission(Permission.ProductViewService))
            {
                if (id != 0)
                {
                    var result = _groupServiceService.GetFromId(id, User.Identity.GetClaims("token"));
                    if (result.IsOK())
                        if (!result.Data.Images.IsNullOrEmpty()) result.Data.Images.OrderBy(e => e.Idx);
                    if (result.IsOK())
                    {
                        var resultToView = new GroupServiceResponseViewModel()
                        {
                            GroupServiceResponse = result.Data
                        };
                        var resultAllTags = _tagsServices.GetListTagsAndSubTagsCache((int)EnumCommonCode.ETagsItemType.GroupService, User.Identity.GetClaims("token"));
                        if(!resultAllTags.ParentTags.IsNullOrEmpty())
                        {
                            resultToView.AllParentTags = resultAllTags.ParentTags;
                        }
                        if (!resultAllTags.SubTags.IsNullOrEmpty())
                        {
                            resultToView.AllSubTags = resultAllTags.SubTags;
                        }
                        return PartialView("Partial/_GroupServiceDetailModal", resultToView);
                    }
                    return View("~/Views/Shared/Error.cshtml");
                }
            }
            return View("Index", "Home");
        }
        #endregion

        #region update info
        [HttpPost]
        public JsonResult GroupServiceTagsUpdateInfo(GroupServiceUpdateInfoViewModel model)
        {
            if (User.CheckPermission(Permission.ProductEditGroupService))
            {
                if (ModelState.IsValid)
                {
                    var request = Mapper.Map<GroupServiceUpdateInfoRequest>(model);
                    var result = _groupServiceService.UpdateInfo(request, User.Identity.GetClaims("token"));
                    if (result.IsOK())
                    {
                        return Json(new JsonClientResult
                        {
                            Success = true,
                            CallBack = "GroupServiceDetail.onRendAfterUpdate();"
                        });
                    }
                    return Json(new JsonClientResult
                    {
                        CallBack = result.Errors.CB_UpdateErrorResult(true)
                    });
                }
                return Json(new JsonClientResult
                {
                    CallBack = $"Common.onError(`{GetModelStateError()}`, true)"
                });
            }
            return Json(new JsonClientResult
            {
                CallBack = $"Common.onError(`You do not have permission!`)"
            });
        }
        #endregion

        #region update image
        [HttpPost]
        public JsonResult GroupServiceUpdateImage(GroupServiceUpdateImageViewModel model)
        {
            if (User.CheckPermission(Permission.ProductEditGroupService))
            {
                if (!model.Images.IsNullOrEmpty() && model.Id != 0)
                {
                    var request = Mapper.Map<GroupServiceUpdateImageRequest>(model);
                    var result = _groupServiceService.UpdateImage(request, User.Identity.GetClaims("token"));
                    if (result.IsOK())
                    {
                        return Json(new JsonClientResult
                        {
                            Success = true,
                            CallBack = "GroupServiceDetail.onRendAfterUpdate();"
                        });
                    }
                    return Json(new JsonClientResult
                    {
                        CallBack = result.Errors.CB_UpdateErrorResult(true)
                    });
                }
                return Json(new JsonClientResult
                {
                    CallBack = $"Common.onError(`{GetModelStateError()}`, true)"
                });
            }
            return Json(new JsonClientResult
            {
                CallBack = $"Common.onError(`You do not have permission!`)"
            });
        }
        #endregion

        #region update number ticket
        [HttpPost]
        public JsonResult GroupServiceUpdateNumberTicket(GroupServiceUpdateNumberTicketViewModel model)
        {
            if (User.CheckPermission(Permission.ProductEditGroupService))
            {
                if (ModelState.IsValid)
                {
                    var request = Mapper.Map<GroupServiceUpdateNumberTicketRequest>(model);
                    var result = _groupServiceService.UpdateNumberTicket(request, User.Identity.GetClaims("token"));
                    if (result.IsOK())
                    {
                        return Json(new JsonClientResult
                        {
                            Success = true,
                            CallBack = "GroupServiceDetail.onRendAfterUpdate();"
                        });
                    }
                    return Json(new JsonClientResult
                    {
                        CallBack = result.Errors.CB_UpdateErrorResult(true)
                    });
                }
                return Json(new JsonClientResult
                {
                    CallBack = $"Common.onError(`{GetModelStateError()}`, true)"
                });
            }
            return Json(new JsonClientResult
            {
                CallBack = $"Common.onError(`You do not have permission!`)"
            });
        }
        #endregion

        #region update tags
        [HttpPost]
        public JsonResult GroupServiceUpdateTags(GroupServiceUpdateTagsViewModel model)
        {
            if (User.CheckPermission(Permission.ProductEditGroupService))
            {
                if (ModelState.IsValid)
                {
                    var request = Mapper.Map<GroupServiceUpdateTagsRequest>(model);
                    var result = _groupServiceService.UpdateTags(request, User.Identity.GetClaims("token"));
                    if (result.IsOK())
                    {
                        return Json(new JsonClientResult
                        {
                            Success = true,
                            CallBack = "GroupServiceDetail.onRendAfterUpdate();"
                        });
                    }
                    return Json(new JsonClientResult
                    {
                        CallBack = result.Errors.CB_UpdateErrorResult(true)
                    });
                }
                return Json(new JsonClientResult
                {
                    CallBack = $"Common.onError(`{GetModelStateError()}`, true)"
                });
            }
            return Json(new JsonClientResult
            {
                CallBack = $"Common.onError(`You do not have permission!`)"
            });
        }
        #endregion

        #region update tags process
        [HttpPost]
        public JsonResult GroupServiceUpdateTagsProcess(GroupServiceUpdateTagsProcessViewModel model)
        {
            if (User.CheckPermission(Permission.ProductEditGroupService))
            {
                if (ModelState.IsValid)
                {
                    var request = Mapper.Map<GroupServiceUpdateTagsProcessRequest>(model);
                    var result = _groupServiceService.UpdateTagsProcess(request, User.Identity.GetClaims("token"));
                    if (result.IsOK())
                    {
                        return Json(new JsonClientResult
                        {
                            Success = true,
                            CallBack = "GroupServiceDetail.onRendAfterUpdate();"
                        });
                    }
                    return Json(new JsonClientResult
                    {
                        CallBack = result.Errors.CB_UpdateErrorResult(true)
                    });
                }
                return Json(new JsonClientResult
                {
                    CallBack = $"Common.onError(`{GetModelStateError()}`, true)"
                });
            }
            return Json(new JsonClientResult
            {
                CallBack = $"Common.onError(`You do not have permission!`)"
            });
        }
        #endregion

        #region update tags view
        [HttpPost]
        public JsonResult GroupServiceUpdateTagsView(GroupServiceUpdateTagsViewViewModel model)
        {
            if (User.CheckPermission(Permission.ProductEditGroupService))
            {
                if (ModelState.IsValid)
                {
                    var request = Mapper.Map<GroupServiceUpdateTagsViewRequest>(model);
                    var result = _groupServiceService.UpdateTagsView(request, User.Identity.GetClaims("token"));
                    if (result.IsOK())
                    {
                        return Json(new JsonClientResult
                        {
                            Success = true,
                            CallBack = "GroupServiceDetail.onRendAfterUpdate();"
                        });
                    }
                    return Json(new JsonClientResult
                    {
                        CallBack = result.Errors.CB_UpdateErrorResult(true)
                    });
                }
                return Json(new JsonClientResult
                {
                    CallBack = $"Common.onError(`{GetModelStateError()}`, true)"
                });
            }
            return Json(new JsonClientResult
            {
                CallBack = $"Common.onError(`You do not have permission!`)"
            });
        }
        #endregion
    }
}