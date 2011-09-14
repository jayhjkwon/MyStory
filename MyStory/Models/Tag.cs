using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyStory.Models
{
    public class Tag
    {
        public int Id { get; set; }
        public string TagText { get; set; }
        public virtual ICollection<Post> Posts { get; set; }
    }
}