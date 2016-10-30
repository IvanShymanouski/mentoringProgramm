using System.Numerics;
using System.Runtime.Caching;

namespace Fibonacci.Cache
{
    public class InMemoryCache : IBigIntegerCache
    {
        private const string _prefix = "fibonacci_";

        public BigInteger? Get(string key)
        {
            ObjectCache cache = MemoryCache.Default;
            return cache.Get(GetKey(key)) as BigInteger?;
        }

        public void Set(string key, BigInteger number)
        {
            ObjectCache cache = MemoryCache.Default;
            cache.Set(GetKey(key), number, ObjectCache.InfiniteAbsoluteExpiration);
        }

        private string GetKey(string value)
        {
            return _prefix + value;
        }
    }
}