using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using Swampnet.Evl;
using System.Linq;

namespace UnitTests
{
    [TestClass]
    public class ObjectExtensionTests
    {
        [TestMethod]
        public void ObjectExtension_ToPublicProperties()
        {
            var o = new SimpleClass()
            {
                SomeInt = 123,
                SomeString = "string"
            };

            var prps = o.GetPublicProperties();

            Assert.IsNotNull(prps);
            Assert.AreEqual(2, prps.Count());

            var someIntProperty = prps.SingleOrDefault(p => p.Name == "SomeInt");
            Assert.IsNotNull(someIntProperty);
            Assert.AreEqual("SimpleClass", someIntProperty.Category);
            Assert.AreEqual("123", someIntProperty.Value);

            var someStringProperty = prps.SingleOrDefault(p => p.Name == "SomeString");
            Assert.AreEqual("SimpleClass", someStringProperty.Category);
            Assert.IsNotNull(someStringProperty);
            Assert.AreEqual("string", someStringProperty.Value);
        }
    }

    public class SimpleClass
    {
        public int SomeInt { get; set; }
        public string SomeString { get; set; }
        private int SomePrivateProperty { get; set; }
    }
}
