using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyStory.Models;

namespace MyStory.Tests.FunctionalTests
{
    public class FunctionalTestHelper
    {
        public static void CreateAccountAndBlog(MyStoryContext context)
        {
            context.Blogs.Add(new Blog 
                            {
                                Title = "blog", 
                                BlogOwner = new Account 
                                            {
                                                Email = "a@a.com", 
                                                Password = "password", 
                                                Name = "john" 
                                            }
                            });
            context.SaveChanges();
        }
    }
}
