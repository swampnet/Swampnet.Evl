﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using Swampnet.Evl.Common;
using Swampnet.Evl.Common.Entities;
using Swampnet.Evl.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace UnitTests
{
	[TestClass]
	public class ExpressionEvaluatorTests
    {
		// Test basic EQ operator
		[TestMethod]
		public void Expression_EQ_01()
		{
			var evt = Mock.Event();
			var expression = new Expression(RuleOperatorType.EQ, RuleOperandType.Category, "test");
            var evaluator = new ExpressionEvaluator();

			var expected = true;
			var actual = evaluator.Evaluate(expression, evt);

			Assert.AreEqual(expected, actual);
		}

		// Test basic EQ operator against a property
		[TestMethod]
		public void Expression_EQ_02()
		{
			var evt = Mock.Event();
			var expression = new Expression(RuleOperatorType.EQ, RuleOperandType.Property, "some-property", "test");
            var evaluator = new ExpressionEvaluator();

            var expected = true;
			var actual = evaluator.Evaluate(expression, evt);

			Assert.AreEqual(expected, actual);
		}


		// Test basic EQ operator against a property that doesn't exist
		[TestMethod]
		public void Expression_EQ_03()
		{
			var evt = Mock.Event();
			var expression = new Expression(RuleOperatorType.EQ, RuleOperandType.Property, "some-non-existant-property", "test");
            var evaluator = new ExpressionEvaluator();

            var expected = false;
			var actual = evaluator.Evaluate(expression, evt);

			Assert.AreEqual(expected, actual);
		}


		// Test basic NOT_EQ operator
		[TestMethod]
		public void Expression_NOT_EQ_01()
		{
			var evt = Mock.Event();
			var expression = new Expression(RuleOperatorType.NOT_EQ, RuleOperandType.Category, "test-xxx");
            var evaluator = new ExpressionEvaluator();

            var expected = true;
			var actual = evaluator.Evaluate(expression, evt);

			Assert.AreEqual(expected, actual);
		}

		// Test basic Expression operator
		[TestMethod]
		public void Expression_Regex_01()
		{
			var evt = Mock.Event();
			var expression = new Expression(RuleOperatorType.REGEX, RuleOperandType.Category, "t.*t");
            var evaluator = new ExpressionEvaluator();

            var expected = true;
			var actual = evaluator.Evaluate(expression, evt);

			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void Expression_LT_01()
		{
			var evt = Mock.Event();
			var expression = new Expression(RuleOperatorType.LT, RuleOperandType.Property, "two", "3");
            var evaluator = new ExpressionEvaluator();

            var expected = true;
			var actual = evaluator.Evaluate(expression, evt);

			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void Expression_LTE_01()
		{
			var evt = Mock.Event();
			var expression = new Expression(RuleOperatorType.LTE, RuleOperandType.Property, "two", "2");
            var evaluator = new ExpressionEvaluator();

            var expected = true;
			var actual = evaluator.Evaluate(expression, evt);

			Assert.AreEqual(expected, actual);
		}


		[TestMethod]
		public void Expression_GT_01()
		{
			var evt = Mock.Event();
			var expression = new Expression(RuleOperatorType.GT, RuleOperandType.Property, "two", "1");
            var evaluator = new ExpressionEvaluator();

            var expected = true;
			var actual = evaluator.Evaluate(expression, evt);

			Assert.AreEqual(expected, actual);
		}


        [TestMethod]
        public void Expression_LT_Date_01()
        {
            //var dt = DateTime.Parse("2008-12-11 03:00:00");

            var evt = Mock.Event();
            var expression = new Expression(RuleOperatorType.LT, RuleOperandType.Property, "some-date", "2009-12-11 03:00:00");
            var evaluator = new ExpressionEvaluator();

            var expected = true;
            var actual = evaluator.Evaluate(expression, evt);

            Assert.AreEqual(expected, actual);
        }


        // Test MATCH_ALL operator
        [TestMethod]
		public void Expression_MATCH_ALL()
		{
			var evt = Mock.Event();
			var expression = new Expression(RuleOperatorType.MATCH_ALL)
			{
				Children = new List<Expression>()
				{
					new Expression(RuleOperatorType.EQ, RuleOperandType.Category, "test"),
					new Expression(RuleOperatorType.NOT_EQ, RuleOperandType.Category, "test-xxx")
				}
			};
            var evaluator = new ExpressionEvaluator();

            var expected = true;
			var actual = evaluator.Evaluate(expression, evt);

			Assert.AreEqual(expected, actual);
		}


		// Test MATCH_ANY operator
		[TestMethod]
		public void Expression_MATCH_ANY()
		{
			var evt = Mock.Event();
			var expression = new Expression(RuleOperatorType.MATCH_ANY)
			{
				Children = new List<Expression>()
				{
					new Expression(RuleOperatorType.EQ, RuleOperandType.Category, "test-xxx"),
					new Expression(RuleOperatorType.EQ, RuleOperandType.Category, "test"),
					new Expression(RuleOperatorType.EQ, RuleOperandType.Category, "test-yyy")
				}
			};
            var evaluator = new ExpressionEvaluator();

            var expected = true;
			var actual = evaluator.Evaluate(expression, evt);

			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void Expression_Complex_01()
		{
			var evt = Mock.Event();
			var expression = new Expression(RuleOperatorType.MATCH_ALL)
			{
				Children = new List<Expression>()
				{
					new Expression(RuleOperatorType.EQ, RuleOperandType.Category, "test"),
					new Expression(RuleOperatorType.MATCH_ANY)
					{
						Children = new List<Expression>()
						{
							new Expression(RuleOperatorType.EQ, RuleOperandType.Property, "some-property", "test-xxx"),
							new Expression(RuleOperatorType.EQ, RuleOperandType.Property, "some-property", "test-yyy"),
							new Expression(RuleOperatorType.EQ, RuleOperandType.Property, "some-property", "test")
						}
					}
				}
			};
            var evaluator = new ExpressionEvaluator();


            var expected = true;
			var actual = evaluator.Evaluate(expression, evt);

			Assert.AreEqual(expected, actual);
		}
	}
}