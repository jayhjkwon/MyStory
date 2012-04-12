using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyStory.Controllers;
using MyStory.Models;
using System.Data.Entity;
using System.Web.Mvc;
using MvcContrib.TestHelper;

namespace MyStory.Tests.FunctionalTests.Controllers
{
    /// <summary>
    /// Integration test case for HomeController
    /// </summary>
    [TestClass]
    public class HomeControllerTest
    {
        private HomeController controller;
        private MyStoryContext context;
        private TestControllerBuilder builder;

        // Use TestInitialize to run code before running each test 
        [TestInitialize()]
        public void TestInitialize() 
        {
            // Arrange
            context = new MyStoryContext();
            context.Database.Delete(); 
            context.Database.Create();

            builder = new TestControllerBuilder();
            controller = new HomeController();
            builder.InitializeController(controller);

        }
        
        // Use TestCleanup to run code after each test has run
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
        public void index_should_return_index_view()
        {
            controller.Index().AssertViewRendered().ForView("Index");
        }

        [TestMethod]
        public void index_shoul_return_zero_account()
        {
            // Act
            var actionResult = controller.Index() as ViewResult;

            // Assert
            int cnt = actionResult.ViewBag.NumberOfAccounts;
            cnt.ShouldEqual(0);
        }

    }
}
