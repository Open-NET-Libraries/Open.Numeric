/*!
 * @author electricessence / https://github.com/electricessence/
 * Licensing: MIT https://github.com/electricessence/Open/blob/dotnet-core/LICENSE.md
 */

using Open.Numeric.Precision;
using System;


namespace Open.Numeric
{
	public static class Extensions
	{
		#region Numeric shortcuts.
		public static bool IsNaN(this double value)
		{
			return double.IsNaN(value);
		}

		public static bool IsNaN(this float value)
		{
			return float.IsNaN(value);
		}

		public static bool IsDefault(this double value)
		{
			// ReSharper disable CompareOfFloatsByEqualityOperator
			return value.Equals(default);
			// ReSharper restore CompareOfFloatsByEqualityOperator
		}
		#endregion

	}

	public static class NumericConvert
	{

		/// <summary>
		/// Accurate way to convert possible float to double by converting to string first.  Avoids tolerance issues.
		/// Uses default double convert if not a float.
		/// </summary>
		public static double ToDouble(object value)
		{
			if (value == null)
				return double.NaN;

			return value is float f
				? f.ToDouble()
				: Convert.ToDouble(value);
		}
	}
}
