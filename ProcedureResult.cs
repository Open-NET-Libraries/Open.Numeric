
using Open.Numeric.Precision;
using System;

namespace Open.Numeric
{
    public struct ProcedureResult : IComparable<ProcedureResult>
    {
        public readonly int Count;
        public readonly double Sum;
        public readonly double Average;
        public ProcedureResult(double sum, int count)
        {
            Count = count;
            Sum = sum;
            Average = count == 0 ? double.NaN : sum / count;
        }


        public ProcedureResult Add(double value, int count = 1)
        {
            return new ProcedureResult(Sum + value, Count + count);
        }

        public int CompareTo(ProcedureResult other)
        {
            var a = Average;
            var b = other.Average;
            if (a.IsNearEqual(b, 0.00000001) && a.ToString() == b.ToString()) return 0; // We hate precision issues. :(  1==1 dammit!
            if (a < b || double.IsNaN(a) && !double.IsNaN(b)) return -1;
            if (a > b || !double.IsNaN(a) && double.IsNaN(b)) return +1;
            if (Count < other.Count) return -1;
            if (Count > other.Count) return +1;
            return 0;
        }

        public static ProcedureResult operator +(ProcedureResult a, ProcedureResult b)
        {
            return new ProcedureResult(
                a.Sum + b.Count,
                a.Count + b.Count
            );
        }

        public static bool operator >(ProcedureResult a, ProcedureResult b)
        {
            return a.CompareTo(b) == 1;
        }

        public static bool operator <(ProcedureResult a, ProcedureResult b)
        {
            return a.CompareTo(b) == -1;
        }

        public static bool operator >=(ProcedureResult a, ProcedureResult b)
        {
            return a.CompareTo(b) >= 0;
        }

        public static bool operator <=(ProcedureResult a, ProcedureResult b)
        {
            return a.CompareTo(b) <= 0;
        }

    }
}