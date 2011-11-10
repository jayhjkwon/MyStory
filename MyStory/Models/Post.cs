using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MyStory.Models.Metadata;
using System.ComponentModel.DataAnnotations;

namespace MyStory.Models
{
    [MetadataType(typeof(PostMetadata))]
    public class Post
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string ContentWithoutHtml { get; set; }
        public string ContentWithHtml { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }

        public virtual ICollection<Tag> Tags { get; set; }
        public Location LocationOfWriting { get; set; }

        public int BlogId { get; set; }
        public virtual Blog Blog { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }
    }
}