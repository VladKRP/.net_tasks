using ORMSample.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DapperSamples
{
    class SupplierEqualityComparer : IEqualityComparer<Supplier>
    {
        public bool Equals(Supplier firstSupplier, Supplier secondSupplier)
        {
            bool isEqual = false;
            if (firstSupplier != null && secondSupplier != null)
            {
                if (firstSupplier.SupplierID != secondSupplier.SupplierID)
                    return false;

                var firstSupplierType = firstSupplier.GetType();
                var secondSupplierType = secondSupplier.GetType();

                if (!firstSupplierType.Equals(secondSupplierType))
                    return false;

                var properties = firstSupplierType.GetProperties();
                if (properties.All(property => object.Equals(property.GetValue(firstSupplier), property.GetValue(secondSupplier))))
                    isEqual = true;
            }
            return isEqual;

        }

        public int GetHashCode(Supplier obj)
        {
            return obj.SupplierID + base.GetHashCode();
        }
    }
}
