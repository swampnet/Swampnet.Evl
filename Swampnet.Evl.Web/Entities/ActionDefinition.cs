namespace Swampnet.Evl.Web.Entities
{
	public class ActionDefinition
	{
		public ActionDefinition()
		{
			IsActive = true;
		}


		public ActionDefinition(string type)
			: this()
		{
			Type = type;
		}

		public string Type { get; set; }
		public bool IsActive { get; set; }
		public Property[] Properties { get; set; }
	}

}
