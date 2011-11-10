using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MyStory.Models.Metadata;
using System.ComponentModel.DataAnnotations;

namespace MyStory.Models
{
    [MetadataType(typeof(TagMetadata))]
    public class Tag
    {
        public int Id { get; set; }
        public string TagText { get; set; }
        public virtual ICollection<Post> Posts { get; set; }
    }
}