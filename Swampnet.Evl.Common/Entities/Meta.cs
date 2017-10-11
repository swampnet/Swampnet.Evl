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

		public ExpressionOperator(RuleOperatorType op, string display, bool isGroup)
			: this()
		{
			Code = op;
			Display = display;
			IsGroup = isGroup;
		}


		[JsonConverter(typeof(StringEnumConverter))]
		public RuleOperatorType Code { get; set; }
		public string Display { get; set; }
		public bool IsGroup { get; set; }
	}

	public class MetaDataCapture
	{
		public string Name { get; set; }
		public bool IsRequired { get; set; }
		public string DataType { get; set; }
		public Option[] Options { get; set; }
	}
}
