using ORMSample.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ORMSample.Interfaces
{
    public interface IReadOnlyDBQueries
    {
        IEnumerable<Product> GetProductsWithCategoryAndSuppliers();

        IEnumerable<EmployeeRegion> GetEmployeesWithRegion();

        IEnumerable<EmployeesInRegion> GetAmountOfEmployeesByRegion();

        IEnumerable<EmployeeSuppliers> GetEmployeeWithSuppliers();
    }
}
