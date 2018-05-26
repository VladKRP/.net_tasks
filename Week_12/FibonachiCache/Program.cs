using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Caching;
using StackExchange.Redis;

namespace FibonachiCache
{
    class Program
    {
        static void Main(string[] args)
        {
            //Memory cache
            MemoryCache memoryCache = new MemoryCache("fibo-cache");

            CalculateFibonachiNumberMemoryCache(4, memoryCache);
            CalculateFibonachiNumberMemoryCache(4, memoryCache);
            CalculateFibonachiNumberMemoryCache(13, memoryCache);
            CalculateFibonachiNumberMemoryCache(13, null);

            //Redis cache

            const string hostname = "localhost";
            ConnectionMultiplexer connectionMultiplexer = ConnectionMultiplexer.Connect(hostname);
            var redisCache = connectionMultiplexer.GetDatabase();

            CalculateFibonachiNumberRedisCache(4, redisCache);
            CalculateFibonachiNumberRedisCache(4, redisCache);

        }

        public static int CalculateFibonachiNumberMemoryCache(int n, MemoryCache cache)
        {
            var cacheKey = cache?.Name + n;

            var cachedValue = cache?.Get(cacheKey) as Nullable<int>;
            if (cachedValue.HasValue)
                    return cachedValue.Value;

            var calculationResult = CalculateFibonachiNumber(n);

                cache?.Set(cacheKey, calculationResult, new DateTimeOffset(DateTime.UtcNow.AddMinutes(5)));

            return calculationResult;
        }

        public static int CalculateFibonachiNumberRedisCache(int n, IDatabase cache)
        {
            var cacheKey = "fibo-cache" + n;

            string cachedValue = cache?.StringGet(cacheKey);
            if (!string.IsNullOrWhiteSpace(cachedValue) && int.TryParse(cachedValue, out int result))
                return result;

            var calculationResult = CalculateFibonachiNumber(n);

            cache?.StringSet(cacheKey, calculationResult);

            return calculationResult;
        }

        public static int CalculateFibonachiNumber(int n)
        {
            if (n <= 2)
                return 1;
            int current = 1;
            int previous = 0;

            for (int i = 1; i < n; i++)
            {
                current += previous;
                previous = current - previous;
            }

            return current;
        }
    }
}
