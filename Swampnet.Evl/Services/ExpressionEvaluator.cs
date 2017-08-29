using System;
using Swampnet.Evl.Common;
using Swampnet.Evl.Common.Entities;
using System.Text.RegularExpressions;
using Swampnet.Evl.Client;

namespace Swampnet.Evl.Services
{
    class ExpressionEvaluator
    {
        public bool Evaluate(Expression expression, Event evt)
        {
            bool result = false;

            switch (expression.Operator)
            {
                case RuleOperatorType.MATCH_ALL:
                    result = MatchAll(expression, evt);
                    break;

                case RuleOperatorType.MATCH_ANY:
                    result = MatchAny(expression, evt);
                    break;

                case RuleOperatorType.EQ:
                    result = Eq(GetOperand(expression, evt), expression.Value);
                    break;

                case RuleOperatorType.NOT_EQ:
                    result = !Eq(GetOperand(expression, evt), expression.Value);
                    break;

                case RuleOperatorType.REGEX:
                    result = MatchExpression(GetOperand(expression, evt), expression.Value);
                    break;

                case RuleOperatorType.LT:
                    result = Lt(GetOperand(expression, evt), expression.Value);
                    break;

                case RuleOperatorType.LTE:
                    result = Eq(GetOperand(expression, evt), expression.Value) || Lt(GetOperand(expression, evt), expression.Value);
                    break;

                case RuleOperatorType.GT:
                    result = Gt(GetOperand(expression, evt), expression.Value);
                    break;

                case RuleOperatorType.GTE:
                    result = Eq(GetOperand(expression, evt), expression.Value) || Gt(GetOperand(expression, evt), expression.Value);
                    break;

                default:
                    throw new NotImplementedException(expression.Operator.ToString());
            }

            return result;
        }


        private string GetOperand(Expression expression, Event evt)
        {
            string op = "";

            switch (expression.Operand)
            {
                case RuleOperandType.Category:
                    op = evt.Category.ToString();
                    break;

                case RuleOperandType.Source:
                    op = evt.Source;
                    break;

                case RuleOperandType.Property:
                    op = evt.Properties.StringValue(expression.Argument);
                    break;

                case RuleOperandType.Summary:
                    op = evt.Summary;
                    break;
            }

            return op;
        }


        private bool Eq(string operand, string value)
        {
            return operand.EqualsNoCase(value);
        }

        private bool Lt(string operand, string value)
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

        private bool Gt(string operand, string value)
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

        private bool MatchExpression(string operand, string value)
        {
            return Regex.IsMatch(operand, value, RegexOptions.IgnoreCase);
        }

        private bool MatchAll(Expression expression, Event evt)
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


        private bool MatchAny(Expression expression, Event evt)
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
    }
}
