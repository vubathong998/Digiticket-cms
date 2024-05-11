using AutoMapper;
using DigiticketCMS.Helpers;
using DigiticketCMS.Models;
using DigiticketCMS.Models.Media;
using Infrastructure.Config;
using Infrastructure.Extensions;
using Services.Interfaces;
using Services.Models;
using Services.Models.Requests.Media;
using Services.Models.Responses.Media;
using Services.Models.Responses.Tags;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace DigiticketCMS.Controllers
{
    [Authorize]
    public class MediaController : BaseController
    {
        #region constructor
        private IMediaServices _mediaServices;
        public MediaController(IMediaServices mediaServices)
        {
            _mediaServices = mediaServices;
        }
        #endregion

        #region get by page
        public ActionResult Media()
        {
            return View();
        }
        public ActionResult MediaGetByPage(MediaGetByPageViewModel model)
        {
            if (model.key == null) model.key = "";
            if (model.key.CheckForKey())
            {
                var request = Mapper.Map<MediaGetByPageRequest>(model);
                request.PageIndex = model.page;
                request.PageSize = model.perpage;
                request.OrderBy = model.field;
                if (!string.IsNullOrEmpty(model.key))
                {
                    request.KeyWord = model.key;
                }
                request.OrderByDesc = model.sort == "desc" || model.sort == "Desc";
                var result = _mediaServices.GetByPage(request, User.Identity.GetClaims("token"));
                if (result.Code > 0 && !result.Data.Result.IsNullOrEmpty())
                {
                    long totalItems = result.Data.TotalCount;
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
                data = new List<MediaGetByPageResponse>()
            }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region add
        public ActionResult MediaAddModal()
        {
            return PartialView("Partial/_MediaAddModal");
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [HttpPost]
        public JsonResult MediaAdd(MediaViewModel requestViewModel, MediaUploadViewModel data)
        {
            if (ModelState.IsValid)
            {
                var fileUpload = Request.Files;
                //var requestMedia = new MediaRequest
                //{
                //    FileRequest = requestViewModel.FileRequest
                //};
                var mediaRequest = new MediaRequest()
                {
                    FileRequest = fileUpload[0]
                };
                var requestData = Mapper.Map<MediaUploadRequest>(data);
                {
                    decimal i;
                    if (data.resizeWidth < data.resizeHeight) i = data.resizeWidth;
                    else i = data.resizeHeight;
                    while (i > 0)
                    {
                        if (data.resizeWidth % i == 0 && data.resizeHeight % i == 0)
                        {
                            requestData.aspectRatioX = data.resizeWidth / i;
                            requestData.aspectRatioY = data.resizeHeight / i;
                            break;
                        }
                        i--;
                    }
                }
                var result = _mediaServices.UploadMedia(mediaRequest, requestData, User.Identity.GetClaims("token"));
                if (result.Code > 0)
                {
                    return Json(new JsonClientResult
                    {
                        Success = true,
                        CallBack = "mediaAddModal.onRendAfterMediaAdd();"
                    }, JsonRequestBehavior.AllowGet);
                }
                return Json(new JsonClientResult
                {
                    CallBack = $"Common.onError(`Không thành công. Đã có lỗi xảy ra!`);"
                }, JsonRequestBehavior.AllowGet);
            }
            return Json(new JsonClientResult
            {
                CallBack = $"Common.onError(`{GetModelStateError()}`);"
            }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region resize
        [HttpGet]
        public ActionResult MediaResize(int Id, string FileName, decimal AspectRatioX, decimal AspectRatioY)
        {
            if (Id != 0)
            {
                ViewBag.Id = Id;
                ViewBag.FileName = FileName;
                ViewBag.AspectRatioX = AspectRatioX;
                ViewBag.AspectRatioY = AspectRatioY;
                return View();
            }
            return RedirectToAction("index", "home");
        }
        public JsonResult RenByRoot(int Id)
        {
            var HTML = "";
            if (Id >= 0)
            {
                var result = _mediaServices.GetByRoot(Id, User.Identity.GetClaims("token"));
                HTML = RenderViewToString("~/Views/Media/Partial/ShowByRootContent.cshtml", result.Data);
            }
            return Json(new JsonClientResult
            {
                Success = true,
                Data = HTML,
                CallBack = "$('.my-loading').hide();"
            }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ResizeMediaAction(MediaResizeViewModel model)
        {
            if (ModelState.IsValid)
            {
                var request = Mapper.Map<MediaResizeRequest>(model);
                var result = _mediaServices.ResizeMedia(request, User.Identity.GetClaims("token"));
                if (result.Code > 0)
                {
                    return Json(new JsonClientResult()
                    {
                        Success = true,
                        CallBack = "mediaResize.onRendAfterResizeMediaAction();"
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
        #endregion

        #region suggest 
        public ActionResult AutoSuggestAvatar(string key = "")
        {
            if (key.CheckForKey())
            {
                var request = new MediaGetByPageRequest()
                {
                    OrderBy = "CreatedDate",
                    OrderByDesc = true,
                    KeyWord = key,
                    PageIndex = 1,
                    PageSize = 20,

                };
                var model = _mediaServices.GetByPage(request, User.Identity.GetClaims("token"));
                return Json(new JsonClientResult
                {
                    Success = true,
                    Data = model.Data != null ? model.Data.Result : new List<MediaGetByPageResponse>()
                }, JsonRequestBehavior.AllowGet);
            }
            return Json(new JsonClientResult()
            {
                Data = new List<MediaGetByPageResponse>()
            });
        }
        public JsonResult SuggestImages(string key = "")
        {
            if (key.CheckForKey())
            {
                var request = new MediaGetByPageRequest()
                {
                    KeyWord = key,
                    PageIndex = 1,
                    PageSize = 20,
                    OrderBy = "createddate",
                    OrderByDesc = true
                };
                var result = _mediaServices.GetByPage(request, User.Identity.GetClaims("token"));
                if (result.Code > 0 && !result.Data.Result.IsNullOrEmpty())
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
                Data = new List<MediaGetByPageResponse>()
            }, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}