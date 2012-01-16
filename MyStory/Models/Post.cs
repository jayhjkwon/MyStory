using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;
using System.Web.Mvc;

namespace MyStory.Models
{
    [MetadataType(typeof(PostMetadata))]
    public class Post
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string ContentWithoutHtml { get; set; }
        public string ContentWithHtml { get; set; }
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

    public class PostMetadata
    {
        [Required]
        [MinLength(1)]
        [MaxLength(125)]
        public string Title { get; set; }

        [Required]
        public string ContentWithoutHtml { get; set; }

        [Required]
        [AllowHtml]
        public string ContentWithHtml { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime DateCreated { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime DateModified { get; set; }
    }
}