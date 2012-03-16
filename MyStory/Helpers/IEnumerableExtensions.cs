using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MyStory.Models;
using System.Text;

namespace MyStory.Helpers
{
    public static class IEnumerableExtensions
    {
        public static string ConverTagToString(this IEnumerable<Tag> tags)
        {
            StringBuilder val = new StringBuilder();

            tags.Select(t => t.TagText).ToList().ForEach(t => val.Append(t + ", "));

            //string result = val.ToString().Length>0 ? val.ToString().Substring(0,val.ToString().Length-2) : null;

            return val.ToString();
        }

        public static string[] ConverTagToStringArray(this IEnumerable<Tag> tags)
        {
            return tags.Select(t => t.TagText).ToArray();
        }
    }
}