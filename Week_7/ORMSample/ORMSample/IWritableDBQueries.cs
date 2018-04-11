using ORMSample.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ORMSample
{
    public interface IWritableDBQueries
    {
        void AddEmployeeWithTerritories();

        void MoveProductsToAnotherCategory(IEnumerable<Product> products, Category category);

        void AddProductsWithSuppliersAndCategories();

        void ChangeProductToAnother();
    }
}
