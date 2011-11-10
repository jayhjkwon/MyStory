using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using MyStory.Models.Metadata;

namespace MyStory.Models
{
    [MetadataType(typeof(BlogMetadata))]
    public class Blog
    {
        public int Id { get; set; }
        public string Title { get; set; }

        public virtual Account BlogOwner { get; set; }
        public virtual ICollection<Post> Posts { get; set; }
    }
}