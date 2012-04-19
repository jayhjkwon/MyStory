using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyStory.Controllers;
using MyStory.Models;
using System.Web.Mvc;
using System.Web;
using Moq;
using MyStory.ViewModels;
using MyStory.Services;
using System.Collections.Specialized;
using System.Web.Script.Serialization;

namespace MyStory.Tests.FunctionalTests.Controllers
{
    /// <summary>
    /// Summary description for PostControllerTest
    /// </summary>
    [TestClass]
    public class PostControllerTest
    {
        private PostController _controller;
        private MyStoryContext _dbContext;

        [TestInitialize()]
        public void TestInitialize()
        {
            // Arrange
            _dbContext = new MyStoryContext();
            _dbContext.Database.Delete(); 
            _dbContext.Database.Create();
        }

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
        public void invalid_model_should_not_save()
        {
            // Arrange
            _controller = new PostController();
            _controller.ModelState.AddModelError("modelerror", "modelerror");
            var postInput = new PostInput();
            
            // Act
            var result = _controller.Write(postInput) as ViewResult;

            // Assert
            result.ViewData.ModelState.IsValid.ShouldBeFalse();
            result.ViewName.ShouldEqual("Write");
        }

        [TestMethod]
        public void valid_model_should_be_saved()
        {
            // Arrange
            FunctionalTestHelper.CreateAutomapperMap();
            FunctionalTestHelper.CreateAccountAndBlog(_dbContext);

            var controllerContext = new Mock<ControllerContext>();
            controllerContext.SetupGet(x => x.HttpContext.Request.IsAuthenticated).Returns(true);
            controllerContext.SetupGet(x => x.HttpContext.User.Identity.Name).Returns("a@a.com");
            
            _controller = new PostController(new TagService());
            _controller.ControllerContext = controllerContext.Object;

            var postInput = new PostInput
            {
                Title = "title",
                Content = "content",                
            };       

            // Act
            var result = _controller.Write(postInput) as RedirectToRouteResult;

            // Assert
            _dbContext.Posts.Count().ShouldEqual(1);
            result.RouteValues["controller"].ShouldEqual("Home");
            result.RouteValues["action"].ShouldEqual("Index");
        }

        [TestMethod]
        public void edit_methood_should_return_postmodel()
        {
            // Arrange
            FunctionalTestHelper.CreateAutomapperMap();
            FunctionalTestHelper.CreateAccountAndBlog(_dbContext);
            FunctionalTestHelper.CreateOnePost(_dbContext);

            var mock = new Mock<ControllerContext>();
            mock.SetupGet(x => x.HttpContext.Request.IsAuthenticated).Returns(true);
            mock.SetupGet(x => x.HttpContext.User.Identity.Name).Returns(FunctionalTestHelper.AccountEmail);


            _controller = new PostController();
            _controller.ControllerContext = mock.Object;

            // Act
            var result = _controller.Edit(1) as ViewResult;
            var post = result.Model as PostInput;

            // Assert
            post.Title.ShouldEqual(FunctionalTestHelper.PostTitle);
            post.Content.ShouldEqual(FunctionalTestHelper.PostContent);
            "Edit".ShouldEqual(result.ViewName);
        }

        [TestMethod]
        public void edit_method_should_validate_model()
        {
            // Arrange
            _controller = new PostController();
            _controller.ModelState.AddModelError("modelerror", "modelerror");

            var result = _controller.Edit(new PostInput()) as ViewResult;
            result.ViewName.SequenceEqual("Edit");
            result.ViewData.ModelState.IsValid.ShouldBeFalse();
        }

        [TestMethod]
        public void edit_method_should_save_post()
        {
            // Arrange
            FunctionalTestHelper.CreateAutomapperMap();
            FunctionalTestHelper.CreateAccountAndBlog(_dbContext);
            FunctionalTestHelper.CreateOnePost(_dbContext);
            
            _controller = new PostController(new TagService());
            _controller.SetFakeControllerContext();

            // valueprovider is needed for TryUpdateModel() method
            FormCollection form = new FormCollection();
            _controller.ValueProvider = form.ToValueProvider();

            var input = new PostInput
            {
                Content = "edited content",
                Id = 1,
                Title = "edited title",
            };

            // Act
            var result = _controller.Edit(input) as RedirectToRouteResult;

            // Assert
            result.RouteValues["controller"].ShouldEqual("Post");
            result.RouteValues["action"].ShouldEqual("Detail");
            var post = _dbContext.Posts.SingleOrDefault(p => p.Id == 1);
            post.ShouldNotBeNull();
            post.Title.Contains("edited");
            post.Content.Contains("edited");
            
        }

        [TestMethod]
        public void delete_method_should_return_httpnotfouldresult()
        {
            // Arrange
            FunctionalTestHelper.CreateAccountAndBlog(_dbContext);
            FunctionalTestHelper.CreateOnePost(_dbContext);

            // Act
            _controller = new PostController();
            var result = _controller.Delete(1000) as HttpNotFoundResult;

            // Assert
            result.ShouldNotBeNull();
            result.ShouldBeOfType(typeof(HttpNotFoundResult));
        }

        [TestMethod]
        public void delete_method_should_delete_post()
        {
            // Arrange
            FunctionalTestHelper.CreateAccountAndBlog(_dbContext);
            FunctionalTestHelper.CreateOnePost(_dbContext);

            _controller = new PostController();
            _controller.SetFakeControllerContext();

            // Act
            var result = _controller.Delete(1) as RedirectToRouteResult;

            // Assert
            result.RouteValues["controller"].ShouldEqual("Home");
            result.RouteValues["action"].ShouldEqual("Index");

            _dbContext.Posts.Count().ShouldEqual(0);
        }

        [TestMethod]
        public void delete_method_should_delete_post_via_ajax()
        {
            // Arrange
            FunctionalTestHelper.CreateAccountAndBlog(_dbContext);
            FunctionalTestHelper.CreateOnePost(_dbContext);

            _controller = new PostController();
            _controller.SetFakeControllerContext(isAjaxRequest:true);

            // Act
            var result = _controller.Delete(1) as JsonResult;

            // Assert
            var serializer = new JavaScriptSerializer();
            var output = serializer.Serialize(result.Data);
            Assert.AreEqual(@"{""success"":true}", output);

            _dbContext.Posts.Count().ShouldEqual(0);
        }

        [TestMethod]
        public void detail_method_should_validate_model()
        {
            // Arrange 
            FunctionalTestHelper.CreateAutomapperMap();
            FunctionalTestHelper.CreateAccountAndBlog(_dbContext);
            FunctionalTestHelper.CreateOnePost(_dbContext);

            // Act
            _controller = new PostController();
            var result = _controller.Detail(1000) as HttpNotFoundResult;

            // Assert
            result.ShouldNotBeNull();
            result.ShouldBeOfType(typeof(HttpNotFoundResult));
        }

        [TestMethod]
        public void detail_method_should_return_post()
        {
            // Arrange 
            FunctionalTestHelper.CreateAutomapperMap();
            FunctionalTestHelper.CreateAccountAndBlog(_dbContext);
            FunctionalTestHelper.CreateOnePost(_dbContext);

            _controller = new PostController();
            _controller.SetFakeControllerContext(true);

            
            // Act
            var result = _controller.Detail(1) as ViewResult;

            // Assert
            result.ViewName.ShouldEqual("Detail");
            var model = result.Model as PostDetailViewModel;
            model.ShouldNotBeNull();
            model.Title.Contains(FunctionalTestHelper.PostTitle);
            model.Content.Contains(FunctionalTestHelper.PostContent);
        }

        
    }
}
