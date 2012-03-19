using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;
using System.Web.Mvc;

namespace MyStory.Models
{
    public class Post
    {
        public Post()
        {
            this.Tags = new List<Tag>();
            this.Comments = new List<Comment>();
        }
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }

        public virtual ICollection<Tag> Tags { get; set; }
        public Location LocationOfWriting { get; set; }
        public int BlogId { get; set; }
        public virtual Blog Blog { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
    }

    public class PostMap : EntityTypeConfiguration<Post>
    {
        public PostMap()
        {
            // Table
            this.ToTable("Posts");

            // Properties
            this.Property(p => p.Title)
                .IsRequired()
                .HasMaxLength(125);

            this.Property(p => p.Content)
                .IsRequired();

            this.Property(p => p.DateCreated)
                .IsRequired();

            // Relationships
            this.HasMany(p => p.Tags)
                .WithMany(t => t.Posts)
                .Map
                (
                    m =>
                    {
                        m.MapLeftKey("PostId");
                        m.MapRightKey("TagId");
                        m.ToTable("TagPost");
                    }
                );

        }
    }

}