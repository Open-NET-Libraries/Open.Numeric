/*!
 * @author electricessence / https://github.com/electricessence/
 * Licensing: MIT https://github.com/electricessence/Open/blob/dotnet-core/LICENSE.md
 */

using System;
using System.Globalization;


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
			return value.Equals(default(double));
			// ReSharper restore CompareOfFloatsByEqualityOperator
		}
		#endregion


		#region Precision Assurance.
		/// <summary>
		/// Accurate way to convert float to decimal by converting to string first.  Avoids tolerance issues.
		/// </summary>
		public static decimal ToDecimal(this float value)
		{
			return decimal.Parse(value.ToString(CultureInfo.InvariantCulture));
		}

		/// <summary>
		/// Shortcut for validating a if a floating point value is considered zero (within epsilon tolerance).
		/// </summary>
		public static bool IsZero(this float value)
		{
			return IsPreciseEqual(value, 0f);
		}

		/// <summary>
		/// Shortcut for validating a if a double tolerance floating point value is considered zero (within epsilon tolerance).
		/// </summary>
		public static bool IsZero(this double value)
		{
			return IsPreciseEqual(value, 0d);
		}


		/// <summary>
		/// Shortcut for validating a if a double tolerance floating point value is considered zero (within epsilon tolerance).
		/// </summary>
		public static bool IsNearZero(this double value, double precision = 0.001)
		{
			return IsNearEqual(value, 0d, precision);
		}

		/// <summary>
		/// Shortcut for returning true zero if a double tolerance floating point value is considered zero (within epsilon tolerance).
		/// </summary>
		public static double FixZero(this double value)
		{
			return !value.Equals(0) && value.IsZero() ? 0 : value;
		}

		static double ReturnZeroIfFinite(this float value)
		{
			if (float.IsNegativeInfinity(value))
				return double.NegativeInfinity;
			if (float.IsPositiveInfinity(value))
				return double.PositiveInfinity;

			return float.IsNaN(value) ? double.NaN : 0D;
		}

		/// <summary>
		/// Returns the number of decimal places before last zero digit.
		/// </summary>
		public static int DecimalPlaces(this double source)
		{
			if (source.IsNaN())
				return 0;

			string valueString = source.ToString();
			var index = valueString.IndexOf('.');
			return index == -1 ? 0 : valueString.Length - index - 1;
		}




		/// <summary>
		/// Shortcut for validating a if a floating point value is close enough to another addValue using the given tolerance tolerance.
		/// </summary>
		public static bool IsNearEqual(this float a, float b, float tolerance)
		{
			return a.Equals(b) || float.IsNaN(a) && float.IsNaN(b) || Math.Abs(a - b) < tolerance;
		}

		/// <summary>
		/// Shortcut for validating a if a double precision floating point value is close enough to another addValue using the given tolerance tolerance.
		/// </summary>
		public static bool IsNearEqual(this double a, double b, double tolerance)
		{
			return a.Equals(b) || double.IsNaN(a) && double.IsNaN(b) || Math.Abs(a - b) < tolerance;
		}

		/// <summary>
		/// Shortcut for validating a if a decimal addValue is close enough to another addValue using the given tolerance tolerance.
		/// </summary>
		public static bool IsNearEqual(this decimal a, decimal b, decimal tolerance)
		{
			return a.Equals(b) || Math.Abs(a - b) < tolerance;
		}

		/// <summary>
		/// Shortcut for validating a if a decimal addValue is close enough to another addValue using the given tolerance tolerance.
		/// </summary>
		public static bool IsRelativeNearEqual(this double a, double b, uint minDecimalPlaces)
		{
			var tolerance = 1/Math.Pow(10,minDecimalPlaces);
			if(a.IsNearEqual(b,tolerance)) return true;
			if(double.IsNaN(a) || double.IsNaN(b)) return false;
			var d = Math.Min(a.DecimalPlaces(),b.DecimalPlaces());
			var divisor = Math.Pow(10,minDecimalPlaces-d);
			a /= divisor;
			b /= divisor;
			return a.IsNearEqual(b,tolerance);
		}

		/// <summary>
		/// Validates if values are equal within epsilon tolerance.
		/// </summary>
		public static bool IsPreciseEqual(this double a, double b, bool stringValidate = false)
		{
			return IsNearEqual(a, b, double.Epsilon) || (stringValidate && !double.IsNaN(a) && !double.IsNaN(b) && a.ToString() == b.ToString());
		}


		/// <summary>
		/// Validates if values are equal within epsilon tolerance.
		/// </summary>
		public static bool IsPreciseEqual(this float a, float b, bool stringValidate = false)
		{
			return IsNearEqual(a, b, float.Epsilon) || (stringValidate && !float.IsNaN(a) && !float.IsNaN(b) && a.ToString() == b.ToString());
		}

		/// <summary>
		/// Validates if values are equal within epsilon tolerance.
		/// </summary>
		public static bool IsPreciseEqual(this double? a, double? b, bool stringValidate = false)
		{
			return !a.HasValue && !b.HasValue || a.HasValue && b.HasValue && a.Value.IsPreciseEqual(b.Value, stringValidate);
		}

		/// <summary>
		/// Validates if values are equal within epsilon tolerance.
		/// </summary>
		public static bool IsPreciseEqual(this float? a, float? b, bool stringValidate = false)
		{
			return !a.HasValue && !b.HasValue || a.HasValue && b.HasValue && a.Value.IsPreciseEqual(b.Value, stringValidate);
		}



		/// <summary>
		/// Shortcut for validating a if a potential floating pointvalue is close enough to another addValue using the given tolerance tolerance.
		/// </summary>
		public static bool IsNearEqual(this IComparable a, IComparable b, double tolerance)
		{
			if(a==null)
				throw new NullReferenceException();
			if(b==null)
				throw new ArgumentNullException("b");

			if (a is float)
				return ((float)a).IsNearEqual((float)b, tolerance);
			if (a is double)
				return ((double)a).IsNearEqual((double)b, tolerance);
			if (a is decimal)
				return ((decimal)a).IsNearEqual((decimal)b, tolerance);

			throw new InvalidCastException();
		}


		/// <summary>
		/// Accurate way to convert float to double by rounding finite values to a decimal point tolerance level.
		/// </summary>
		public static double ToDouble(this float value, int precision)
		{
			if (precision < 0 || precision > 15)
				throw new ArgumentOutOfRangeException("precision", precision, "Must be bewteen 0 and 15.");

			double result = value.ReturnZeroIfFinite();
			// ReSharper disable RedundantCast
			return result.IsZero() ? Math.Round((double)value, precision) : result;
			// ReSharper restore RedundantCast
		}

		/// <summary>
		/// Accurate way to convert float to double by converting to string first.  Avoids tolerance issues.
		/// </summary>
		public static double ToDouble(this float value)
		{
			double result = value.ReturnZeroIfFinite();
			return result.IsZero() ? double.Parse(value.ToString(CultureInfo.InvariantCulture)) : result;
		}

		/// <summary>
		/// Accurate way to convert possible float to double by converting to string first.  Avoids tolerance issues.
		/// </summary>
		public static double ToDouble(this float? value)
		{
			return value.HasValue ? value.Value.ToDouble() : double.NaN;
		}


		/// <summary>
		/// Accurate way to convert a possible float to double by rounding finite values to a decimal point tolerance level.
		/// </summary>
		/// <param name="tolerance"></param>	
		public static double ToDouble(this float? value, int precision)
		{
			if (precision < 0 || precision > 15)
				throw new ArgumentOutOfRangeException("precision", precision, "Must be bewteen 0 and 15.");
			return value.HasValue ? value.Value.ToDouble(precision) : double.NaN;
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

            return value is float ? ((float)value).ToDouble() : Convert.ToDouble(value);
        }
    }
}
