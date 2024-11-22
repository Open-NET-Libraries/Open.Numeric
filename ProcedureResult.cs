using System;
using System.Globalization;
#if NET7_0_OR_GREATER
using System.Numerics;
#endif

namespace Open.Numeric;

public readonly record struct ProcedureResult : IComparable<ProcedureResult>, IProcedureResult<double>
{
	public int Count { get; }
	public double Sum { get; }
	public double Average { get; }

	public ProcedureResult(double sum, int count)
	{
		Count = count;
		Sum = sum;
		Average = count == 0 ? 0 : sum / count;
	}

	public ProcedureResult Add(double value, int count = 1)
		=> new(Sum + value, Count + count);

	[System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0046:Convert to conditional expression")]
	public int CompareTo(ProcedureResult other)
	{
		var a = Average;
		var b = other.Average;
		if (a.IsNearEqual(b, 0.00000001) && a.ToString(CultureInfo.InvariantCulture) == b.ToString(CultureInfo.InvariantCulture))
			return 0; // We hate precision issues. :(  1==1 dammit!

		if (a < b || double.IsNaN(a) && !double.IsNaN(b)) return -1;
		if (a > b || !double.IsNaN(a) && double.IsNaN(b)) return +1;
		if (Count < other.Count) return -1;
		if (Count > other.Count) return +1;
		return 0;
	}

	public static ProcedureResult operator +(ProcedureResult a, ProcedureResult b)
		=> new(
			a.Sum + b.Sum,
			a.Count + b.Count
		);

	public static bool operator >(ProcedureResult a, ProcedureResult b) => a.CompareTo(b) == 1;

	public static bool operator <(ProcedureResult a, ProcedureResult b) => a.CompareTo(b) == -1;

	public static bool operator >=(ProcedureResult a, ProcedureResult b) => a.CompareTo(b) >= 0;

	public static bool operator <=(ProcedureResult a, ProcedureResult b) => a.CompareTo(b) <= 0;
}

#if NET7_0_OR_GREATER

public readonly record struct ProcedureResult<T>
	: IComparable<ProcedureResult<T>>, IProcedureResult<T>
	where T : notnull, INumber<T>
{
	public int Count { get; }
	public T Sum { get; }
	public T Average { get; }

	public ProcedureResult(T sum, int count)
	{
		Count = count;
		Sum = sum;
		Average = count == 0 ? 0 : sum / (dynamic)count;
	}

	public ProcedureResult<T> Add(T value, int count = 1)
		=> new(Sum + value, Count + count);

	[System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0046:Convert to conditional expression")]
	public int CompareTo(ProcedureResult<T> other)
	{
		var a = Average;
		var b = other.Average;
		if (a < b || T.IsNaN(a) && !T.IsNaN(b)) return -1;
		if (a > b || !T.IsNaN(a) && T.IsNaN(b)) return +1;
		if (Count < other.Count) return -1;
		if (Count > other.Count) return +1;
		return 0;
	}

	public static ProcedureResult<T> operator +(ProcedureResult<T> a, ProcedureResult<T> b)
		=> new(
			a.Sum + b.Sum,
			a.Count + b.Count
		);

	public static bool operator >(ProcedureResult<T> a, ProcedureResult<T> b)
		=> a.CompareTo(b) == 1;

	public static bool operator <(ProcedureResult<T> a, ProcedureResult<T> b)
		=> a.CompareTo(b) == -1;

	public static bool operator >=(ProcedureResult<T> a, ProcedureResult<T> b)
		=> a.CompareTo(b) >= 0;

	public static bool operator <=(ProcedureResult<T> a, ProcedureResult<T> b)
		=> a.CompareTo(b) <= 0;
}

#endif