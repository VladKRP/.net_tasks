using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace CachingSolutionsSamples.CacheEngines
{
    interface IMemoryCache<T>: ICache<T>
    {
        void Set(string forUser, IEnumerable<T> entities, CacheItemPolicy cacheItemPolicy );
    }
}
