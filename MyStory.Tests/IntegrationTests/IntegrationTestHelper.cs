using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyStory.Models;

namespace MyStory.Tests.IntegrationTests
{
    public class IntegrationTestHelper
    {
        public static void CreateAccountAndBlog(MyStoryContext context)
        {
            context.Blogs.Add(new Blog { Title = "blog", BlogOwner = new Account { Email = "a@a.com", Password = "password", FullName = "john" } });
            context.SaveChanges();
        }
    }
}
