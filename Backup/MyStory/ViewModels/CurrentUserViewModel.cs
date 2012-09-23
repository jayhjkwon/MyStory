using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyStory.ViewModels
{
    public class CurrentUserViewModel
    {
        public string Email { get; set; }   
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsGravatarUse { get; set; }
    }
}