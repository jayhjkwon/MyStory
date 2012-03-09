using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyStory.Models;
using MyStory.Helpers;
using MyStory.ViewModels;
using AutoMapper;

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
            return View("Edit", post);
        }

        [Authorize]
        [HttpPost]
        public ActionResult Edit(PostInput input)
        {
            if (!ModelState.IsValid)
                return View("Edit", input);

            var post = Mapper.Map<PostInput, Post>(input);
            post.BlogId = GetCurrentBlog().Id;
            post.DateModified = DateTime.Now;
            dbContext.Entry(post).State = System.Data.EntityState.Modified;
            dbContext.SaveChanges();

            return RedirectToAction("Index", "Home");
        }

    }
}
