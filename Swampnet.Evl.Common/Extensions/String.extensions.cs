using System;
using System.Collections.Generic;
using System.Text;

namespace Swampnet.Evl
{
    public static class StringExtensions
    {
		public static bool EqualsNoCase(this string lhs, string rhs)
		{
			return lhs.Equals(rhs, StringComparison.OrdinalIgnoreCase);
		}
    }
}
