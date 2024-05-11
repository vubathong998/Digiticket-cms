using AutoMapper;
using DigiticketCMS.Config;
using DigiticketCMS.Helpers;
using DigiticketCMS.Helpers.Cookie;
using DigiticketCMS.Models;
using DigiticketCMS.Models.Content;
using DigiticketCMS.Models.Tags;
using DigiticketCMS.Services.Models;
using Infrastructure.Config;
using Infrastructure.Extensions;
using Newtonsoft.Json;
using Services.Implement;
using Services.Interfaces;
using Services.Models.Requests.Banner;
using Services.Models.Requests.Content;
using Services.Models.Responses.Content;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;
using Utilities.Caching;

namespace DigiticketCMS.Controllers
{
    [Authorize]
    public class LandingPageController : BaseController
    {
        #region Constructor
        private IContentServices _contentServices;
        private ILibServices _libServices;
        public LandingPageController(IContentServices contentServices,
                                     ILibServices libServices)
        {
            _contentServices = contentServices;
            _libServices = libServices;
        }
        #endregion

        #region Template component type
        [HttpGet]
        public ActionResult TemplateComponentType()
        {
            if (User.CheckPermission(Permission.SysWorkGroup))
            {
                var resultToView = new List<TemplateComponentTypeResponse>();
                var result = _contentServices.TemplateComponentTypeGetList("", User.Identity.GetClaims("token"));
                if (result.IsOK())
                {
                    resultToView = result.Data;
                }
                return View(resultToView);
            }
            return RedirectToAction("Home", "index");
        }

        public JsonResult TemplateComponentTypeRendTable(string key)
        {
            if (key == null) key = "";
            if (User.CheckPermission(Permission.SysWorkGroup))
            {
                var result = _contentServices.TemplateComponentTypeGetList(key, User.Identity.GetClaims("token"));
                if (result.IsOK())
                {
                    return Json(new JsonClientResult()
                    {
                        Success = true,
                        Data = RenderViewToString("~/Views/LandingPage/Partial/TemplateComponentType/_Table.cshtml", result.Data)
                    }, JsonRequestBehavior.AllowGet);
                }
                return Json(new JsonClientResult()
                {
                    Message = result.Errors.GetResultError()
                });
            }
            return Json(new JsonClientResult()
            {
                Message = "You do not have permission!"
            });
        }

        public ActionResult TemplateComponentTypeAddOrEditModal(int? id)
        {
            if (id == null)
            {
                if (User.CheckPermission(Permission.SysWorkGroup))
                {
                    return PartialView("~/Views/LandingPage/Partial/TemplateComponentType/_AddOrEditModal.cshtml", new TemplateComponentTypeResponse()
                    {
                        Id = 0
                    });
                }
            }
            else
            {
                if (User.CheckPermission(Permission.LandingPageEdit))
                {
                    var result = _contentServices.TemplateComponentTypeGetById((int)id, User.Identity.GetClaims("token"));
                    if (result.IsOK())
                    {
                        return PartialView("~/Views/LandingPage/Partial/TemplateComponentType/_AddOrEditModal.cshtml", result.Data);
                    }
                    return PartialView("~/Views/Shared/_ErrorViewModal.cshtml", new ErrorViewModel() { Title = result.Errors.GetResultError() });
                }
            }
            return PartialView("~/Views/Shared/_ErrorViewModal.cshtml", new ErrorViewModel()
            {
                Title = "You do not have permission!"
            });
        }

