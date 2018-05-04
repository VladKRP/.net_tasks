
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
    public class GeneralManager<T> : IManager<T> where T: class
    {
        protected readonly ICache<T> _cache;

        public GeneralManager(ICache<T> cache){
            _cache = cache;
        }

        public IEnumerable<T> GetAll()
        {
            var user = Thread.CurrentPrincipal.Identity.Name;
            var entities = _cache.Get(user);

            if (entities == null)
            {
                Console.WriteLine("From DB");//for test?

                using (var context = new Northwind())
                {
                    context.Configuration.LazyLoadingEnabled = false;//getting troubles with Employee entity
                    context.Configuration.ProxyCreationEnabled = false;
                    entities = context.Set<T>().ToList();
                    _cache.Set(user, entities);
                }
            }

            return entities;
        }
    }
}
