using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Swampnet.Evl
{
    /// <summary>
    /// String extensions / helper methods
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Perform a case insensitive comparison
        /// </summary>
        /// <param name="lhs"></param>
        /// <param name="rhs"></param>
        /// <returns></returns>
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


        /// <summary>
        /// Converts a <see cref="string"/> to a strong type.
        /// </summary>
        /// <typeparam name="T">An <see cref="IConvertible"/> type to return from the <see cref="string"/>.</typeparam>
        /// <param name="value">The <see cref="string"/> value to convert.</param>
        /// <returns>The strong type value of the <see cref="string"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T As<T>(this string value) where T : IConvertible =>
            (T)Convert.ChangeType(value, typeof(T));


        /// <summary>
        /// Converts an <see cref="IConvertible"/> to a <see cref="string"/>.
        /// </summary>
        /// <typeparam name="T">The <see cref="IConvertible"/> to convert.</typeparam>
        /// <param name="value">The <see cref="IConvertible"/> value to convert.</param>
        /// <returns>The <see cref="string"/> representation of the <see cref="IConvertible"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string AsString<T>(this T value) where T : IConvertible =>
            (string)Convert.ChangeType(value, typeof(string));
    }
}
