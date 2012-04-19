﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyStory.Models;
using MyStory.ViewModels;
using System.Web.Security;

namespace MyStory.Controllers
{
    public class AccountController : MyStoryController
    {
        public ActionResult Register()
        {
            // because mystory is self-hosted blog engine
            // only one account-blog is allowed
            // so prevent creating account !!
            var numberOfAccounts = DbContext.Accounts.Count();
            ViewBag.NumberOfAccounts = numberOfAccounts;

            return View();
        }

        [HttpPost]
        public ActionResult Register(AccountInput accountInput)
        {
            if (!ModelState.IsValid)
                return View(accountInput);

            var blog = new Blog
            {
                Title = accountInput.BlogTitle,
                BlogOwner = new Account
                {
                    Email = accountInput.AccountEmail,
                    Name = accountInput.AccountName,
                    Password = accountInput.AccountPassword,
                    Description = accountInput.AccountDescription,
                    IsGravatarUse = accountInput.IsGravatarUse
                }
            };

            DbContext.Blogs.Add(blog);
            DbContext.SaveChanges();

            FormsAuthentication.SetAuthCookie(accountInput.AccountEmail, accountInput.RememberMe);
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public ActionResult LogOn()
        {
            return View();
        }

        [HttpPost]
        public ActionResult LogOn(LogOnInput input)
        {
            if (!ModelState.IsValid)
                return View(input);

            var accountFromDb = DbContext.Accounts.FirstOrDefault(a => a.Email == input.AccountEmail && a.Password == input.AccountPassword);

            if (accountFromDb == null)
            {
                ModelState.AddModelError("LogOnFailed", "Email or Password is wrong");
                return View(input);
            }
            else
            {
                FormsAuthentication.SetAuthCookie(input.AccountEmail, input.RememberMe);

                if (string.IsNullOrEmpty(input.ReturnUrl))
                    return RedirectToAction("Index", "Home");
                else
                    return Redirect(input.ReturnUrl);
            }

        }

        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }

        [ChildActionOnly]
        public ActionResult CurrentUser()
        {
            if (!HttpContext.Request.IsAuthenticated)
                return View("CurrentUser");

            var currentUserViewModel = GetCurrentUserViewModel();

            return View("CurrentUser", currentUserViewModel);
        }

        [ChildActionOnly]
        public ActionResult Sidebar()
        {
            var user = DbContext.Accounts.FirstOrDefault(a=> !a.Name.ToUpper().Contains("test"));
            if (user != null)
            {
                var blogOwner = new CurrentUserViewModel
                {
                    Email = user.Email,
                    Name = user.Name,
                    Description = user.Description,
                    IsGravatarUse = user.IsGravatarUse
                };

                return View("Sidebar", blogOwner);
            }

            return View("Sidebar");
            
        }

        private CurrentUserViewModel GetCurrentUserViewModel()
        {
            var user = GetCurrentUser();
            if (user == null) return null;

            var currentUser = new CurrentUserViewModel
            {
                Email = HttpContext.User.Identity.Name,
                Name = user.Name,
                Description = user.Description
            };

            return currentUser;
        }
    }
}
