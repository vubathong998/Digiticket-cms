using DigiticketCMS.Helpers.Language;
using DigiticketCMS.Models;
using Infrastructure.Extensions;
using Microsoft.Ajax.Utilities;
using Services.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace DigiticketCMS.Controllers
{
    public class BaseController : Controller 
    {
        public ActionResult SetLanguage(string lang)
        {
            try
            {
                LanguageManage.SetLanguage(lang);
                return Redirect(Request.UrlReferrer.AbsoluteUri);
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Home");
            }
        }
        protected ActionResult RedirectToErrorPage(string pageName = "NotFound")
        {
            return View(string.Format("../Error/{0}", pageName));
        }
        protected string GetModelStateError()
        {
            return string.Join("; ", ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage + x.Exception));
        }
        public string RenderViewToString(string viewName, object model = null)
        {
            if (string.IsNullOrEmpty(viewName))
                viewName = ControllerContext.RouteData.GetRequiredString("action");
            using (var sw = new StringWriter())
            {
                var result = PartialView(viewName);
                result.View = ViewEngines.Engines.FindPartialView(ControllerContext, viewName).View;
                if (model != null)
                {
                    result.ViewData = new ViewDataDictionary(model);
                }
                ViewContext vc = new ViewContext(ControllerContext, result.View, result.ViewData, result.TempData, sw);
                result.View.Render(vc, sw);
                return sw.GetStringBuilder().ToString();
            }
        }

        [HttpGet]
        public JsonResult SelectPageSize(string pagesize)
        {
            var html = RenderViewToString("_SelectPageSize", pagesize);
            return Json(new JsonClientResult
            {
                Success = true,
                Data = html
            }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Pagination(PagingInfo model)
        {
            var html = RenderViewToString("_Pagination", model);
            return Json(new JsonClientResult
            {
                Success = true,
                Data = html
            }, JsonRequestBehavior.AllowGet);
        }
    }
}