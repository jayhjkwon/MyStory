using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity.ModelConfiguration;

namespace MyStory.Models.Mapping
{
    public class CommentMap : EntityTypeConfiguration<Comment>
    {
        public CommentMap()
        {
            // Table
            this.ToTable("Comments");

            // PK
            this.HasKey(t => t.Id);

            // Columns
            this.Property(t => t.AuthorName).HasColumnName("AuthorName");
            this.Property(t => t.AuthorEmail).HasColumnName("AuthorEmail");
            this.Property(t => t.AuthorWebSiteUrl).HasColumnName("AuthorWebSiteUrl");
            this.Property(t => t.DateCreated).HasColumnName("DateCreated");
            this.Property(t => t.ContentWithoutHtml).HasColumnName("ContentWithoutHtml");
            this.Property(t => t.IsOpenId).HasColumnName("IsOpenId");



        }
    }
}