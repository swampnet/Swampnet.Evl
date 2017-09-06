namespace Swampnet.Evl.Web.Entities
{
    public class Expression
	{
		public Expression()
		{
		}

		public Expression(string op)
			: this(op, null, null, null)
		{
		}

		public Expression(string op, string operand, string arg, string value)
			: this()
		{
			Operator = op;
			Operand = operand;
			Argument = arg;
			Value = value;
			IsActive = true;
		}

		public string Operator { get; set; }
		public string Operand { get; set; }
		public string Argument { get; set; }
		public string Value { get; set; }
		public bool IsActive { get; set; }

		public Expression[] Children { get; set; }
	}

}
