using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace MyStory.Models.Infrastructure
{
    public class MyStoryDbInitializationStrategy : DropCreateDatabaseAlways<MyStoryContext>
    {
        protected override void Seed(MyStoryContext context)
        {
            base.Seed(context);

            context.Database.ExecuteSqlCommand
                ("ALTER TABLE Accounts ADD CONSTRAINT uc_Account_Email UNIQUE NONCLUSTERED(Email)");
        }
    }
}