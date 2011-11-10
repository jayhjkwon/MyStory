using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace MyStory.Models.Metadata
{
    public class BlogMetadata
    {
        [Required]
        [MinLength(1)]
        [MaxLength(125)]
        public string Title { get; set; }
    }
}