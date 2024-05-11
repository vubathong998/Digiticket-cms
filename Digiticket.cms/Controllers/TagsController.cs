using AutoMapper;
using DigiticketCMS.Config;
using DigiticketCMS.Helpers;
using DigiticketCMS.Models;
using DigiticketCMS.Models.Tags;
using DigiticketCMS.Services.Models;
using Infrastructure.Config;
using Infrastructure.Extensions;
using Newtonsoft.Json;
using Services.Interfaces;
using Services.Models.Requests.Tags;
using Services.Models.Responses.Tags;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Utilities.Caching;

namespace DigiticketCMS.Controllers
{
    [Authorize]
    public class TagsController : BaseController
    {
        #region constructor
        private ITagsServices _tagsServices;
        private ILibServices _libServices;
        public TagsController(ITagsServices tagsServices,
                              ILibServices libServices)
        {
            _tagsServices = tagsServices;
            _libServices = libServices;
        }
        #endregion

        #region get by page
        public ActionResult Tags(ProductTagsAddModalViewModel model)
        {
            if (User.CheckPermission(Permission.Tags))
            {
                model.Categories = _libServices.CategoryGetAll(User.Identity.GetClaims("token")).Data;
                model.Destinations = _libServices.DestinationGetAll(User.Identity.GetClaims("token")).Data;
                return View(model);
            }
            return RedirectToAction("Index", "Home");
        }

        public JsonResult TagsGetByPage(ProductTagsGetByPageViewModel model)
        {
            if (User.CheckPermission(Permission.TagsView))
            {
                if (model.key == null) model.key = "";
                if (model.key.CheckForKey())
                {
                    var request = Mapper.Map<ProductTagsGetByPageRequest>(model);
                    request.PageIndex = model.page;
                    request.PageSize = model.perpage;
                    request.FieldName = model.field;
                    request.Orderby = model.sort;
                    request.Filter = new List<FilterRequest>();

                    if (!string.IsNullOrEmpty(model.CategoryId) && model.CategoryId != "null")
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
                    request.Filter.Add(new FilterRequest()
                    {
                        Opt = FilterOptionOpt.AND,
                        Name = "parentId",
                        Opt1 = FilterOptionOpt1.EqualTo,
                        Type = FilterOptionType.Number,
                        Values = model.ParentId.ToString()
                    });
                    if (!string.IsNullOrEmpty(model.key))
                    {
                        request.Keyword = model.key;
                    }
                    var result = _tagsServices.GetByPage(request, User.Identity.GetClaims("token"));
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
                    data = new List<ProductTagsResponse>()
                }, JsonRequestBehavior.AllowGet);
            }
            return Json(new JsonDataTable
            {
                meta = null,
                data = null
            }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region add
        [HttpGet]
        public ActionResult TagsAddModal(TagsAddModalViewModel model)
        {
            if (User.CheckPermission(Permission.TagsAdd))
            {
                if (string.IsNullOrEmpty(model.DestinationId) && string.IsNullOrEmpty(model.CategoryId))
                {
                    model.Categories = _libServices.CategoryGetAll(User.Identity.GetClaims("token")).Data;
                    model.Destinations = _libServices.DestinationGetAll(User.Identity.GetClaims("token")).Data;
                    return PartialView("Partial/_TagsAddModal", model);
                }
                return PartialView("Partial/_TagsAddModal", model);
            }
            return PartialView("~/Views/Shared/_ErrorViewModal.cshtml", new ErrorViewModel()
            {
                Title = "You do not have permission!",
            });
        }

        [HttpPost]
        public JsonResult Add(ProductTagsAddViewModel model)
        {
            if (User.CheckPermission(Permission.TagsAdd))
            {
                if (ModelState.IsValid)
                {
                    var request = Mapper.Map<ProductTagsAddRequest>(model);
                    var result = _tagsServices.Add(request, User.Identity.GetClaims("token"));
                    if (result.IsOK())
                    {
                        var keyToken = CacheNameConfig.GroupServiceTags;
                        CacheUtility.Delete(keyToken);
                        var infoTagJustAdded_JSON = new ProductTagsForViewResponse();
                        if (model.TypeResponse == 2 || model.TypeResponse == 3)
                        {
                            var infoTagJustAdded = _tagsServices.GetById(result.Data, User.Identity.GetClaims("token"));
                            if (infoTagJustAdded.IsOK() && infoTagJustAdded.Data != null)
                            {
                                infoTagJustAdded_JSON = Mapper.Map<ProductTagsForViewResponse>(infoTagJustAdded.Data);
                                infoTagJustAdded_JSON.TypeResponse = model.TypeResponse;
                            }
                        }
                        return Json(new JsonClientResult
                        {
                            Success = true,
                            CallBack = $"TagsGetByPage.onRendAfterUpdate('Thêm mới thành công', `{JsonConvert.SerializeObject(infoTagJustAdded_JSON)}`);",
                        });
                    }
                    return Json(new JsonClientResult
                    {
                        CallBack = result.Errors.CB_UpdateErrorResult()
                    });
                }
                return Json(new JsonClientResult
                {
                    CallBack = $"Common.onError(`{GetModelStateError()}`);"
                });
            }
            return Json(new JsonClientResult
            {
                CallBack = $"Common.onError(`You do not have permission`);"
            });
        }

        [HttpPost]
        public JsonResult AddTagReturnPartial(ProductTagsAddViewModel model)
        {
            if (User.CheckPermission(Permission.TagsAdd))
            {
                if (ModelState.IsValid)
                {
                    var request = Mapper.Map<ProductTagsAddRequest>(model);
                    var result = _tagsServices.Add(request, User.Identity.GetClaims("token"));
                    if (result.IsOK())
                    {
                        var keyToken = CacheNameConfig.GroupServiceTags;
                        CacheUtility.Delete(keyToken);
                       
                        
                    }
                    return Json(new JsonClientResult
                    {
                        CallBack = result.Errors.CB_UpdateErrorResult()
                    });
                }
                return Json(new JsonClientResult
                {
                    CallBack = $"Common.onError(`{GetModelStateError()}`);"
                });
            }
            return Json(new JsonClientResult
            {
                CallBack = $"Common.onError(`You do not have permission`);"
            });
        }

        #endregion

        #region update idx
        [HttpGet]
        public JsonResult TagsUpdateIdx(ProductTagsUpdateIdxViewModel model)
        {
            if (User.CheckPermission(Permission.TagsEdit))
            {
                if (ModelState.IsValid)
                {
                    var request = Mapper.Map<ProductTagsUpdateIdxRequest>(model);
                    var result = _tagsServices.UpdateIdx(request, User.Identity.GetClaims("token"));
                    if (result.IsOK())
                    {
                        return Json(new JsonClientResult
                        {
                            Success = true,
                            CallBack = "TagsGetByPage.onRendAfterUpdate();"
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
                CallBack = $"Common.onError(`you do not have permission`);"
            }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region suggest
        [HttpGet]
        public JsonResult SuggesTagsInUpdateTags(int type, int? parentId, string key = "")
        {
            var model = new List<ProductTagsResponse>();
            if (key.CheckForKey())
            {
                TagsGetListRequest request = new TagsGetListRequest()
                {
                    Key = key,
                    CategoryId = null,
                    DestinationId = null,
                    Type = type
                };
                if (parentId != null) request.ParentId = (int)parentId;

                var result = _tagsServices.GetList(request, User.Identity.GetClaims("token"));
                if (result.IsOK())
                {
                    model = result.Data;
                }
            }
            return Json(new JsonClientResult
            {
                Success = true,
                Data = model
            }, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}