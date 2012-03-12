using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;

namespace MyStory.Models
{
    [MetadataType(typeof(AccountMetadata))]
    public class Account
    {
        public int Id { get; set; } // key
        public string Email { get; set; }   // unique key, used when siging in
        public string Name { get; set; }    // unique key
        public string Password { get; set; }
        public bool IsOpenId { get; set; }
        public string GravatarUrl { get; set; }
    
        public virtual Blog Blog { get; set; }
    }

    public class AccountMap : EntityTypeConfiguration<Account>
    {
        public AccountMap()
        {
            // Table
            this.ToTable("Accounts");
            
            // Relationships
            this.HasOptional(t => t.Blog)
                .WithRequired(b => b.BlogOwner)
                .WillCascadeOnDelete(true)
                ;
        }
    }

    public class AccountMetadata
    {
        [Required]
        [StringLength(125)]
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
        public string Name { get; set; }
    }
}