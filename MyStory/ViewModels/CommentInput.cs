using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace MyStory.ViewModels
{
    public class CommentInput
    {
        public int Id { get; set; }

        [Required]
        [StringLength(125)]
        public string Name { get; set; }

        [Required]
        [StringLength(125)]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        public string WebSiteUrl { get; set; }

        [Required]
        public DateTime DateCreated { get; set; }

        [Required]
        [StringLength(125)]
        public string Content { get; set; }

        public bool IsOpenId { get; set; }

        public int PostId { get; set; }
    }
}