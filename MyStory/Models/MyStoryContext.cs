using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using MyStory.Models;
using MyStory.Models.Infrastructure;
using System.Data.Objects;

namespace MyStory.Models
{
    public class MyStoryContext : DbContext
    {
        public MyStoryContext()
        {
            Database.SetInitializer<MyStoryContext>
                (new MyStoryDbInitializationStrategy());
        }

        // for integration test purpose
        public MyStoryContext(string connectionStringName) : base(connectionStringName)
        {
        }

        public DbSet<Account> Accounts { get; set; }
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Commenter> Commenters { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Tag> Tags { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new AccountMap());
            modelBuilder.Configurations.Add(new BlogMap());
            modelBuilder.Configurations.Add(new CommentMap());
            modelBuilder.Configurations.Add(new CommenterMap());
            modelBuilder.Configurations.Add(new LocationMap());
            modelBuilder.Configurations.Add(new PostMap());
            modelBuilder.Configurations.Add(new TagMap());
        }

        
    }
}