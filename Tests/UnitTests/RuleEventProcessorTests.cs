using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Swampnet.Evl.Actions;
using Swampnet.Evl.Common;
using Swampnet.Evl.Entities;
using Swampnet.Evl.Interfaces;
using Swampnet.Evl.Services;
using System;
using System.Collections.Generic;
using System.Text;

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
                            new ActionDefinition("ChangeCategory")
                            {
                                Properties = new[]
                                {
                                    new Property("category", "test-updated")
                                }
                            }
                        }
                    }
                }),
                Mock.ActionHandlers());

            //
            Assert.AreEqual("test", evt.Category);

            processor.Process(evt);

            Assert.AreEqual("test-updated", evt.Category);
        }


        [TestMethod]
        public void RuleEventProcessor_Test_02()
        {
            var evt = Mock.Event();
            var processor = new RuleEventProcessor(
                // Setting up two rules, the first of which requires the second rule to fire first. (Testing
                // that we keep evaluating rules until we either run out of rules to run, or all the rules
                // return false.
                Mock.RuleLoader(new[]
                {
                    // Rule: If category = 'test-updated' then add a property
                    new Rule()
                    {
                        Expression = new Expression(RuleOperatorType.EQ, RuleOperandType.Category, "test-updated"),
                        Actions = new[]
                        {
                            new ActionDefinition("AddProperty")
                            {
                                Properties = new[]
                                {
                                    new Property("new-property", "test")
                                }
                            }
                        }
                    },


                    // Rule: Change 'test' category to 'test-updated'
                    new Rule()
                    {
                        Expression = new Expression(RuleOperatorType.EQ, RuleOperandType.Category, "test"),
                        Actions = new[]
                        {
                            new ActionDefinition("ChangeCategory")
                            {
                                Properties = new[]
                                {
                                    new Property("category", "test-updated")
                                }
                            }
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
