using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;

namespace MyStory.Infrastructure.Common
{
    public class SlugConverter
    {
        public string CreateSlug(string Title)
        {
            string Slug = Title.ToLower();
 
            // Replace - with empty space
            Slug = Slug.Replace("-", " ");

            // Replace unwanted characters with space
            Slug = Regex.Replace(Slug, @"[^a-z0-9\s-]", " ");
            
            // Replace multple white spaces with single space
            Slug = Regex.Replace(Slug, @"\s+", " ").Trim();
            
            // Replace white space with -
            Slug = Slug.Replace(" ", "-");
 
            return Slug;
        }
    }
}