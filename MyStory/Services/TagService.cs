using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MyStory.ViewModels;
using MyStory.Models;

namespace MyStory.Services
{
    public class TagService
    {
        public void UpdateTag(MyStoryContext dbContext, PostInput input, Post post)
        {
            if (post.Tags != null) post.Tags.Clear();

            if (input.Tags != null)
            {
                foreach (var item in input.Tags.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries))
                {
                    var tagText = item.Trim();

                    if (!string.IsNullOrWhiteSpace(tagText))
                    {
                        var cnt = dbContext.Tags.Count(t => t.TagText == tagText);
                        if (cnt > 0)
                        {
                            var tag = dbContext.Tags.First(t => t.TagText == tagText);
                            post.Tags.Add(tag);
                        }
                        else
                        {
                            post.Tags.Add(new Tag { TagText = tagText });
                        }
                    }
                }
            }
        }
    }
}