using System.Collections.Generic;

using Epam.Internet_shop.Common.Entities.CommodityUnit;

namespace Epam.Internet_shop.DAL.Contracts
{
    public interface IProductDao
    {
        Product GetProduct(int id);

        IEnumerable<Product> GetAllProducts();

        IEnumerable<Product> GetProductsByCategory(int id);

        bool IsProduct(int id);

        int AddProduct(Product product);

        int ChangeProduct(Product product);

        void RemoveProduct(int id);
    }
}
