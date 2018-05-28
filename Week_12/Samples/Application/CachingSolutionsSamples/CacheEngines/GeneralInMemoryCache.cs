using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace CachingSolutionsSamples.CacheEngines
{
    class GeneralInMemoryCache<T>: ICache<T>
    {
        private readonly ObjectCache cache;
        private readonly string prefix;

        public GeneralInMemoryCache()
        {
            cache = MemoryCache.Default;
            prefix = typeof(T).Name;
        }

        public IEnumerable<T> Get(string forUser)
        {
            return cache.Get(prefix + forUser) as IEnumerable<T>;
        }

        public void Set(string forUser, IEnumerable<T> entities, DateTime? expiry = null)
        {
            DateTimeOffset timeOffset = ObjectCache.InfiniteAbsoluteExpiration;
            if (expiry.HasValue)
                timeOffset = new DateTimeOffset(expiry.Value);
            cache.Set(prefix + forUser, entities, timeOffset);
        }

        public void Set(string forUser, IEnumerable<T> entities, CacheItemPolicy cacheItemPolicy)
        {
            cache.Set(prefix + forUser, entities, cacheItemPolicy);
        }

        public void Delete(string forUser)
        {
            cache.Remove(prefix + forUser);
        }

        
    }
}

