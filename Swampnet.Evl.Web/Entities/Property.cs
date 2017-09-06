namespace Swampnet.Evl.Web.Entities
{
    public class Property
	{
		public Property()
		{
		}

		public Property(string name, string value)
			: this()
		{
			Name = name;
			Value = value;
		}

		public string Category { get; set; }
		public string Name { get; set; }
		public string Value { get; set; }
	}

}
