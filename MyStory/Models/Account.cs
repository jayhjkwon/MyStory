using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MyStory.Models.Metadata;

namespace MyStory.Models
{
    [System.ComponentModel.DataAnnotations.MetadataType(typeof(AccountMetadata))]
    public class Account
    {
        public int Id { get; set; } // key
        public string Email { get; set; }   // used when siging in
        public string FullName { get; set; }
        public string Password { get; set; }
        public bool IsOpenId { get; set; }
        public string GravatarUrl { get; set; }
    
        public virtual Blog Blog { get; set; }
    }
}