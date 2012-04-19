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
    /// <summary>
    /// Integration test case for HomeController
    /// </summary>
    [TestClass]
    public class HomeControllerTest
    {
        private HomeController _controller;
        private MyStoryContext _context;

        // Use TestInitialize to run code before running each test 
        [TestInitialize()]
        public void TestInitialize() 
        {
            // Arrange
            _context = new MyStoryContext();
            _context.Database.Delete(); 
            _context.Database.Create();
        }
        
        // Use TestCleanup to run code after each test has run
        [TestCleanup()]
        public void TestCleanup() 
        {
            if (_controller != null)
            {
                _context.Database.Delete();
                _controller.Dispose();
            }
        }

        [TestMethod]
        public void index_shoul_return_zero_account()
        {
            // Arrange
            _controller = new HomeController();

            // Act
            var actionResult = _controller.Index() as ViewResult;

            // Assert
            int cnt = actionResult.ViewBag.NumberOfAccounts;
            cnt.ShouldEqual(0);
        }

        [TestMethod]
        public void index_shoul_return_one_account()
        {
            // Arrange
            FunctionalTestHelper.CreateAccountAndBlog(_context);
            _controller = new HomeController();

            // Act
            var actionResult = _controller.Index() as ViewResult;

            // Assert
            int cnt = actionResult.ViewBag.NumberOfAccounts;
            cnt.ShouldEqual(1);
        }

        [TestMethod]
        public void index_shoul_return_one_post()
        {
            // Arrange
            FunctionalTestHelper.CreateAutomapperMap();
            FunctionalTestHelper.CreateAccountAndBlog(_context);
            FunctionalTestHelper.CreateOnePost(_context);

            _controller = new HomeController();

            // Act
            var actionResult = _controller.Index() as ViewResult;

            // Assert
            actionResult.ViewName.ShouldEqual("Index");

            var vm = actionResult.Model as List<PostListViewModel>;
            vm.Count.ShouldBeGreaterThan(0);
            vm.First().Id.ShouldNotBeNull();
            vm.First().Title.ShouldNotBeNull();
            vm.First().Content.ShouldNotBeNull();
            vm.First().Title.ShouldEqual(FunctionalTestHelper.PostTitle);
            vm.First().Content.ShouldContain(FunctionalTestHelper.PostContent);
        }

    }
}
