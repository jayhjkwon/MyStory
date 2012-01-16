using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyStory.Models.Infrastructure;
using MyStory.Models;
using System.Data.Entity;

namespace MyStory.Tests.UnitTests.Models
{
    [TestClass]
    public class BaseModelTest
    {
        protected MyStoryContext context;
        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        // Use ClassInitialize to run code before running the first test in the class
        [ClassInitialize()]
        public static void ClassInitialize(TestContext testContext)
        {
        }

        // Use ClassCleanup to run code after all tests in a class have run
        [ClassCleanup()]
        public static void ClassCleanup()
        {
        }

        // Use TestInitialize to run code before running each test 
        [TestInitialize()]
        public void TestInitialize()
        {
            context = new MyStoryContext();
            Database.SetInitializer<MyStoryContext>(new MyStoryDbInitializationStrategy());

            // create test data
            Account account = new Account
            {
                Email = "a@aa.com",
                Password = "password",
                FullName = "JonDoe"
            };
           

            context.Accounts.Add(account);

            context.SaveChanges();
        }

        // Use TestCleanup to run code after each test has run
        [TestCleanup()]
        public void TestCleanup() 
        {
            if (context != null)
            {
                context.Dispose();
            }

        }

    }
}
