using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace MyStory.Infrastructure.Models
{
    public class DbInitializationStrategy : DropCreateDatabaseIfModelChanges<MyStoryContext>
    {
        protected override void Seed(MyStoryContext context)
        {
            base.Seed(context);
        }
    }
}