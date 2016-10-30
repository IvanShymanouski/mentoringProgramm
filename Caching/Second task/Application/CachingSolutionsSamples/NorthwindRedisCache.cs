using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NorthwindLibrary;
using StackExchange.Redis;
using System.Xml.Serialization;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;

namespace CachingSolutionsSamples
{
    class NorthwindRedisCache : INorthwindCache
    {
        private const string DefaultPrefix = "Cache_";
        private ConnectionMultiplexer redisConnection;
        DataContractSerializer serializer = new DataContractSerializer(
            typeof(IEnumerable<Category>));

        public NorthwindRedisCache(string hostName)
        {
            redisConnection = ConnectionMultiplexer.Connect(hostName);
        }

        public IEnumerable<T> Get<T>(string forUser)
        {
            var db = redisConnection.GetDatabase();
            byte[] s = db.StringGet(GetKey<T>(forUser));
            if (s == null)
                return null;

            return (IEnumerable<T>)serializer
                .ReadObject(new MemoryStream(s));
        }

        public void Set<T>(string forUser, IEnumerable<T> entities)
        {
            var db = redisConnection.GetDatabase();
            var key = GetKey<T>(forUser);

            if (entities == null)
            {
                db.StringSet(key, RedisValue.Null);
            }
            else
            {
                var stream = new MemoryStream();
                serializer.WriteObject(stream, entities);
                db.StringSet(key, stream.ToArray(), TimeSpan.FromSeconds(30)); // 30c чтобы было удобнее проверять, что работает
            }
        }

        private string GetKey<T>(string forUser)
        {
            return DefaultPrefix + typeof(T) + forUser;
        }
    }
}
