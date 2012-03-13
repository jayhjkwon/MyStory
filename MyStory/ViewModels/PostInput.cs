using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace MyStory.ViewModels
{
    public class PostInput
    {
        [Required]
        [HiddenInput]
        public int Id { get; set; }

        [Required]
        [MinLength(1)]
        [MaxLength(125)]
        public string Title { get; set; }

        [Required]
        [AllowHtml]
        [Display(Name="Content")]
        public string Content { get; set; }

        [HiddenInput]
        public double? Longitude { get; set; }

        [HiddenInput]
        public double? Latitude { get; set; }

        //public List<TagViewModel> Tags { get; set; }
        public string Tags { get; set; }
    }
}