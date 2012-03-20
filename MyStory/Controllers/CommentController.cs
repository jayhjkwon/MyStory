using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyStory.Controllers
{
    public class CommentController : Controller
    {
        [ChildActionOnly]
        public ActionResult Sidebar()
        {
            return View();
        }

    }
}
