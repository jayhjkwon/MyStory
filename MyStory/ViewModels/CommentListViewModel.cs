using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace MyStory.ViewModels
{
    public class CommentListViewModel
    {
        public IList<CommentViewModel> Comments { get; set; }

        public class CommentViewModel
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Email { get; set; }
            public string WebSiteUrl { get; set; }
            public DateTime DateCreated { get; set; }
            public string Content { get; set; }
            public bool IsOpenId { get; set; }
            public int PostId { get; set; }
        }
    }
}