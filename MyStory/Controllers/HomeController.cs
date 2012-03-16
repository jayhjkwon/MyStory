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

        public ActionResult Index()
        {
            ViewBag.NumberOfAccounts = dbContext.Accounts.Count();

            var md = new Markdown();
            md.SafeMode = true;
            md.ExtraMode = true;

            var posts = dbContext.Posts.OrderByDescending(p => p.DateCreated).ToList();
            foreach (var item in posts)
            {
                item.Content = md.Transform(item.Content.Length > 500 ? item.Content.Substring(0, 500) : item.Content);
                item.Title = item.Title.Length > 20 ? item.Title.Substring(0, 20) : item.Title;
            }

            return View("Index", posts);
        }

    }
}
