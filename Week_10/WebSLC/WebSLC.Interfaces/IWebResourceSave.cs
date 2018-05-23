using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSLC.Models;

namespace WebSLC.Interfaces
{
    public interface IWebResourceSave
    {
        void Save(WebResourceBase entity);
    }
}
