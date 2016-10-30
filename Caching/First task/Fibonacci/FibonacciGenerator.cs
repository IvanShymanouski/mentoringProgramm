using System;
using System.Numerics;
using System.Threading;

namespace Fibonacci
{
    public class FibonacciGenerator : IFibonacciGenerator
    {
        public BigInteger GetNumber(BigInteger index)
        {
            if (index == 0)
            {
                return BigInteger.Zero;
            }

            if (index == 1 || index == 2)
            {
                return BigInteger.One;
            }

            var previous = BigInteger.One;
            var current = BigInteger.One;
            for (int i = 2; i <= index; i++)
            {
                Thread.Sleep(new TimeSpan(1000)); //for emulate long work
                var next = previous + current;
                previous = current;
                current = next;
            }

            return current;
        }
    }
}
