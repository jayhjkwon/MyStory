using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyStory.Models
{
    public class Account
    {
        public string Email { get; set; }
        public string FullName { get; set; }
        public string Password { get; set; }
        public bool IsOpenId { get; set; }
        public int? BlogId { get; set; }
        public virtual Blog Blog { get; set; }
        public string GravatarUrl { get; set; }
    }
}