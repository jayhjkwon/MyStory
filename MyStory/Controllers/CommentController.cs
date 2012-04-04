using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyStory.ViewModels;
using MyStory.Models;
using AutoMapper;

namespace MyStory.Controllers
{
    public class CommentController : MyStoryController
    {
        [ChildActionOnly]
        public ActionResult Sidebar()
        {
            var recentComments = dbContext.Comments.OrderByDescending(c => c.DateCreated).Take(10).ToList();
            var recentCommentsViewModel = Mapper.Map<List<Comment>, List<CommentSidebarViewModel>>(recentComments);
            recentCommentsViewModel.ForEach(c =>
            {
                if (c.Content.Length > 50)
                    c.Content = c.Content.Substring(0, 50);
            });

            return View("Sidebar", recentCommentsViewModel);
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
                var commentSummary = new MyStory.ViewModels.PostCommentsViewModel.CommentSummary 
                { 
                    CommenterEmail = comment.Commenter.Email,
                    CommenterName = comment.Commenter.Name,
                    CommenterOpenId = comment.Commenter.OpenId,
                    CommenterUrl = comment.Commenter.Url,
                    Content = comment.Content,
                    DateCreated = comment.DateCreated,
                    Id = comment.Id
                };
                
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
            var commenter = dbContext.Commenters.SingleOrDefault(c => c.Email == input.Email) ?? GetCommenter(input);

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

        private Commenter GetCommenter(CommentInput input)
        {
            if (!string.IsNullOrWhiteSpace(input.OpenId) && dbContext.Commenters.Count(c => c.OpenId == input.OpenId) > 0)
            {
                // this means that user changed email and user name in comment input form that has a openid value as hidden field
                // that is, user does not want use openid, so that openid will not be saved
                return new Commenter
                {
                    Email = input.Email,
                    Name = input.Name,
                    Url = input.Url
                };
            }
            else
            {
                return new Commenter
                {
                    Email = input.Email,
                    Name = input.Name,
                    OpenId = input.OpenId,
                    Url = input.Url
                };
            }
        }

        

    }
}
