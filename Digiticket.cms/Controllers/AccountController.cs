using AutoMapper;
using DigiticketCMS.Helpers.Cookie;
using DigiticketCMS.Models;
using DigiticketCMS.Models.ViewModel;
using Infrastructure.Config;
using Infrastructure.Extensions;
using Newtonsoft.Json;
using Services.Interfaces;
using Services.Models;
using System;
using System.Linq;
using System.Security.Policy;
using System.Web.Mvc;
using Utilities.Logs;

namespace DigiticketCMS.Controllers
{
    public class AccountController : BaseController
    {
        #region Constructor
        private IAccountServices _accountServices;
        public AccountController(IAccountServices accountServices)
        {
            _accountServices = accountServices;
        }
        #endregion

        #region Login
        [AllowAnonymous]
        public ActionResult Login(string ReturnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            ViewBag.ReturnUrl = ReturnUrl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model, string ReturnUrl)
        {
            if (ModelState.IsValid)
            {
                LoginRequest request = new LoginRequest();
                request = Mapper.Map<LoginRequest>(model);
                var result = _accountServices.LoginCms(request);
                if (result.Code > 0)
                {
                    if (result.Data != null)
                    {
                        UserCookie userCookie = new UserCookie();
                        userCookie = Mapper.Map<UserCookie>(result.Data);
                        var token = userCookie.AccessToken;
                        var resWG = _accountServices.GetListWorkgroup(token);
                        NLogManager.LogMessage(string.Format("Login ResWG: {0}. Access token: {1} ", resWG, token));
                        if (resWG.Code > 0 && !resWG.Data.IsNullOrEmpty())
                        {
                            var res = _accountServices.SelectWorkgroup(resWG.Data.Where(x => x.Code.Contains(WorkgroupConfig.WorkgroupDefault)).FirstOrDefault().Id.ToString(), token);
                            NLogManager.LogMessage(string.Format("Login res before ResWG: {0}. data {1}. token: {2} ", resWG, resWG.Data.Where(x => x.Code.Contains(WorkgroupConfig.WorkgroupDefault)).FirstOrDefault().Id.ToString(), token));
                            if (res.Code > 0 && res.Data != null)
                            {
                                userCookie = Mapper.Map<UserCookie>(res.Data);
                                userCookie.ExpiresDate = DateTime.Now.AddSeconds(userCookie.Expires);
                                CookieManage.SetCookie(SystemConfig.System_AuthCookieName,
                                                       JsonConvert.SerializeObject(userCookie),
                                                       TimeSpan.FromSeconds(userCookie.Expires + 3600));
                                if (!string.IsNullOrEmpty(ReturnUrl) && ReturnUrl != "/")
                                {
                                    return Redirect(ReturnUrl);
                                }
                                return RedirectToAction("Index", "Home");
                            }
                            else
                            {
                                ViewBag.Message = "Tài khoản không hợp lệ!";
                            }
                        }
                    }
                    else
                    {
                        ViewBag.Message = "Tài khoản không hợp lệ!";
                    }
                }
                else if (result.Code == -13) // chưa xác nhận
                {
                    ResendConfirmAccountRequest req = new ResendConfirmAccountRequest()
                    {
                        UserName = model.Username
                    };
                    var res = _accountServices.ResendConfirmAccount(req);
                    if (result.Code > 0)
                    {
                        ViewBag.Resend = true;
                    }
                    else
                    {
                        ViewBag.Resend = false;
                    }
                    return RedirectToAction("ConfirmAccount", "Account", new { model.Username, ReturnUrl });
                }
                else
                {
                    ViewBag.Message = result.Message;
                }
            }
            else
            {
                ViewBag.Message = GetModelStateError();
            }
            return View(model);
        }
        #endregion

        #region Register
        [AllowAnonymous]
        public ActionResult Register(string ReturnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            ViewBag.ReturnUrl = ReturnUrl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterViewModel model, string ReturnUrl)
        {
            if (ModelState.IsValid)
            {
                RegisterRequest request = new RegisterRequest();
                request = Mapper.Map<RegisterRequest>(model);
                var result = _accountServices.RegisterCms(request);
                if (result.Code > 0)
                {
                    return RedirectToAction("ConfirmAccount", "Account", new { username = model.Username, returnUrl = ReturnUrl });
                }
                else
                {
                    ViewBag.Message = result.Message;
                }
            }
            else
            {
                ViewBag.Message = GetModelStateError();
            }
            return View(model);
        }
        #endregion

