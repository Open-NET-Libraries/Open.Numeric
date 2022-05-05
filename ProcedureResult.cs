using Open.Numeric.Precision;
using System;
using System.Globalization;

namespace Open.Numeric;

public struct ProcedureResult : IComparable<ProcedureResult>, IProcedureResult<double>
{
	public int Count { get; }
	public double Sum { get; }
	public double Average { get; }

	public ProcedureResult(double sum, int count)
	{
		Count = count;
		Sum = sum;
		Average = count == 0 ? double.NaN : sum / count;
	}

	public ProcedureResult Add(double value, int count = 1)
		=> new(Sum + value, Count + count);

	[System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0046:Convert to conditional expression")]
	public int CompareTo(ProcedureResult other)
	{
		var a = Average;
		var b = other.Average;
		if (a.IsNearEqual(b, 0.00000001) && a.ToString(CultureInfo.InvariantCulture) == b.ToString(CultureInfo.InvariantCulture)) return 0; // We hate precision issues. :(  1==1 dammit!
		if (a < b || double.IsNaN(a) && !double.IsNaN(b)) return -1;
		if (a > b || !double.IsNaN(a) && double.IsNaN(b)) return +1;
		if (Count < other.Count) return -1;
		if (Count > other.Count) return +1;
		return 0;
	}

	public static ProcedureResult operator +(ProcedureResult a, ProcedureResult b)
		=> new(a.Sum + b.Sum, a.Count + b.Count);

	public static bool operator >(ProcedureResult a, ProcedureResult b) => a.CompareTo(b) == 1;

	public static bool operator <(ProcedureResult a, ProcedureResult b) => a.CompareTo(b) == -1;

	public static bool operator >=(ProcedureResult a, ProcedureResult b) => a.CompareTo(b) >= 0;

	public static bool operator <=(ProcedureResult a, ProcedureResult b) => a.CompareTo(b) <= 0;
}
