using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyStory.Models;
using MyStory.Helpers;
using MyStory.ViewModels;

namespace MyStory.Controllers
{
    public abstract class MyStoryController : Controller
    {
        // if lazy exception occurs in view, use OSIV pattern (e.g. HttpContext.Items)
        protected MyStoryContext dbContext;

        public MyStoryController()
        {
            dbContext = new MyStoryContext();
            var blog = dbContext.Blogs.FirstOrDefault();
            if (blog != null)
            {
                ViewBag.BlogName = blog.Title;
            }
        }

        protected override void Dispose(bool disposing)
        {
            dbContext.Dispose();
            base.Dispose(disposing);
        }

        protected Account GetCurrentUser()
        {
            if (!HttpContext.Request.IsAuthenticated)
                return null;

            return dbContext.SelectUserByEmail(HttpContext.User.Identity.Name);
        }

        protected Blog GetCurrentBlog()
        {
            if (!HttpContext.Request.IsAuthenticated)
                return null;

            var email = HttpContext.User.Identity.Name;
            return dbContext.Blogs.SingleOrDefault(b => b.BlogOwner.Email == email);
        }
    }
}
