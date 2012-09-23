using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MyStory.Models;

namespace MyStory.QueryObjects
{
    public class PostQuery : IQuery<Post>
    {
        public int? CurrentPageNumber { get; set; }
        public int? PostsPerPage { get; set; }
        public string Tag { get; set; }

        public IQueryable<Post> GetQuery(MyStoryContext dbContext)
        { 
            var query = dbContext.Posts.OrderByDescending(p => p.DateCreated).AsQueryable();

            if (!string.IsNullOrWhiteSpace(Tag))
            {   
                query = query.Where(p => p.Tags.Any(t => t.TagText == Tag));
            }

            // skip & take logic should be come at the end of the query
            if (CurrentPageNumber != null && PostsPerPage != null)
            {
                query = query.Skip((CurrentPageNumber.Value - 1) * PostsPerPage.Value).Take(PostsPerPage.Value);
            }

            return query;
        }
    }
}