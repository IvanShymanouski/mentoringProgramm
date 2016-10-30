using System.Numerics;

namespace Fibonacci.Cache
{
    public interface IBigIntegerCache
    {
        BigInteger? Get(string key);

        void Set(string key, BigInteger number);
    }
}