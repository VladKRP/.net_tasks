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
        void AddEmployeeWithTerritories(Employee employee);

        void ChangeProductsCategory(Category currentCategory, Category newCategory);

        void AddProductsWithSuppliersAndCategories(IEnumerable<Product> products);

        void ReplaceProductWhileOrderNotShipped(Product orderProduct, Product sameProduct);
    }
}
