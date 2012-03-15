using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyStory.Helpers;

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
            var tags = dbContext.Tags.Where(t => t.TagText.ToUpper().Contains(term.ToUpper())).Select(t => t.TagText).ToList();
            
            // #2
            //var allTags = dbContext.Tags.ToList();
            //var tags = allTags.Where(t => t.TagText.Contains(term, StringComparison.OrdinalIgnoreCase)).Select(t=>t.TagText).ToList();

            if (Request.IsAjaxRequest())
            {
                return Json(tags, JsonRequestBehavior.AllowGet);
            }

            return View("Search", tags);
        }

    }
}
