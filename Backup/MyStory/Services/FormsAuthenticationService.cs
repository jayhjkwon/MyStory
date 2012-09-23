using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace MyStory.Services
{
    public interface IAuthenticationService
    {
        void SetAuthCookie(string userName, bool rememberMe);
        void LogOut();
    }

    public class FormsAuthenticationService : IAuthenticationService
    {
        public void SetAuthCookie(string userName, bool rememberMe)
        {
            FormsAuthentication.SetAuthCookie(userName, rememberMe);
        }

        public void LogOut()
        {
            FormsAuthentication.SignOut();
        }
    }
}