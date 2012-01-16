using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity.ModelConfiguration;

namespace MyStory.Models
{
    public class Location
    {
        public double Longitude { get; set; }
        public double Latitude { get; set; }
    }

    public class LocationMap : ComplexTypeConfiguration<Location>
    {
        public LocationMap()
        {
            this.Property(t => t.Latitude).HasColumnName("Latitude");
            this.Property(t => t.Longitude).HasColumnName("Longitude");
        }
    }
}