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
        public static string BlogTitle { get {return "blog";} }
        public static string AccountEmail { get { return "a@a.com"; } }
        public static string AccountPasword { get { return "password"; } }
        public static string AccountName { get { return "john"; } }
        public static string PostContent { get { return "content"; } }
        public static string PostTitle { get { return "title"; } }
        public static string CommenterEmail { get { return "sujin"; } }
        public static string CommenterName { get { return "c@c.com"; } }
        private static string _commentContent;
        public static string CommentContent 
        {
            get
            {
                return _commentContent ?? "this is content"; 
            } 
            set 
            {
                _commentContent = value;
            }
        }

        public static void CreateAutomapperMap()
        {
            new CommentMapper().Execute();
            new PostMapper().Execute();
        }

        public static void CreateAccountAndBlog(MyStoryContext dbContext)
        {
            dbContext.Blogs.Add(new Blog 
                            {
                                Title = BlogTitle, 
                                BlogOwner = new Account 
                                            {
                                                Email = AccountEmail, 
                                                Password = AccountPasword, 
                                                Name = AccountName 
                                            }
                            });
            dbContext.SaveChanges();
        }

        public static void CreateOnePost(MyStoryContext dbContext)
        {
            var blogId = dbContext.Blogs.SingleOrDefault().Id;

            dbContext.Posts.Add(new Post
            {
                Id = 1,
                BlogId = blogId,
                Content = PostContent,
                DateCreated = DateTime.Now,
                DateModified = DateTime.Now,
                Title = PostTitle,
                LocationOfWriting = new Location()
            });

            dbContext.SaveChanges();
        }

        public static void CreateOneCommenter(MyStoryContext dbContext)
        {
            var commenter = new Commenter
            {
                Id = 1,
                Name = CommenterName,
                Email = CommenterEmail
            };

            dbContext.Commenters.Add(commenter);
            dbContext.SaveChanges();
        }

        public static void CreateOneComment(MyStoryContext dbContext)
        {
            var comment = new Comment
            {
                Content = CommentContent,
                DateCreated = DateTime.Now,
                PostId = 1,
                CommenterId = 1,
            };

            dbContext.Comments.Add(comment);
            dbContext.SaveChanges();
        }

        
    }
}
