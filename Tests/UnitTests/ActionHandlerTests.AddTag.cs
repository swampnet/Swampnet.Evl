using Microsoft.VisualStudio.TestTools.UnitTesting;
using Swampnet.Evl.Actions;
using Swampnet.Evl.Client;
using Swampnet.Evl.Common.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using UnitTests.Mocks;

namespace UnitTests
{
    [TestClass]
    public class AddTagActionHandlerTests
    {
        [TestMethod]
        public void Action_AddTag()
        {
            var evt = Mock.Event();
            var action = new AddTagActionHandler();

            Assert.IsFalse(evt.Tags.Contains("new-tag"));

            action.ApplyAsync(
                evt,
                new ActionDefinition()
                {
                    Properties = new[]
                    {
                        new Property("tag", "new-tag")
                    }
                },
                new Rule() { Name = "Mocked Rule" }).Wait();

            Assert.IsTrue(evt.Tags.Contains("new-tag"));
        }
    }
}
