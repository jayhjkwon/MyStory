using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using MyStory.Models;

namespace MyStory.QueryObjects
{
    public interface IQuery<T> where T : class
    {
        IQueryable<T> GetQuery(MyStoryContext context);
    }
}