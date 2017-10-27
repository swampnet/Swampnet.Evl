using Microsoft.VisualStudio.TestTools.UnitTesting;
using Swampnet.Evl.Actions;
using Swampnet.Evl.Client;
using Swampnet.Evl.Common.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using UnitTests.Mocks;
using System.Linq;

namespace UnitTests
{
	[TestClass]
	public class RemoveTagActionHandlerTests
	{
		[TestMethod]
		public void Action_RemoveTag()
		{
            var evt = Mock.Event();
            evt.Tags.Add("new-tag");

            var action = new RemoveTagActionHandler();

            Assert.IsTrue(evt.Tags.Contains("new-tag"));

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

            Assert.IsFalse(evt.Tags.Contains("new-tag"));
        }


        [TestMethod]
        public void Action_RemoveTag_DoesntExistInFirstPlace()
        {
            var evt = Mock.Event();

            var action = new RemoveTagActionHandler();

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

            Assert.IsFalse(evt.Tags.Contains("new-tag"));
        }


        [TestMethod]
        public void Action_RemoveTag_MultipleInstances()
        {
            var evt = Mock.Event();

            evt.Tags.Add("new-tag");
            evt.Tags.Add("new-tag");
            evt.Tags.Add("new-tag");

            var action = new RemoveTagActionHandler();

            Assert.IsTrue(evt.Tags.Count(t => t == "new-tag") == 3);

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

            Assert.IsFalse(evt.Tags.Contains("new-tag"));
        }
    }
}
