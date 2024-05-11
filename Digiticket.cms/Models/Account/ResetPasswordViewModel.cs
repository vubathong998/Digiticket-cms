using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DigiticketCMS.Models.ViewModel
{
    public class ResetPasswordViewModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string Code { get; set; }
        public string ReturnUrl { get; set; }
    }
}