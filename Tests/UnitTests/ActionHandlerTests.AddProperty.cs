using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Swampnet.Evl;
using Swampnet.Evl.Actions;
using Swampnet.Evl.Common;
using Swampnet.Evl.Client;

namespace UnitTests
{
    [TestClass]
    public class AddPropertyActionHandlerTests
    {
        [TestMethod]
        public void Action_AddProperty()
        {
            var evt = Mock.Event();

            // Assert property doesn't exist
            Assert.IsFalse(evt.Properties.Any(p => p.Name == "new-property"));

            var addPropertyAction = new AddPropertyActionHandler();

            addPropertyAction.Apply(
                evt, 
                new[] 
                {
                    new Property("new-property", "new property value")
                });

            // Assert property now exists and has correct value
            Assert.IsTrue(evt.Properties.Any(p => p.Name == "new-property"));
            Assert.AreEqual(evt.Properties.StringValue("new-property"), "new property value");
        }

        [TestMethod]
        public void Action_AddMultipleProperties()
        {
            var evt = Mock.Event();

            // Assert properties don't exist
            Assert.IsFalse(evt.Properties.Any(p => p.Name == "new-property"));
            Assert.IsFalse(evt.Properties.Any(p => p.Name == "new-property-02"));

            var addPropertyAction = new AddPropertyActionHandler();

            addPropertyAction.Apply(
                evt,
                new[]
                {
                    new Property("new-property", "new property value"),
                    new Property("new-property-02", "new property value 02")
                });

            // Assert property now exists and has correct value
            Assert.IsTrue(evt.Properties.Any(p => p.Name == "new-property"));
            Assert.AreEqual(evt.Properties.StringValue("new-property"), "new property value");

            Assert.IsTrue(evt.Properties.Any(p => p.Name == "new-property-02"));
            Assert.AreEqual(evt.Properties.StringValue("new-property-02"), "new property value 02");
        }

        [TestMethod]
        public void Action_NullEventProperties()
        {
            var evt = Mock.Event();
            evt.Properties = null;

            var addPropertyAction = new AddPropertyActionHandler();

            addPropertyAction.Apply(
                evt,
                new[]
                {
                    new Property("new-property", "new property value")
                });

            // Assert property now exists and has correct value
            Assert.IsTrue(evt.Properties.Any(p => p.Name == "new-property"));
            Assert.AreEqual(evt.Properties.StringValue("new-property"), "new property value");
        }

        [TestMethod]
        public void Action_NullArguments()
        {
            var evt = Mock.Event();
            var propertyCount = evt.Properties.Count;

            var addPropertyAction = new AddPropertyActionHandler();

            addPropertyAction.Apply(
                evt,
                null);

            // Make sure we didn't add any properties
            Assert.AreEqual(evt.Properties.Count, propertyCount);
        }

        [TestMethod]
        public void Action_EmptyArguments()
        {
            var evt = Mock.Event();
            var propertyCount = evt.Properties.Count;

            var addPropertyAction = new AddPropertyActionHandler();

            addPropertyAction.Apply(
                evt,
                null);

            // Make sure we didn't add any properties
            Assert.AreEqual(evt.Properties.Count, propertyCount);
        }
    }
}
