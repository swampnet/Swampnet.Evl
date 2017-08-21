using Microsoft.VisualStudio.TestTools.UnitTesting;
using Swampnet.Evl;
using Swampnet.Evl.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace UnitTests
{
    [TestClass]
    public class PropertyTests
    {
        [TestMethod]
        public void Properties_StringValue_01()
        {
            var properties = Mock.Properties();
            var expected = "test";
            var actual = properties.StringValue("some-property");

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Properties_StringValue_Default()
        {
            var properties = Mock.Properties();
            var expected = "default-value";
            var actual = properties.StringValue("some-madeup-property", expected);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Properties_StringValue_NullProperties()
        {
            IEnumerable<IProperty> properties = null;

            var expected = "default-value";
            var actual = properties.StringValue("my-property", "default-value");
            Assert.AreEqual(expected, actual);
        }
    }
}
