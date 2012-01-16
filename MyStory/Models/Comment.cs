using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;

namespace MyStory.Models
{
    [MetadataType(typeof(CommentMetadata))]
    public class Comment
    {
        public int Id { get; set; }
        public string AuthorName {  get; set; }
        public string AuthorEmail { get; set; }
        public string AuthorWebSiteUrl { get; set; }
        public DateTime DateCreated { get; set; }
        public string ContentWithoutHtml { get; set; }
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
        }
    }

    public class CommentMetadata
    {
        [Required]
        [MinLength(1)]
        [MaxLength(125)]
        public string AuthorName { get; set; }

        [Required]
        [MinLength(1)]
        [MaxLength(125)]
        [DataType(DataType.EmailAddress)]
        public string AuthorEmail { get; set; }

        [MaxLength(250)]
        public string AuthorWebSiteUrl { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime DateCreated { get; set; }

        [Required]
        [MinLength(1)]
        [MaxLength(500)]
        public string ContentWithoutHtml { get; set; }
    }
}