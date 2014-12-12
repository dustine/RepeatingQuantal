using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace RepeatingQuantal
{
    public static class AdditionalMath
    {
        private static IDictionary<Tuple<int, int>, int> _prevMO = new Dictionary<Tuple<int, int>, int>();

        /// <summary>
        /// Greatest common divider, using the Euclidian Algorithm
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static int Gcd(int a, int b)
        {
            while (true)
            {
                if (b == 0) return a;
                var a1 = a;
                a = b;
                b = a1 % b;
            }
        }

        public static Tuple<int, int> GetRepeatingDecimalLength(int nBase, int number, bool hasMemory = false)
        {
            return GetRepeatingDecimalLength(nBase, number, PrimeFactorsOf(number), hasMemory);
        }

        public static Tuple<int, int> GetRepeatingDecimalLength(int nBase, int number,
                    IEnumerable<int> factors, bool hasMemory = false)
        {
            var baseFactors = PrimeFactorsOf(nBase).Distinct();
            var eFactors = factors as IList<int> ?? factors.ToList();
            var factorList = eFactors.Distinct().ToDictionary(k => k, v => eFactors.Count(w => w == v));

            // transient: the non-repeating tidbit in the start!
            var notCoprimes = factorList.Where(kv => baseFactors.Contains(kv.Key)).ToList();
            var transient = 0;
            if (notCoprimes.Any())
            {
                transient = notCoprimes.Max(kv => kv.Value);
            }

            // period: the length of the repeating decimal
            var coprimes = factorList.Where(kv => !baseFactors.Contains(kv.Key)).ToList();
            var period = 0;
            if (!coprimes.Any()) return Tuple.Create(transient, period);
            if (hasMemory)
            {
                period = coprimes.Select(kv => StoringMultiplicativeOrder(nBase, (int)Math.Pow(kv.Key, kv.Value)))
                                 .Aggregate(Lcm);
            }
            else
            {
                period = coprimes.Select(kv => MultiplicativeOrder(nBase, (int)Math.Pow(kv.Key, kv.Value)))
                                 .Aggregate(Lcm);
            }

            return Tuple.Create(transient, period);
        }

        /// <summary>
        /// Least common multiple
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static int Lcm(int a, int b)
        {
            return ((System.Math.Abs(a)) / Gcd(a, b)) * System.Math.Abs(b);
        }

        public static IEnumerable<int> PrimeFactorsOf(int n, IEnumerable<int> primes)
        {
            var result = new List<int>();
            var rest = n;
            var limitedPrimes = primes.TakeWhile(p => p <= n);
            var enumerable = limitedPrimes as IList<int> ?? limitedPrimes.ToList();
            foreach (var prime in enumerable)
            {
                if (rest == 1) break;
                while (rest % prime == 0)
                {
                    result.Add(prime);
                    rest /= prime;
                }
            }
            return result.DefaultIfEmpty(n);
        }

        /// <summary>
        /// More effective than PrimeFactorsOf(int n, IEnumerable primes);
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        public static IEnumerable<int> PrimeFactorsOf(int n)
        {
            var result = new List<int>();
            if (n == 1) return result;
            var rest = n;
            foreach (var i in Enumerable.Range(2, n - 1))
            {
                if (rest == 1) break;
                while (rest % i == 0)
                {
                    result.Add(i);
                    rest /= i;
                }
            }
            return result.DefaultIfEmpty(n);
        }

        public static IEnumerable<int> PrimesTo(int n)
        {
            var isPrime = new BitArray(n, true);
            var limit = System.Math.Sqrt(n);
            isPrime[0] = false;
            for (var i = 2; i <= limit; i++)
            {
                if (!isPrime[i - 1]) continue;
                for (var j = i * i; j <= n; j += i)
                    isPrime[j - 1] = false;
            }
            return Enumerable.Range(1, n).Where(i => isPrime[i - 1]).ToList();
        }

        private static int MultiplicativeOrder(int nBase, int modulo)
        {
            //Debug.Assert(Gcd(nBase, modulo) == 1,"Modulo isn't coprime to nBase");
            for (var i = 1; i < modulo; i++)
            {
                if (BigInteger.ModPow(nBase, i, modulo).Equals(1)) return i;
            }
            throw new ArgumentException("Invalid modulo");
        }

        private static int StoringMultiplicativeOrder(int nBase, int modulo)
        {
            int value;
            if (!_prevMO.TryGetValue(Tuple.Create(nBase, modulo), out value))
            {
                _prevMO.Add(Tuple.Create(nBase, modulo), MultiplicativeOrder(nBase, modulo));
            }
            return value;
        }

        /*  easiest way:
         *      when number is requested
         *      get factors for X, store them;
         *      determine if X is prime (common in all bases)
         */

        /*  That basic sieve:
         *
         *  Input: an integer n > 1
         *
         *  Let A be an array of Boolean values, indexed by integers 2 to n,
         *  initially all set to true.
         *
         *      for n = 2, 3, 4, ..., not exceeding √n:
         *      if A[n] is true:
         *      for j = n^2, n^2+n, n^2+2i, ..., not exceeding n :
         *          A[j] := false
         *
         *  Output: all n such that A[n] is true.
         */
    }
}