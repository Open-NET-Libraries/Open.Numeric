using System;
using System.Collections.Generic;
using System.Linq;

namespace Open.Numeric.Primes
{
    public static class Number
    {
        static bool IsPrimeInternal(ulong value)
        {
            if (value < 380000)
            {
                // This method is faster up until a point.
                double squared = Math.Sqrt(value);
                ulong flooredAndSquared = Convert.ToUInt64(Math.Floor(squared));

                for (ulong idx = 3; idx <= flooredAndSquared; idx++)
                {
                    if (value % idx == 0)
                    {
                        return false;
                    }
                }
            }
            else
            {
                ulong divisor = 6;
                while (divisor * divisor - 2 * divisor + 1 <= value)
                {

                    if (value % (divisor - 1) == 0)
                        return false;

                    if (value % (divisor + 1) == 0)
                        return false;

                    divisor += 6;
                }
            }



            return true;
        }

        /// <summary>
        /// Validates if a number is prime.
        /// </summary>
        /// <param name="value">Value to verify.</param>
        /// <returns>True if the provided value is a prime number</returns>
        public static bool IsPrime(ulong value)
        {
            switch (value)
            {
                // 0 and 1 are not prime numbers
                case 0:
                case 1:
                    return false;

                case 2:
                case 3:
                    return true;

                default:
                    return value % 2 != 0
                        && value % 3 != 0
                        && IsPrimeInternal(value);
            }

        }

        /// <summary>
        /// Validates if a number is prime.
        /// </summary>
        /// <param name="value">Value to verify.</param>
        /// <returns>True if the provided value is a prime number</returns>
        public static bool IsPrime(long value)
        {
            return IsPrime((ulong)Math.Abs(value));
        }

        public static bool IsPrime(double value)
        {
            return (Math.Floor(value) == value)
                ? IsPrime((long)value)
                : false;
        }

        public static IEnumerable<ulong> MultiplesOf(ulong value)
        {
            ulong last = 1;

            foreach (var p in Prime.Numbers())
            {
                ulong stop = value / last; // The list of possibilities shrinks for each test.
                if (p > stop) break; // Exceeded possibilities? 
                while ((value % p) == 0)
                {
                    value /= p;
                    yield return p;
                    if (value == 1) yield break;
                }
                last = p;
            }

            yield return value;
        }
    }

    public static class Prime
    {

        internal static IEnumerable<ulong> ValidPrimeTests(ulong staringAt = 2)
        {
            if (staringAt > 2)
            {
                if (staringAt % 2 == 0)
                    staringAt++;
            }
            {
                yield return 2;
                staringAt = 3;
            }

            for (ulong n = staringAt; n < ulong.MaxValue - 1; n += 2)
                yield return n;
        }

        internal static IEnumerable<long> ValidPrimeTests(long staringAt = 2)
        {
            var sign = staringAt < 0 ? -1 : 1;
            staringAt = Math.Abs(staringAt);

            if (staringAt > 2)
            {
                if (staringAt % 2 == 0)
                    staringAt++;
            }
            {
                yield return sign * 2;
                staringAt = 3;
            }

            for (long n = staringAt; n < long.MaxValue - 1; n += 2)
                yield return sign * n;
        }

        /// <summary>
        /// Returns an enumerable that will iterate every prime starting at the starting value.
        /// </summary>
        /// <param name="staringAt">Allows for skipping ahead any integer before checking for inclusive and subsequent primes.</param>
        /// <returns></returns>
        public static IEnumerable<ulong> Numbers(ulong staringAt = 2)
        {
            return ValidPrimeTests(staringAt)
                .Where(v => Number.IsPrime(v));
        }

        /// <summary>
        /// Returns an enumerable that will iterate every prime starting at the starting value.
        /// </summary>
        /// <param name="staringAt">Allows for skipping ahead any integer before checking for inclusive and subsequent primes.  Passing a negative number here will produce a negative set of prime numbers.</param>
        /// <returns></returns>
        public static IEnumerable<long> Numbers(long staringAt)
        {
            return ValidPrimeTests(staringAt)
                .Where(v => Number.IsPrime(v));
        }

        /// <summary>
        /// Returns a parallel enumerable that will iterate every prime starting at the starting value.
        /// </summary>
        /// <param name="staringAt">Allows for skipping ahead any integer before checking for inclusive and subsequent primes.</param>
        /// <param name="degreeOfParallelism">Operates in parallel unless 1 is specified.</param>
        /// <returns></returns>
        public static ParallelQuery<ulong> NumbersInParallel(ulong staringAt = 2, ushort? degreeOfParallelism = null)
        {
            var tests = ValidPrimeTests(staringAt)
                .AsParallel().AsOrdered();

            if (degreeOfParallelism.HasValue)
                tests = tests.WithDegreeOfParallelism(degreeOfParallelism.Value);

            return tests.Where(v => Number.IsPrime(v));
        }

        /// <summary>
        /// Finds the next prime number after the number given.
        /// </summary>
        /// <param name="after">The excluded lower boundary to start with.</param>
        /// <returns>The next prime after the number provided.</returns>
        public static ulong Next(ulong after)
        {
            return Numbers(after + 1).First();
        }


        /// <summary>
        /// Finds the next prime number after the number given.
        /// </summary>
        /// <param name="after">The excluded lower boundary to start with.  If this number is negative, then the result will be the next greater magnitude value prime as negative number.</param>
        /// <returns>The next prime after the number provided.</returns>
        public static long Next(long after)
        {
            var sign = after < 0 ? -1 : 1;
            after = Math.Abs(after);
            return sign * Numbers(after + 1).First();
        }

        /// <summary>
        /// Finds the next prime number after the number given.
        /// </summary>
        /// <param name="after">The excluded lower boundary to start with.  If this number is negative, then the result will be the next greater magnitude value prime as negative number.</param>
        /// <returns>The next prime after the number provided.</returns>
        public static long Next(double value)
        {
            return Next((long)value);
        }

    }


    namespace Extensions
    {
        public static class PrimeExtensions
        {
            public static bool IsPrime(this ulong value)
            {
                return Number.IsPrime(value);
            }
            public static bool IsPrime(this long value)
            {
                return Number.IsPrime(value);
            }
            public static bool IsPrime(this double value)
            {
                return Number.IsPrime(value);
            }
            public static ulong NextPrime(this ulong value)
            {
                return Prime.Next(value);
            }
            public static long NextPrime(this long value)
            {
                return Prime.Next(value);
            }
            public static long NextPrime(this double value)
            {
                return Prime.Next(value);
            }
        }
    }
}

