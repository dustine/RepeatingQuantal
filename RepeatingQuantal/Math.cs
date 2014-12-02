using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepeatingQuantal
{
    class CustomMath
    {
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
                b = a1%b;
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
            return ((Math.Abs(a))/Gcd(a, b))*Math.Abs(b);
        }
    }
}
