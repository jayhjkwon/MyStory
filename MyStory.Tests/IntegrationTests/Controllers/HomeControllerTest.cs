﻿using System;
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
            context = new MyStoryContext("MyStorySQLCEDB");
            context.Database.Delete(); 
            context.Database.Create();
            controller = new HomeController(context);
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
            Assert.AreEqual(0, actionResult.ViewBag.NumberOfAccounts);
        }

        [TestMethod]
        public void test_should_add_blog()
        {
            var actionResult = controller.Test() as ViewResult;
            var blog = (Blog)actionResult.Model;
            Assert.AreEqual("t1", blog.Title);
            Assert.AreEqual("email", blog.BlogOwner.Email);    
        }
    }
}
