using System.Numerics;

namespace Fibonacci
{
    public interface IFibonacciGenerator // переносить в другой проект лень
    {
        BigInteger GetNumber(BigInteger index);
    }
}