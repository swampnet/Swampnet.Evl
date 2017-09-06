namespace Swampnet.Evl.Web.Entities
{
    public class Rule
	{
		public string Id { get; set; }
		public string Name { get; set; }
		public Expression Expression { get; set; }
		public ActionDefinition[] Actions { get; set; }
	}

}
