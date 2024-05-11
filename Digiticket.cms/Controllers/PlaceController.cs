using DigiticketCMS.Helpers.Cookie;
using DigiticketCMS.Models.Product;
using DigiticketCMS.Models;
using Infrastructure.Config;
using Infrastructure.Extensions;
using Newtonsoft.Json;
using Services.Models.Requests.Product;
using Services.Models.Responses.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Services.Interfaces;
using Services.Implement;

namespace DigiticketCMS.Controllers
{
    [Authorize]
    public class PlaceController : BaseController
    {
        private IPlaceServices _placeService;
        public PlaceController(IPlaceServices placeService)
        {
            _placeService = placeService;
        }

        public JsonResult SuggestPlaceInBaseInfo(string key)
        {
            if(key ==  null)
            {
                key = "";
            }
            var placeCookieName = PlaceInProductBaseInfoConfig.CookieName;
            var placeCookie = CookieManage.GetCookie(placeCookieName);
            if (string.IsNullOrEmpty(placeCookie))
            {
                var SSID__Result = _placeService.GetSSID(User.Identity.GetClaims("token"));
                if (SSID__Result.IsOK())
                {
                    var SSIDCookie = new { placeCookieName = SSID__Result.Data };
                    CookieManage.SetCookie(placeCookieName,
                                           JsonConvert.SerializeObject(SSIDCookie),
                                           TimeSpan.FromMinutes(10));
                }
                else
                {
                    return Json(new JsonClientResult
                    {
                        Success = true,
                        Data = new ProductPlaceSuggestResponse() { }
                    }, JsonRequestBehavior.AllowGet);
                }
            }
            var SSID = JsonConvert.DeserializeObject<ProductPlaceJSON_SSID>(CookieManage.GetCookie(placeCookieName));
            var request = new ProductPlaceSuggestRequest()
            {
                Keyword = key,
                Ssid = SSID.PlaceCookieName
            };
            var model = _placeService.Suggest(request, User.Identity.GetClaims("token"));
            if (model.IsOK())
            {
                return Json(new JsonClientResult
                {
                    Success = true,
                    Data = model.Data != null ? model.Data.Predictions : new List<Prediction>()
                }, JsonRequestBehavior.AllowGet);
            }
            return Json(new JsonClientResult
            {
                Success = true,
                Data = new ProductPlaceSuggestResponse() { }
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult DeleteTokenSSIC()
        {
            Response.Cookies[PlaceInProductBaseInfoConfig.CookieName].Value = string.Empty;
            Response.Cookies[PlaceInProductBaseInfoConfig.CookieName].Expires = DateTime.Now.AddMonths(-1);
            return Json(new { }, JsonRequestBehavior.AllowGet);
        }
    }
}