using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using MyStory.Infrastructure.Models;

namespace MyStory.Tests.Models
{
    public class DatabaseCreationTest
    {
        private MyStoryContext context;

        // This ctor will be executed whenever each test method executes
        public DatabaseCreationTest()
        {
            context = new MyStoryContext();
        }


        [Fact]
        public void accounts_should_have_nothing()
        {
            Assert.NotNull(context);

            var accounts = context.Accounts;

            Assert.Equal(0, accounts.Count());
        }
    }
}
