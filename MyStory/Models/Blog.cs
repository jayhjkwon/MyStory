using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyStory.Models
{
    public class Blog
    {
        public int Id { get; set; }
        public string Title { get; set; }

        public virtual Account BlogOwner { get; set; }
    }
}