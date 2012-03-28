using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyStory.Helpers
{
    public static class UrlHelperExtensions
    {
        public static string ConvertToAbsoluteUrl(this UrlHelper url, string relativeUrl)
        {
            string absoluteUrl = string.Empty;

            if (string.IsNullOrWhiteSpace(relativeUrl))
                return absoluteUrl;

            absoluteUrl = string.Format("{0}{1}", "http://", relativeUrl.ToLower().Replace("http://", ""));

            return absoluteUrl;
        }
    }
}