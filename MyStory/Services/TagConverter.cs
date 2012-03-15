using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MyStory.Models;

namespace MyStory.Services
{
    public class TagConverter
    {
        private readonly MyStoryContext dbContext;

        public TagConverter(MyStoryContext context)
        {
            this.dbContext = context;
        }

        public List<Tag> ConvertToTagList(string tags)
        {
            List<Tag> tagList = new List<Tag>();

            if (string.IsNullOrWhiteSpace(tags))
                return null;

            var tagsArray = tags.Split(new string[]{","}, StringSplitOptions.RemoveEmptyEntries);

            foreach (var item in tagsArray)
            {
                var itemTrimed = item.Trim();
                var tag = dbContext.Tags.FirstOrDefault(t => t.TagText == itemTrimed);
                if (tag != null)
                {
                    tagList.Add(tag);
                }
                else
                {
                    if (!string.IsNullOrWhiteSpace(itemTrimed))
                        tagList.Add(new Tag { TagText = itemTrimed });
                }
            }

            return tagList;
        }
    }
}