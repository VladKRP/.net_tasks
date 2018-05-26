using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CachingSolutionsSamples.Managers
{
    interface IManager<T>
    {
        IEnumerable<T> GetAll();
        void DeleteAll();
    }
}
