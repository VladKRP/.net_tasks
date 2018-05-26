
using CachingSolutionsSamples.CacheEngines;
using NorthwindLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CachingSolutionsSamples.Managers
{
    class NorthwindMemoryCacheManager<T> : IManager<T> where T: class
    {
        protected readonly IMemoryCache<T> _cache;
        private readonly DateTime? _cacheExpiryDate;
        private readonly CacheItemPolicy _cachePolicy;

        public NorthwindMemoryCacheManager(IMemoryCache<T> cache){
            _cache = cache;
        }

        public NorthwindMemoryCacheManager(IMemoryCache<T> cache, DateTime cacheExpiryDate)
        {
            _cache = cache;
            _cacheExpiryDate = cacheExpiryDate;
        }

        public NorthwindMemoryCacheManager(IMemoryCache<T> cache, CacheItemPolicy cachePolicy)
        {
            _cache = cache;
            _cachePolicy = cachePolicy;
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
                    if(_cachePolicy != null)
                        _cache.Set(user, entities, _cachePolicy);
                    else
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

            if(entities != null)
            {
                _cache.Delete(user);
            }
        }
    }
}
