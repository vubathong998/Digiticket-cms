using System;
using System.Linq;
using Infrastructure.Extensions;
using System.Web.UI;
using System.Web.Mvc;
using System.Security.Principal;
using System.Collections.Generic;
using Services.Models;
using Microsoft.Ajax.Utilities;

namespace DigiticketCMS.Helpers
{
    public static class BaseClassHelpers
    {
        /// <summary>
        /// convert string to true false is like js
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsTrueOrFalse(this string value)
        {
            if (value == null
                || value == ""
                || value == "null"
                || value == "false"
                || value == "0"
                || value == "00000000-0000-0000-0000-000000000000"
                || value == "undefined"
                || value == "NaN")
                return false;
            return true;
        }
        public static bool IsTrueOrFalse(this int? value)
        {
            if (value == null
                || value <= 0
                || value == null)
                return false;
            return true;
        }
        public static bool IsTrueOrFalse(this Guid? value)
        {
            if (value == Guid.Empty || value == null) return false;
            return true;
        }
        /// <summary>
        /// trả về true false nhưng giud empty vẫn về true
        /// return true and false is like js but guid empty return true
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsTrueOrFalseButGuidEmpty(this string value)
        {
            if (value == null
                || value == ""
                || value == "null"
                || value == "false"
                || value == "0"
                || value == "undefined"
                || value == "NaN")
                return false;
            return true;
        }

        public static string FilterStrict(this string value)
        {
            if (value == null
                || value == "null"
                || value == "false"
                || value == "0"
                || value == "undefined"
                || value == "NaN")
                return "";
            return value;
        }

        public static bool CheckPermission(this IPrincipal principal, string permission)
        {
            var pers = principal.Identity.GetClaims("Permission").Split('~').ToList();
            return pers.Contains(permission);
        }
        public static bool CheckPermission(this IPrincipal principal, List<string> permission)
        {
            var pers = principal.Identity.GetClaims("Permission").Split('~').ToList();
            return permission.All(p => pers.Contains(p));
        }
        public static string CB_UpdateErrorResult(this ICollection<ApiError> error, bool keepModal = false)
        {
            if (!error.IsNullOrEmpty())
            {
                var theKeepModal__Str = keepModal ? ", true" : "";
                var errMess = String.Join(". ", error.Select(e => e.Message));
                return $"Common.onError(`{errMess}` {theKeepModal__Str});";
            }
            var theKeepModal__str = keepModal ? "``, true" : "";
            return $"Common.onError({theKeepModal__str});";

        }
        public static string GetResultError(this ICollection<ApiError> error)
        {
            if (!error.IsNullOrEmpty())
            {
                return String.Join(". ", error.Select(e => e.Message));
            }
            return "Common.onError();";
        }
        public static bool CheckForKey(this string value)
        {
            if (value != null)
                if (value.Length > 1 || value == "") return true;
            return false;
        }
    }
}