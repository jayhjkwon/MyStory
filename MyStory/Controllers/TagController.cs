using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyStory.Helpers;
using MyStory.ViewModels;
using MyStory.QueryObjects;
using AutoMapper;
using MyStory.Models;

namespace MyStory.Controllers
{
    public class TagController : MyStoryController
    {
        public ActionResult Search()
        {
            var term = Request.Params["term"].ToString();

            // TODO 
            // This is dilema, that is, L2E does not translate custom expression, we cannot use Contains() extension method
            // That means we need to convert TagText to upper case or lower cas to compare case insensitive in query level,
            // we know, this will cause performance problem
            // So, instead I just select everything then filtered in Linq To Object level
            // But, This altanative also causes performance problem
            // I need to find more elegance way to solve this issue.

            // #1
            var tags = DbContext.Tags.Where(t => t.TagText.ToUpper().Contains(term.ToUpper())).Select(t => t.TagText).ToList();
            
            // #2
            //var allTags = dbContext.Tags.ToList();
            //var tags = allTags.Where(t => t.TagText.Contains(term, StringComparison.OrdinalIgnoreCase)).Select(t=>t.TagText).ToList();

            if (Request.IsAjaxRequest())
            {
                return Json(tags, JsonRequestBehavior.AllowGet);
            }

            return View("Search", tags);
        }

        [ChildActionOnly]
        public ActionResult Sidebar()
        {
            var tags = from t in DbContext.Tags
                       where t.Posts.Count > 0
                       orderby t.Posts.Count descending
                       select new TagSidebarViewModel
                       {
                           Id = t.Id,
                           TagText = t.TagText,
                           Count = t.Posts.Count
                       };

            return View("Sidebar", tags.ToList());
        }

        public ActionResult Index(string tag=null, int page=1)
        {
            ViewBag.TagText = tag;

            int perPage = page == 1 ? 20 : 10;

            var posts = new PostQuery() 
                        {
                            CurrentPageNumber = page, 
                            PostsPerPage = perPage, 
                            Tag=tag 
                        }
                        .GetQuery(DbContext)
                        .ToList();

            var postListViewModel = Mapper.Map<List<Post>, List<PostListViewModel>>(posts);

            return View("Index", postListViewModel);
        }

    }
}
