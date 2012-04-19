using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyStory.Controllers;
using MyStory.Models;
using System.Web.Mvc;
using MyStory.ViewModels;
using MyStory.Services;
using Moq;

namespace MyStory.Tests.FunctionalTests.Controllers
{
    [TestClass]
    public class AccountControllerTest
    {
        private AccountController _controller;
        private MyStoryContext _dbContext;

        // Use TestInitialize to run code before running each test 
        [TestInitialize()]
        public void TestInitialize()
        {
            // Arrange
            _dbContext = new MyStoryContext();
            _dbContext.Database.Delete();
            _dbContext.Database.Create();
        }

        // Use TestCleanup to run code after each test has run
        [TestCleanup()]
        public void TestCleanup()
        {
            if (_controller != null)
            {
                _dbContext.Database.Delete();
                _controller.Dispose();
            }
        }

        [TestMethod]
        public void Only_One_Account_Should_Be_Registered()
        {
            // Arrange
            FunctionalTestHelper.CreateAccountAndBlog(_dbContext);
            _controller = new AccountController();

            // Act
            var result = _controller.Register() as ViewResult;

            // Assert
            result.ViewName.ShouldEqual("Register");
            var exist = (bool)result.ViewBag.AlreadyOneAccountExist;
            exist.ShouldBeTrue();
        }

        [TestMethod]
        public void Register_Method_Should_Validate_Model()
        {
            // Arrange
            _controller = new AccountController();
            _controller.ModelState.AddModelError("error", "model error");

            // Act
            var result = _controller.Register(new AccountInput()) as ViewResult;

            // Assert
            result.ViewName.ShouldEqual("Register");
            var model = result.Model as AccountInput;
            model.AccountDescription.ShouldBeNull();
            model.AccountEmail.ShouldBeNull();
            model.AccountName.ShouldBeNull();
            model.AccountPassword.ShouldBeNull();
            model.BlogTitle.ShouldBeNull();
            model.IsGravatarUse.ShouldBeFalse();
            model.RememberMe.ShouldBeFalse();
        }

        [TestMethod]
        public void Register_Account()
        {
            // Arrange
            var mock = new Mock<IAuthenticationService>();
            mock.Setup(x => x.SetAuthCookie(It.IsAny<string>(), It.IsAny<bool>())).Verifiable();

            _controller = new AccountController(mock.Object);

            var input = new AccountInput
                            {
                                BlogTitle = "blog title",
                                AccountEmail = "x@x.com",
                                AccountName = "accountname",
                                AccountPassword = "password",
                                AccountDescription = "description",
                                IsGravatarUse = true
                            };

            // Act
            var result = _controller.Register(input) as RedirectToRouteResult;

            // Assert
            result.RouteValues["controller"].ShouldEqual("Home");
            result.RouteValues["action"].ShouldEqual("Index");
            _dbContext.Accounts.Count().ShouldEqual(1);
            _dbContext.Blogs.Count().ShouldEqual(1);
        }
    }
}
