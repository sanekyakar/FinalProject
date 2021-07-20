using System.Collections.Generic;
using System.Linq;

using Epam.Internet_shop.Logger.Contracts;
using Epam.Internet_shop.BLL.Contracts;
using Epam.Internet_shop.DAL.Contracts;
using Epam.Internet_shop.Common.Entities.CommodityUnit;
using System;

namespace Epam.Internet_shop.BLL
{
    public class CommodityUnitBll : ICommodityUnitBll
    {
        private readonly ILogger _logger;
        private readonly ICategoryBll _categoryBll;
        private readonly IProductBll _productBll;
        private readonly IStatusBll _statusBll;
        private readonly IStoreBll _storeBll;
        private readonly IVendorBll _vendorBll;
        private readonly ICommodityUnitDao _commodityUnitDao;

        public CommodityUnitBll(ILogger logger, ICategoryBll categoryBll, IProductBll productBll, IStatusBll statusBll, IStoreBll storeBll, IVendorBll vendorBll, ICommodityUnitDao commodityUnitDao)
        {
            _logger = logger;
            _categoryBll = categoryBll;
            _productBll = productBll;
            _statusBll = statusBll;
            _storeBll = storeBll;
            _vendorBll = vendorBll;
            _commodityUnitDao = commodityUnitDao;
        }

        public IEnumerable<CommodityUnit> GetAllCommodityUnits()
        {
            _logger.Info($"BLL.{nameof(CommodityUnitBll)}.{nameof(GetAllCommodityUnits)}: Getting all commodity units");

            return _commodityUnitDao.GetAllCommodityUnits();
        }

        public CommodityUnit GetCommodityUnitOrNull(int id)
        {
            _logger.Info($"BLL.{nameof(CommodityUnitBll)}.{nameof(GetCommodityUnitOrNull)}: Getting commodity units id = " + id);

            if (_commodityUnitDao.IsCommodityUnit(id))
            {
                _logger.Info($"BLL.{nameof(CommodityUnitBll)}.{nameof(GetCommodityUnitOrNull)}: Commodity units id = {id} received");

                return _commodityUnitDao.GetCommodityUnit(id);
            }
            else
            {
                _logger.Warn($"BLL.{nameof(CommodityUnitBll)}.{nameof(GetCommodityUnitOrNull)}: Commodity units id = {id} not found");

                return null;
            }
        }

        public IEnumerable<CommodityUnit> GetCommodityUnitsByCategory(int id)
        {
            _logger.Info($"BLL.{nameof(CommodityUnitBll)}.{nameof(GetCommodityUnitsByCategory)}: Getting commodities units by category id = " + id);

            var commodityUnits = _commodityUnitDao.GetAllCommodityUnits()
                .Where(
                    unit => unit.Product is null ?
                    false : unit.Product.Category is null ?
                    false : unit.Product.Category.Id == id
                );

            foreach (var item in commodityUnits)
            {
                yield return item;
            }
        }

        public IEnumerable<CommodityUnit> GetCommodityUnitsByProduct(int id)
        {
            _logger.Info($"BLL.{nameof(CommodityUnitBll)}.{nameof(GetCommodityUnitsByCategory)}: Getting commodities units by product id = " + id);

            foreach (var item in _commodityUnitDao.GetCommodityUnitsByProduct(id))
            {
                yield return item;
            }
        }

        public IEnumerable<CommodityUnit> GetCommodityUnitsByStatus(int id)
        {
            _logger.Info($"BLL.{nameof(CommodityUnitBll)}.{nameof(GetCommodityUnitsByCategory)}: Getting commodities units by status id = " + id);

            foreach (var item in _commodityUnitDao.GetCommodityUnitsByStatus(id))
            {
                yield return item;
            }
        }

        public IEnumerable<CommodityUnit> GetCommodityUnitsByStore(int id)
        {
            _logger.Info($"BLL.{nameof(CommodityUnitBll)}.{nameof(GetCommodityUnitsByCategory)}: Getting commodities units by store id = " + id);

            foreach (var item in _commodityUnitDao.GetCommodityUnitsByStore(id))
            {
                yield return item;
            }
        }

        public IEnumerable<CommodityUnit> GetCommodityUnitsByVendor(int id)
        {
            _logger.Info($"BLL.{nameof(CommodityUnitBll)}.{nameof(GetCommodityUnitsByCategory)}: Getting commodities units by vendor id = " + id);

            foreach (var item in _commodityUnitDao.GetCommodityUnitsByVendor(id))
            {
                yield return item;
            }
        }

        public int SetCommodityUnit(CommodityUnit commodityUnit)
        {
            _logger.Info($"BLL.{nameof(CommodityUnitBll)}.{nameof(SetCommodityUnit)}: Retention of commodity unit");

            if (commodityUnit.Product != null)
            {
                _logger.Info($"BLL.{nameof(CommodityUnitBll)}.{nameof(SetCommodityUnit)}: Product discovered");

                commodityUnit.Product.Id = _productBll.SetProduct(commodityUnit.Product);

                if (commodityUnit.Product.Category != null)
                {
                    _logger.Info($"BLL.{nameof(CommodityUnitBll)}.{nameof(SetCommodityUnit)}: Category discovered");

                    commodityUnit.Product.Category.Id = _categoryBll.SetCategory(commodityUnit.Product.Category);
                }
                else
                {
                    _logger.Info($"BLL.{nameof(CommodityUnitBll)}.{nameof(SetCommodityUnit)}: Category not discovered");
                }
            }
            else
            {
                _logger.Info($"BLL.{nameof(CommodityUnitBll)}.{nameof(SetCommodityUnit)}: Product not discovered");
            }

            if (commodityUnit.Status != null)
            {
                _logger.Info($"BLL.{nameof(CommodityUnitBll)}.{nameof(SetCommodityUnit)}: Status discovered");

                commodityUnit.Status.Id = _statusBll.SetStatus(commodityUnit.Status);
            }
            else
            {
                _logger.Info($"BLL.{nameof(CommodityUnitBll)}.{nameof(SetCommodityUnit)}: Status not discovered");
            }

            if (commodityUnit.Store != null)
            {
                _logger.Info($"BLL.{nameof(CommodityUnitBll)}.{nameof(SetCommodityUnit)}: Store discovered");

                commodityUnit.Store.Id = _storeBll.SetStore(commodityUnit.Store);
            }
            else
            {
                _logger.Info($"BLL.{nameof(CommodityUnitBll)}.{nameof(SetCommodityUnit)}: Store not discovered");
            }

            if (commodityUnit.Vendor != null)
            {
                _logger.Info($"BLL.{nameof(CommodityUnitBll)}.{nameof(SetCommodityUnit)}: Vendor discovered");

                commodityUnit.Vendor.Id = _vendorBll.SetVendor(commodityUnit.Vendor);
            }
            else
            {
                _logger.Info($"BLL.{nameof(CommodityUnitBll)}.{nameof(SetCommodityUnit)}: Vendor not discovered");
            }

            if (commodityUnit.Id != null)
            {
                int id = _commodityUnitDao.ChangeCommodityUnit(commodityUnit);

                _logger.Info($"BLL.{nameof(CommodityUnitBll)}.{nameof(SetCommodityUnit)}: Commodity unit id = {id} changed");

                return id;
            }
            else
            {
                int id = _commodityUnitDao.AddCommodityUnit(commodityUnit);

                _logger.Info($"BLL.{nameof(CommodityUnitBll)}.{nameof(SetCommodityUnit)}: Commodity unit id = {id} added");

                return id;
            }
        }

    }
}
