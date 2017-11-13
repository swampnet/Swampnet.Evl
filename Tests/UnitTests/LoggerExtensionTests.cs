using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Swampnet.Evl;
using UnitTests.Mocks;
using Serilog.Sinks.Evl;
using Swampnet.Evl.Client;

namespace UnitTests
{
    [TestClass]
    public class LoggerExtensionTests
    {
        //[TestMethod]
        //public void LoggerExtension_AddProperties_FromObject()
        //{
        //    var logger = Mock.Logger();
        //    var simple = new SimpleClass() { SomeInt = 123, SomeString = "456" };

        //    logger.WithProperties(simple);

        //    var result = (MockedLogger)logger;

        //    Assert.AreEqual(2, result.Properties.Count());
        //}

        [TestMethod]
        public void LoggerExtension_AddProperty()
        {
            var logger = Mock.Logger();

            logger = logger.WithProperty("name-01", "value-01");
            logger = logger.WithProperties(new[] {
                new Property("name-02", "value-02"),
                new Property("name-03", "value-03")
            });

            var result = (MockedLogger)logger;

            Assert.IsTrue(result.Properties.StringValue("name-01") == "value-01");
            Assert.IsTrue(result.Properties.StringValue("name-02") == "value-02");
            Assert.IsTrue(result.Properties.StringValue("name-03") == "value-03");
        }


        [TestMethod]
        public void LoggerExtension_AddPropertyWithCategory()
        {
            var logger = Mock.Logger();

            logger = logger.WithProperty("name-01", "value-01", "category-01");

            var result = (MockedLogger)logger;

            var x = result.Properties.Single();

            Assert.AreEqual($"category-01{EvlSink.CATEGORY_SPLIT}name-01", x.Name);
        }


        [TestMethod]
        public void LoggerExtension_AddTag()
        {
            var logger = Mock.Logger();

            logger = logger.WithTag("tag-01");

            var result = (MockedLogger)logger;

            var x = result.Properties.Single();

            // ~TAG~~CATEGORY~~TAG~   Hmmm, seems wordy...
            Assert.AreEqual($"{EvlSink.TAG_CATEGORY}{EvlSink.CATEGORY_SPLIT}{EvlSink.TAG_CATEGORY}", x.Name);
            Assert.AreEqual($"tag-01", x.Value);
        }


        [TestMethod]
        public void LoggerExtension_AddTags()
        {
            var logger = Mock.Logger();

            logger = logger.WithTags(new[] { "tag-01", "tag-02" });

            var result = (MockedLogger)logger;

            var x = result.Properties.ToArray();

            Assert.AreEqual(2, x.Length);

            Assert.AreEqual($"{EvlSink.TAG_CATEGORY}{EvlSink.CATEGORY_SPLIT}{EvlSink.TAG_CATEGORY}", x[0].Name);
            Assert.AreEqual($"tag-01", x[0].Value);

            Assert.AreEqual($"{EvlSink.TAG_CATEGORY}{EvlSink.CATEGORY_SPLIT}{EvlSink.TAG_CATEGORY}", x[1].Name);
            Assert.AreEqual($"tag-02", x[1].Value);
        }


        [TestMethod]
        public void LoggerExtension_WithMemberName()
        {
            var logger = Mock.Logger();

            logger = logger.WithMembername();

            var result = (MockedLogger)logger;

            var x = result.Properties.Single();

            Assert.AreEqual("LoggerExtension_WithMemberName", x.Value);
        }



        public class SimpleClass
        {
            public int SomeInt { get; set; }
            public string SomeString { get; set; }
        }
    }
}
