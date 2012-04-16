using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyStory.Models;
using MyStory.Infrastructure.AutoMapper;

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

        public static void CreateOnePost(MyStoryContext context)
        {
            var blogId = context.Blogs.SingleOrDefault().Id;

            context.Posts.Add(new Post
            {
                BlogId = blogId,
                Content = "content",
                DateCreated = DateTime.Now,
                DateModified = DateTime.Now,
                Title = "title",
                LocationOfWriting = new Location()
            });

            context.SaveChanges();
        }

        public static void CreateAutomapperMap()
        {
            new CommentMapper().Execute();
            new PostMapper().Execute();
        }
    }
}
