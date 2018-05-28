using CachingSolutionsSamples.CacheEngines;
using NorthwindLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TableDependency.Abstracts;
using TableDependency.EventArgs;

namespace CachingSolutionsSamples.Managers
{
    class NorthwindCacheManager<T> : IDisposable, IManager<T> where T : class, new()
    {
        private readonly ICache<T> _cache;

        private readonly ITableDependency<T> _tableDependency;
        private readonly DateTime? _cacheExpiryDate;

        private readonly string user = Thread.CurrentPrincipal.Identity.Name;


        public NorthwindCacheManager(ICache<T> cache)
        {
            _cache = cache;
        }

        public NorthwindCacheManager(ICache<T> cache, DateTime cacheExpiryDate)
        {
            _cache = cache;
            _cacheExpiryDate = cacheExpiryDate;
        }

        public NorthwindCacheManager(ICache<T> cache, ITableDependency<T> tableDependency)
        {
            _cache = cache;
            _tableDependency = tableDependency;
            _tableDependency.OnChanged += _tableDependency_OnChanged;
            _tableDependency.Start();

            void _tableDependency_OnChanged(object sender, RecordChangedEventArgs<T> e)
            {
                _cache.Delete(user);
                Console.WriteLine("Table changed");
            }
        }

        public IEnumerable<T> GetAll()
        {
            var entities = _cache.Get(user);

            if (entities == null)
            {
                Console.WriteLine("From DB");//for test

                using (var context = new Northwind())
                {
                    context.Configuration.LazyLoadingEnabled = false;
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
            if (_cache.Get(user) != null)
                _cache.Delete(user);
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    _tableDependency?.Stop();
                    disposed = true;
                }
            }    
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
