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
            post.Tags.Clear();
            if (input.Tags != null)
            {
                foreach (var item in input.Tags.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries))
                {
                    if (!string.IsNullOrWhiteSpace(item))
                    {
                        var cnt = dbContext.Tags.Count(t => t.TagText == item);
                        if (cnt > 0)
                        {
                            post.Tags.Add(dbContext.Tags.First(t => t.TagText == item));
                        }
                        else
                        {
                            post.Tags.Add(new Tag { TagText = item });
                        }
                    }
                }
            }
        }
    }
}