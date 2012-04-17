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

namespace MyStory.Tests.FunctionalTests.Controllers
{
    /// <summary>
    /// Summary description for PostControllerTest
    /// </summary>
    [TestClass]
    public class PostControllerTest
    {
        private PostController controller;
        private MyStoryContext dbContext;

        [TestInitialize()]
        public void TestInitialize()
        {
            // Arrange
            dbContext = new MyStoryContext();
            dbContext.Database.Delete(); 
            dbContext.Database.Create();
        }

        [TestCleanup()]
        public void TestCleanup()
        {
            if (controller != null)
            {
                dbContext.Database.Delete();
                controller.Dispose();
            }
        }

        [TestMethod]
        public void invalid_model_should_not_save()
        {
            // Arrange
            controller = new PostController();
            controller.ModelState.AddModelError("modelerror", "modelerror");
            var postInput = new PostInput();
            
            // Act
            var result = controller.Write(postInput) as ViewResult;

            // Assert
            result.ViewData.ModelState.IsValid.ShouldBeFalse();
            result.ViewName.ShouldEqual("Write");
        }

        [TestMethod]
        public void valid_model_should_be_saved()
        {
            // Arrange
            FunctionalTestHelper.CreateAutomapperMap();
            FunctionalTestHelper.CreateAccountAndBlog(dbContext);

            var controllerContext = new Mock<ControllerContext>();
            controllerContext.SetupGet(x => x.HttpContext.Request.IsAuthenticated).Returns(true);
            controllerContext.SetupGet(x => x.HttpContext.User.Identity.Name).Returns("a@a.com");
            
            controller = new PostController(new TagService());
            controller.ControllerContext = controllerContext.Object;

            var postInput = new PostInput
            {
                Title = "title",
                Content = "content",                
            };       

            // Act
            var result = controller.Write(postInput) as RedirectToRouteResult;

            // Assert
            dbContext.Posts.Count().ShouldEqual(1);
            result.RouteValues["controller"].ShouldEqual("Home");
            result.RouteValues["action"].ShouldEqual("Index");
        }

        [TestMethod]
        public void edit_methood_should_return_postmodel()
        {
            // Arrange
            FunctionalTestHelper.CreateAutomapperMap();
            FunctionalTestHelper.CreateAccountAndBlog(dbContext);
            FunctionalTestHelper.CreateOnePost(dbContext);

            var mock = new Mock<ControllerContext>();
            mock.SetupGet(x => x.HttpContext.Request.IsAuthenticated).Returns(true);
            mock.SetupGet(x => x.HttpContext.User.Identity.Name).Returns("a@a.com");


            controller = new PostController();
            controller.ControllerContext = mock.Object;

            // Act
            var result = controller.Edit(1) as ViewResult;
            var post = result.Model as PostInput;

            // Assert
            "title".ShouldEqual(post.Title);
            "content".ShouldEqual(post.Content);
            "Edit".ShouldEqual(result.ViewName);
        }

        [TestMethod]
        public void edit_method_should_validate_model()
        {
            // Arrange
            controller = new PostController();
            controller.ModelState.AddModelError("modelerror", "modelerror");

            var result = controller.Edit(new PostInput()) as ViewResult;
            result.ViewName.SequenceEqual("Edit");
            result.ViewData.ModelState.IsValid.ShouldBeFalse();
        }

        [TestMethod]
        public void edit_method_should_save_post()
        {
            // Arrange
            FunctionalTestHelper.CreateAutomapperMap();
            FunctionalTestHelper.CreateAccountAndBlog(dbContext);
            FunctionalTestHelper.CreateOnePost(dbContext);
            
            controller = new PostController(new TagService());
            controller.SetFakeControllerContext();

            // valueprovider is needed for TryUpdateModel() method
            FormCollection form = new FormCollection();
            controller.ValueProvider = form.ToValueProvider();

            var input = new PostInput
            {
                Content = "edited content",
                Id = 1,
                Title = "edited title",
            };

            // Act
            var result = controller.Edit(input) as RedirectToRouteResult;

            // Assert
            result.RouteValues["controller"].ShouldEqual("Post");
            result.RouteValues["action"].ShouldEqual("Detail");
            var post = dbContext.Posts.SingleOrDefault(p => p.Id == 1);
            post.ShouldNotBeNull();
            post.Title.Contains("edited");
            post.Content.Contains("edited");
            
        }

        [TestMethod]
        public void delete_method_should_return_httpnotfouldresult()
        {
            // Arrange
            FunctionalTestHelper.CreateAccountAndBlog(dbContext);
            FunctionalTestHelper.CreateOnePost(dbContext);

            // Act
            controller = new PostController();
            var result = controller.Delete(1000) as HttpNotFoundResult;

            // Assert
            result.ShouldNotBeNull();
            result.ShouldBeOfType(typeof(HttpNotFoundResult));
        }

        [TestMethod]
        public void delete_method_should_delete_post()
        {
            // Arrange
            FunctionalTestHelper.CreateAccountAndBlog(dbContext);
            FunctionalTestHelper.CreateOnePost(dbContext);

            controller = new PostController();
            controller.SetFakeControllerContext();

            // Act
            var result = controller.Delete(1) as RedirectToRouteResult;

            // Assert
            result.RouteValues["controller"].ShouldEqual("Home");
            result.RouteValues["action"].ShouldEqual("Index");

            dbContext.Posts.Count().ShouldEqual(0);
        }

        [TestMethod]
        public void detail_method_should_validate_model()
        {
            // Arrange 
            FunctionalTestHelper.CreateAutomapperMap();
            FunctionalTestHelper.CreateAccountAndBlog(dbContext);
            FunctionalTestHelper.CreateOnePost(dbContext);

            // Act
            controller = new PostController();
            var result = controller.Detail(1000) as HttpNotFoundResult;

            // Assert
            result.ShouldNotBeNull();
            result.ShouldBeOfType(typeof(HttpNotFoundResult));
        }

        [TestMethod]
        public void detail_method_should_return_post()
        {
            // Arrange 
            FunctionalTestHelper.CreateAutomapperMap();
            FunctionalTestHelper.CreateAccountAndBlog(dbContext);
            FunctionalTestHelper.CreateOnePost(dbContext);

            controller = new PostController();
            controller.SetFakeControllerContext(false);

            
            // Act
            var result = controller.Detail(1) as ViewResult;

            // Assert
            result.ViewName.ShouldEqual("Detail");
            var model = result.Model as PostDetailViewModel;
            model.ShouldNotBeNull();
            model.Title.Contains("title");
            model.Content.Contains("content");
        }

        
    }
}
