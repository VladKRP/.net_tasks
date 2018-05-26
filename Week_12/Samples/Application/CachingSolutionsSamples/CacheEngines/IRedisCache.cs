using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CachingSolutionsSamples.CacheEngines
{
    interface IRedisCache<T>: ICache<T>
    {
    }
}
