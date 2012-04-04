using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyStory.ViewModels
{
    public class CommentSidebarViewModel
    {
        public int PostId { get; set; }
        public int CommentId { get; set; }
        public string Content { get; set; }
        public DateTime DateCreated { get; set; }
        public string CommenterName { get; set; }
    }
}