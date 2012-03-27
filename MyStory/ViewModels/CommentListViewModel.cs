using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace MyStory.ViewModels
{
    public class CommentListViewModel
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime DateCreated { get; set; }
        public int PostId { get; set; }
        public string CommenterEmail { get; set; }
        public string CommenterName { get; set; }
        public string CommenterUrl { get; set; }
    }
}