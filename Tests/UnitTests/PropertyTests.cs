using UnitTests.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Swampnet.Evl;
using Swampnet.Evl.Client;
using System;
using System.Collections.Generic;

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
            var actual = properties.StringValue("my-property", expected);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Properties_IntValue_01()
        {
            var properties = Mock.Properties();
            var expected = 1;
            var actual = properties.IntValue("one");

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Properties_IntValue_Default()
        {
            var properties = Mock.Properties();
            var expected = 123;
            var actual = properties.IntValue("some-madeup-property", expected);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Properties_DoubleValue_01()
        {
            var properties = Mock.Properties();
            var expected = 1.23;
            var actual = properties.DoubleValue("one-point-two-three");

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Properties_DoubleValue_Default()
        {
            var properties = Mock.Properties();
            var expected = 123.45;
            var actual = properties.DoubleValue("some-madeup-property", expected);

            Assert.AreEqual(expected, actual);
        }


        [TestMethod]
        public void Properties_DateValue_01()
        {
            var properties = Mock.Properties();
            var expected = DateTime.Parse("2008-12-11 03:00:00");
            var actual = properties.DateTimeValue("some-date");

            Assert.AreEqual(expected, actual);
        }


        [TestMethod]
        public void Properties_DateValue_Default()
        {
            var properties = Mock.Properties();
            var expected = DateTime.Parse("2008-12-11 03:00:00");
            var actual = properties.DateTimeValue("some-madeup-property", expected);

            Assert.AreEqual(expected, actual);
        }

    }
}
