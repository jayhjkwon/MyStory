using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace MyStory.ViewModels
{
    public class AccountInput
    {
        [Required]
        [StringLength(125)]
        [DataType(DataType.EmailAddress)]
        [Display(Name="Email", Description="The email will be used for signing in")]
        public string AccountEmail { get; set; }

        [Required]
        [MinLength(6)]
        [MaxLength(12)]
        [DataType(DataType.Password)]
        [Display(Name="Password")]
        public string AccountPassword { get; set; }

        [Required]
        [MinLength(1)]
        [MaxLength(125)]
        [Display(Name="User Name", Description="This name will be used in blog")]
        public string AccountFullName { get; set; }

        [Required]
        [MinLength(1)]
        [MaxLength(125)]
        [Display(Name="Blog Title")]
        public string BlogTitle { get; set; }

        [Display(Name="Remember Me?")]
        public bool RememberMe { get; set; }
    }
}