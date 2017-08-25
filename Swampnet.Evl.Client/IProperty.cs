using System;
using System.Collections.Generic;
using System.Text;

namespace Swampnet.Evl.Client
{
    public interface IProperty
	{
		string Category { get; set; }

		string Name { get; set; }

		string Value { get; set; }
	}
}
