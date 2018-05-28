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

namespace CachingSolutionsSamples.CacheEngines
{
    class GeneralRedisCache<T> : ICache<T>
    {
        private readonly IConnectionMultiplexer redisConnection;
        private readonly string prefix;
        private readonly DataContractSerializer serializer;

        public GeneralRedisCache(string hostName)
        {
            redisConnection = ConnectionMultiplexer.Connect(hostName);
            serializer = new DataContractSerializer(typeof(IEnumerable<T>));
            prefix = typeof(T).Name;
        }

        public IEnumerable<T> Get(string forUser)
        {
            var database = redisConnection.GetDatabase();
            byte[] entities = database.StringGet(prefix + forUser);
            if (entities == null)
                return null;

            return (IEnumerable<T>)serializer
                .ReadObject(new MemoryStream(entities));

        }

        public void Set(string forUser, IEnumerable<T> entities, DateTime? expiry = null)
        {
            var database = redisConnection.GetDatabase();
            var key = prefix + forUser;

            if (entities != null)
            {
                var stream = new MemoryStream();
                serializer.WriteObject(stream, entities);

                var currentDate = DateTime.UtcNow;
                if (expiry.HasValue && expiry.Value > currentDate)
                {
                    var remaining = expiry.Value - currentDate;
                    database.StringSet(key, stream.ToArray(), remaining);
                }
                else
                    database.StringSet(key, stream.ToArray());
            }
            else
                database.StringSet(key, RedisValue.Null);
        }

        public void Delete(string forUser)
        {
            var database = redisConnection.GetDatabase();
            var key = prefix + forUser;

            database.KeyDelete(key);
        }
    }
}
