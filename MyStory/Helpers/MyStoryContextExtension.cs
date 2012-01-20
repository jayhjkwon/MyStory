using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MyStory.Models;

namespace MyStory.Helpers
{
    public static class MyStoryContextExtension
    {
        //public static Account GetCurrentUser(this MyStoryContext context, HttpContext httpContext)
        //{
        //    //if (!HttpContext.Current.Request.IsAuthenticated)
        //    //    return null;
        //    if (!httpContext.Request.IsAuthenticated)
        //        return null;

        //    return context.GetUserByEmail(HttpContext.Current.User.Identity.Name);
        //}

        public static Account GetUserByEmail(this MyStoryContext context, string email)
        {
            return context.Accounts.SingleOrDefault(a => a.Email == email);
        }

        //public static Blog GetBlogOfCurrentUser(this MyStoryContext context, HttpContext httpContext)
        //{
        //    //if (!HttpContext.Current.Request.IsAuthenticated)
        //    //    return null;
        //    if (!httpContext.Request.IsAuthenticated)
        //        return null;

        //    var email = HttpContext.Current.User.Identity.Name;
        //    return context.Blogs.SingleOrDefault(b => b.BlogOwner.Email == email);
        //}
    }

}