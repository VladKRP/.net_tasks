
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
        private ChangeMonitor _changeMonitor;
        //private CacheItemPolicy _cacheItemPolicy;

        public NorthwindMemoryCacheManager(IMemoryCache<T> cache){
            _cache = cache;
        }

        public NorthwindMemoryCacheManager(IMemoryCache<T> cache, DateTime cacheExpiryDate)
        {
            _cache = cache;
            _cacheExpiryDate = cacheExpiryDate;
        }

        //public NorthwindMemoryCacheManager(IMemoryCache<T> cache, CacheItemPolicy cacheItemPolicy)
        //{
        //    _cache = cache;
        //    _cacheItemPolicy = cacheItemPolicy;
        //}

        public NorthwindMemoryCacheManager(IMemoryCache<T> cache, ChangeMonitor changeMonitor)
        {
            _cache = cache;
            _changeMonitor = changeMonitor;
        }

        public IEnumerable<T> GetAll()
        {
            var user = Thread.CurrentPrincipal.Identity.Name;
            var entities = _cache.Get(user);

            if (entities == null || (_changeMonitor != null && _changeMonitor.HasChanged))//exception
            {                    
                Console.WriteLine("From DB");//for test
               
                using (var context = new Northwind())
                {
                    context.Configuration.LazyLoadingEnabled = false;
                    context.Configuration.ProxyCreationEnabled = false;
                    entities = context.Set<T>().ToList();
                    if(_changeMonitor != null)
                    {
                        CacheItemPolicy cachePolicy = new CacheItemPolicy();
                        _cache.Set(user, entities, cachePolicy);
                        cachePolicy.ChangeMonitors.Add(_changeMonitor);
                    }
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
