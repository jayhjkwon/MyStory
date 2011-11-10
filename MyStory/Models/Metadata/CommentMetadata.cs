using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace MyStory.Models.Metadata
{
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