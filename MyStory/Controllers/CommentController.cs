using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyStory.ViewModels;
using MyStory.Models;

namespace MyStory.Controllers
{
    public class CommentController : MyStoryController
    {
        [ChildActionOnly]
        public ActionResult Sidebar()
        {
            return View();
        }

        public ActionResult PostCommentsList(int? postId)
        {
            if (postId == null)
                return View("PostCommentsList");

            var comments = dbContext.Comments.Where(c => c.PostId == postId).OrderBy(c=>c.DateCreated);

            var postCommentsViewModel = new PostCommentsViewModel();
            postCommentsViewModel.PostId = postId.Value;
            //TODO use automapper instead of manual mapping
            foreach (var comment in comments)
            {
                var commentSummary = new MyStory.ViewModels.PostCommentsViewModel.CommentSummary();
                commentSummary.CommenterEmail = comment.Commenter.Email;
                commentSummary.CommenterName = comment.Commenter.Name;
                commentSummary.CommenterOpenId = comment.Commenter.OpenId;
                commentSummary.CommenterUrl = comment.Commenter.Url;
                commentSummary.Content = comment.Content;
                commentSummary.DateCreated = comment.DateCreated;
                commentSummary.Id = comment.Id;
                postCommentsViewModel.Comments.Add(commentSummary);
            }

            return View("PostCommentsList", postCommentsViewModel);
        }

        [HttpPost]
        public ActionResult Write([Bind(Prefix="CommentInput")] CommentInput input, int id)
        {
            if (!ModelState.IsValid)
            {
                var errors = (from item in ModelState
                              where item.Value.Errors.Any()
                              select new { ErrorKey = item.Key, ErrorMessage = item.Value.Errors.FirstOrDefault().ErrorMessage }).ToDictionary(a => a.ErrorKey, a=>a.ErrorMessage);
                TempData["commentInputData"] = input;
                TempData["commentInputDataErrors"] = errors;
                return RedirectToAction("Detail", "Post", new { id = id, errorFromCommentInput = true });
            }

            // TODO handle commenter info when info in db is different from info from ui
            var commenter = dbContext.Commenters.SingleOrDefault(c => c.Email == input.Email) ?? new Commenter 
                {
                    Email = input.Email,
                    Name = input.Name,
                    OpenId = input.OpenId,
                    Url = input.Url
                };

            dbContext.Comments.Add(new Comment
            {
                PostId = id,
                Content = input.Content,
                DateCreated = DateTime.Now,
                Commenter = commenter
            });

            dbContext.SaveChanges();

            return RedirectToAction("Detail", "Post", new { id = id });
        }

        

    }
}
