using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using MyStory.Models.Mapping;
using MyStory.Models;

namespace MyStory.Infrastructure.Models
{
    public class MyStoryContext : DbContext 
    {
        static MyStoryContext()
        {
            Database.SetInitializer<MyStoryContext>(new DropCreateDatabaseAlways<MyStoryContext>());
        }

        public MyStoryContext()
            : base("name=MyStorySQLCEDB")
        {}

        public IDbSet<Account> Accounts { get; set; }
        public IDbSet<Blog> Blogs { get; set; }
        public IDbSet<Comment> Comments { get; set; }
        public IDbSet<Post> Posts { get; set; }
        public IDbSet<Tag> Tags { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<IncludeMetadataConvention>();

            modelBuilder.Configurations.Add(new AccountMap());
            modelBuilder.Configurations.Add(new BlogMap());
            modelBuilder.Configurations.Add(new CommentMap());
            modelBuilder.Configurations.Add(new LocationMap());
            modelBuilder.Configurations.Add(new PostMap());
            modelBuilder.Configurations.Add(new TagMap());
        }
    }
}