using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;

namespace MyStory.Models
{
    public class Blog
    {
        public Blog()
        {
            this.Posts = new List<Post>();
        }
        public int Id { get; set; }
        public string Title { get; set; }

        public virtual Account BlogOwner { get; set; }
        public virtual ICollection<Post> Posts { get; set; }
    }

    public class BlogMap : EntityTypeConfiguration<Blog>
    {
        public BlogMap()
        {
            // Table
            this.ToTable("Blogs");

            this.Property(b => b.Title)
                .IsRequired()
                .HasMaxLength(125);

            // Relationships
            this.HasRequired(t => t.BlogOwner)
                .WithOptional()
                ;
        }
    }
}