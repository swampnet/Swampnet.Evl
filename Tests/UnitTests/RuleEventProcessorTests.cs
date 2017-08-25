using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Swampnet.Evl.Common;
using Swampnet.Evl.Common.Entities;
using Swampnet.Evl.Services;
using Swampnet.Evl.EventProcessors;
using Swampnet.Evl.Client;

namespace UnitTests
{
    [TestClass]
    public class RuleEventProcessorTests
    {
        [TestMethod]
        public void RuleEventProcessor_Test_01()
        {
            var evt = Mock.Event();
            var processor = new RuleEventProcessor(
                Mock.RuleLoader(new[]
                {
                    // Rule: Change 'test' category to 'test-updated'
                    new Rule()
                    {
                        Expression = new Expression(RuleOperatorType.EQ, RuleOperandType.Category, "test"),
                        Actions = new[]
                        {
                            new ActionDefinition(
                                "ChangeCategory", 
                                new[] {
                                    new Property("category", "test-updated")
                                })
                        }
                    }
                }),
                Mock.ActionHandlers());

            //
            Assert.AreEqual("test", evt.Category);

            processor.Process(evt);

            Assert.AreEqual("test-updated", evt.Category);
        }


        // Setting up two rules, the first of which requires the second rule to fire first. (Testing
        // that we keep evaluating rules until we either run out of rules to run, or all the rules
        // return false.)
        [TestMethod]
        public void RuleEventProcessor_Test_02()
        {
            var evt = Mock.Event();
            var processor = new RuleEventProcessor(
                Mock.RuleLoader(new[]
                {
                    // Rule: If category = 'test-updated' then add a property. Note that we're relying on the next
                    //       rule to actually *change* the category to that!
                    new Rule()
                    {
                        Expression = new Expression(RuleOperatorType.EQ, RuleOperandType.Category, "test-updated"),
                        Actions = new[]
                        {
                            new ActionDefinition(
                                "AddProperty",
                                new[]  {
                                    new Property("new-property", "test")
                                })
                        }
                    },


                    // Rule: Change 'test' category to 'test-updated'
                    new Rule()
                    {
                        Expression = new Expression(RuleOperatorType.EQ, RuleOperandType.Category, "test"),
                        Actions = new[]
                        {
                            new ActionDefinition(
                                "ChangeCategory",
                                new[] {
                                    new Property("category", "test-updated")
                                })
                        }
                    }
                }),
                Mock.ActionHandlers());

            //
            Assert.AreEqual("test", evt.Category);
            Assert.IsFalse(evt.Properties.Any(p => p.Name.Equals("new-property")));

            processor.Process(evt);

            Assert.AreEqual("test-updated", evt.Category);
            Assert.IsTrue(evt.Properties.Any(p => p.Name.Equals("new-property")));
        }
    }
}
