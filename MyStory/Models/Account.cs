using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;

namespace MyStory.Models
{
    public class Account
    {
        public int Id { get; set; } // key
        public string Email { get; set; }   // unique key, used when siging in
        public string Name { get; set; }    // unique key
        public string Password { get; set; }
        public string Description { get; set; }
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

            // Properties
            this.Property(a => a.Email)
                .IsRequired()
                .HasMaxLength(125);

            this.Property(a => a.Password)
                .IsRequired()
                .HasMaxLength(12)
                .IsUnicode(false);  // password shouldn't be unicode(nvarchar), but varchar

            this.Property(a => a.Name)
                .IsRequired()
                .HasMaxLength(125);

            this.Property(a => a.Description)
                .HasMaxLength(125);
            
            // Relationships
            this.HasOptional(t => t.Blog)
                .WithRequired(b => b.BlogOwner)
                .WillCascadeOnDelete(true)
                ;
        }
    }
}