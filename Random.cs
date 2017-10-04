/*!
 * @author electricessence / https://github.com/electricessence/
 * Licensing: MIT https://github.com/electricessence/Genetic-Algorithm-Platform/blob/master/LICENSE.md
 */

using System;
using System.Collections.Generic;
using System.Linq;

namespace Open.Numeric
{
	public static class RandomUtilities
	{
		static Lazy<Random> R = new Lazy<Random>(() => new Random());
		public static Random Random
		{
			get
			{
				return R.Value;
			}
		}
		public static T RandomSelectOne<T>(this IList<T> source)
		{
			return source[R.Value.Next(source.Count)];
		}
		public static T RandomPluck<T>(this IList<T> source)
		{
			var e = source[R.Value.Next(source.Count)];
			source.Remove(e);
			return e;
		}

		public static T RandomSelectOne<T>(this T[] source)
		{
			return source[R.Value.Next(source.Length)];
		}
		public static T RandomSelectOne<T>(this ICollection<T> source)
		{
			return source.Skip(R.Value.Next(source.Count)).First();
		}
		public static T RandomSelectOne<T>(this IEnumerable<T> source)
		{
			return source.Skip(R.Value.Next(source.Count())).First();
		}

		public static int NextRandomIntegerExcluding(
			int range,
			HashSet<int> excludeSet)
		{
			if (range < 0)
				throw new ArgumentOutOfRangeException("range", range, "Must be a number greater than zero.");
			var r = new List<int>();

			for (var i = 0; i < range; ++i)
			{
				if (!excludeSet.Contains(i)) r.Add(i);
			}

			return r.RandomSelectOne();
		}

		public static uint NextRandomIntegerExcluding(
			uint range,
			HashSet<uint> excludeSet)
		{
			var r = new List<uint>();

			for (uint i = 0; i < range; ++i)
			{
				if (!excludeSet.Contains(i)) r.Add(i);
			}

			return r.RandomSelectOne();
		}

		public static uint NextRandomIntegerExcluding(
			int range,
			HashSet<uint> excludeSet)
		{
			if (range < 0)
				throw new ArgumentOutOfRangeException("range", range, "Must be a number greater than zero.");
			var r = new List<uint>();

			for (uint i = 0; i < range; ++i)
			{
				if (!excludeSet.Contains(i)) r.Add(i);
			}

			return r.RandomSelectOne();
		}


		public static int NextRandomIntegerExcluding(
			int range,
			IEnumerable<int> excluding)
		{
			return NextRandomIntegerExcluding(range, new HashSet<int>(excluding));
		}

		public static int NextRandomIntegerExcluding(
			int range,
			int excluding)
		{
			if (range < 0)
				throw new ArgumentOutOfRangeException("range", range, "Must be a number greater than zero.");
			var r = new List<int>();

			for (var i = 0; i < range; ++i)
			{
				if (excluding != i) r.Add(i);
			}

			return r.RandomSelectOne();
		}

		public static uint NextRandomIntegerExcluding(
			int range,
			uint excluding)
		{
			if (range < 0)
				throw new ArgumentOutOfRangeException("range", range, "Must be a number greater than zero.");
			var r = new List<uint>();

			for (uint i = 0; i < range; ++i)
			{
				if (excluding != i) r.Add(i);
			}

			return r.RandomSelectOne();
		}

		public static uint NextRandomIntegerExcluding(
			uint range,
			uint excluding)
		{
			var r = new List<uint>();

			for (uint i = 0; i < range; ++i)
			{
				if (excluding != i) r.Add(i);
			}

			return r.RandomSelectOne();
		}

	}



}
