using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MyStory.Tests.UnitTests
{
    [TestClass]
    public class AuthorizedActionMethodsTest
    {
        // TODO test if Authorize attribute is declared
        [TestMethod]
        public void non_auth_user_cannot_access_write_form_method()
        {
            
        }

        [TestMethod]
        public void only_admin_can_access_write_form_method()
        {
            
        }
    }
}
