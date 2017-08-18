using System;
using System.Collections.Generic;
using System.Text;

namespace Swampnet.Evl
{
    public static class StringExtensions
    {
		public static bool EqualsNoCase(this string lhs, string rhs)
		{
			// Both null -> true
			if(lhs == null && rhs == null)
			{
				return true;
			}

			// One null -> false
			if (lhs == null || rhs == null)
			{
				return false;
			}

			return lhs.Equals(rhs, StringComparison.OrdinalIgnoreCase);
		}
    }
}
