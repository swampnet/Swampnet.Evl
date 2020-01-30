using System;
using System.Collections.Generic;
using System.Linq;

namespace Swampnet.Evl
{
    public class Expression
    {
        public Expression()
        {
            IsActive = true;
            Children = new Expression[0];
            Operator = ExpressionOperatorType.MATCH_ALL;
        }

        public Expression(ExpressionOperatorType op, ExpressionOperandType operand, string arg, object value)
            : this()
        {
            Operator = op;
            Operand = operand;
            Argument = arg;
            Value = value?.ToString();
        }


        public Expression(ExpressionOperatorType op, ExpressionOperandType operand, object value)
            : this(op, operand, null, value)
        {
        }

        public Expression(ExpressionOperatorType op, object value)
            : this(op, ExpressionOperandType.Null, null, value)
        {
        }


        public Expression(ExpressionOperatorType op)
            : this(op, ExpressionOperandType.Null, null, null)
        {
        }


        public ExpressionOperatorType Operator { get; set; }

        public ExpressionOperandType Operand { get; set; }

        public string Argument { get; set; }     // eg, property name

        public string Value { get; set; }

        public Expression[] Children { get; set; }

        public bool IsActive { get; set; }

        public bool IsContainer => Operator == ExpressionOperatorType.MATCH_ALL || Operator == ExpressionOperatorType.MATCH_ANY;

        public override string ToString()
        {
            return IsContainer
                ? $"{Operator} ({Children.Length} children)"
                : $"{Operand} {Operator} {Argument} '{Value}'";
        }
    }


    public enum ExpressionOperandType
    {
        Null,
        Source,
        Category,
        Summary,
        Property
    }


    public enum ExpressionOperatorType
    {
        //[Display(Name = "@TODO: Friendly name")]
        NULL,

        EQ,
        NOT_EQ,
        REGEX,
        GT,
        GTE,
        LT,
        LTE,

        TAGGED,
        NOT_TAGGED,

        MATCH_ALL,
        MATCH_ANY
    }

}
