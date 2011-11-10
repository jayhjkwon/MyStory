using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyStory.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public string AuthorName {  get; set; }
        public string AuthorEmail { get; set; }
        public string AuthorWebSiteUrl { get; set; }
        public DateTime DateCreated { get; set; }
        public string ContentWithoutHtml { get; set; }
        public bool IsOpenId { get; set; }

        public int PostId { get; set; }
        public virtual Post Post { get; set; }
    }
}