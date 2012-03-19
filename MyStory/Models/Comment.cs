using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;

namespace MyStory.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public string AuthorName {  get; set; }
        public string AuthorEmail { get; set; }
        public string AuthorWebSiteUrl { get; set; }
        public DateTime DateCreated { get; set; }
        public string Content { get; set; }
        public bool IsOpenId { get; set; }

        public int PostId { get; set; }
        public virtual Post Post { get; set; }
    }

    public class CommentMap : EntityTypeConfiguration<Comment>
    {
        public CommentMap()
        {
            // Table
            this.ToTable("Comments");

            // Properties
            this.Property(c => c.AuthorName)
                .IsRequired()
                .HasMaxLength(125);

            this.Property(c => c.AuthorEmail)
                .IsRequired()
                .HasMaxLength(125);

            this.Property(c => c.DateCreated)
                .IsRequired();

            this.Property(c => c.Content)
                .IsRequired()
                .HasMaxLength(500);
        }
    }

}