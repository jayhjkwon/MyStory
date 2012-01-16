using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;

namespace MyStory.Models
{
    [MetadataType(typeof(BlogMetadata))]
    public class Blog
    {
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

            // Relationships
            this.HasRequired(t => t.BlogOwner)
                .WithOptional()
                ;
        }
    }

    public class BlogMetadata
    {
        [Required]
        [MinLength(1)]
        [MaxLength(125)]
        public string Title { get; set; }
    }
}