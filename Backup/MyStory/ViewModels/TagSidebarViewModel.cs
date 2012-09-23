using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace MyStory.ViewModels
{
    public class TagSidebarViewModel
    {
        public int Id { get; set; }
        public string TagText { get; set; }
        public int Count { get; set; }
    }
}