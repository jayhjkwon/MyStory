using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyStory.Infrastructure.Common
{
    public class CommenterCookieManager
    {
        public readonly static string COMMENTER_COOKIE_NAME = "commenter";

        public static void SetCommenterCookieValue(HttpResponseBase response, string cookieValue)
        {
            if (string.IsNullOrWhiteSpace(cookieValue))
                return;

            response.Cookies.Add(new HttpCookie(COMMENTER_COOKIE_NAME, cookieValue) { });
        }

        public static string GetCommenterCookieValue(HttpRequestBase request)
        {
            var cookie = request.Cookies[COMMENTER_COOKIE_NAME];
            if (cookie != null)
                return cookie.Value;
            else
                return null;
        }
    }
}