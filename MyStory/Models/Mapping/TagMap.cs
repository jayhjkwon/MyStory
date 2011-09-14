using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity.ModelConfiguration;

namespace MyStory.Models.Mapping
{
    public class TagMap : EntityTypeConfiguration<Tag>
    {
        public TagMap()
        {
            // Table
            this.ToTable("Tags");

            // PK
            this.HasKey(t=>t.Id);

            // Columns
            this.Property(t => t.TagText).HasColumnName("TagText");

            // Relationships
            // Relationships with Post entity is done in PostMap class
        }
    }
}