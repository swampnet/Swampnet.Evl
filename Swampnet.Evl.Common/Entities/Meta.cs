using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Swampnet.Evl.Common.Entities
{
    public class MetaData
	{
		public ActionMetaData[] ActionMetaData { get; set; }
		public ExpressionOperator[] Operators { get; set; }
		public MetaDataCapture[] Operands { get; set; }
    }


    public class Option
	{
		public Option()
		{
		}


        public Option(string display)
            : this()
        {
            Display = display;
            Value = display;
        }

		public Option(string display, string value)
			: this(display)
		{
            Value = value;
		}

		public string Display { get; set; }
		public string Value { get; set; }
	}


	public class ActionMetaData
	{
		public string Type { get; set; }
		public MetaDataCapture[] Properties { get; set; }

    }



	public class ExpressionOperator
	{
		public ExpressionOperator()
		{
		}

		public ExpressionOperator(RuleOperatorType op, string display)
			: this()
		{
			Code = op;
			Display = display;

            switch (Code)
            {
                case RuleOperatorType.MATCH_ALL:
                case RuleOperatorType.MATCH_ANY:
                    IsGroup = true;
                    RequiresOperand = false;
                    RequiresValue = false;
                    break;

                case RuleOperatorType.TAGGED:
                case RuleOperatorType.NOT_TAGGED:
                    IsGroup = false;
                    RequiresOperand = false;
                    RequiresValue = true;
                    break;

                default:
                    IsGroup = false;
                    RequiresOperand = true;
                    RequiresValue = true;
                    break;
            }
		}


		[JsonConverter(typeof(StringEnumConverter))]
		public RuleOperatorType Code { get; set; }
		public string Display { get; set; }
		public bool IsGroup { get; set; }
        public bool RequiresOperand { get; set; }
        public bool RequiresValue { get; set; }
    }

	public class MetaDataCapture
	{
		public string Name { get; set; }
		public bool IsRequired { get; set; }
		public string DataType { get; set; }
		public Option[] Options { get; set; }
	}
}
