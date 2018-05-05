using System.Collections.Generic;

namespace Open.Numeric
{
	public static class UEnumerable
	{
		public static IEnumerable<ushort> Range(ushort start, ushort count)
		{
			while (count > 0)
			{
				yield return start;
				start++;
				count--;
			}
		}

		public static IEnumerable<ushort> Range(ushort count)
		{
			return Range(0, count);
		}


		public static IEnumerable<uint> Range(uint start, uint count)
		{
			while (count > 0)
			{
				yield return start;
				start++;
				count--;
			}
		}

		public static IEnumerable<uint> Range(uint count)
		{
			return Range(0, count);
		}
	}
}
