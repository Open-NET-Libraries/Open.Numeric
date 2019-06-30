using System;
using System.Linq;

namespace Open.Numeric
{
	public class ProcedureResults : IProcedureResult<ReadOnlyMemory<double>>
	{
		public int Count { get; }
		public ReadOnlyMemory<double> Sum { get; }
		public ReadOnlyMemory<double> Average { get; }
		public ProcedureResults(in ReadOnlySpan<double> sum, int count)
		{
			Count = count;
			var s = sum.ToArray();
			Sum = s;
			Average = s.Select(v => count == 0 ? double.NaN : v / count).ToArray();
		}

		static double[] SumValues(in ReadOnlySpan<double> a, in ReadOnlySpan<double> b)
		{
			if (a.Length != b.Length)
				throw new ArgumentException("Length mismatch.");

			var len = a.Length;
			var result = new double[len];
			for (var i = 0; i < len; i++)
				result[i] = a[i] + b[i];

			return result;
		}

		public ProcedureResults Add(in ReadOnlySpan<double> values, int count = 1)
			=> new ProcedureResults(SumValues(Sum.Span, in values), Count + count);

		public static ProcedureResults operator +(ProcedureResults a, ProcedureResults b)
			=> new ProcedureResults(SumValues(a.Sum.Span, b.Sum.Span), a.Count + b.Count);

	}
}
