using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyStory.Controllers;
using MyStory.Models;
using System.Data.Entity;
using System.Web.Mvc;

namespace MyStory.Tests.IntegrationTests.Controllers
{
    /// <summary>
    /// Integration test case for HomeController
    /// </summary>
    [TestClass]
    public class HomeControllerTest
    {
        private HomeController controller;
        private MyStoryContext context;

        public HomeControllerTest(){}

        // Use TestInitialize to run code before running each test 
        [TestInitialize()]
        public void TestInitialize() 
        {
            // Arrange
            context = new MyStoryContext();
            context.Database.Delete(); 
            context.Database.Create();
            controller = new HomeController();
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
        public void index_shoul_return_zero_account()
        {
            // Act
            var actionResult = controller.Index() as ViewResult;

            // Assert
            int cnt = actionResult.ViewBag.NumberOfAccounts;
            cnt.ShouldEqual(0);
        }

        [TestMethod]
        public void test_should_add_blog()
        {
            //// Act
            //var actionResult = controller.Test() as ViewResult;
            //var blog = (Blog)actionResult.Model;
         
            //// Assert
            //blog.Title.ShouldEqual("t1");
            //blog.BlogOwner.Email.ShouldEqual("email");
        }
    }
}
