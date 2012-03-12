using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyStory.Models;
using MyStory.Helpers;
using MyStory.ViewModels;
using AutoMapper;
using MarkdownDeep;

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
            if (!ModelState.IsValid)
                return View("Write", input);

            var blogId = GetCurrentBlog().Id;

            var post = Mapper.Map<PostInput, Post>(input);
            post.BlogId = blogId;
            post.DateCreated = post.DateModified = DateTime.Now;
            
            dbContext.Posts.Add(post);
            dbContext.SaveChanges();

            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var post = dbContext.Posts.SingleOrDefault(p => p.Id == id);
            var postInput = Mapper.Map<Post, PostInput>(post);
            return View("Edit", postInput);
        }

        [Authorize]
        [HttpPost]
        public ActionResult Edit(PostInput input)
        {
            if (!ModelState.IsValid)
                return View("Edit", input);

            var post = dbContext.Posts.Single(p => p.Id == input.Id);
            if (!TryUpdateModel(post))
            {
                return View();
            }
            post.DateModified = DateTime.Now;
            dbContext.SaveChanges();

            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        [HttpPost]
        public ActionResult Delete(int id)
        {
            var post = dbContext.Posts.Single(p => p.Id == id);
            if (post == null)
                return HttpNotFound();

            dbContext.Posts.Remove(post);
            dbContext.SaveChanges();

            if (Request.IsAjaxRequest()) 
            {
                return Json(new { success = true }); 
            } else
            {
                return RedirectToAction("Index", "Home"); 
            }
        }

        public ActionResult Detail(int id)
        {
            var post = dbContext.Posts.SingleOrDefault(p => p.Id == id);

            if (post == null)
                return HttpNotFound();

            var md = new Markdown();
            md.SafeMode = true;
            md.ExtraMode = true;
            post.Content = md.Transform(post.Content);

            var postDetailViewModel = Mapper.Map<Post, PostDetailViewModel>(post);

            return View("Detail", postDetailViewModel);
        }

    }
}
