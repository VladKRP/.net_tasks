using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSLC.Interfaces
{
    public interface ILinkStorage
    {
        void Add(Uri link);
        void Clear();
        bool Exists(Uri link);
    }
}
