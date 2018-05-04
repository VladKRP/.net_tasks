using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CachingSolutionsSamples.Managers
{
    public interface IManager<T>
    {
        IEnumerable<T> GetAll();
    }
}
