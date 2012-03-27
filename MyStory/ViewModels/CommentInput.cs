using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace MyStory.ViewModels
{
    public class CommentInput
    {
        [Required]
        [StringLength(125)]
        public string Name { get; set; }

        [Required]
        [StringLength(125)]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        
        [DataType(DataType.Url)]
        public string Url { get; set; }

        [Required]
        public string Content { get; set; }

        public string OpenId { get; set; }
    }
}