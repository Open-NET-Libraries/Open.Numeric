using Open.Numeric.Precision;
using System;

namespace Open.Numeric;

public static class Extensions
{
	#region Numeric shortcuts.
	public static bool IsNaN(this double value) => double.IsNaN(value);

	public static bool IsNaN(this float value) => float.IsNaN(value);

	public static bool IsDefault(this double value) => value.Equals(default);
	#endregion

}

public static class NumericConvert
{
	/// <summary>
	/// Accurate way to convert possible float to double by converting to string first.  Avoids tolerance issues.
	/// Uses default double convert if not a float.
	/// </summary>
	public static double ToDouble(object value)
		=> value is null
			? double.NaN
			: value is float f
			? f.ToDouble()
			: Convert.ToDouble(value);
}