        [HttpPost]
        public JsonResult TemplateComponentTypeAdd(string name)
        {
            if (User.CheckPermission(Permission.SysWorkGroup))
            {
                var result = _contentServices.TemplateComponentTypeAdd(name, User.Identity.GetClaims("token"));
                if (result.IsOK())
                {
                    return Json(new JsonClientResult
                    {
                        Success = true,
                        CallBack = "templateComponentType.onRendAfterTemplateComponentTypeAddOrEdit('Thêm mới thành công');"
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

        [HttpPost]
        public JsonResult TemplateComponentTypeEdit(TemplateComponentTypeEditViewModel model)
        {
            if (User.CheckPermission(Permission.SysWorkGroup))
            {
                if (ModelState.IsValid)
                {
                    var request = Mapper.Map<TemplateComponentTypeEditRequest>(model);
                    var result = _contentServices.TemplateComponentTypeEdit(request, User.Identity.GetClaims("token"));
                    if (result.IsOK())
                    {
                        return Json(new JsonClientResult
                        {
                            Success = true,
                            CallBack = "templateComponentType.onRendAfterTemplateComponentTypeAddOrEdit('Update thành công');"
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

        [HttpGet]
        public JsonResult TemplateComponentTypeDelete(int id)
        {
            if (User.CheckPermission(Permission.SysWorkGroup))
            {
                if (id != 0)
                {
                    var result = _contentServices.TemplateComponentTypeDelete(id, User.Identity.GetClaims("token"));
                    if (result.IsOK())
                    {
                        return Json(new JsonClientResult
                        {
                            Success = true,
                            CallBack = "templateComponentType.onRendAfterTemplateComponentTypeAddOrEdit('Xóa thành công');"
                        }, JsonRequestBehavior.AllowGet);
                    }
                }
                return Json(new JsonClientResult
                {
                    Message = "Id cannot be null or empty"
                });
            }
            return Json(new JsonClientResult
            {
                Message = "You do not have permission!"
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult TemplateComponentTypeGetListSuggest(string key)
        {
            if (User.CheckPermission(Permission.SysWorkGroup))
            {
                if (key == null) key = "";
                if (key.CheckForKey())
                {
                    var result = _contentServices.TemplateComponentTypeGetList(key, User.Identity.GetClaims("token"));
                    if (result.IsOK())
                    {
                        return Json(new JsonClientResult()
                        {
                            Success = true,
                            Data = result.Data
                        }, JsonRequestBehavior.AllowGet);
                    }
                    return Json(new JsonClientResult()
                    {
                        Message = result.Errors.GetResultError()
                    });
                }
                return Json(new JsonClientResult()
                {
                    Message = "Nhập tối thiểu 2 ký tự!"
                });
            }
            return Json(new JsonClientResult
            {
                Message = "You do not have permission!"
            }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Template component
        public ActionResult TemplateComponent()
        {
            if (User.CheckPermission(Permission.LandingPage))
            {
                return View();
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public ActionResult TemplateComponentGetByPage(TemplateComponentGetByPageViewModel model)
        {
            if (User.CheckPermission(Permission.SettingView))
            {
                if (model.key == null) model.key = "";
                if (model.key.CheckForKey())
                {
                    var request = Mapper.Map<TemplateComponentGetByPageRequest>(model);
                    request.PageIndex = model.page;
                    request.PageSize = model.perpage;
                    request.FieldName = model.field;
                    request.Orderby = model.sort;
                    request.Keyword = model.key;
                    //if (model.Type != null) request.Type = model.Type.ToString();
                    //else request.Type = "";
                    var result = _contentServices.TemplateComponentGetByPage(request, User.Identity.GetClaims("token"));
                    if (result.IsOK() && result.Data != null)
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
                data = new List<TemplateComponentResponse>()
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult TemplateComponentAddOrEditModal(int? id)
        {
            if (User.CheckPermission(Permission.LandingPage))
            {
                if (id == null)
                {
                    return PartialView("~/Views/LandingPage/Partial/TemplateComponent/_AddOrEditModal.cshtml", new TemplateComponentResponse()
                    {
                        Id = 0
                    });
                }
                else
                {
                    var result = _contentServices.TemplateComponentGetById((int)id, User.Identity.GetClaims("token"));
                    if (result.IsOK())
                    {
                        return PartialView("~/Views/LandingPage/Partial/TemplateComponent/_AddOrEditModal.cshtml", result.Data);
                    }
                    return PartialView("~/Views/Shared/_ErrorViewModal.cshtml", new ErrorViewModel() { Title = result.Errors.GetResultError() });
                }
            }
            return PartialView("~/Views/Shared/_ErrorViewModal.cshtml", new ErrorViewModel()
            {
                Title = "You do not have permission!"
            });
        }

        [HttpPost]
        public JsonResult TemplateComponentAdd(TemplateComponentAddViewModel model)
        {
            if (User.CheckPermission(Permission.LandingPage))
            {
                var request = Mapper.Map<TemplateComponentAddRequest>(model);
                var result = _contentServices.TemplateComponentAdd(request, User.Identity.GetClaims("token"));
                if (result.IsOK())
                {
                    return Json(new JsonClientResult
                    {
                        Success = true,
                        CallBack = "templateComponent.onRendAfterTemplateComponentUpdate('Thêm mới thành công');"
                    }, JsonRequestBehavior.AllowGet);
                }
                return Json(new JsonClientResult
                {
                    Message = result.Errors.GetResultError()
                }, JsonRequestBehavior.AllowGet);
            }
            return Json(new JsonClientResult
            {
                CallBack = $"Common.onError('You do not have permission!');"
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult TemplateComponentEdit(TemplateComponentEditViewModel model)
        {
            if (User.CheckPermission(Permission.LandingPage))
            {
                if (ModelState.IsValid)
                {
                    var request = Mapper.Map<TemplateComponentEditRequest>(model);
                    var result = _contentServices.TemplateComponentEdit(request, User.Identity.GetClaims("token"));
                    if (result.IsOK())
                    {
                        return Json(new JsonClientResult
                        {
                            Success = true,
                            CallBack = "templateComponent.onRendAfterTemplateComponentUpdate('Update thành công');"
                        }, JsonRequestBehavior.AllowGet);
                    }
                    return Json(new JsonClientResult
                    {
                        Message = result.Errors.GetResultError()
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

        [HttpGet]
        public JsonResult TemplateComponentDelete(int id)
        {
            if (id != 0)
            {
                var result = _contentServices.TemplateComponentDelete(id, User.Identity.GetClaims("token"));
                if (result.IsOK())
                {
                    return Json(new JsonClientResult
                    {
                        Success = true,
                        CallBack = "templateComponent.onRendAfterTemplateComponentUpdate('Xóa thành công');"
                    }, JsonRequestBehavior.AllowGet);
                }
                return Json(new JsonClientResult
                {
                    CallBack = result.Errors.CB_UpdateErrorResult()
                });
            }
            return Json(new JsonClientResult
            {
                Message = "Id cannot be null or empty"
            });
        }
        #endregion

        #region Template
        public ActionResult Template()
        {
            if (User.CheckPermission(Permission.LandingPage))
            {
                return View();
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public JsonResult TemplateGetByPage(TemplateGetByPageViewModel model)
        {
            if (User.CheckPermission(Permission.SettingView))
            {
                if (model.key == null) model.key = "";
                if (model.key.CheckForKey())
                {
                    var request = Mapper.Map<TemplateGetByPageRequest>(model);
                    request.PageIndex = model.page;
                    request.PageSize = model.perpage;
                    request.FieldName = model.field;
                    request.Orderby = model.sort;
                    request.Keyword = model.key;
                    var result = _contentServices.TemplateGetByPage(request, User.Identity.GetClaims("token"));
                    if (result.IsOK() && result.Data != null)
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
            }
            return Json(new JsonDataTable
            {
                meta = null,
                data = new List<TemplateResponse>()
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult TemplateGetByPageForSuggest(string key)
        {
            if (User.CheckPermission(Permission.SettingView))
            {
                if (key == null) key = "";
                if (key.CheckForKey())
                {
                    var request = new TemplateGetByPageRequest();
                    request.Keyword = key;
                    request.PageIndex = 1;
                    request.FieldName = "CreatedDate";
                    request.Orderby = "Desc";
                    request.PageSize = 30;
                    var result = _contentServices.TemplateGetByPageForSuggest(request, User.Identity.GetClaims("token"));
                    if (result.IsOK() && result.Data != null)
                    {
                        return Json(new JsonDataTable
                        {
                            data = result.Data.Result
                        }, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            return Json(new JsonDataTable
            {
                data = new List<TemplateResponse>()
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult TemplateAddOrEditModal(int? id)
        {
            if (User.CheckPermission(Permission.LandingPage))
            {
                if (id == null)
                {
                    return PartialView("~/Views/LandingPage/Partial/Template/_AddOrEditModal.cshtml", new TemplateResponse()
                    {
                        Id = 0
                    });
                }
                else
                {
                    var result = _contentServices.TemplateGetById((int)id, User.Identity.GetClaims("token"));
                    if (result.IsOK())
                    {
                        return PartialView("~/Views/LandingPage/Partial/Template/_AddOrEditModal.cshtml", result.Data);
                    }
                    return PartialView("~/Views/Shared/_ErrorViewModal.cshtml", new ErrorViewModel() { Title = result.Errors.GetResultError() });
                }
            }
            return PartialView("~/Views/Shared/_ErrorViewModal.cshtml", new ErrorViewModel()
            {
                Title = "You do not have permission!"
            });
        }

        [HttpPost]
        public JsonResult TemplateAdd(TemplateAddViewModel model)
        {
            if (User.CheckPermission(Permission.LandingPage))
            {
                var request = Mapper.Map<TemplateAddRequest>(model);
                var result = _contentServices.TemplateAdd(request, User.Identity.GetClaims("token"));
                if (result.IsOK())
                {
                    return Json(new JsonClientResult
                    {
                        Success = true,
                        CallBack = "template.onRendAfterTemplateUpdate('Thêm mới thành công');"
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

        [HttpPost]
        public JsonResult TemplateEdit(TemplateEditViewModel model)
        {
            if (User.CheckPermission(Permission.LandingPage))
            {
                if (ModelState.IsValid)
                {
                    var request = Mapper.Map<TemplateEditRequest>(model);
                    var result = _contentServices.TemplateEdit(request, User.Identity.GetClaims("token"));
                    if (result.IsOK())
                    {
                        return Json(new JsonClientResult
                        {
                            Success = true,
                            CallBack = "template.onRendAfterTemplateEdit('Update thành công');"
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

        [HttpGet]
        public JsonResult TemplateDelete(int id)
        {
            if (id != 0)
            {
                var result = _contentServices.TemplateDelete(id, User.Identity.GetClaims("token"));
                if (result.IsOK())
                {
                    return Json(new JsonClientResult
                    {
                        Success = true,
                        CallBack = "template.onRendAfterTemplateUpdate('Xóa thành công');"
                    }, JsonRequestBehavior.AllowGet);
                }
                return Json(new JsonClientResult
                {
                    CallBack = result.Errors.CB_UpdateErrorResult()
                });
            }
            return Json(new JsonClientResult
            {
                Message = "Id cannot be null or empty"
            });
        }

        public ActionResult TemplateDetail(int id)
        {
            if (id != 0)
            {
                var result = _contentServices.TemplateGetById(id, User.Identity.GetClaims("token"));
                if (result.IsOK())
                {
                    return View(result.Data);
                }
                return View("~/Views/Shared/Error.cshtml", new ErrorViewModel()
                {
                    Title = result.Errors.GetResultError()
                });
            }
            return View("~/Views/Shared/Error.cshtml", new ErrorViewModel()
            {
                Title = $"Id is not be null or empty. Id is {id}"
            });
        }

        public JsonResult TemplateGetByTemplate(int id)
        {
            if (id != 0)
            {
                var result = _contentServices.TemplateComponentGetByTemplate(id, User.Identity.GetClaims("token"));
                if (result.IsOK())
                {
                    return Json(new JsonClientResult()
                    {
                        Success = true,
                        Data = RenderViewToString("~/Views/LandingPage/Partial/Template/_Table.cshtml", result.Data)
                    }, JsonRequestBehavior.AllowGet);
                }
                return Json(new JsonClientResult()
                {
                    Success = true,
                    Data = RenderViewToString("~/Views/Shared/_ErrorViewModal.cshtml", new ErrorViewModel()
                    {
                        Title = result.Errors.GetResultError()
                    })
                }, JsonRequestBehavior.AllowGet);
            }
            return Json(new JsonClientResult()
            {
                Message = $"Id is not be null or empty. Id is {id}"
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult TemplateMapModal(int id)
        {
            if (User.CheckPermission(Permission.LandingPage))
            {
                return PartialView("~/Views/LandingPage/Partial/Template/_MapModal.cshtml", id);

            }
            return PartialView("~/Views/Shared/_ErrorViewModal.cshtml", new ErrorViewModel()
            {
                Title = "You do not have permission!"
            });
        }

        public JsonResult TemplateMap(TemplateMapViewModel model)
        {
            if (User.CheckPermission(Permission.LandingPage))
            {
                var request = Mapper.Map<TemplateMapRequest>(model);
                var result = _contentServices.TemplateMap(request, User.Identity.GetClaims("token"));
                if (result.IsOK())
                {
                    return Json(new JsonClientResult
                    {
                        Success = true,
                        CallBack = "template.onRendAfterTemplateUpdate('Map thành công!');"
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

        [HttpGet]
        public JsonResult TemplateMapEdit(TemplateMapEditViewModel model)
        {
            if (User.CheckPermission(Permission.LandingPage))
            {
                if (ModelState.IsValid)
                {
                    var request = Mapper.Map<TemplateMapEditRequest>(model);
                    var result = _contentServices.TemplateMapEdit(request, User.Identity.GetClaims("token"));
                    if (result.IsOK())
                    {
                        return Json(new JsonClientResult
                        {
                            Success = true,
                            CallBack = "template.onRendAfterTemplateUpdate('Update idx thành công!');"
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

        public JsonResult TemplateDeleteMap(int id)
        {
            if (id != 0)
            {
                var result = _contentServices.TemplateMapDelete(id, User.Identity.GetClaims("token"));
                if (result.IsOK())
                {
                    return Json(new JsonClientResult
                    {
                        Success = true,
                        CallBack = "template.onRendAfterTemplateUpdate('Xóa map thành công!');"
                    }, JsonRequestBehavior.AllowGet);
                }
                return Json(new JsonClientResult
                {
                    CallBack = result.Errors.CB_UpdateErrorResult()
                });
            }
            return Json(new JsonClientResult()
            {
                Message = $"Id is not be null or empty. Id is {id}"
            }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Page
        public ActionResult Page()
        {
            if (User.CheckPermission(Permission.LandingPage))
            {
                return View();
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public ActionResult PageGetByPage(PageGetByPageViewModel model)
        {
            if (User.CheckPermission(Permission.LandingPageView))
            {
                if (model.key == null) model.key = "";
                if (model.key.CheckForKey())
                {
                    var request = Mapper.Map<PageGetByPageRequest>(model);
                    request.PageIndex = model.page;
                    request.PageSize = model.perpage;
                    request.FieldName = model.field;
                    request.Orderby = model.sort;
                    request.Keyword = model.key;
                    request.Status = model.Status;
                    if (model.TemplateId != null) request.TemplateId = (int)model.TemplateId;
                    var result = _contentServices.PageGetByPage(request, User.Identity.GetClaims("token"));
                    if (result.IsOK() && result.Data != null)
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
                data = new List<PageResponse>()
            }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult PageAddOrEditModal(Guid? id, int? type)
        {
            if (User.CheckPermission(Permission.LandingPage))
            {
                if (id == null || id == Guid.Empty)
                {
                    return PartialView("~/Views/LandingPage/Partial/Page/_AddOrEditModal.cshtml", new PageResponse()
                    {
                        Id = Guid.Empty
                    });
                }
                else
                {
                    var result = _contentServices.PageGetById((Guid)id, User.Identity.GetClaims("token"));
                    if (result.IsOK())
                    {
                        return PartialView("~/Views/LandingPage/Partial/Page/_AddOrEditModal.cshtml", result.Data);
                    }
                    return PartialView("~/Views/Shared/_ErrorViewModal.cshtml", new ErrorViewModel() { Title = result.Errors.GetResultError() });
                }
            }
            return PartialView("~/Views/Shared/_ErrorViewModal.cshtml", new ErrorViewModel()
            {
                Title = "You do not have permission!"
            });
        }

        [HttpPost]
        public JsonResult PageAdd(PageAddViewModel model)
        {
            if (User.CheckPermission(Permission.LandingPageAdd))
            {
                var request = Mapper.Map<PageAddRequest>(model);
                var result = _contentServices.PageAdd(request, User.Identity.GetClaims("token"));
                if (result.IsOK())
                {
                    return Json(new JsonClientResult
                    {
                        Success = true,
                        CallBack = "pageFunc.onRendAfterUpdatePage('Thêm mới thành công');"
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
        [HttpPost]
        public JsonResult PageEdit(PageEditViewModel model)
        {
            if (User.CheckPermission(Permission.LandingPageEdit))
            {
                if (ModelState.IsValid)
                {
                    var request = Mapper.Map<PageEditRequest>(model);
                    var result = _contentServices.PageEdit(request, User.Identity.GetClaims("token"));
                    if (result.IsOK())
                    {
                        return Json(new JsonClientResult
                        {
                            Success = true,
                            CallBack = "pageFunc.onRendAfterUpdatePage('Update thành công')"
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

        [HttpGet]
        public JsonResult PageDelete(Guid id)
        {
            if (User.CheckPermission(Permission.LandingPageEdit))
            {
                if (id != Guid.Empty)
                {
                    var result = _contentServices.PageDelete(id, User.Identity.GetClaims("token"));
                    if (result.IsOK())
                    {
                        return Json(new JsonClientResult
                        {
                            Success = true,
                            CallBack = "pageFunc.onRendAfterUpdatePage('Xóa thành công');"
                        }, JsonRequestBehavior.AllowGet);
                    }
                    return Json(new JsonClientResult
                    {
                        CallBack = result.Errors.CB_UpdateErrorResult()
                    });
                }
                return Json(new JsonClientResult
                {
                    Message = $"Id cannot be null or empty. Id = {id}"
                });
            }
            return Json(new JsonClientResult
            {
                CallBack = $"Common.onError('You do not have permission!');"
            }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult PageEditStatus(PageUpdateStatusViewModel model)
        {
            if (User.CheckPermission(Permission.LandingPageEdit))
            {
                if (ModelState.IsValid)
                {
                    var request = Mapper.Map<PageUpdateStatusRequest>(model);
                    var result = _contentServices.PageStatusEdit(request, User.Identity.GetClaims("token"));
                    if (result.IsOK())
                    {
                        return Json(new JsonClientResult
                        {
                            Success = true,
                            CallBack = "pageFunc.onRendAfterUpdatePage('Update status thành công!');"
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

        public ActionResult PageDetail(Guid id)
        {
            if (User.CheckPermission(Permission.LandingPageView))
            {
                if (id != Guid.Empty)
                {
                    var result = _contentServices.PageGetById(id, User.Identity.GetClaims("token"));
                    if (result.IsOK())
                    {
                        return View(result.Data);
                    }
                    return View("~/Views/Shared/Error.cshtml", new ErrorViewModel()
                    {
                        Title = result.Errors.GetResultError()
                    });
                }
                return View("~/Views/Shared/Error.cshtml", new ErrorViewModel()
                {
                    Title = $"Id is not null or empty. Id = {id}",
                });
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public JsonResult PageUpdateURLTarget(PageUpdateURLTargetViewModel model)
        {
            if (User.CheckPermission(Permission.LandingPageEdit))
            {
                if (ModelState.IsValid)
                {
                    var request = Mapper.Map<PageUpdateURLTargetRequest>(model);
                    var result = _contentServices.PageURLTargetEdit(request, User.Identity.GetClaims("token"));
                    if (result.IsOK())
                    {
                        return Json(new JsonClientResult
                        {
                            Success = true,
                            CallBack = "Common.onSuccess()"
                        });
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
        #endregion

        #region Page component
        public JsonResult PageComponentGetByPageId(Guid id)
        {
            if (User.CheckPermission(Permission.LandingPageView))
            {
                if (id != Guid.Empty)
                {
                    var result = _contentServices.PageComponentGetByPageId(id, User.Identity.GetClaims("token"));
                    if (result.IsOK())
                    {
                        return Json(new JsonClientResult()
                        {
                            Success = true,
                            Data = RenderViewToString("~/Views/LandingPage/Partial/PageTemplate/_Table.cshtml", result.Data.OrderBy(d => d.Idx).ToList())
                        }, JsonRequestBehavior.AllowGet);
                    }
                    return Json(new JsonClientResult()
                    {
                        Data = RenderViewToString("~/Views/Shared/_ErrorViewModal.cshtml", new ErrorViewModel()
                        {
                            Title = result.Errors.GetResultError()
                        })
                    }, JsonRequestBehavior.AllowGet);
                }
                return Json(new JsonClientResult()
                {
                    Data = RenderViewToString("~/Views/Shared/_ErrorViewModal.cshtml", new ErrorViewModel()
                    {
                        Title = $"Id is not null or empty. Id = {id}"
                    })
                }, JsonRequestBehavior.AllowGet);
            }
            return Json(new JsonClientResult()
            {
                Message = "You do not have permission!"
            });
        }
        public ActionResult PageComponentAddOrEditModal(Guid? id, Guid? PageId, int? templateId, string viewImage)
        {
            var resultToView = new ToViewPageComponentAddOrEditViewModel();
            if (id == null || PageId != null)
            {
                if (User.CheckPermission(Permission.LandingPageAdd))
                {
                    resultToView.Category = _libServices.CategoryGetAll(User.Identity.GetClaims("token")).Data;
                    resultToView.Destination = _libServices.DestinationGetAll(User.Identity.GetClaims("token")).Data;
                    resultToView.PageId = PageId;
                    var templateComponent = _contentServices.TemplateComponentGetByTemplate((int)templateId, User.Identity.GetClaims("token"));
                    if (templateComponent.IsOK())
                    {
                        resultToView.TemplateComponent = templateComponent.Data;
                    }
                    else
                    {
                        resultToView.IsError = true;
                        resultToView.MessageErr = templateComponent.Errors.GetResultError();
                    }
                    return PartialView("~/Views/LandingPage/Partial/PageComponent/_PageComponentAddOrEditModal.cshtml", resultToView);
                }
            }
            else
            {
                if (User.CheckPermission(Permission.LandingPageEdit))
                {
                    var result = _contentServices.PageComponentGetById((Guid)id, User.Identity.GetClaims("token"));
                    if (result.IsOK())
                    {
                        resultToView.ViewImage = viewImage;
                        resultToView.Id = id;
                        resultToView.PageComponentResponse = result.Data;
                        return PartialView("~/Views/LandingPage/Partial/PageComponent/_PageComponentAddOrEditModal.cshtml", resultToView);
                    }
                    return PartialView("~/Views/Shared/_ErrorViewModal.cshtml", new ErrorViewModel() { Title = result.Errors.GetResultError() });
                }
            }
            return PartialView("~/Views/Shared/_ErrorViewModal.cshtml", new ErrorViewModel()
            {
                Title = "You do not have permission!"
            });
        }
        [HttpPost]
        public JsonResult PageComponentAdd(PageComponentAddViewModel model)
        {
            if (User.CheckPermission(Permission.LandingPageAdd))
            {
                var request = Mapper.Map<PageComponentAddRequest>(model);
                var result = _contentServices.PageComponentAdd(request, User.Identity.GetClaims("token"));
                if (result.IsOK())
                {
                    return Json(new JsonClientResult
                    {
                        Success = true,
                        CallBack = "pageComponent.onRendAfterUpdatePageComponent('Thêm mới thành công');"
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

        [HttpPost]
        public JsonResult PageComponentEdit(PageComponentEditViewModel model)
        {
            if (User.CheckPermission(Permission.LandingPageEdit))
            {
                if (ModelState.IsValid)
                {
                    var request = Mapper.Map<PageComponentEditRequest>(model);
                    var result = _contentServices.PageComponentEdit(request, User.Identity.GetClaims("token"));
                    if (result.IsOK())
                    {
                        return Json(new JsonClientResult
                        {
                            Success = true,
                            CallBack = "pageComponent.onRendAfterUpdatePageComponent('Update thành công');"
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
                CallBack = "Common.onError('You do not have permission!');"
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult PageComponentUpdateidx(PageComponentUpdateIdxViewModel model)
        {
            if (User.CheckPermission(Permission.LandingPageEdit))
            {
                if (!model.Components.IsNullOrEmpty())
                {
                    var request = Mapper.Map<PageComponentUpdateIdxRequest>(model);
                    var result = _contentServices.PageComponentEditIdx(request, User.Identity.GetClaims("token"));
                    if (result.IsOK())
                    {
                        return Json(new JsonClientResult
                        {
                            Success = true,
                        });
                    }
                    return Json(new JsonClientResult
                    {
                        CallBack = result.Errors.CB_UpdateErrorResult()
                    });
                }
                return Json(new JsonClientResult
                {
                    CallBack = "Common.onError(`Data is null or empty`);"
                });
            }
            return Json(new JsonClientResult
            {
                CallBack = "Common.onError('You do not have permission!');"
            }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult PageComponentStatusEdit(PageComponentStatusEditViewModel model)
        {
            if (User.CheckPermission(Permission.LandingPageEdit))
            {
                if (ModelState.IsValid)
                {
                    var request = Mapper.Map<PageComponentStatusEditRequest>(model);
                    var result = _contentServices.PageComponentEditStatus(request, User.Identity.GetClaims("token"));
                    if (result.IsOK())
                    {
                        return Json(new JsonClientResult
                        {
                            Success = true,
                            CallBack = "pageComponent.onRendAfterUpdatePageComponent('Update status thành công!');"
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

        public ActionResult PageComponentDetail(Guid id)
        {
            if (User.CheckPermission(Permission.LandingPageView))
            {
                if (id != Guid.Empty)
                {
                    var result = _contentServices.PageComponentGetById(id, User.Identity.GetClaims("token"));
                    if (result.IsOK())
                    {
                        var resultToView = new PageComponentDetailToViewViewModel();
                        var categoriesResult = _libServices.CategoryGetAll(User.Identity.GetClaims("token"));
                        if (categoriesResult.IsOK() && categoriesResult.Data != null)
                        {
                            resultToView.PageComponentResponse = result.Data;
                            bool isParent = categoriesResult.Data.Any(c => c.Id == result.Data.CategoryId);
                            var parentCategoryId = categoriesResult.Data.Where(cate => cate.Id == result.Data.CategoryId).ToList();
                            if (isParent)
                            {
                                if (categoriesResult.Data.Count > 0)
                                {
                                    resultToView.CategoriesResponse = String.Join(",", parentCategoryId.Select(c => c.Id)) + "," + String.Join(",", parentCategoryId[0].SubCategory.Select(sc => sc.Id));
                                }
                            }
                            else
                            {
                                if (categoriesResult.Data.Any(c => c.SubCategory.Any(sc => sc.Id == result.Data.CategoryId)))
                                {
                                    resultToView.CategoriesResponse = result.Data.CategoryId.ToString();
                                }
                            }
                        }
                        return View(resultToView);
                    }
                    return View("~/Views/Shared/Error.cshtml", new ErrorViewModel()
                    {
                        Title = result.Errors.GetResultError()
                    });
                }
                return View("~/Views/Shared/Error.cshtml", new ErrorViewModel()
                {
                    Title = $"Id is not null or empty. Id = {id}",
                });
            }
            return RedirectToAction("Index", "Home");
        }
        #endregion

        #region Page component items
        [HttpGet]
        public ActionResult PageComponentItemsGetByPage(PageComponentItemsGetByPageViewModel model)
        {
            if (User.CheckPermission(Permission.SettingView))
            {
                var request = Mapper.Map<PageComponentItemsGetByPageRequest>(model);
                request.PageIndex = model.page;
                request.PageSize = model.perpage;
                request.FieldName = model.field;
                request.Orderby = model.sort;
                if (model.PageComponentId != Guid.Empty)
                {
                    request.Filter.Add(new FilterRequest()
                    {
                        Name = "PageComponentId",
                        Values = model.PageComponentId.ToString(),
                        Opt = FilterOptionOpt.AND,
                        Opt1 = FilterOptionOpt1.EqualTo,
                        Type = FilterOptionType.Guid
                    });
                }
                if (model.Type != null)
                {
                    request.Filter.Add(new FilterRequest()
                    {
                        Name = "Type",
                        Values = model.Type.ToString(),
                        Opt = FilterOptionOpt.AND,
                        Opt1 = FilterOptionOpt1.EqualTo,
                        Type = FilterOptionType.Number
                    });
                }
                var result = _contentServices.PageComponentItemGetByPage(request, User.Identity.GetClaims("token"));
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
                meta = null,
                data = new List<PageComponentItemsResponse>()
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult PageComponentItemsAddOrEditModal(int? id, Guid? pageComponentId, int? type)
        {
            if (id == null)
            {
                if (User.CheckPermission(Permission.LandingPageAdd))
                {
                    if (
                        type == 1
                        || type == 2
                        || type == 8
                        || type == 9
                        || type == 10
                        || type == 11
                        || type == 12
                        || type == 13
                        || type == 14
                        || type == 17
                        || type == 18
                        || type == 20
                        || type == 21
                        || type == 22
                        || type == 24
                        || type == 25
                        )
                    {
                        return PartialView("~/Views/LandingPage/Partial/PageComponentItems/AddSpecTypeModal/_JustPicture.cshtml",
                            new ToViewPageComponentItemsAddOrEditViewModel()
                            {
                                PageComponentId = pageComponentId,
                                Type = (int)type
                            });
                    }
                    if (type == 3)
                    {
                        return PartialView("~/Views/LandingPage/Partial/PageComponentItems/AddSpecTypeModal/_APictureATitADescr.cshtml",
                            new ToViewPageComponentItemsAddOrEditViewModel()
                            {
                                PageComponentId = pageComponentId,
                                Type = (int)type
                            });
                    }
                    if (
                        type == 4
                        || type == 7
                        || type == 23
                        || type == 26
                        )
                    {
                        return PartialView("~/Views/LandingPage/Partial/PageComponentItems/AddSpecTypeModal/_OnlyPickProduct.cshtml",
                            new ToViewPageComponentItemsAddOrEditViewModel()
                            {
                                PageComponentId = pageComponentId,
                                Type = (int)type
                            });
                    }
                    if (
                        type == 5
                        || type == 19
                        )
                    {
                        return PartialView("~/Views/LandingPage/Partial/PageComponentItems/AddSpecTypeModal/_ProductAndPicture.cshtml",
                            new ToViewPageComponentItemsAddOrEditViewModel()
                            {
                                PageComponentId = pageComponentId,
                                Type = (int)type
                            });
                    }
                    if (type == 6)
                    {
                        return PartialView("~/Views/LandingPage/Partial/PageComponentItems/AddSpecTypeModal/_TitleAndLink.cshtml",
                            new ToViewPageComponentItemsAddOrEditViewModel()
                            {
                                PageComponentId = pageComponentId,
                                Type = (int)type
                            });
                    }
                    if (
                        type == 15
                        || type == 16
                        )
                    {
                        return PartialView("~/Views/LandingPage/Partial/PageComponentItems/AddSpecTypeModal/_APictureAndATitle.cshtml",
                            new ToViewPageComponentItemsAddOrEditViewModel()
                            {
                                PageComponentId = pageComponentId,
                                Type = (int)type
                            });
                    }
                    if (
                        type == 27
                        )
                    {
                        return PartialView("~/Views/LandingPage/Partial/PageComponentItems/AddSpecTypeModal/_Page.cshtml",
                            new ToViewPageComponentItemsAddOrEditViewModel()
                            {
                                PageComponentId = pageComponentId,
                                Type = (int)type
                            });
                    }

                    return PartialView("~/Views/Shared/_ErrorViewModal.cshtml", new ErrorViewModel()
                    {
                        Title = $"Type is not correct. Type = {type}!"
                    });
                }
            }
            else
            {
                if (User.CheckPermission(Permission.LandingPageEdit))
                {
                    var resultToView = new ToViewPageComponentItemsAddOrEditViewModel();
                    var result = _contentServices.PageComponentItemsGetById((int)id, User.Identity.GetClaims("token"));
                    if (result.IsOK())
                    {
                        if (
                            type == 1
                            || type == 2
                            || type == 8
                            || type == 9
                            || type == 10
                            || type == 11
                            || type == 12
                            || type == 13
                            || type == 14
                            || type == 17
                            || type == 18
                            || type == 20
                            || type == 21
                            || type == 22
                            || type == 24
                            || type == 25
                            )
                        {
                            resultToView.PageComponentItemsResponse = result.Data;
                            return PartialView("~/Views/LandingPage/Partial/PageComponentItems/AddSpecTypeModal/_JustPicture.cshtml", resultToView);
                        }
                        if (
                            type == 3
                            )
                        {
                            resultToView.PageComponentItemsResponse = result.Data;
                            return PartialView("~/Views/LandingPage/Partial/PageComponentItems/AddSpecTypeModal/_APictureATitADescr.cshtml", resultToView);
                        }
                        if (
                            type == 4
                            || type == 7
                            || type == 23
                            || type == 26
                            )
                        {
                            resultToView.PageComponentItemsResponse = result.Data;
                            return PartialView("~/Views/LandingPage/Partial/PageComponentItems/AddSpecTypeModal/_OnlyPickProduct.cshtml", resultToView);
                        }
                        if (
                            type == 5
                            || type == 19
                            )
                        {
                            resultToView.PageComponentItemsResponse = result.Data;
                            return PartialView("~/Views/LandingPage/Partial/PageComponentItems/AddSpecTypeModal/_ProductAndPicture.cshtml", resultToView);
                        }
                        if (type == 6)
                        {
                            resultToView.PageComponentItemsResponse = result.Data;
                            return PartialView("~/Views/LandingPage/Partial/PageComponentItems/AddSpecTypeModal/_TitleAndLink.cshtml", resultToView);
                        }
                        if (
                            type == 15
                            || type == 16
                            )
                        {
                            resultToView.PageComponentItemsResponse = result.Data;
                            return PartialView("~/Views/LandingPage/Partial/PageComponentItems/AddSpecTypeModal/_APictureAndATitle.cshtml", resultToView);
                        }
                        if (type == 27)
                        {
                            resultToView.PageComponentItemsResponse = result.Data;
                            return PartialView("~/Views/LandingPage/Partial/PageComponentItems/AddSpecTypeModal/_Page.cshtml", resultToView);
                        }
                        return PartialView("~/Views/Shared/_ErrorViewModal.cshtml", new ErrorViewModel()
                        {
                            Title = $"Type is not correct. Type = {type}!"
                        });
                    }
                    return PartialView("~/Views/Shared/_ErrorViewModal.cshtml", new ErrorViewModel() { Title = result.Errors.GetResultError() });
                }
            }
            return PartialView("~/Views/Shared/_ErrorViewModal.cshtml", new ErrorViewModel()
            {
                Title = "You do not have permission!"
            });
        }

        [HttpPost]
        public JsonResult PageComponentItemsAdd(PageComponentItemsAddViewModel model)
        {
            if (User.CheckPermission(Permission.LandingPageAdd))
            {
                var request = Mapper.Map<PageComponentItemsAddRequest>(model);
                var result = _contentServices.PageComponentItemAdd(request, User.Identity.GetClaims("token"));
                if (result.IsOK())
                {
                    return Json(new JsonClientResult
                    {
                        Success = true,
                        CallBack = "pageComponentItems.onRendAfterPageComponentItemsUpdateNoReload('Thêm mới thành công');"
                    }, JsonRequestBehavior.AllowGet);
                }
                return Json(new JsonClientResult
                {
                    Message = result.Errors.GetResultError()
                }, JsonRequestBehavior.AllowGet);
            }
            return Json(new JsonClientResult
            {
                CallBack = $"Common.onError('You do not have permission!');"
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult PageComponentItemsEdit(PageComponentItemsEditViewModel model)
        {
            if (User.CheckPermission(Permission.LandingPageEdit))
            {
                if (ModelState.IsValid)
                {
                    var request = Mapper.Map<PageComponentItemsEditRequest>(model);
                    var result = _contentServices.PageComponentItemsEdit(request, User.Identity.GetClaims("token"));
                    if (result.IsOK())
                    {
                        return Json(new JsonClientResult
                        {
                            Success = true,
                            CallBack = "pageComponentItems.onRendAfterPageComponentItemsUpdate('Update thành công');"
                        }, JsonRequestBehavior.AllowGet);
                    }
                    return Json(new JsonClientResult
                    {
                        Message = result.Errors.GetResultError()
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

        [HttpGet]
        public JsonResult PageComponentItemsDelete(int id)
        {

            if (User.CheckPermission(Permission.LandingPageEdit))
            {
                if (id != 0)
                {
                    var result = _contentServices.PageComponentItemsDelete(id, User.Identity.GetClaims("token"));
                    if (result.IsOK())
                    {
                        return Json(new JsonClientResult
                        {
                            Success = true,
                            CallBack = "pageComponentItems.onRendAfterPageComponentItemsUpdate('Xóa thành công');"
                        }, JsonRequestBehavior.AllowGet);
                    }
                    return Json(new JsonClientResult
                    {
                        CallBack = result.Errors.CB_UpdateErrorResult()
                    });
                }
                return Json(new JsonClientResult
                {
                    Message = "Id cannot be null or empty"
                });
            }
            return Json(new JsonClientResult
            {
                Message = "You do not have permission!"
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult PageComponentItemsUpdateidx(PageComponentItemsUpdateIdxViewModel model)
        {
            if (User.CheckPermission(Permission.LandingPageEdit))
            {
                if (!model.ComponentItems.IsNullOrEmpty())
                {
                    var request = Mapper.Map<PageComponentItemsUpdateIdxRequest>(model);
                    var result = _contentServices.PageComponentItemsEditIdx(request, User.Identity.GetClaims("token"));
                    if (result.IsOK())
                    {
                        return Json(new JsonClientResult
                        {
                            Success = true,
                        });
                    }
                    return Json(new JsonClientResult
                    {
                        CallBack = result.Errors.CB_UpdateErrorResult()
                    });
                }
                return Json(new JsonClientResult
                {
                    CallBack = "Common.onError(`Data is null or empty`);"
                });
            }
            return Json(new JsonClientResult
            {
                CallBack = "Common.onError('You do not have permission!');"
            }, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}