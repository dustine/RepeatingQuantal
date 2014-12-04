using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Numerics;

namespace RepeatingQuantal
{
    public class AdditionalMath
    {
        #region Singleton usage AdditionalMath.Instance

        private static readonly AdditionalMath mInstance = new AdditionalMath();

        static AdditionalMath()
        {
        }

        private AdditionalMath()
        {
        }

        public static AdditionalMath Instance { get { return mInstance; } }

        #endregion Singleton usage AdditionalMath.Instance

        public static List<List<int>> FactorsTo(int n)
        {
            var primes = PrimesTo(n).AsReadOnly();
            return Enumerable.Range(1, n)
                             .Select(i => FactorsOf(i, primes))
                             .ToList();
        }

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

        public static int MultiplicativeOrder(int numBase, int modulo)
        {
            if (Gcd(numBase, modulo) != 1) throw new ArgumentException();
            for (var i = 1; i < modulo; i++)
            {
                if (BigInteger.ModPow(numBase, i, modulo).Equals(1)) return i;
            }
            throw new ArgumentException();
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
         *      for i = 2, 3, 4, ..., not exceeding √n:
         *      if A[i] is true:
         *      for j = i^2, i^2+i, i^2+2i, ..., not exceeding n :
         *          A[j] := false
         *
         *  Output: all i such that A[i] is true.
         */

        public static List<int> PrimesTo(int n)
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

        private static List<int> FactorsOf(int i, ReadOnlyCollection<int> primes)
        {
            throw new NotImplementedException();
        }
    }
}