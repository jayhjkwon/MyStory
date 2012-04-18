using System;
using System.Diagnostics;
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
        private CommentController _controller;
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
        public void sidebar_method_should_return_comment()
        {
            FunctionalTestHelper.CreateAutomapperMap();
            FunctionalTestHelper.CreateAccountAndBlog(_dbContext);
            FunctionalTestHelper.CreateOnePost(_dbContext);
            FunctionalTestHelper.CreateOneCommenter(_dbContext);
            FunctionalTestHelper.CreateOneComment(_dbContext);

            _controller = new CommentController();

            var result = _controller.Sidebar() as ViewResult;

            result.ShouldNotBeNull();
            result.ViewName.ShouldEqual("Sidebar");
            var model = result.Model as List<CommentSidebarViewModel>;
            model.ShouldNotBeNull();
            model.Count.ShouldEqual(1);
        }

        [TestMethod]
        public void comment_should_less_then_50()
        {
            FunctionalTestHelper.CreateAutomapperMap();
            FunctionalTestHelper.CreateAccountAndBlog(_dbContext);
            FunctionalTestHelper.CreateOnePost(_dbContext);
            FunctionalTestHelper.CreateOneCommenter(_dbContext);
            FunctionalTestHelper.CommentContent = "GreaterThan50GreaterThan50GreaterThan50GreaterThan50GreaterThan50";
            FunctionalTestHelper.CreateOneComment(_dbContext);

            _controller = new CommentController();

            var result = _controller.Sidebar() as ViewResult;

            result.ViewName.ShouldEqual("Sidebar");
            var model = result.Model as List<CommentSidebarViewModel>;
            model.ShouldNotBeNull();
            model.Count.ShouldEqual(1);
            model.First().Content.Length.ShouldEqual(50);
        }

        [TestMethod]
        public void postcommentslist_method_should_validate_postid_isnull()
        {
            _controller = new CommentController();
            var result = _controller.PostCommentsList(null) as ViewResult;
            result.ViewName.ShouldEqual("PostCommentsList");
            result.Model.ShouldBeNull();
        }

        [TestMethod]
        public void postcommentslist_method_should_return_model()
        {
            FunctionalTestHelper.CreateAutomapperMap();
            FunctionalTestHelper.CreateAccountAndBlog(_dbContext);
            FunctionalTestHelper.CreateOnePost(_dbContext);
            FunctionalTestHelper.CreateOneCommenter(_dbContext);
            FunctionalTestHelper.CreateOneComment(_dbContext);

            _controller = new CommentController();
            var result = _controller.PostCommentsList(1) as ViewResult;
            
            result.ViewName.ShouldEqual("PostCommentsList");
            var model = result.Model as PostCommentsViewModel;
            model.ShouldNotBeNull();
            model.PostId.ShouldEqual(1);
            model.Comments.ShouldNotBeNull();
            model.Comments.Count.ShouldEqual(1);
        }

        [TestMethod]
        public void write_should_validate_model()
        {
            _controller = new CommentController();
            _controller.ModelState.AddModelError("modelerror","modelerror");

            var result = _controller.Write(new CommentInput(), 0) as RedirectToRouteResult;

            result.RouteValues["controller"].ShouldEqual("Post");
            result.RouteValues["action"].ShouldEqual("Detail");
            result.RouteValues["id"].ShouldEqual(0);
            result.RouteValues["errorFromCommentInput"].ShouldEqual(true);

            _controller.TempData["commentInputData"].ShouldNotBeNull();
            _controller.TempData["commentInputDataErrors"].ShouldNotBeNull();
            
        }

        [TestMethod]
        public void write_should_save_comment()
        {
            FunctionalTestHelper.CreateAutomapperMap();
            FunctionalTestHelper.CreateAccountAndBlog(_dbContext);
            FunctionalTestHelper.CreateOnePost(_dbContext);
            FunctionalTestHelper.CreateOneCommenter(_dbContext);

            _controller = new CommentController();

            var commentInput = new CommentInput
                                   {
                                       Content = "content",
                                       Email = FunctionalTestHelper.CommenterEmail,
                                       Name = FunctionalTestHelper.CommenterName,
                                   };
            var result = _controller.Write(commentInput, 1) as RedirectToRouteResult;

            result.RouteValues["controller"].ShouldEqual("Post");
            result.RouteValues["action"].ShouldEqual("Detail");
            result.RouteValues["id"].ShouldEqual(1);

            _dbContext.Comments.Count().ShouldEqual(1);
            var comment = _dbContext.Comments.FirstOrDefault();
            comment.Content.ShouldEqual("content");
            comment.Commenter.Email.ShouldEqual(FunctionalTestHelper.CommenterEmail);
            comment.Commenter.Name.ShouldEqual(FunctionalTestHelper.CommenterName);

        }
    }
}

