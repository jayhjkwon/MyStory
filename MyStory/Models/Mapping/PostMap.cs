using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity.ModelConfiguration;

namespace MyStory.Models.Mapping
{
    public class PostMap : EntityTypeConfiguration<Post>
    {
        public PostMap()
        {
            // Table
            this.ToTable("Posts");

            // PK
            this.HasKey(t => t.Id);
            
            // Columns
            this.Property(t => t.Title).HasColumnName("Title");
            this.Property(t => t.ContentWithHtml).HasColumnName("ContentWithHtml");
            this.Property(t => t.ContentWithoutHtml).HasColumnName("ContentWithoutHtml");
            this.Property(t => t.DateCreated).HasColumnName("DateCreated");
            this.Property(t => t.DateModified).HasColumnName("DateModified");

            // Relationships
            this.HasMany(p => p.Tags)
                .WithMany(t => t.Posts)
                .Map
                (
                    m =>
                    {
                        m.MapLeftKey("PostId");
                        m.MapRightKey("ItemId");
                        m.ToTable("TagPost");
                    }
                );

        }
    }
}