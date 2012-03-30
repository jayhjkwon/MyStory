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
using MyStory.Services;
using System.Data.EntityClient;
using System.Configuration;
using MyStory.Infrastructure.Common;


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
        [ValidateInput(false)]
        public ActionResult Write(PostInput input)
        {
            if (!ModelState.IsValid)
                return View("Write", input);

            var blogId = GetCurrentBlog().Id;

            var post = Mapper.Map<PostInput, Post>(input);
            post.BlogId = blogId;
            post.DateCreated = post.DateModified = DateTime.Now;

            TagService svc = new TagService();
            svc.UpdateTag(dbContext, input, post);
            
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
            postInput.Tags = post.Tags.ConverTagToString();
            return View("Edit", postInput);
        }   

        [Authorize]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(PostInput input)
        {
            if (!ModelState.IsValid)
                return View("Edit", input);

            var post = dbContext.Posts.Single(p => p.Id == input.Id);

            if (TryUpdateModel(post, "", null, new string[]{"Tags"}))
            {
                post.DateModified = DateTime.Now;

                TagService svc = new TagService();
                svc.UpdateTag(dbContext, input, post);

                dbContext.Entry(post).State = System.Data.EntityState.Modified;
                dbContext.SaveChanges();

                return RedirectToAction("Detail", "Post", new { id = input.Id });
            }

            return View("Edit", input);
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="errorFromCommentInput">true, if errors occurred while saving comment</param>
        /// <returns></returns>
        public ActionResult Detail(int id, bool errorFromCommentInput=false)
        {
            //ViewBag.FaceBookAppId = ConfigurationManager.AppSettings["facebook.appid"];
            //ViewBag.FaceBookAppSecret = ConfigurationManager.AppSettings["facebook.appsecret"];

            var post = dbContext.Posts.SingleOrDefault(p => p.Id == id);

            if (post == null)
                return HttpNotFound();

            var md = new Markdown();
            md.SafeMode = true;
            md.ExtraMode = true;
            post.Content = md.Transform(post.Content);

            var postDetailViewModel = Mapper.Map<Post, PostDetailViewModel>(post);
            postDetailViewModel.Tags = post.Tags.ConverTagToStringArray();

            // set Commenter to post comment
            var commentInputData = TempData["commentInputData"] as CommentInput;    // tempdata when modelstateerror occurred in /Comment/Write
            if (errorFromCommentInput && commentInputData != null)
            {
                var modelStateErrors = TempData["commentInputDataErrors"] as Dictionary<string, string>;
                foreach (var item in modelStateErrors)
                {
                    ModelState.AddModelError(item.Key, item.Value);
                }

                postDetailViewModel.CommentInput = commentInputData as CommentInput;
            }
            else
            {
                SetCommenter(postDetailViewModel);
            }

            return View("Detail", postDetailViewModel);
        }

        private void SetCommenter(PostDetailViewModel vm)
        {
            // admin
            if (Request.IsAuthenticated)
            {
                var admin = base.GetCurrentUser();

                vm.CommentInput = new CommentInput
                {
                    Email = admin.Email,
                    Name = admin.Name,
                    IsBlogOwner = true
                };
                return;
            }

            // visitor
            var email = CommenterCookieManager.GetCommenterCookieValue(Request);
            var commenter = dbContext.Commenters.SingleOrDefault(c => c.Email == email);
            if (commenter != null)
            {
                vm.CommentInput = new CommentInput
                {
                    Email = commenter.Email,
                    Name = commenter.Name,
                    OpenId = commenter.OpenId,
                    Url = commenter.Url
                };
            }

        }

        

        

    }
}
