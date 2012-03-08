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
    public class PostController : MyStoryController
    {
        public PostController(){}

        public PostController(MyStoryContext context)
        {
            base.dbContext = context;
        }

        [Authorize]
        [HttpGet]
        public ActionResult Write()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult Write(PostInput input)
        {
            // for test purpose, explicitly invoke controller lifecycle regarding validation
            //bool val = TryValidateModel(input);

            if (!ModelState.IsValid)
                return View("Write", input);

            var blogId = GetBlogOfCurrentUser().Id;

            var post = new Post
            {
                BlogId = blogId,
                Title = input.Title,
                ContentWithHtml = input.ContentWithHtml,
                DateCreated = DateTime.Now,
                DateModified = DateTime.MaxValue,
                LocationOfWriting = new Location{ Latitude = input.Latitude, Longitude = input.Longitude}   // LocationOfWriting is ComplexType so it needs to allocate instance
            };
            
            dbContext.Posts.Add(post);
            dbContext.SaveChanges();

            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var post = dbContext.Posts.SingleOrDefault(p => p.Id == id);
             return View("Edit", post);
        }

        [Authorize]
        [HttpPost]
        public ActionResult Edit(Post post)
        {
            var postDb = dbContext.Posts.SingleOrDefault(p => p.Id == post.Id);
            if (postDb == null) return View("Edit");

            if (TryUpdateModel(postDb))
            {
                postDb.DateModified = DateTime.Now;
                dbContext.SaveChanges();
            }
            else
            {
                // don't need to add modelstate error explicitly, error adds automatically
                return View("Edit", postDb);
            }

            return RedirectToAction("Index", "Home");
        }

    }
}
