using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Swampnet.Evl;
using Swampnet.Evl.Client;

namespace UnitTests
{
    [TestClass]
    public class ExceptionExtensionTests
    {
        [TestMethod]
        public void ExceptionExtensions_AddData()
        {
            var ex = new Exception();

            ex.AddData("one", 1);

            Assert.IsTrue(ex.Data.Contains("one"));
            Assert.AreEqual(ex.Data["one"], 1);
        }


        [TestMethod]
        public void ExceptionExtensions_UpdateData()
        {
            var ex = new Exception();

            ex.AddData("one", 1);

            Assert.IsTrue(ex.Data.Contains("one"));
            Assert.AreEqual(ex.Data["one"], 1);

            ex.AddData("one", "uno");

            Assert.IsTrue(ex.Data.Contains("one"));
            Assert.AreEqual(ex.Data["one"], "uno");
        }


        [TestMethod]
        public void ExceptionExtensions_AddProperty()
        {
            var ex = new Exception();

            ex.AddData(new Property("one", 1));

            Assert.IsTrue(ex.Data.Contains("one"));
            Assert.AreEqual(ex.Data["one"], "1");
        }


        [TestMethod]
        public void ExceptionExtensions_UpdateProperty()
        {
            var ex = new Exception();

            ex.AddData(new Property("one", 1));

            Assert.IsTrue(ex.Data.Contains("one"));
            Assert.AreEqual(ex.Data["one"], "1");

            ex.AddData(new Property("one", "uno"));

            Assert.IsTrue(ex.Data.Contains("one"));
            Assert.AreEqual(ex.Data["one"], "uno");
        }


        [TestMethod]
        public void ExceptionExtensions_AddProperties()
        {
            var ex = new Exception();

            ex.AddData(
                new[] {
                    new Property("one", 1),
                    new Property("two", 2),
                    new Property("three", 3)
                });

            Assert.IsTrue(ex.Data.Contains("one"));
            Assert.AreEqual(ex.Data["one"], "1");

            Assert.IsTrue(ex.Data.Contains("two"));
            Assert.AreEqual(ex.Data["two"], "2");

            Assert.IsTrue(ex.Data.Contains("three"));
            Assert.AreEqual(ex.Data["three"], "3");
        }

    }
}
