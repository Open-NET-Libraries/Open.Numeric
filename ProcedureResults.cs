using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Open.Numeric;

public class ProcedureResults : IProcedureResult<ImmutableArray<double>>
{
	public int Count { get; }
	public ImmutableArray<double> Sum { get; }
	public ImmutableArray<double> Average { get; }

	public static ProcedureResults Empty { get; } = new();

	protected ProcedureResults()
	{
		Count = 0;
		Sum = ImmutableArray<double>.Empty;
		Average = ImmutableArray<double>.Empty;
	}

	[System.Diagnostics.CodeAnalysis.SuppressMessage("Roslynator",
		"RCS1242:Do not pass non-read-only struct by read-only reference.",
		Justification = "Option should be allowed since ImmutableArrays are not yet readonly structs.")]
	public ProcedureResults(in ImmutableArray<double> sum, int count)
	{
		Sum = sum;
		Count = count;
		Average = sum.Select(v => count == 0 ? double.NaN : v / count).ToImmutableArray();
	}

	public ProcedureResults(IEnumerable<double> sum, int count)
		: this(sum is ImmutableArray<double> s ? s : sum.ToImmutableArray(), count)
	{
	}

	public ProcedureResults(ReadOnlySpan<double> sum, int count)
		: this(ToImmutableArray(sum), count)
	{
	}

	static ImmutableArray<T> ToImmutableArray<T>(ReadOnlySpan<T> source)
	{
		var len = source.Length;
		var builder = ImmutableArray.CreateBuilder<T>(len);
		for (var i = 0; i < len; i++)
			builder.Add(source[i]);
		return builder.MoveToImmutable();
	}

	static ImmutableArray<double> SumValues(IReadOnlyList<double> a, IReadOnlyList<double> b)
	{
		var len = a.Count;
		if (len != b.Count)
			throw new ArgumentException("Length mismatch.");

		var builder = ImmutableArray.CreateBuilder<double>(len);
		for (var i = 0; i < len; i++)
			builder.Add(a[i] + b[i]);

		return builder.MoveToImmutable();
	}

	static ImmutableArray<double> SumValues(IReadOnlyList<double> a, ReadOnlySpan<double> b)
	{
		var len = a.Count;
		if (len != b.Length)
			throw new ArgumentException("Length mismatch.");

		var builder = ImmutableArray.CreateBuilder<double>(len);
		for (var i = 0; i < len; i++)
			builder.Add(a[i] + b[i]);

		return builder.MoveToImmutable();
	}

	public ProcedureResults Add(IReadOnlyList<double> values, int count = 1)
		=> new(SumValues(Sum, values), Count + count);

	public ProcedureResults Add(ReadOnlySpan<double> values, int count = 1)
		=> new(SumValues(Sum, values), Count + count);

	public static ProcedureResults operator +(ProcedureResults a, ProcedureResults b)
		=> new(SumValues(a.Sum, b.Sum), a.Count + b.Count);
}
