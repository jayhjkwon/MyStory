using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;

namespace MyStory.Models
{
    [MetadataType(typeof(TagMetadata))]
    public class Tag
    {
        public int Id { get; set; }
        public string TagText { get; set; } // unique key
        public virtual ICollection<Post> Posts { get; set; }
    }

    public class TagMap : EntityTypeConfiguration<Tag>
    {
        public TagMap()
        {
            // Table
            this.ToTable("Tags");

            // Relationships
            // Relationships with Post entity is defined in PostMap class
        }
    }

    public class TagMetadata
    {
        [Required]
        [MinLength(1)]
        [MaxLength(125)]
        public string TagText { get; set; }
    }
}