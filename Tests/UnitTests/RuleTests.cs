using Microsoft.VisualStudio.TestTools.UnitTesting;
using Swampnet.Evl.Common;
using Swampnet.Evl.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace UnitTests
{
	[TestClass]
	public class RuleTests
    {
		// Test basic EQ operator
		[TestMethod]
		public void Rule_EQ_01()
		{
			var evt = Mock.Event();
			var rule = new Rule(RuleOperatorType.EQ, RuleOperandType.Category, "test");

			var expected = true;
			var actual = rule.Evaluate(evt);

			Assert.AreEqual(expected, actual);
		}

		// Test basic EQ operator against a property
		[TestMethod]
		public void Rule_EQ_02()
		{
			var evt = Mock.Event();
			var rule = new Rule(RuleOperatorType.EQ, RuleOperandType.Property, "some-property", "test");

			var expected = true;
			var actual = rule.Evaluate(evt);

			Assert.AreEqual(expected, actual);
		}


		// Test basic EQ operator against a property that doesn't exist
		[TestMethod]
		public void Rule_EQ_03()
		{
			var evt = Mock.Event();
			var rule = new Rule(RuleOperatorType.EQ, RuleOperandType.Property, "some-non-existant-property", "test");

			var expected = false;
			var actual = rule.Evaluate(evt);

			Assert.AreEqual(expected, actual);
		}


		// Test basic NOT_EQ operator
		[TestMethod]
		public void Rule_NOT_EQ_01()
		{
			var evt = Mock.Event();
			var rule = new Rule(RuleOperatorType.NOT_EQ, RuleOperandType.Category, "test-xxx");

			var expected = true;
			var actual = rule.Evaluate(evt);

			Assert.AreEqual(expected, actual);
		}

		// Test basic Expression operator
		[TestMethod]
		public void Rule_Expression_01()
		{
			var evt = Mock.Event();
			var rule = new Rule(RuleOperatorType.MATCH_EXPRESSION, RuleOperandType.Category, "t.*t");

			var expected = true;
			var actual = rule.Evaluate(evt);

			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void Rule_LT_01()
		{
			var evt = Mock.Event();
			var rule = new Rule(RuleOperatorType.LT, RuleOperandType.Property, "two", "3");

			var expected = true;
			var actual = rule.Evaluate(evt);

			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void Rule_LTE_01()
		{
			var evt = Mock.Event();
			var rule = new Rule(RuleOperatorType.LTE, RuleOperandType.Property, "two", "2");

			var expected = true;
			var actual = rule.Evaluate(evt);

			Assert.AreEqual(expected, actual);
		}


		[TestMethod]
		public void Rule_GT_01()
		{
			var evt = Mock.Event();
			var rule = new Rule(RuleOperatorType.GT, RuleOperandType.Property, "two", "1");

			var expected = true;
			var actual = rule.Evaluate(evt);

			Assert.AreEqual(expected, actual);
		}


		// Test MATCH_ALL operator
		[TestMethod]
		public void Rule_MATCH_ALL()
		{
			var evt = Mock.Event();
			var rule = new Rule(RuleOperatorType.MATCH_ALL)
			{
				Children = new List<Rule>()
				{
					new Rule(RuleOperatorType.EQ, RuleOperandType.Category, "test"),
					new Rule(RuleOperatorType.NOT_EQ, RuleOperandType.Category, "test-xxx")
				}
			};

			var expected = true;
			var actual = rule.Evaluate(evt);

			Assert.AreEqual(expected, actual);
		}


		// Test MATCH_ANY operator
		[TestMethod]
		public void Rule_MATCH_ANY()
		{
			var evt = Mock.Event();
			var rule = new Rule(RuleOperatorType.MATCH_ANY)
			{
				Children = new List<Rule>()
				{
					new Rule(RuleOperatorType.EQ, RuleOperandType.Category, "test-xxx"),
					new Rule(RuleOperatorType.EQ, RuleOperandType.Category, "test"),
					new Rule(RuleOperatorType.EQ, RuleOperandType.Category, "test-yyy")
				}
			};


			var expected = true;
			var actual = rule.Evaluate(evt);

			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void Rule_Complex_01()
		{
			var evt = Mock.Event();
			var rule = new Rule(RuleOperatorType.MATCH_ALL)
			{
				Children = new List<Rule>()
				{
					new Rule(RuleOperatorType.EQ, RuleOperandType.Category, "test"),
					new Rule(RuleOperatorType.MATCH_ANY)
					{
						Children = new List<Rule>()
						{
							new Rule(RuleOperatorType.EQ, RuleOperandType.Property, "some-property", "test-xxx"),
							new Rule(RuleOperatorType.EQ, RuleOperandType.Property, "some-property", "test-yyy"),
							new Rule(RuleOperatorType.EQ, RuleOperandType.Property, "some-property", "test")
						}
					}
				}
			};


			var expected = true;
			var actual = rule.Evaluate(evt);

			Assert.AreEqual(expected, actual);
		}

		static class Mock
		{
			public static Event Event()
			{
				return new Event()
				{
					Source = "source",
					Category = "test",
					Properties = new List<Property>()
					{
						new Property("some-property", "test"),
						new Property("some-other-property", "some-other-value"),
						new Property("one", "1"),
						new Property("two", "2"),
						new Property("three", "3")
					}
				};
			}
		}
	}
}
