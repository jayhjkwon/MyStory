using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace MyStory.Models.Metadata
{
    public class AccountMetadata
    {
        [Required]
        [StringLength(255)]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [MinLength(6)]
        [MaxLength(12)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [MinLength(1)]
        [MaxLength(125)]
        public string FullName { get; set; }
    }
}
