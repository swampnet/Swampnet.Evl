using System;
using System.Collections.Generic;
using System.Text;

namespace Swampnet.Evl.Common.Entities
{
    public class Permission
    {
		public string Name { get; set; }
		public bool IsEnabled { get; set; }

		public override string ToString()
		{
			return Name;
		}
	}
}
