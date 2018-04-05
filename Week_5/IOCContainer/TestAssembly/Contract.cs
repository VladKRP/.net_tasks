using IOCContainer.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestAssembly
{
    public class Contract
    {
        [Export]
        public class ContractBLL { }

        [Export]
        public class ContractDLL { }
    }
}
