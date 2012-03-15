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

            if (Request.IsAjaxRequest())
            {
                var tags = new string[]{"ActionScript", "AppleScript", "Asp", "BASIC", "C", "C++", "Clojure", "COBOL", "ColdFusion", "Erlang",
        	"Fortran", "Groovy", "Haskell", "Java", "JavaScript", "Lisp", "Perl", "PHP", "Python", "Ruby", "Scala", "Scheme"};
                tags = tags.Where(t=>t.Contains(term, StringComparison.OrdinalIgnoreCase)).ToArray();

                return Json(tags, JsonRequestBehavior.AllowGet);
            }

            return View("Search", dbContext.Tags.ToList());
        }

    }
}
