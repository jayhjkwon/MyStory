using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyStory.Infrastructure
{
    public class OnlyBlogOwner : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);

            //if (!filterContext.HttpContext.User.Identity.IsAuthenticated)
            //    filterContext.Result = new HttpUnauthorizedResult();

            if (filterContext.Result is HttpUnauthorizedResult)
            {
                filterContext.Controller.ViewData.ModelState.AddModelError("auth", "user is not authorized");
            }
        }
    }
}