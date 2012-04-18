using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyStory.Controllers;
using MyStory.Models;
using System.Data.Entity;
using System.Web.Mvc;
using MyStory.ViewModels;

namespace MyStory.Tests.FunctionalTests.Controllers
{
    [TestClass]
    public class CommentControllerTest
    {
        private CommentController controller;
        private MyStoryContext dbContext;

        // Use TestInitialize to run code before running each test 
        [TestInitialize()]
        public void TestInitialize()
        {
            // Arrange
            dbContext = new MyStoryContext();
            dbContext.Database.Delete();
            dbContext.Database.Create();
        }

        // Use TestCleanup to run code after each test has run
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
        public void sidebar_method_should_return_comment()
        {
            FunctionalTestHelper.CreateAutomapperMap();
            FunctionalTestHelper.CreateAccountAndBlog(dbContext);
            FunctionalTestHelper.CreateOnePost(dbContext);
            FunctionalTestHelper.CreateOneCommenter(dbContext);
            FunctionalTestHelper.CreateOneComment(dbContext);

            controller = new CommentController();

            var result = controller.Sidebar() as ViewResult;
            
            result.ViewName.ShouldEqual("Sidebar");
            var model = result.Model as List<CommentSidebarViewModel>;
            model.ShouldNotBeNull();
            model.Count.ShouldEqual(1);
        }

        [TestMethod]
        public void comment_should_less_then_50()
        {
            FunctionalTestHelper.CreateAutomapperMap();
            FunctionalTestHelper.CreateAccountAndBlog(dbContext);
            FunctionalTestHelper.CreateOnePost(dbContext);
            FunctionalTestHelper.CreateOneCommenter(dbContext);
            FunctionalTestHelper.CommentContent = "GreaterThan50GreaterThan50GreaterThan50GreaterThan50GreaterThan50";
            FunctionalTestHelper.CreateOneComment(dbContext);

            controller = new CommentController();

            var result = controller.Sidebar() as ViewResult;

            result.ViewName.ShouldEqual("Sidebar");
            var model = result.Model as List<CommentSidebarViewModel>;
            model.ShouldNotBeNull();
            model.Count.ShouldEqual(1);
            model.First().Content.Length.ShouldEqual(50);
        }
    }
}
