/*!
 * @author electricessence / https://github.com/electricessence/
 * Licensing: MIT https://github.com/electricessence/Genetic-Algorithm-Platform/blob/master/LICENSE.md
 */

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Open.Numeric
{
	public static class RandomUtilities
	{
		static readonly Lazy<Random> R = new Lazy<Random>(() => new Random());
		public static Random Random => R.Value;

		[SuppressMessage("ReSharper", "PossibleNullReferenceException")]
		public static bool TryRandomPluck<T>(this LinkedList<T> source, out T value)
		{
			if (source.Count == 0)
			{
				value = default;
				return false;
			}

			var r = R.Value.Next(source.Count);
			var node = source.First;
			for (var i = 0; i <= r; i++)
				node = node.Next;
			value = node.Value;
			source.Remove(node);
			return true;
		}

		public static T RandomPluck<T>(this LinkedList<T> source)
		{
			if (source.TryRandomPluck(out var value))
				return value;

			throw new InvalidOperationException("Source collection is empty.");
		}

		public static bool TryRandomPluck<T>(this IList<T> source, out T value)
		{
			if (source.Count == 0)
			{
				value = default;
				return false;
			}

			var r = R.Value.Next(source.Count);
			value = source[r];
			source.RemoveAt(r);
			return true;
		}

		public static T RandomPluck<T>(this IList<T> source)
		{
			if (source.TryRandomPluck(out var value))
				return value;

			throw new InvalidOperationException("Source collection is empty.");
		}

		public static bool TryRandomSelectOne<T>(
			this IReadOnlyList<T> source,
			out T value,
			ISet<T> excluding = null)
		{
			var count = source.Count;
			if (count == 0)
			{
				value = default;
				return false;
			}

			if (excluding != null && excluding.Count != 0)
				return source
					.Where(o => !excluding.Contains(o))
					.ToArray()
					.TryRandomSelectOne(out value);

			value = source[R.Value.Next(count)];
			return true;

		}

		public static T RandomSelectOne<T>(
			this IReadOnlyList<T> source,
			ISet<T> excluding = null)
		{
			if (source.Count == 0)
				throw new InvalidOperationException("Source collection is empty.");

			if (source.TryRandomSelectOne(out T value, excluding))
				return value;

			throw new InvalidOperationException("Exclusion set invalidates the source.  No possible value can be selected.");
		}

		public static bool TryRandomSelectOneExcept<T>(
			this IReadOnlyCollection<T> source,
			T excluding,
			out T value)
		{
			var count = source.Count;
			if (count == 0)
			{
				value = default;
				return false;
			}

			return source
				.Where(o => !excluding.Equals(o))
				.ToArray()
				.TryRandomSelectOne(out value);
		}

		public static T RandomSelectOneExcept<T>(
			this IReadOnlyCollection<T> source,
			T excluding)
		{
			if (source.Count == 0)
				throw new InvalidOperationException("Source collection is empty.");

			if (source.TryRandomSelectOneExcept(excluding, out T value))
				return value;

			throw new InvalidOperationException("Exclusion set invalidates the source.  No possible value can be selected.");
		}

		public static ushort NextRandomIntegerExcluding(
			ushort range,
			ISet<ushort> excludeSet)
		{
			if (range == 0)
				throw new ArgumentOutOfRangeException(nameof(range), range, "Must be a number greater than zero.");

			if (excludeSet == null || excludeSet.Count == 0)
				return (ushort)R.Value.Next(range);

			if (excludeSet.Count == 1)
				return (ushort)NextRandomIntegerExcluding(range, excludeSet.Single());

			return UEnumerable
				.Range(range)
				.Where(i => !excludeSet.Contains(i))
				.ToArray()
				.RandomSelectOne();
		}

		public static int NextRandomIntegerExcluding(
			int range,
			ISet<int> excludeSet)
		{
			if (range <= 0)
				throw new ArgumentOutOfRangeException(nameof(range), range, "Must be a number greater than zero.");

			if (excludeSet == null || excludeSet.Count == 0)
				return R.Value.Next(range);

			if (excludeSet.Count == 1)
				return NextRandomIntegerExcluding(range, excludeSet.Single());

			return Enumerable
				.Range(0, range)
				.Where(i => !excludeSet.Contains(i))
				.ToArray()
				.RandomSelectOne();
		}

		public static ushort NextRandomIntegerExcluding(
			ushort range,
			IEnumerable<ushort> excluding)
		{
			return NextRandomIntegerExcluding(range, new HashSet<ushort>(excluding));
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
			if (range <= 0)
				throw new ArgumentOutOfRangeException(nameof(range), range, "Must be a number greater than zero.");

			if (excluding == 0)
			{
				if (range == 1)
					throw new ArgumentException("No value is available with a range of 1 and exclusion of 0.", "range");
				return R.Value.Next(range - 1) + 1;
			}

			if (excluding >= range || excluding < 0)
				return R.Value.Next(range);

			if (excluding == range - 1)
				return R.Value.Next(range - 1);

			return Enumerable
				.Range(0, range)
				.Where(i => i != excluding)
				.ToArray()
				.RandomSelectOne();
		}

		public static int NextRandomIntegerExcluding(
			int range,
			uint excluding)
		{
			return NextRandomIntegerExcluding(range,
				excluding > int.MaxValue ? -1 : (int)excluding);
		}

	}

}
