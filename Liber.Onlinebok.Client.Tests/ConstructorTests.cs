using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Liber.Onlinebok.Client.Tests
{
    [TestClass]
    public class ConstructorTests
    {
        [TestMethod]
        public void ConstructClient()
        {
            _ = new LiberOnlinebokClient(Guid.NewGuid(), "test", new System.Net.CookieContainer());
        }
    }
}
