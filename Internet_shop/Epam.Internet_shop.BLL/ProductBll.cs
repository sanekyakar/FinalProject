using System.Collections.Generic;
using System.Linq;

using Epam.Internet_shop.Logger.Contracts;
using Epam.Internet_shop.BLL.Contracts;
using Epam.Internet_shop.DAL.Contracts;
using Epam.Internet_shop.Common.Entities.CommodityUnit;

namespace Epam.Internet_shop.BLL
{
    public class ProductBll : IProductBll
    {
        private readonly ILogger _logger;
        private readonly ICategoryBll _categoryBll;
        private readonly IProductDao _productDao;
        private readonly ICommodityUnitDao _commodityUnitDao;

        public ProductBll(ILogger logger, ICategoryBll categoryBll, IProductDao productDao, ICommodityUnitDao commodityUnitDao)
        {
            _logger = logger;
            _categoryBll = categoryBll;
            _productDao = productDao;
            _commodityUnitDao = commodityUnitDao;
        }

        public IEnumerable<Product> GetAllProducts()
        {
            _logger.Info($"BLL.{nameof(ProductBll)}.{nameof(GetAllProducts)}: Getting all products");

            foreach (var item in _productDao.GetAllProducts())
            {
                yield return item;
            }
        }

        public IEnumerable<ProductWithUnits> GetAllProductsWithUnits()
        {
            _logger.Info($"BLL.{nameof(ProductBll)}.{nameof(GetAllProductsWithUnits)}: Getting all products with units");

            foreach (var item in _productDao.GetAllProducts())
            {
                yield return new ProductWithUnits
                (
                    item.Id,
                    item.Name,
                    item.ImageInBase64Src,
                    item.Discription,
                    item.Category,
                    _commodityUnitDao.GetAllCommodityUnits()
                        .Where(unit => unit.Product is null ? false : unit.Product.Id == item.Id)
                        .ToList()
                );
            }

            _logger.Info($"BLL.{nameof(ProductBll)}.{nameof(GetAllProductsWithUnits)}: Received all products with units");

            yield break;
        }

        public Product GetProductOrNull(int id)
        {
            _logger.Info($"BLL.{nameof(ProductBll)}.{nameof(GetProductOrNull)}: Getting the product id = " + id);

            if (_productDao.IsProduct(id))
            {
                _logger.Info($"BLL.{nameof(ProductBll)}.{nameof(GetProductOrNull)}: Product id = {id} received");

                return _productDao.GetProduct(id);
            }
            else
            {
                _logger.Warn($"BLL.{nameof(ProductBll)}.{nameof(GetProductOrNull)}: Product id = {id} not found");

                return null;
            }
        }

        public IEnumerable<Product> GetProductsByCategory(int id)
        {
            _logger.Info($"BLL.{nameof(ProductBll)}.{nameof(GetProductsByCategory)}: Getting products by category id = " + id);

            foreach (var item in _productDao.GetProductsByCategory(id))
            {
                yield return item;
            }
        }

        public IEnumerable<ProductWithUnits> GetProductsWithUnitsByCategory(int id)
        {
            _logger.Info($"BLL.{nameof(ProductBll)}.{nameof(GetProductsWithUnitsByCategory)}: Getting products with units by category id = " + id);

            foreach (var product in _productDao.GetProductsByCategory(id))
            {
                yield return new ProductWithUnits
                (
                    product.Id,
                    product.Name,
                    product.ImageInBase64Src,
                    product.Discription,
                    product.Category,
                    _commodityUnitDao.GetCommodityUnitsByProduct(product.Id ?? -1).ToList()
                );
            }

            _logger.Info($"BLL.{nameof(ProductBll)}.{nameof(GetProductsWithUnitsByCategory)}: Received all products with units by category id = " + id);

            yield break;
        }

        public ProductWithUnits GetProductWithUnitsOrNull(int id)
        {
            _logger.Info($"BLL.{nameof(ProductBll)}.{nameof(GetProductWithUnitsOrNull)}: Getting the product with units id = " + id);

            if (_productDao.IsProduct(id))
            {
                var product = _productDao.GetProduct(id);

                var UnitList = _commodityUnitDao.GetCommodityUnitsByProduct(id).ToList();

                _logger.Info($"BLL.{nameof(ProductBll)}.{nameof(GetProductWithUnitsOrNull)}: Product with units id = {id} received");

                return new ProductWithUnits(product.Id, product.Name, product.ImageInBase64Src, product.Discription, product.Category , UnitList);
            }
            else
            {
                _logger.Warn($"BLL.{nameof(ProductBll)}.{nameof(GetProductWithUnitsOrNull)}: Product with units id = {id} not found");

                return null;
            }
        }

        public void RemoveProduct(int id)
        {
            _logger.Info($"BLL.{nameof(ProductBll)}.{nameof(RemoveProduct)}: Deleting the product id = " + id);

            if (_productDao.IsProduct(id))
            {
                _productDao.RemoveProduct(id);

                _logger.Info($"BLL.{nameof(ProductBll)}.{nameof(RemoveProduct)}: Product removed id = " + id);
            }
            else
            {
                _logger.Warn($"BLL.{nameof(ProductBll)}.{nameof(RemoveProduct)}: Product id = {id} not found");
            }
        }

        public int SetProduct(Product product)
        {
            _logger.Info($"BLL.{nameof(ProductBll)}.{nameof(SetProduct)}: Retention of the product");

            if (product.Category != null)
            {
                _logger.Info($"BLL.{nameof(ProductBll)}.{nameof(SetProduct)}: Category discovered");

                product.Category.Id = _categoryBll.SetCategory(product.Category);
            }
            else
            {
                _logger.Info($"BLL.{nameof(ProductBll)}.{nameof(SetProduct)}: Category not discovered");
            }

            if (product.Id != null)
            {
                int id = _productDao.ChangeProduct(product);

                _logger.Info($"BLL.{nameof(ProductBll)}.{nameof(SetProduct)}: Product id = {id} changed");

                return id;
            }
            else
            {
                int id = _productDao.AddProduct(product);

                _logger.Info($"BLL.{nameof(ProductBll)}.{nameof(SetProduct)}: Product id = {id} added");

                return id;
            }
        }
    }
}
