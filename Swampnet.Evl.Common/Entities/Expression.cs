using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Collections.Generic;

namespace Swampnet.Evl.Common.Entities
{
    public class Expression
    {
		public Expression()
		{
			IsActive = true;
			Children = new Expression[0];
			Operator = RuleOperatorType.MATCH_ALL;
		}

		public Expression(RuleOperatorType op, RuleOperandType operand, string arg, object value)
			: this()
		{
			Operator = op;
			Operand = operand;
			Argument = arg;
			Value = value?.ToString();
		}


		public Expression(RuleOperatorType op, RuleOperandType operand, object value)
			: this(op, operand, null, value)
		{			
		}


		public Expression(RuleOperatorType op)
			: this(op, RuleOperandType.Null, null, null)
		{
		}


        [JsonConverter(typeof(StringEnumConverter))]
        public RuleOperatorType	Operator { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public RuleOperandType Operand { get; set; }

		public string Argument { get; set; }     // eg, property name

        public string Value { get; set; }

        public Expression[] Children { get; set; }

		public bool IsActive { get; set; }

		[JsonIgnore]
        public bool IsContainer => Operator == RuleOperatorType.MATCH_ALL || Operator == RuleOperatorType.MATCH_ANY;

        public override string ToString()
        {
            return IsContainer 
                ? $"{Operator} ({Children.Length} children)"
                : $"{Operand} {Operator} {Argument} '{Value}'";
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
		NULL,

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
