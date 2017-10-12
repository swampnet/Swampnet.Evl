using Microsoft.VisualStudio.TestTools.UnitTesting;
using UnitTests.Mocks;
using Swampnet.Evl;
using Serilog;
using Swampnet.Evl.Client;
using System.Linq;
using Serilog.Sinks.Evl;

namespace UnitTests
{
    [TestClass]
    public class SerilogSinkTests
    {
        [TestMethod]
        public void SerilogSink_AddProperty()
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
        public void SerilogSink_AddPropertyWithCategory()
        {
            var logger = Mock.Logger();

            logger = logger.WithProperty("name-01", "value-01", "category-01");

            var result = (MockedLogger)logger;

            var x = result.Properties.Single();

            Assert.AreEqual($"category-01{EvlSink.CATEGORY_SPLIT}name-01", x.Name);
        }


        [TestMethod]
        public void SerilogSink_AddTag()
        {
            var logger = Mock.Logger();

            logger = logger.WithTag("tag-01");

            var result = (MockedLogger)logger;

            var x = result.Properties.Single();

            // ~TAG~~CATEGORY~~TAG~   Hmmm, seems wordy...
            Assert.AreEqual($"{EvlSink.TAG_CATEGORY}{EvlSink.CATEGORY_SPLIT}{EvlSink.TAG_CATEGORY}", x.Name);
            Assert.AreEqual($"tag-01", x.Value);
        }
    }
}
