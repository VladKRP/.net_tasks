using CachingSolutionsSamples.CacheEngines;
using NorthwindLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CachingSolutionsSamples.Managers
{
    class NorthwindRedisCacheManager<T>: IManager<T> where T: class
    {
        protected readonly IRedisCache<T> _cache;
        private readonly DateTime? _cacheExpiryDate;

        public NorthwindRedisCacheManager(IRedisCache<T> cache, DateTime? cacheExpiryDate = null)
        {
            _cache = cache;
            _cacheExpiryDate = cacheExpiryDate;
        }

        public IEnumerable<T> GetAll()
        {
            var user = Thread.CurrentPrincipal.Identity.Name;
            var entities = _cache.Get(user);

            if (entities == null)
            {
                Console.WriteLine("From DB");//for test

                using (var context = new Northwind())
                {
                    context.Configuration.LazyLoadingEnabled = false;//getting troubles with Employee entity
                    context.Configuration.ProxyCreationEnabled = false;
                    entities = context.Set<T>().ToList();
                    _cache.Set(user, entities, _cacheExpiryDate);
                }
            }
            else
                Console.WriteLine("From Cache");//for test

            return entities;
        }

        public void DeleteAll()
        {
            var user = Thread.CurrentPrincipal.Identity.Name;
            var entities = _cache.Get(user);

            if (entities != null)
            {
                _cache.Delete(user);
            }
        }
    }
}
