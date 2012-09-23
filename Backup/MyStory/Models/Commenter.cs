using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity.ModelConfiguration;

namespace MyStory.Models
{
    public class Commenter
    {
        public int Id { get; set; }  // PK

        public string Email { get; set; }   // Unique Key
        public string Name { get; set; }
        public string Url { get; set; }

        public string OpenId { get; set; }  // ClaimedIdentifier if openId used
    }

    public class CommenterMap : EntityTypeConfiguration<Commenter>
    {
        public CommenterMap()
        {
            // Table
            this.ToTable("Commenters");

            // Properties
            this.Property(c => c.Email)
                .IsRequired()
                .HasMaxLength(125);

            this.Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(125);

            this.Property(c => c.Url)
                .HasMaxLength(125);


        }
    }
}