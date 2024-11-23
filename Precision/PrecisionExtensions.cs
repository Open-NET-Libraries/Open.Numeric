using System;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Diagnostics.CodeAnalysis;

namespace Open.Numeric.Precision;

public static class PrecisionExtensions
{
	/// <summary>
	/// Accurate way to convert float to decimal by converting to string first.  Avoids tolerance issues.
	/// </summary>
	public static decimal ToDecimal(this float value, IFormatProvider? formatProvider = null)
	{
		formatProvider ??= CultureInfo.InvariantCulture;
		return decimal.Parse(value.ToString(formatProvider), formatProvider);
	}

	/// <summary>
	/// Shortcut for validating a if a floating point value is considered zero (within epsilon tolerance).
	/// </summary>
	public static bool IsZero(this float value) => value.IsPreciseEqual(0f);

	/// <summary>
	/// Shortcut for validating a if a double tolerance floating point value is considered zero (within epsilon tolerance).
	/// </summary>
	public static bool IsZero(this double value) => value.IsPreciseEqual(0d);

	/// <summary>
	/// Shortcut for validating a if a double tolerance floating point value is considered zero (within provided tolerance).
	/// </summary>
	public static bool IsNearZero(this double value, double precision = 0.001)
		=> value.IsNearEqual(0d, precision);

	/// <summary>
	/// Shortcut for returning true zero if a double tolerance floating point value is considered zero (within epsilon tolerance).
	/// </summary>
	public static double FixZero(this double value)
		=> !value.Equals(0) && value.IsZero() ? 0 : value;

	private static double ReturnZeroIfFinite(this float value)
		=> float.IsNegativeInfinity(value)
		? double.NegativeInfinity
		: float.IsPositiveInfinity(value)
		? double.PositiveInfinity
		: float.IsNaN(value) ? double.NaN : 0D;

	/// <summary>
	/// Returns the number of decimal places before last zero digit.
	/// </summary>
#pragma warning disable IDE0079 // Remove unnecessary suppression
	[SuppressMessage("Globalization", "CA1307:Specify StringComparison for clarity")]
#pragma warning restore IDE0079 // Remove unnecessary suppression
	public static int DecimalPlaces(this double source)
	{
		if (source.IsNaN())
			return 0;

		var valueString = source.ToString(CultureInfo.InvariantCulture); // To
		int index = valueString.IndexOf('.');
		return index == -1 ? 0 : valueString.Length - index - 1;
	}

	/// <summary>
	/// Shortcut for validating a if a floating point value is close enough to another addValue using the given tolerance.
	/// </summary>
	public static bool IsNearEqual(this float a, float b, float tolerance)
		=> a.Equals(b)
		|| float.IsNaN(a) && float.IsNaN(b)
		|| Math.Abs(a - b) < tolerance;

	/// <summary>
	/// Shortcut for validating a if a double precision floating point value is close enough to another addValue using the given tolerance.
	/// </summary>
	public static bool IsNearEqual(this double a, double b, double tolerance)
		=> a.Equals(b)
		|| double.IsNaN(a) && double.IsNaN(b)
		|| Math.Abs(a - b) < tolerance;

	/// <summary>
	/// Shortcut for validating a if a decimal addValue is close enough to another addValue using the given tolerance.
	/// </summary>
	public static bool IsNearEqual(this decimal a, decimal b, decimal tolerance)
		=> a.Equals(b)
		|| Math.Abs(a - b) < tolerance;

	/// <summary>
	/// Shortcut for validating a if a decimal addValue is close enough to another addValue using the given tolerance.
	/// </summary>
	public static bool IsRelativeNearEqual(this double a, double b, uint minDecimalPlaces)
	{
		var tolerance = 1 / Math.Pow(10, minDecimalPlaces);
		if (a.IsNearEqual(b, tolerance)) return true;
		if (double.IsNaN(a) || double.IsNaN(b)) return false;
		var d = Math.Min(a.DecimalPlaces(), b.DecimalPlaces());
		var divisor = Math.Pow(10, minDecimalPlaces - d);
		a /= divisor;
		b /= divisor;
		return a.IsNearEqual(b, tolerance);
	}

	/// <summary>
	/// Validates if values are equal within epsilon tolerance.
	/// </summary>
	public static bool IsPreciseEqual(this double a, double b, bool stringValidate = false)
		=> a.IsNearEqual(b, double.Epsilon)
		|| stringValidate && !double.IsNaN(a) && !double.IsNaN(b)
			&& a.ToString(CultureInfo.InvariantCulture) == b.ToString(CultureInfo.InvariantCulture);

