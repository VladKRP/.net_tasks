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
		private readonly ConnectionMultiplexer redisConnection;
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
			byte[] s = database.StringGet(prefix + forUser);
			if (s == null)
				return null;

			return (IEnumerable<T>)serializer
				.ReadObject(new MemoryStream(s));

		}

		public void Set(string forUser, IEnumerable<T> entities)
		{
			var database = redisConnection.GetDatabase();
			var key = prefix + forUser;

			if (entities == null)
			{
				database.StringSet(key, RedisValue.Null);
			}
			else
			{
				var stream = new MemoryStream();
				serializer.WriteObject(stream, entities);
				database.StringSet(key, stream.ToArray(), TimeSpan.FromMinutes(1));
			}
		}

        public void Delete(string forUser)
        {
            var database = redisConnection.GetDatabase();
            var key = prefix + forUser;

            database.KeyDelete(key);
        }
    }
}
