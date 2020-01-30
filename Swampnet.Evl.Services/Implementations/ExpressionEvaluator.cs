using Swampnet.Evl.Services.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Swampnet.Evl.Services.Implementations
{

    /// <summary>
    /// Expression Evaluator
    /// </summary>
    class ExpressionEvaluator
    {
        public bool Evaluate(Expression expression, EventEntity evt)
        {
            bool result = false;

            switch (expression.Operator)
            {
                case ExpressionOperatorType.MATCH_ALL:
                    result = MatchAll(expression, evt);
                    break;

                case ExpressionOperatorType.MATCH_ANY:
                    result = MatchAny(expression, evt);
                    break;

                case ExpressionOperatorType.EQ:
                    result = EQ(GetOperand(expression, evt), expression.Value);
                    break;

                case ExpressionOperatorType.NOT_EQ:
                    result = !EQ(GetOperand(expression, evt), expression.Value);
                    break;

                case ExpressionOperatorType.REGEX:
                    result = MatchExpression(GetOperand(expression, evt), expression.Value);
                    break;

                case ExpressionOperatorType.LT:
                    result = LT(GetOperand(expression, evt), expression.Value);
                    break;

                case ExpressionOperatorType.LTE:
                    result = EQ(GetOperand(expression, evt), expression.Value)
                          || LT(GetOperand(expression, evt), expression.Value);
                    break;

                case ExpressionOperatorType.GT:
                    result = GT(GetOperand(expression, evt), expression.Value);
                    break;

                case ExpressionOperatorType.GTE:
                    result = EQ(GetOperand(expression, evt), expression.Value)
                          || GT(GetOperand(expression, evt), expression.Value);
                    break;

                case ExpressionOperatorType.TAGGED:
                    result = IsTagged(evt, expression.Value);
                    break;

                case ExpressionOperatorType.NOT_TAGGED:
                    result = !IsTagged(evt, expression.Value);
                    break;

                default:
                    throw new NotImplementedException(expression.Operator.ToString());
            }

            return result;
        }


        private string GetOperand(Expression expression, EventEntity evt)
        {
            string op = "";

            switch (expression.Operand)
            {
                case ExpressionOperandType.Category:
                    op = evt.Category.Name;
                    break;

                case ExpressionOperandType.Source:
                    op = evt.Source.Name;
                    break;

                case ExpressionOperandType.Property:
                    op = evt.Properties.StringValue(expression.Argument);
                    break;

                case ExpressionOperandType.Summary:
                    op = evt.Summary;
                    break;
            }

            return op;
        }


        private bool EQ(string operand, string value)
        {
            return operand.EqualsNoCase(value);
        }

        /// <summary>
        /// LT - Less than
        /// </summary>
        /// <remarks>
        /// Currently only supports numeric and dates
        /// </remarks>
        private bool LT(string operand, string value)
        {
            if (double.TryParse(operand, out double lhs_nmber) && double.TryParse(value, out double rhs_number))
            {
                return lhs_nmber < rhs_number;
            }

            if (DateTime.TryParse(operand, out DateTime lhs_date) && DateTime.TryParse(value, out DateTime rhs_date))
            {
                return lhs_date < rhs_date;
            }

            return false;
        }

        /// <summary>
        /// GT - Greater than
        /// </summary>
        /// <remarks>
        /// Currently only supports numeric and dates
        /// </remarks>
        private bool GT(string operand, string value)
        {
            if (double.TryParse(operand, out double lhs_nmber) && double.TryParse(value, out double rhs_number))
            {
                return lhs_nmber > rhs_number;
            }

            if (DateTime.TryParse(operand, out DateTime lhs_date) && DateTime.TryParse(value, out DateTime rhs_date))
            {
                return lhs_date > rhs_date;
            }

            return false;
        }

        /// <summary>
        /// Match regular expression
        /// </summary>
        /// <returns></returns>
        private bool MatchExpression(string operand, string value)
        {
            return Regex.IsMatch(operand, value, RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// Returns true if all children expressions evaluate to true
        /// </summary>
        private bool MatchAll(Expression expression, EventEntity evt)
        {
            foreach (var child in expression.Children)
            {
                if (!Evaluate(child, evt))
                {
                    return false;
                }
            }

            return true;
        }


        /// <summary>
        /// Returns true if at least one child expression evaluates to true
        /// </summary>
        /// <remarks>
        /// This will return true on the first expressionthat returns true, so may not evaluate all the expressions
        /// </remarks>
        private bool MatchAny(Expression expression, EventEntity evt)
        {
            foreach (var child in expression.Children)
            {
                if (Evaluate(child, evt))
                {
                    return true;
                }
            }
            return false;
        }


        /// <summary>
        /// Return true f event has the specified tag
        /// </summary>
        /// <param name="evt"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private bool IsTagged(EventEntity evt, string value)
        {
            var tags = evt.EventTags.Select(x => x.Tag.Name);

            if (!tags.Any())
            {
                return false;
            }

            return tags.Select(t => t.ToUpperInvariant()).Contains(value.ToUpperInvariant());
        }
    }
}
