using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity.ModelConfiguration;

namespace MyStory.Models.Mapping
{
    public class BlogMap : EntityTypeConfiguration<Blog>
    {
        public BlogMap()
        {
            // Table
            this.ToTable("Blogs");

            // PK
            this.HasKey(t => t.Id);

            // Columns
            this.Property(t => t.Title).HasColumnName("Title");

            // Relationships
            this.HasRequired(t => t.BlogOwner)
                .WithMany()
                .HasForeignKey(t => t.AccountEmail);


        }
    }
}