using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using MyStory.Models;
using MyStory.Models.Infrastructure;

namespace MyStory.Controllers
{
    public class HomeController : MyStoryController
    {
        public HomeController(){}

        // for test purpose
        public HomeController(MyStoryContext context)
        {
            base.dbContext = context;
        }

        public ActionResult Index()
        {
            ViewBag.NumberOfAccounts = dbContext.Accounts.Count();

            if (ViewBag.NumberOfAccounts == 0)
            {
                return View();
            }

            var posts = dbContext.Posts.Include(p => p.Blog);
                        
            return View("Index", posts);
        }

        public ActionResult Test()
        {
            var blog = dbContext.Blogs.Add(new Blog { Title = "t1", BlogOwner = new Account { Email = "email", Password = "password", FullName = "name" } });
            return View(blog);
        }

        [Authorize]
        public ActionResult About()
        {
            return View();
        }
    }
}
