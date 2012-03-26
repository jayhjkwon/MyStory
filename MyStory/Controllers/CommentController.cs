using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyStory.Controllers
{
    public class CommentController : MyStoryController
    {
        [ChildActionOnly]
        public ActionResult Sidebar()
        {
            return View();
        }

        public ActionResult List(int postId)
        {
            if (postId == null)
                return View("List");

            var comments = dbContext.Comments.Where(c => c.PostId == postId);

            return View("List", comments);
        }

    }
}
