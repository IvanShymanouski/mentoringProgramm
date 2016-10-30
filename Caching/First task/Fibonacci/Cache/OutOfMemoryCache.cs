using System;
using System.IO;
using System.Numerics;
using System.Runtime.Serialization;
using StackExchange.Redis;


namespace Fibonacci.Cache
{
    public class OutOfMemoryCache : IBigIntegerCache, IDisposable
    {
        private const string Prefix = "fibonacci_";

        private ConnectionMultiplexer _redis;
        private DataContractSerializer _serializer;
        private IServer _server;
        private bool _disposed = false;

        public OutOfMemoryCache(string hostName)
        {
            _redis = ConnectionMultiplexer.Connect(hostName);
            _server = _redis.GetServer(_redis.GetEndPoints()[0]);
            _serializer = new DataContractSerializer(typeof(BigInteger?));
        }

        public BigInteger? Get(string key)
        {
            var db = _redis.GetDatabase();
            var result = db.StringGet(GetKey(key));
            if (result.IsNull)
            {
                return null;
            }

            return (BigInteger)_serializer.ReadObject(new MemoryStream(result));
        }

        public void Set(string key, BigInteger number)
        {
            var db = _redis.GetDatabase();
            var stream = new MemoryStream();
            _serializer.WriteObject(stream, number);
            db.StringSet(GetKey(key), stream.ToArray());
        }
        
        public void Dispose()
        {
           Dispose(true);
           GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                   _server.FlushAllDatabases();
                }
                _disposed = true;
            }
        }

        private string GetKey(string value)
        {
            return Prefix + value;
        }

        ~OutOfMemoryCache()
        {
            Dispose(false);
        }
    }
}
