using Swampnet.Evl.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Swampnet.Evl.Entities
{
    public class Expression
    {
		public Expression()
		{
			Children = new List<Expression>();
			Operator = RuleOperatorType.MATCH_ALL;
		}

		public Expression(RuleOperatorType op, RuleOperandType operand, string arg, string value)
			: this()
		{
			Operator = op;
			Operand = operand;
			Argument = arg;
			Value = value;
		}


		public Expression(RuleOperatorType op, RuleOperandType operand, string value)
			: this(op, operand, null, value)
		{			
		}


		public Expression(RuleOperatorType op)
			: this(op, RuleOperandType.Null, null, null)
		{
		}


		public RuleOperatorType	Operator { get; set; }
		public RuleOperandType Operand { get; set; }
		public string Argument { get; set; }	 // eg, property name
		public string Value { get; set; }
		public List<Expression> Children { get; set; }

		public bool Evaluate(Event evt)
		{
			bool result = false;

			switch (Operator)
			{
				case RuleOperatorType.MATCH_ALL:
					result = MatchAll(evt);
					break;

				case RuleOperatorType.MATCH_ANY:
					result = MatchAny(evt);
					break;

				case RuleOperatorType.EQ:
					result = Eq(GetOperand(evt), Value);
					break;

				case RuleOperatorType.NOT_EQ:
					result = !Eq(GetOperand(evt), Value);
					break;

				case RuleOperatorType.REGEX:
					result = MatchExpression(GetOperand(evt), Value);
					break;

				case RuleOperatorType.LT:
					result = Lt(GetOperand(evt), Value);
					break;

				case RuleOperatorType.LTE:
					result = Eq(GetOperand(evt), Value) || Lt(GetOperand(evt), Value);
					break;

				case RuleOperatorType.GT:
					result = Gt(GetOperand(evt), Value);
					break;

				case RuleOperatorType.GTE:
					result = Eq(GetOperand(evt), Value) || Gt(GetOperand(evt), Value);
					break;

				default:
					throw new NotImplementedException(Operator.ToString());
			}

			return result;
		}

        public override string ToString()
        {
            return $"{Operator} {Operand} {Argument} '{Value}'";
        }


        private string GetOperand(Event evt)
		{
			string op = "";

			switch (Operand)
			{
				case RuleOperandType.Category:
					op = evt.Category;
					break;

				case RuleOperandType.Source:
					op = evt.Source;
					break;

				case RuleOperandType.Property:
					op = evt.Properties.StringValue(Argument);
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
			if(double.TryParse(operand, out double lhs_nmber) && double.TryParse(value, out double rhs_number))
			{
				return lhs_nmber < rhs_number;
			}

			if(DateTime.TryParse(operand, out DateTime lhs_date) && DateTime.TryParse(value, out DateTime rhs_date))
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

		private bool MatchAll(Event evt)
		{
			foreach(var rule in Children)
			{
				if (!rule.Evaluate(evt))
				{
					return false;
				}
			}

			return true;
		}


		private bool MatchAny(Event evt)
		{
			foreach(var rule in Children)
			{
				if (rule.Evaluate(evt))
				{
					return true;
				}
			}
			return false;
		}
	}

	public enum RuleOperandType
	{
		Null,
		Source,
		Category,
		Summary,
		Property
	}

	public enum RuleOperatorType
	{
		EQ,
		NOT_EQ,
		REGEX,
		GT,
		GTE,
		LT,
		LTE,

		MATCH_ALL,
		MATCH_ANY
	}
}