	/// <summary>
	/// Validates if values are equal within epsilon tolerance.
	/// </summary>
	public static bool IsPreciseEqual(this float a, float b, bool stringValidate = false)
		=> a.IsNearEqual(b, float.Epsilon)
		|| stringValidate && !float.IsNaN(a) && !float.IsNaN(b)
			&& a.ToString(CultureInfo.InvariantCulture) == b.ToString(CultureInfo.InvariantCulture);

	/// <summary>
	/// Validates if values are equal within epsilon tolerance.
	/// </summary>
	public static bool IsPreciseEqual(this double? a, double? b, bool stringValidate = false)
		=> !a.HasValue && !b.HasValue
		|| a.HasValue && b.HasValue && a.Value.IsPreciseEqual(b.Value, stringValidate);

	/// <summary>
	/// Validates if values are equal within epsilon tolerance.
	/// </summary>
	public static bool IsPreciseEqual(this float? a, float? b, bool stringValidate = false)
		=> !a.HasValue && !b.HasValue
		|| a.HasValue && b.HasValue && a.Value.IsPreciseEqual(b.Value, stringValidate);

	/// <summary>
	/// Shortcut for validating a if a potential floating point value is close enough to another addValue using the given tolerance.
	/// </summary>
	public static bool IsNearEqual(this IComparable a, IComparable b, IComparable tolerance)
	{
		if (a is null)
			throw new ArgumentNullException(nameof(a));
		if (b is null)
			throw new ArgumentNullException(nameof(b));
		Contract.EndContractBlock();

		return a.Equals(b) || a switch
		{
			float f => f.IsNearEqual((float)b, (float)tolerance),
			double d => d.IsNearEqual((double)b, (double)tolerance),
			decimal @decimal => @decimal.IsNearEqual((decimal)b, (decimal)tolerance),
			_ => throw new InvalidCastException(),
		};
	}

	/// <summary>
	/// Accurate way to convert float to double by rounding finite values to a decimal point tolerance level.
	/// </summary>
	public static double ToDouble(this float value, int precision)
	{
		if (precision is < 0 or > 15)
			throw new ArgumentOutOfRangeException(nameof(precision), precision, "Must be between 0 and 15.");
		Contract.EndContractBlock();

		var result = value.ReturnZeroIfFinite();
		// ReSharper disable RedundantCast
		return result.IsZero() ? Math.Round(value, precision) : result;
		// ReSharper restore RedundantCast
	}

	/// <summary>
	/// Accurate way to convert float to double by converting to string first.  Avoids tolerance issues.
	/// </summary>
	public static double ToDouble(this float value, IFormatProvider? formatProvider = null)
	{
		formatProvider ??= CultureInfo.InvariantCulture;
		var result = value.ReturnZeroIfFinite();
		return result.IsZero() ? double.Parse(value.ToString(formatProvider), formatProvider) : result;
	}

	/// <summary>
	/// Accurate way to convert possible float to double by converting to string first.  Avoids tolerance issues.
	/// </summary>
	public static double ToDouble(this float? value, IFormatProvider? formatProvider = null)
		=> value?.ToDouble(formatProvider) ?? double.NaN;

	/// <summary>
	/// Accurate way to convert a possible float to double by rounding finite values to a decimal point tolerance level.
	/// </summary>
	public static double ToDouble(this float? value, int precision)
	{
		if (precision is < 0 or > 15)
			throw new ArgumentOutOfRangeException(nameof(precision), precision, "Must be between 0 and 15.");
		Contract.EndContractBlock();

		return value?.ToDouble(precision) ?? double.NaN;
	}

	/// <summary>
	/// Ensures addition tolerance by trimming off unexpected imprecision.
	/// </summary>
	public static double SumAccurate(this double source, double value)
	{
		var result = source + value;
		var vp = source.DecimalPlaces();
		if (vp > 15)
			return result;
		var ap = value.DecimalPlaces();
		if (ap > 15)
			return result;

		var digits = Math.Max(vp, ap);

		return Math.Round(result, digits);
	}

	/// <summary>
	/// Ensures addition tolerance by trimming off unexpected imprecision.
	/// </summary>
	public static double ProductAccurate(this double source, double value)
	{
		var result = source * value;
		var vp = source.DecimalPlaces();
		if (vp > 15)
			return result;
		var ap = value.DecimalPlaces();
		if (ap > 15)
			return result;

		var digits = Math.Max(vp, ap);

		return Math.Round(result, digits);
	}

	/// <summary>
	/// Ensures addition tolerance by using integer math.
	/// </summary>
	public static double SumUsingIntegers(this double source, double value)
	{
		var x = Math.Pow(10, Math.Max(source.DecimalPlaces(), value.DecimalPlaces()));

		var v = (long)(source * x);
		var a = (long)(value * x);
		var result = v + a;
		return result / x;
	}
}
