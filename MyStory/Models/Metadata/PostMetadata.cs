using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace MyStory.Models.Metadata
{
    public class PostMetadata
    {
        [Required]
        [MinLength(1)]
        [MaxLength(125)]
        public string Title { get; set; }

        [Required]
        public string ContentWithoutHtml { get; set; }

        [Required]
        public string ContentWithHtml { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime DateCreated { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime DateModified { get; set; }
    }
}