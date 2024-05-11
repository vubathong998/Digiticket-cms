using AutoMapper;
using DigiticketCMS.Config;
using DigiticketCMS.Helpers;
using DigiticketCMS.Models;
using DigiticketCMS.Models.Lib;
using DigiticketCMS.Models.Product;
using DigiticketCMS.Models.Tags;
using DigiticketCMS.Services.Models;
using Infrastructure.Config;
using Infrastructure.Extensions;
using Newtonsoft.Json;
using Services;
using Services.Interfaces;
using Services.Models;
using Services.Models.Requests.Media;
using Services.Models.Requests.Product;
using Services.Models.Requests.Tags;
using Services.Models.Responses.Lib;
using Services.Models.Responses.Media;
using Services.Models.Responses.Product;
using Services.Models.Responses.Tags;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace DigiticketCMS.Controllers
{
    [Authorize]
    public class ProductController : BaseController
    {
        #region constructor
        private IProductServices _productServices;
        private ILibServices _libServices;
        private ITagsServices _tagServices;
        private IMediaServices _mediaServices;
        public ProductController(IProductServices productServices,
                                 ILibServices libServices,
                                 ITagsServices tagServices,
                                 IMediaServices mediaServices)
        {
            _productServices = productServices;
            _libServices = libServices;
            _tagServices = tagServices;
            _mediaServices = mediaServices;
        }
        #endregion

        #region get by page
        public ActionResult Product()
        {
            if (User.CheckPermission(Permission.Product))
            {
                var result = new ProductInfoToView();
                result.Categories = _libServices.CategoryGetAll(User.Identity.GetClaims("token")).Data;
                result.Destinations = _libServices.DestinationGetAll(User.Identity.GetClaims("token")).Data;
                return View(result);
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public ActionResult ProductGetByPage(ProductGetByPageViewModel model)
        {
            if (User.CheckPermission(Permission.Product))
            {
                if (model.key == null) model.key = "";
                if (model.key.CheckForKey())
                {
                    var request = Mapper.Map<ProductGetByPageRequest>(model);
                    request.PageIndex = model.page;
                    request.PageSize = model.perpage;
                    request.FieldName = model.field;
                    request.Orderby = model.sort;
                    if (!string.IsNullOrEmpty(model.key))
                    {
                        request.Keyword = model.key;
                    }
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
                    if (!string.IsNullOrEmpty(model.StringIsPublic))
                    {
                        request.Filter.Add(new FilterRequest()
                        {
                            Opt = FilterOptionOpt.AND,
                            Name = "isPublic",
                            Opt1 = FilterOptionOpt1.EqualTo,
                            Type = FilterOptionType.Boolean,
                            Values = model.StringIsPublic
                        });
                    }
                    if (model.Status != null)
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
                    if (model.SupplierId != Guid.Empty && model.SupplierId != null)
                    {
                        request.Filter.Add(new FilterRequest()
                        {
                            Opt = FilterOptionOpt.AND,
                            Name = "supplierId",
                            Opt1 = FilterOptionOpt1.EqualTo,
                            Type = FilterOptionType.String,
                            Values = model.SupplierId.ToString()
                        });
                    }
                    if (model.IsHot != null)
                    {
                        request.Filter.Add(new FilterRequest()
                        {
                            Opt = FilterOptionOpt.AND,
                            Name = "isHot",
                            Opt1 = FilterOptionOpt1.EqualTo,
                            Type = FilterOptionType.Boolean,
                            Values = model.IsHot.ToString()
                        });
                    }
                    if (model.IsHome != null)
                    {
                        request.Filter.Add(new FilterRequest()
                        {
                            Opt = FilterOptionOpt.AND,
                            Name = "isHome",
                            Opt1 = FilterOptionOpt1.EqualTo,
                            Type = FilterOptionType.Boolean,
                            Values = model.IsHome.ToString()
                        });
                    }
                    if (model.CategoryIdRange.IsTrueOrFalse())
                    {
                        request.Filter.Add(new FilterRequest()
                        {
                            Opt = FilterOptionOpt.AND,
                            Name = "categoryId",
                            Opt1 = FilterOptionOpt1.In,
                            Type = FilterOptionType.List,
                            Values = JsonConvert.SerializeObject(model.CategoryIdRange.Split(','))
                        });
                    }
                    var result = _productServices.GetByPage(request, User.Identity.GetClaims("token"));
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

        #region get list by tenant from digipost
        [HttpGet]
        public JsonResult productGetListByTenantFromDigipost(ProductGetListByTenantFromDigipostViewModel model)
        {
            if (model.Id != null)
            {
                var request = Mapper.Map<ProductGetListByTenantFromDigipostRequest>(model);
                var result = _productServices.GetListByTenantDigipost(request, User.Identity.GetClaims("token"));
                if (result.IsOK())
                {
                    return Json(new JsonDataTable
                    {
                        data = result.Data
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
            return Json(new JsonClientResult
            {
                Message = GetModelStateError()
            }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region import
        [HttpGet]
        public ActionResult ImportProductFromDigipost(ProductImportProductFromDigipostViewModel model)
        {
            if (User.CheckPermission(Permission.ProductImportServicePrice))
            {
                if (ModelState.IsValid)
                {
                    var request = Mapper.Map<ProductImportProductFromDigipostRequest>(model);
                    var result = _productServices.ImportProductFromDigipost(request, User.Identity.GetClaims("token"));
                    if (result.IsOK())
                    {
                        return Json(new JsonClientResult
                        {
                            Success = true,
                            Data = model.ProductId
                        }, JsonRequestBehavior.AllowGet);
                    }
                    return Json(new JsonClientResult
                    {
                        Message = result.Errors.ToString()
                    }, JsonRequestBehavior.AllowGet);
                }
                return Json(new JsonClientResult
                {
                    Message = GetModelStateError()
                }, JsonRequestBehavior.AllowGet);
            }
            return Json(new JsonClientResult
            {
                Message = "You do not have permission!"
            }, JsonRequestBehavior.AllowGet);
        }
        #endregion

            #region detail
        public ActionResult ProductDetail(Guid productId)
        {
            if (User.CheckPermission(Permission.Product))
            {
                if (productId != Guid.Empty)
                {
                    ProductDetailViewModel result = new ProductDetailViewModel();
                    result.ProductId = productId;
                    var ProductResponse = _productServices.ProductGetById(productId, User.Identity.GetClaims("token"));
                    if (ProductResponse.IsOK())
                    {
                        result.ProductResponse = Mapper.Map<ProductResponse>(ProductResponse.Data);
                        if (!result.ProductResponse.Images.IsNullOrEmpty())
                            result.ProductResponse.Images = result.ProductResponse.Images.OrderBy(e => e.Idx).ToList();
                        if (!result.ProductResponse.TagsGroup.IsNullOrEmpty())
                        {
                            foreach (var item in result.ProductResponse.TagsGroup)
                            {
                                var RequestFullTagsGroup = new TagsGetListRequest()
                                {
                                    ParentId = item.Id,
                                    Type = 2
                                };
                                var ResultFullTagsGroup = _tagServices.GetList(RequestFullTagsGroup, User.Identity.GetClaims("token"));
                                if (ResultFullTagsGroup.IsOK())
                                {
                                    result.FullTagsGroup.Add(new TagsGroupResponse()
                                    {
                                        Name = item.Name,
                                        Id = item.Id,
                                        SubOptions = Mapper.Map<List<TagsGroupResponse>>(ResultFullTagsGroup.Data)
                                    });
                                }
                            }
                        }
                        return View(result);
                    }
                    return View("~/Views/Shared/Error.cshtml");
                }
            }
            return RedirectToAction("Index", "Home");
        }

        public ActionResult ProductTagHighLightModal(string data)
        {
            var taghighlight = new List<ProductTagsResponse>();
            try
            {
                taghighlight = JsonConvert.DeserializeObject(data) as List<ProductTagsResponse>;
            }
            catch (Exception e)
            {
                return PartialView("~/Views/Shared/_ErrorViewModal.cshtml", new ErrorViewModel()
                {
                    Title = e.Message,
                });
            }
            if (taghighlight != null)
            {
                return PartialView();
            }
        }

        [ChildActionOnly]
        public ActionResult ProductDetailTagsComponentView(ProductDetailViewModel model)
        {
            if (User.CheckPermission(Permission.Product))
            {
                var response = new ProductDetailTagsPartialViewModel();
                response.ProductResponse = Mapper.Map<ProductResponse>(model.ProductResponse);
                var LibTagViewResponse = _libServices.TagView(User.Identity.GetClaims("token"));
                var TagsResponse = _tagServices.GetListTagsCache((int)EnumCommonCode.ETagsItemType.ProductHighlight, User.Identity.GetClaims("token"));
                if (!TagsResponse.Tags.IsNullOrEmpty())
                {
                    response.TagsResponse = TagsResponse.Tags;
                }
                if (LibTagViewResponse.IsOK())
                {
                    response.LibTagViewResponse = Mapper.Map<List<LibTagViewResponse>>(LibTagViewResponse.Data);
                    return PartialView("Partial/_ProductDetailTags", response);
                }
            }
            return PartialView("~/Views/Shared/Error.cshtml");
        }
        #endregion

        #region show update main info modal
        public ActionResult ProductUpdateMainInfoModal(Guid id, string name)
        {
            if (User.CheckPermission(Permission.ProductEditInfo))
            {
                if (id != null)
                {
                    var result = new LibViewModel();
                    result.Id = id;
                    result.Name = name;
                    result.Category = _libServices.CategoryGetAll(User.Identity.GetClaims("token")).Data;
                    result.Destination = _libServices.DestinationGetAll(User.Identity.GetClaims("token")).Data;
                    return PartialView("Partial/_ProductUpdateMainInfoModal", result);
                }
                return PartialView("~/Views/Shared/_ErrorViewModal.cshtml", new ErrorViewModel()
                {
                    Title = "Id cannot be null!",
                });
            }
            return PartialView("~/Views/Shared/_ErrorViewModal.cshtml", new ErrorViewModel()
            {
                Title = "Thông tin chính chưa được update!",
                Description = "Thông tin chính phải được cập nhật trước khi xem thông tin chi tiết! Vui lòng liên hệ với người có quyền update thông tin chính update trước!"
            });
        }
        #endregion

        #region update main info
        public JsonResult ProductUpdateMainInfo(ProductUpdateMainInfoViewModel model)
        {
            if (User.CheckPermission(Permission.ProductEditInfo))
            {
                if (ModelState.IsValid)
                {
                    var request = Mapper.Map<ProductUpdateMainInfoRequest>(model);
                    var result = _productServices.UpdateMainInfo(request, User.Identity.GetClaims("token"));
                    if (result.IsOK())
                    {
                        return Json(new JsonClientResult()
                        {
                            Success = true,
                            CallBack = "ProductDetail.onRendAfterUpdateMainInfo(true);"
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
            return Json(new JsonClientResult()
            {
                CallBack = $"Common.onError(`You do not have permission`);"
            }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region update base info
        [HttpPost]
        public JsonResult ProductUpdateBaseInfo(ProductUpdateBaseInfoViewModel model)
        {
            if (User.CheckPermission(Permission.ProductEditInfo))
            {
                if (ModelState.IsValid)
                {
                    var request = Mapper.Map<ProductUpdateBaseInfoRequest>(model);
                    var result = _productServices.UpdateBaseInfo(request, User.Identity.GetClaims("token"));
                    if (result.IsOK())
                    {
                        return Json(new JsonClientResult()
                        {
                            Success = true,
                            CallBack = "Common.onSuccess();"
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
            return Json(new JsonClientResult()
            {
                CallBack = $"Common.onError(`You do not have permission`);"
            }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region update image
        [HttpGet]
        public JsonResult SuggesImages(string key)
        {
            MediaGetByPageRequest request = new MediaGetByPageRequest()
            {
                OrderBy = "CreatedDate",
                OrderByDesc = true,
                KeyWord = key,
                PageIndex = 1,
                PageSize = 20,
            };
            var result = _mediaServices.GetByPage(request, User.Identity.GetClaims("token"));
            var model = new PagingResultv2<MediaGetByPageResponse>();
            if (result.Code > 0)
            {
                model = result.Data;
            }
            return Json(new JsonClientResult
            {
                Success = true,
                Data = model.Result
            }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ProductUpdateImage(ProductUpdateImageViewModel model)
        {
            if (User.CheckPermission(Permission.ProductEditImage))
            {
                if (ModelState.IsValid)
                {
                    var request = Mapper.Map<ProductUpdateImageRequest>(model);
                    var videosRequest = Mapper.Map<List<BaseImg>>(model.Videos);
                    if (!model.Videos.IsNullOrEmpty())
                    {
                        request.Images.AddRange(videosRequest);
                    }
                    var result = _productServices.UpdateImage(request, User.Identity.GetClaims("token"));
                    if (result.IsOK())
                    {
                        return Json(new JsonClientResult()
                        {
                            Success = true,
                            CallBack = "Common.onSuccess();"
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
            return Json(new JsonClientResult()
            {
                CallBack = $"Common.onError(`You do not have permission`);"
            }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region update tags
        [HttpPost]
        public JsonResult ProductUpdateTags(ProductUpdateTagsViewModel model)
        {
            if (User.CheckPermission(Permission.ProductEditTags))
            {
                if (ModelState.IsValid)
                {
                    var request = Mapper.Map<ProductUpdateTagsRequest>(model);
                    var result = _productServices.UpdateTags(request, User.Identity.GetClaims("token"));
                    if (result.IsOK())
                    {
                        return Json(new JsonClientResult()
                        {
                            Success = true,
                            CallBack = "Common.onSuccess();"
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
            return Json(new JsonClientResult()
            {
                CallBack = $"Common.onError(`You do not have permission`);"
            }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region update tagsgroup
        public JsonResult ProductUpdateTagsGroup(ProductUpdateTagsGroupViewModel model)
        {
            if (User.CheckPermission(Permission.ProductEditTagsGroup))
            {
                if (ModelState.IsValid)
                {
                    var request = Mapper.Map<ProductUpdateTagsGroupRequest>(model);
                    var result = _productServices.UpdateTagsGroup(request, User.Identity.GetClaims("token"));
                    if (result.IsOK())
                    {
                        return Json(new JsonClientResult()
                        {
                            Success = true,
                            CallBack = "Common.onSuccess();"
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
            return Json(new JsonClientResult()
            {
                CallBack = $"Common.onError(`You do not have permission`);"
            }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region update seo info
        [HttpPost]
        public JsonResult ProductUpdateSeoInfo(ProductUpdateSeoInfoViewModel model)
        {
            if (User.CheckPermission(Permission.ProductEditSEO))
            {
                if (ModelState.IsValid)
                {
                    var request = Mapper.Map<ProductUpdateSeoInfoRequest>(model);
                    var result = _productServices.UpdateSeoInfo(request, User.Identity.GetClaims("token"));
                    if (result.IsOK())
                    {
                        return Json(new JsonClientResult()
                        {
                            Success = true,
                            CallBack = "Common.onSuccess();"
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
            return Json(new JsonClientResult()
            {
                CallBack = $"Common.onError(`You do not have permission`);"
            }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region update config order 
        [HttpPost]
        public JsonResult ProductUpdateConfigOrder(ProductUpdateConfigOderViewModel model)
        {
            if (User.CheckPermission(Permission.ProductViewConfig))
            {
                if (ModelState.IsValid)
                {
                    var request = Mapper.Map<ProductUpdateConfigOderRequest>(model);
                    var result = _productServices.UpdateConfigOrder(request, User.Identity.GetClaims("token"));
                    if (result.IsOK())
                    {
                        return Json(new JsonClientResult()
                        {
                            Success = true,
                            CallBack = "Common.onSuccess();"
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
            return Json(new JsonClientResult()
            {
                CallBack = $"Common.onError(`You do not have permission`);"
            }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region update status order 
        [HttpPost]
        public JsonResult ProductUpdateStatus(ProductUpdateStatusViewModel model)
        {
            if (User.CheckPermission(Permission.ProductEditStatus))
            {
                if (ModelState.IsValid)
                {
                    var request = Mapper.Map<ProductUpdateStatusRequest>(model);
                    var result = _productServices.UpdateStatus(request, User.Identity.GetClaims("token"));
                    if (result.IsOK())
                    {
                        return Json(new JsonClientResult()
                        {
                            Success = true,
                            CallBack = "Common.onSuccess();"
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
            return Json(new JsonClientResult()
            {
                CallBack = $"Common.onError(`You do not have permission`);"
            }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region update idx
        [HttpGet]
        public JsonResult ProductUpdateIdx(ProductUpdateIdxViewModel model)
        {
            if (User.CheckPermission(Permission.TagsEdit))
            {
                if (ModelState.IsValid)
                {
                    var request = Mapper.Map<ProductUpdateIdxRequest>(model);
                    var result = _productServices.UpdateIdx(request, User.Identity.GetClaims("token"));
                    if (result.IsOK())
                    {
                        return Json(new JsonClientResult
                        {
                            Success = true,
                            CallBack = "productGetByPage.onRendAfterUpdateIdx();"
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

        #region toggle public
        [HttpGet]
        public JsonResult TogglePublic(Guid id, bool isChecked)
        {
            if (User.CheckPermission(Permission.ProductPublicProduct))
            {

                if (id != null)
                {
                    if (isChecked)
                    {
                        var result = _productServices.UpdateUnPublic(id, User.Identity.GetClaims("token"));
                        if (result.IsOK())
                        {
                            return Json(new JsonClientResult()
                            {
                                Success = true,
                                CallBack = "productGetByPage.onRendAfterTogglePublic('Đã bỏ public thành công!')",
                                Data = "false"
                            }, JsonRequestBehavior.AllowGet);
                        }
                        else return Json(new JsonClientResult()
                        {
                            CallBack = result.Errors.CB_UpdateErrorResult()
                        }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        var result = _productServices.UpdatePublic(id, User.Identity.GetClaims("token"));
                        if (result.IsOK())
                        {
                            return Json(new JsonClientResult()
                            {
                                Success = true,
                                CallBack = "productGetByPage.onRendAfterTogglePublic('Đã public thành công!')",
                                Data = "true"
                            }, JsonRequestBehavior.AllowGet);
                        }
                        else return Json(new JsonClientResult()
                        {
                            CallBack = result.Errors.CB_UpdateErrorResult()
                        }, JsonRequestBehavior.AllowGet);
                    }
                }
                return Json(new JsonClientResult()
                {
                    CallBack = $"Common.onError(`Error! \"ID is empty\"`);"
                }, JsonRequestBehavior.AllowGet);
            }
            return Json(new JsonClientResult()
            {
                CallBack = $"Common.onError(`You do not have permisson`);"
            }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region and tags

        [HttpGet]
        public JsonResult SuggesTagsInUpdateTagsHightlight(string key)
        {
            //var HTML = "";
            ProductTagsGetByPageRequest request = new ProductTagsGetByPageRequest()
            {
                FieldName = "CreatedDate",
                Orderby = "desc",
                Keyword = key,
                PageIndex = 1,
                PageSize = 20,
            };
            //request.Filter.Add(new FilterRequest()
            //{
            //    name = "type",
            //    values = FilterOptionTagsType.Product,

            //});
            var result = _tagServices.GetByPage(request, User.Identity.GetClaims("token"));
            var model = new PagingResult<ProductTagsResponse>();
            if (result.IsOK())
            {
                model = result.Data;
            }
            return Json(new JsonClientResult
            {
                Success = true,
                Data = model.Result
            }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region hot
        public ActionResult ProductHot()
        {
            if (User.CheckPermission(Permission.Setting))
            {
                var result = new ProductInfoToView();
                result.Categories = _libServices.CategoryGetAll(User.Identity.GetClaims("token")).Data;
                result.Destinations = _libServices.DestinationGetAll(User.Identity.GetClaims("token")).Data;
                return View(result);
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public ActionResult ProductAddOrUpdateHotModal()
        {
            if (User.CheckPermission(Permission.SettingEdit))
            {
                var result = new ProductInfoToView();
                result.Categories = _libServices.CategoryGetAll(User.Identity.GetClaims("token")).Data;
                result.Destinations = _libServices.DestinationGetAll(User.Identity.GetClaims("token")).Data;
                return PartialView("Partial/_ProductUpdateHotModal", result);
            }
            return PartialView("~/Views/Shared/_ErrorViewModal.cshtml", new ErrorViewModel()
            {
                Title = "You do not have permission",
            });
        }

        public ActionResult ProductAddOrUnHot(ProductUpdateHotViewModel model)
        {
            if (User.CheckPermission(Permission.SettingEdit))
            {
                if (ModelState.IsValid)
                {
                    var request = Mapper.Map<ProductUpdateHotRequest>(model);
                    var result = _productServices.UpdateHot(request, User.Identity.GetClaims("token"));
                    if (result.IsOK())
                    {
                        return Json(new JsonClientResult()
                        {
                            Success = true,
                            CallBack = "productGetByPage.onRendAfterAddOrUnHot();"
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
            return Json(new JsonClientResult()
            {
                CallBack = $"Common.onError(`you do not have permission`);"
            }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region home
        public ActionResult ProductHome()
        {
            if (User.CheckPermission(Permission.Setting))
            {
                var result = new ProductInfoToView();
                result.Categories = _libServices.CategoryGetAll(User.Identity.GetClaims("token")).Data;
                result.Destinations = _libServices.DestinationGetAll(User.Identity.GetClaims("token")).Data;
                return View(result);
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public ActionResult ProductAddOrUpdateHomeModal()
        {
            if (User.CheckPermission(Permission.SettingEdit))
            {
                var result = new ProductInfoToView();
                result.Categories = _libServices.CategoryGetAll(User.Identity.GetClaims("token")).Data;
                result.Destinations = _libServices.DestinationGetAll(User.Identity.GetClaims("token")).Data;
                return PartialView("Partial/_ProductUpdateHomeModal", result);
            }
            return PartialView("~/Views/Shared/_ErrorViewModal.cshtml", new ErrorViewModel()
            {
                Title = "You do not have permission",
            });
        }

        public ActionResult ProductAddOrUnHome(ProductUpdateHomeViewModel model)
        {
            if (User.CheckPermission(Permission.SettingEdit))
            {
                if (ModelState.IsValid)
                {
                    var request = Mapper.Map<ProductUpdateHomeRequest>(model);
                    var result = _productServices.UpdateHome(request, User.Identity.GetClaims("token"));
                    if (result.IsOK())
                    {
                        return Json(new JsonClientResult()
                        {
                            Success = true,
                            CallBack = "productGetByPage.onRendAfterAddOrUnHome();"
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
            return Json(new JsonClientResult()
            {
                CallBack = $"Common.onError(`you do not have permission`);"
            }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Update query price
        public JsonResult ProductUpdateQueryPrice(Guid id)
        {
            if (User.CheckPermission(Permission.ProductUpdateQueryPrice))
            {
                if (id != Guid.Empty)
                {
                    var result = _productServices.UpdateQueryPrice(id, User.Identity.GetClaims("token"));
                    if (result.IsOK())
                    {
                        return Json(new JsonClientResult()
                        {
                            Success = true,
                            CallBack = "ProductDetail.onRendAfterUpdateQueryPrice();"
                        }, JsonRequestBehavior.AllowGet);
                    }
                    return Json(new JsonClientResult()
                    {
                        CallBack = result.Errors.CB_UpdateErrorResult()
                    }, JsonRequestBehavior.AllowGet);
                }
                return Json(new JsonClientResult()
                {
                    CallBack = $"Common.onError(`Error! ID is not null or empty. Id = {id}`);"
                }, JsonRequestBehavior.AllowGet);
            }
            return Json(new JsonClientResult()
            {
                CallBack = $"Common.onError(`you do not have permission`);"
            }, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}
