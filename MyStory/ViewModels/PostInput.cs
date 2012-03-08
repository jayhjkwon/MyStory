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
        [MinLength(1)]
        [MaxLength(125)]
        public string Title { get; set; }

        [Required]
        [AllowHtml]
        [Display(Name="Content")]
        public string ContentWithHtml { get; set; }

        [ScaffoldColumn(false)]
        public double? Longitude { get; set; }

        [ScaffoldColumn(false)]
        public double? Latitude { get; set; }
    }
}