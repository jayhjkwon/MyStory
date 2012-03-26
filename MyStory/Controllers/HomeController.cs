using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using MyStory.Models;
using MyStory.Models.Infrastructure;
using MarkdownDeep;
using AutoMapper;
using MyStory.ViewModels;

namespace MyStory.Controllers
{
    public class HomeController : MyStoryController
    {
        public HomeController() { }

        // for test purpose
        public HomeController(MyStoryContext context)
        {
            base.dbContext = context;
        }

        public ActionResult Index(int page=1)
        {
            ViewBag.NumberOfAccounts = dbContext.Accounts.Count();

            var md = new Markdown();
            md.SafeMode = true;
            md.ExtraMode = true;

            int perPage = page == 1 ? 20 : 10;
            var query = dbContext.Posts.OrderByDescending(p => p.DateCreated);
            var posts = query.Skip((page - 1) * perPage).Take(perPage).ToList();
            foreach (var item in posts)
            {
                // TODO move transform logic into PostMapper
                item.Content = md.Transform(item.Content.Length > 500 ? item.Content.Substring(0, 500) : item.Content);
            }

            var postListViewModel = Mapper.Map<List<Post>, List<PostListViewModel>>(posts);

            return View("Index", postListViewModel);
        }

       

    }
}
