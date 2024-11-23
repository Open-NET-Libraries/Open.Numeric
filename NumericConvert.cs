using Open.Numeric.Precision;
using System;
using System.Globalization;

namespace Open.Numeric;

public static class NumericConvert
{
	/// <summary>
	/// Accurate way to convert possible float to double by converting to string first.  Avoids tolerance issues.
	/// Uses default double convert if not a float.
	/// </summary>
	public static double ToDouble(object value, IFormatProvider? provider = null)
		=> value is null
			? double.NaN
			: value is float f
			? f.ToDouble(provider)
			: Convert.ToDouble(value, provider ?? CultureInfo.InvariantCulture);
}
