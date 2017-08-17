using Swampnet.Evl.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Swampnet.Evl
{
	public static class PropertyExtensions
	{
		public static void Add(this ICollection<IProperty> properties, IProperty property)
		{
			properties?.Add(property);
		}
	}
}
