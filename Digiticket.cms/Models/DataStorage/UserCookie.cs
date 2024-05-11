using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DigiticketCMS.Models
{
    public class UserCookie
    {
        public string AccessToken { get; set; }
        public int Expires { get; set; }
        public DateTime ExpiresDate { get; set; }
        public string RefreshToken { get; set; }
    }
}