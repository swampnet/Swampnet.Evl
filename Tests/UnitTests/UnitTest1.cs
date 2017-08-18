using Microsoft.VisualStudio.TestTools.UnitTesting;
using Swampnet.Evl.Common;
using Swampnet.Evl.Interfaces;
using Swampnet.Evl.Services;
using System;
using System.Threading.Tasks;

namespace UnitTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var processor = new MockedEventProcessor();
            var queue = new EventProcessorQueue(new[] { processor });

            queue.Enqueue(new Event()
            {
                Summary = "Mocked Event",
                TimestampUtc = DateTime.UtcNow
            });

            Task.Delay(1000).Wait();

            Assert.IsTrue(processor.WasCalled);
        }
    }


    public class MockedEventProcessor : IEventProcessor
    {
        public int Priority => 0;
        public bool WasCalled { get; set; } = false;

        public void Process(Event evt)
        {
            WasCalled = true;
        }
    }
}
