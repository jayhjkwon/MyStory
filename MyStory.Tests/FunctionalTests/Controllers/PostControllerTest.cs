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
using MvcContrib.TestHelper;

namespace MyStory.Tests.FunctionalTests.Controllers
{
    /// <summary>
    /// Summary description for PostControllerTest
    /// </summary>
    [TestClass]
    public class PostControllerTest
    {
        private PostController controller;
        private MyStoryContext context;
        private TestControllerBuilder builder;

        public PostControllerTest(){}

        [TestInitialize()]
        public void TestInitialize()
        {
            // Arrange
            context = new MyStoryContext();
            context.Database.Delete(); 
            context.Database.Create();

            builder = new TestControllerBuilder();
            controller = new PostController();
            builder.InitializeController(controller);
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
        public void non_auth_user_cannot_access_write_form_method()
        {
            var mock = new Mock<ControllerContext>();
            mock.SetupGet(x => x.HttpContext.Request.IsAuthenticated).Returns(false);
            controller.ControllerContext = mock.Object;

            controller.Request.IsAuthenticated.ShouldEqual(false);

            var result = controller.Write() as ViewResult;
            var r = result.ViewData.ModelState.IsValid;
            var modelstates = result.ViewData.ModelState["auth"];
            
        }

        [TestMethod]
        public void only_admin_can_access_write_form_method()
        {
            var mock = new Mock<ControllerContext>();
            mock.SetupGet(x => x.HttpContext.Request.IsAuthenticated).Returns(true);
            mock.SetupGet(x => x.HttpContext.User.Identity.Name).Returns("a@a.com");
            controller.ControllerContext = mock.Object;
            
            controller.Request.IsAuthenticated.ShouldEqual(true);
            controller.Write().AssertViewRendered().ForView("Write");
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
            FunctionalTestHelper.CreateAccountAndBlog(context);

            var mock = new Mock<ControllerContext>();
            mock.SetupGet(x => x.HttpContext.Request.IsAuthenticated).Returns(true);
            mock.SetupGet(x => x.HttpContext.User.Identity.Name).Returns("a@a.com");

            controller.ControllerContext = mock.Object;

            var input = new PostInput();
            input.Title = null;
            input.Content = null;

            // #1 test validation
            //var actionResult = controller.Write(input) as ViewResult;
            //Assert.IsFalse(actionResult.ViewData.ModelState.IsValid);

            // #2 test data inpu failure and view result
            controller.ModelState.AddModelError("InvalidModel", "Title and Content are empty");
            var actionResult = controller.Write(input) as ViewResult;
            
            actionResult.ViewName.ShouldEqual("Write");
            actionResult.Model.ShouldEqual(input);
            context.Posts.Count().ShouldEqual(0);
        }

        [TestMethod]
        public void edit_getmethod_should_return_postmodel()
        {
            // Arrange
            FunctionalTestHelper.CreateAccountAndBlog(context);
            context.Posts.Add(new Post 
            { 
                Title = "title", 
                Content = "content", 
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
