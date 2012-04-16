using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using MyStory.Models;
using MyStory.Controllers;
using System.Web.Mvc;

namespace MyStory.Tests.UnitTests.Controllers
{
    [TestClass]
    public class HomeControllerTests
    {
        [TestMethod]
        public void Index()
        {
            var mockCtx = new Mock<ContextHelper>();
            mockCtx.Setup(x => x.GetAccountNumber()).Returns(10);
            
            HomeController homeController = new HomeController(mockCtx.Object);
            var result = homeController.Index() as ViewResult;
            var cnt = (int)result.ViewBag.NumberOfAccounts;

            cnt.ShouldEqual(10);
        }
    }
}
