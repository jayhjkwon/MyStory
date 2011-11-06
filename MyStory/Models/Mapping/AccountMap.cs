using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity.ModelConfiguration;

namespace MyStory.Models.Mapping
{
    public class AccountMap : EntityTypeConfiguration<Account>
    {
        public AccountMap()
        {
            // Table
            this.ToTable("Accounts");

            // PK
            this.HasKey(t => t.Id);

            // Columns
            this.Property(t => t.FullName)
                .HasColumnName("FullName");
            this.Property(t => t.GravatarUrl)
                .HasColumnName("GravatarUrl");
            this.Property(t => t.IsOpenId)
                .HasColumnName("IsOpenId");
            this.Property(t => t.Password)
                .HasColumnName("Password");

            // Relationships
            this.HasOptional(t => t.Blog)
                .WithRequired(b => b.BlogOwner)
                .WillCascadeOnDelete(true)
                ;
                

        }
    }
}