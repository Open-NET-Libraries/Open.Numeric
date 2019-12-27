using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections.Immutable;

namespace Open.Numeric
{
	public class ProcedureResults : IProcedureResult<ImmutableArray<double>>
	{
		public int Count { get; }
		public ImmutableArray<double> Sum { get; }
		public ImmutableArray<double> Average { get; }

		public static ProcedureResults Empty { get; }
			= new ProcedureResults();

		protected ProcedureResults()
		{
			Count = 0;
			Sum = ImmutableArray<double>.Empty;
			Average = ImmutableArray<double>.Empty;
		}

		public ProcedureResults(ImmutableArray<double> sum, int count)
		{
			Sum = sum;
			Count = count;
			Average = sum.Select(v => count == 0 ? double.NaN : v / count).ToImmutableArray();
		}

		public ProcedureResults(IEnumerable<double> sum, int count)
			:this(sum is ImmutableArray<double> s ? s : sum.ToImmutableArray(), count)
		{
		}


		public ProcedureResults(in ReadOnlySpan<double> sum, int count)
			: this(ToImmutableArray(in sum), count)
		{
		}

		static ImmutableArray<T> ToImmutableArray<T>(in ReadOnlySpan<T> source)
		{
			var len = source.Length;
			var builder = ImmutableArray.CreateBuilder<T>(len);
			for (var i = 0; i < len; i++)
				builder[i] = source[i];
			return builder.MoveToImmutable();
		}

		static ImmutableArray<double> SumValues(IList<double> a, IList<double> b)
		{
			var len = a.Count;
			if (len != b.Count)
				throw new ArgumentException("Length mismatch.");

			var result = ImmutableArray.CreateBuilder<double>(len);
			for (var i = 0; i < len; i++)
				result[i] = a[i] + b[i];

			return result.MoveToImmutable();
		}

		static ImmutableArray<double> SumValues(IList<double> a, in ReadOnlySpan<double> b)
		{
			var len = a.Count;
			if (len != b.Length)
				throw new ArgumentException("Length mismatch.");

			var result = ImmutableArray.CreateBuilder<double>(len);
			for (var i = 0; i < len; i++)
				result[i] = a[i] + b[i];

			return result.MoveToImmutable();
		}

		public ProcedureResults Add(IList<double> values, int count = 1)
			=> new ProcedureResults(SumValues(Sum, values), Count + count);

		public ProcedureResults Add(in ReadOnlySpan<double> values, int count = 1)
			=> new ProcedureResults(SumValues(Sum, in values), Count + count);

		public static ProcedureResults operator +(ProcedureResults a, ProcedureResults b)
			=> new ProcedureResults(SumValues(a.Sum, b.Sum), a.Count + b.Count);

	}
}