        #region ConfirmAccount
        [HttpGet]
        public ActionResult ConfirmAccount(string username, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            if (string.IsNullOrEmpty(username))
            {
                return RedirectToAction("Login", "Account");
            }
            return View(new ConfirmAccountViewModel() { Username = username, ReturnUrl = returnUrl });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult ConfirmAccount(ConfirmAccountViewModel model)
        {
            if (ModelState.IsValid)
            {
                ConfirmAccountRequest request = new ConfirmAccountRequest();
                request = Mapper.Map<ConfirmAccountRequest>(model);
                var result = _accountServices.ConfirmAccount(request);
                if (result.Code > 0)
                {
                    return Json(new JsonClientResult()
                    {
                        Success = true,
                        Message = result.Message,
                        CallBack = "swal.fire({type: 'success',  title: 'Thành công!', text: 'Vui lòng đăng nhập.', onAfterClose: function () { window.location.href = '" + SystemConfig.System_Domain + "/Account/Login?ReturnUrl=" + model.ReturnUrl + "'; } });"
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new JsonClientResult()
                    {
                        Success = false,
                        Message = result.Message,
                        CallBack = ""
                    }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                ViewBag.Message = GetModelStateError();
                return Json(new JsonClientResult()
                {
                    Success = false,
                    Message = GetModelStateError(),
                    CallBack = ""
                }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult ResendConfirmAccount(string username, string returnUrl)
        {
            ResendConfirmAccountRequest request = new ResendConfirmAccountRequest()
            {
                UserName = username
            };
            var result = _accountServices.ResendConfirmAccount(request);
            if (result.Code > 0)
            {
                ViewBag.Resend = true;
            }
            else
            {
                ViewBag.Resend = false;
            }
            return RedirectToAction("ConfirmAccount", "Account", new { username, returnUrl });
        }
        #endregion

        #region ForgotPassword
        public ActionResult ForgotPassword(string ReturnUrl, string UserName)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            ViewBag.ReturnUrl = ReturnUrl;
            return View(new ForgotPasswordViewModel { Username = UserName });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ForgotPassword(ForgotPasswordViewModel model, string ReturnUrl)
        {
            if (ModelState.IsValid)
            {
                ForgotPasswordRequest request = new ForgotPasswordRequest();
                request = Mapper.Map<ForgotPasswordRequest>(model);
                var result = _accountServices.ForgotPassword(request);
                if (result.Code > 0)
                {
                    return RedirectToAction("ResetPassword", "Account", new { username = model.Username, returnUrl = ReturnUrl });
                }
                else
                {
                    ViewBag.Message = result.Message;
                }
            }
            else
            {
                ViewBag.Message = GetModelStateError();
            }
            return View(model);
        }
        #endregion

        #region ResetPassword
        public ActionResult ResetPassword(string username, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            return View(new ResetPasswordViewModel() { UserName = username, ReturnUrl = returnUrl });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult ResetPassword(ResetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                ResetPasswordRequest request = new ResetPasswordRequest();
                request = Mapper.Map<ResetPasswordRequest>(model);
                var result = _accountServices.ResetPassword(request);
                if (result.Code > 0)
                {
                    return Json(new JsonClientResult()
                    {
                        Success = true,
                        Message = result.Message,
                        CallBack = "swal.fire({type: 'success',  title: 'Thành công!', text: 'Vui lòng đăng nhập.', onAfterClose: function () { window.location.href = '" + SystemConfig.System_Domain + "/Account/Login?ReturnUrl=" + model.ReturnUrl + "'; } });"
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new JsonClientResult()
                    {
                        Success = false,
                        Message = result.Message,
                        CallBack = ""
                    }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                ViewBag.Message = GetModelStateError();
                return Json(new JsonClientResult()
                {
                    Success = false,
                    Message = GetModelStateError(),
                    CallBack = ""
                }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        [Authorize]
        public ActionResult Logout()
        {
            Response.Cookies[SystemConfig.System_AuthCookieName].Value = string.Empty;
            Response.Cookies[SystemConfig.System_AuthCookieName].Expires = DateTime.Now.AddMonths(-1);
            return RedirectToAction("Login", "Account");
        }
    }
}