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
        public void test()
        {
            var blog = dbContext.Posts.Include("Tags").Include("Comments");
            var blogList = blog.ToList();
            Assert.IsNull(null);
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

        //[TestMethod]
        //public void edit_postmethod_should_not_update_invalid_mode()
        //{
        //    // Arrange
        //    IntegrationTestHelper.CreateAccountAndBlog(context);
        //    context.Posts.Add(new Post
        //    {
        //        Title = "title",
        //        Content = "content",
        //        BlogId = 1,
        //        LocationOfWriting = new Location(),
        //        DateCreated = DateTime.Now,
        //        DateModified = DateTime.Now
        //    });
        //    context.SaveChanges();

        //    var mock = new Mock<ControllerContext>();
        //    mock.SetupGet(x => x.HttpContext.Request.IsAuthenticated).Returns(true);
        //    mock.SetupGet(x => x.HttpContext.User.Identity.Name).Returns("a@a.com");

        //    controller.ControllerContext = mock.Object;

        //    var post = context.Posts.Find(1);
        //    post.Title = null;
        //    post.Content = null;

        //    controller.ModelState.AddModelError("error", "error");

        //    // Act            
        //    var result = controller.Edit(post) as ViewResult;

        //    // Assert
        //    Assert.AreEqual("Edit", result.ViewName);
        //    Assert.AreEqual("title", context.Posts.SingleOrDefault(p => p.Id == 1).Title);
        //    Assert.AreEqual("content", context.Posts.SingleOrDefault(p => p.Id == 1).Content);

        //    //"Edit".ShouldEqual(result.ViewName);
        //    //"title".ShouldEqual(context.Posts.SingleOrDefault(p => p.Id == 1).Title);
        //    //"content".ShouldEqual(context.Posts.SingleOrDefault(p => p.Id == 1).ContentWithHtml);
        //}

        [TestMethod]
        public void edit_postmethod_should_update_valid_model()
        {
        }
        
    }
}
