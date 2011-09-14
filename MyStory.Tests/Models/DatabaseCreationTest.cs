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

        public DatabaseCreationTest()
        {
            context = new MyStoryContext();
        }

        [Fact]
        public void test()
        {
            Assert.NotNull(context);
        }

        [Fact]
        public void test1()
        {
            Assert.NotNull(context);

            var accounts = context.Accounts.ToList();

            Assert.Equal(0, accounts.Count);
        }
    }
}
