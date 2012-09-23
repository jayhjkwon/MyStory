using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyStory.ViewModels
{
    public class PostDetailViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime DateCreated { get; set; }
        public string[] Tags { get; set; }

        public CommentInput CommentInput { get; set; }
    }
}