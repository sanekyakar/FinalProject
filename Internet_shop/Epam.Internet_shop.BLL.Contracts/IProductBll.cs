using System.Collections.Generic;

using Epam.Internet_shop.Common.Entities.CommodityUnit;

namespace Epam.Internet_shop.BLL.Contracts
{
    public interface IProductBll
    {
        Product GetProductOrNull(int id);

        ProductWithUnits GetProductWithUnitsOrNull(int id);

        IEnumerable<Product> GetAllProducts();

        IEnumerable<ProductWithUnits> GetAllProductsWithUnits();

        IEnumerable<Product> GetProductsByCategory(int id);

        IEnumerable<ProductWithUnits> GetProductsWithUnitsByCategory(int id);

        int SetProduct(Product product);

        void RemoveProduct(int id);
    }
}
