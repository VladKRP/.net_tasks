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

            MemoryCache memoryCache = new MemoryCache("fibo-cache");

            CalculateFibonachiNumber(4, memoryCache);
            CalculateFibonachiNumber(4, memoryCache);
            CalculateFibonachiNumber(13, memoryCache);
            CalculateFibonachiNumber(13, null);
        }

        public static int CalculateFibonachiNumber(int n, MemoryCache cache)
        {
            var cachedValue = cache?.Get(n.ToString()) as Nullable<int>;
            if (cachedValue.HasValue)
                    return cachedValue.Value;

            var result = CalculateFibonachiNumber(n);

            if(cache != null)
                cache.Add(new CacheItem(n.ToString(), result), new CacheItemPolicy());

            return result;
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
