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

namespace MyStory.Tests.IntegrationTests.Controllers
{
    /// <summary>
    /// Summary description for PostControllerTest
    /// </summary>
    [TestClass]
    public class PostControllerTest
    {
        private PostController controller;
        private MyStoryContext context;

        public PostControllerTest(){}

        [TestInitialize()]
        public void TestInitialize()
        {
            // Arrange
            context = new MyStoryContext("MyStorySQLCEDB");
            //context = new MyStoryContext();
            if (context.Database.Exists())
                context.Database.Delete(); 
            context.Database.Create();
            controller = new PostController(context);
        }

        [TestCleanup()]
        public void TestCleanup()
        {
            if (controller != null)
            {
                context.Database.Delete();
                controller.Dispose();
            }
        }

        [TestMethod]
        public void test()
        {
            var blog = context.Posts.Include("Tags").Include("Comments");
            var blogList = blog.ToList();
            Assert.IsNull(null);
        }

        [TestMethod]
        public void write_should_validate_model()
        {
            IntegrationTestHelper.CreateAccountAndBlog(context);

            var mock = new Mock<ControllerContext>();
            mock.SetupGet(x => x.HttpContext.Request.IsAuthenticated).Returns(true);
            mock.SetupGet(x => x.HttpContext.User.Identity.Name).Returns("a@a.com");

            controller.ControllerContext = mock.Object;

            var input = new PostInputViewModel();
            input.Title = null;
            input.ContentWithHtml = null;

            // #1 test validation
            //var actionResult = controller.Write(input) as ViewResult;
            //Assert.IsFalse(actionResult.ViewData.ModelState.IsValid);

            // #2 test data inpu failure and view result
            controller.ModelState.AddModelError("InvalidModel", "Title and Content are empty");
            var actionResult = controller.Write(input) as ViewResult;
            Assert.AreEqual("Write", actionResult.ViewName);
            Assert.AreEqual(input, actionResult.Model);
            Assert.AreEqual(0, context.Posts.Count());
        }

        [TestMethod]
        public void edit_getmethod_should_return_postmodel()
        {
            // Arrange
            IntegrationTestHelper.CreateAccountAndBlog(context);
            context.Posts.Add(new Post 
            { 
                Title = "title", 
                ContentWithHtml = "content", 
                BlogId = 1, 
                LocationOfWriting = new Location(), 
                DateCreated=DateTime.Now, 
                DateModified=DateTime.Now
            });
            context.SaveChanges();

            var mock = new Mock<ControllerContext>();
            mock.SetupGet(x => x.HttpContext.Request.IsAuthenticated).Returns(true);
            mock.SetupGet(x => x.HttpContext.User.Identity.Name).Returns("a@a.com");

            controller.ControllerContext = mock.Object;

            // Act
            var result = controller.Edit(1) as ViewResult;
            var post = result.Model as Post;

            // Assert
            Assert.AreEqual("title", post.Title);
            Assert.AreEqual("content", post.ContentWithHtml);
            Assert.AreEqual("Edit", result.ViewName);
        }

        [TestMethod]
        public void edit_postmethod_should_not_update_invalid_mode()
        {
            // Arrange
            IntegrationTestHelper.CreateAccountAndBlog(context);
            context.Posts.Add(new Post
            {
                Title = "title",
                ContentWithHtml = "content",
                BlogId = 1,
                LocationOfWriting = new Location(),
                DateCreated = DateTime.Now,
                DateModified = DateTime.Now
            });
            context.SaveChanges();

            var mock = new Mock<ControllerContext>();
            mock.SetupGet(x => x.HttpContext.Request.IsAuthenticated).Returns(true);
            mock.SetupGet(x => x.HttpContext.User.Identity.Name).Returns("a@a.com");

            controller.ControllerContext = mock.Object;

            var post = context.Posts.Find(1);
            post.Title = null;
            post.ContentWithHtml = null;

            controller.ModelState.AddModelError("error", "error");

            // Act            
            var result = controller.Edit(post) as ViewResult;

            // Assert
            Assert.AreEqual("Edit", result.ViewName);
            Assert.AreEqual("title", context.Posts.SingleOrDefault(p => p.Id == 1).Title);
            Assert.AreEqual("content", context.Posts.SingleOrDefault(p => p.Id == 1).ContentWithHtml);
        }

        [TestMethod]
        public void edit_postmethod_should_update_valid_model()
        {
        }
        
    }
}
