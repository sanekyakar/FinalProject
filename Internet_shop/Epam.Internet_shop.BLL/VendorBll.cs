using System.Collections.Generic;
using System.Linq;

using Epam.Internet_shop.Logger.Contracts;
using Epam.Internet_shop.BLL.Contracts;
using Epam.Internet_shop.DAL.Contracts;
using Epam.Internet_shop.Common.Entities.CommodityUnit;

namespace Epam.Internet_shop.BLL
{
    public class VendorBll : IVendorBll
    {
        private readonly ILogger _logger;
        private readonly IVendorDao _vendorDao;
        private readonly ICommodityUnitDao _commodityUnitDao;

        public VendorBll(ILogger logger, IVendorDao vendorDao, ICommodityUnitDao commodityUnitDao)
        {
            _logger = logger;
            _vendorDao = vendorDao;
            _commodityUnitDao = commodityUnitDao;
        }

        public IEnumerable<Vendor> GetAllVendors()
        {
            _logger.Info($"BLL.{nameof(VendorBll)}.{nameof(GetAllVendors)}: Getting all vendors");

            foreach (var item in _vendorDao.GetAllVendors())
            {
                yield return item;
            }
        }

        public IEnumerable<VendorWithUnits> GetAllVendorsWithUnits()
        {
            _logger.Info($"BLL.{nameof(VendorBll)}.{nameof(GetAllVendorsWithUnits)}: Getting all vendors with units");

            foreach (var item in _vendorDao.GetAllVendors())
            {
                yield return new VendorWithUnits
                (
                    item.Id,
                    item.Name,
                    _commodityUnitDao.GetCommodityUnitsByVendor(item.Id ?? -1).ToList()
                );
            }

            _logger.Info($"BLL.{nameof(VendorBll)}.{nameof(GetAllVendorsWithUnits)}: Received all vendors with units");

            yield break;
        }

        public Vendor GetVendorOrNull(int id)
        {
            _logger.Info($"BLL.{nameof(VendorBll)}.{nameof(GetVendorOrNull)}: Getting the vendor id = " + id);

            if (_vendorDao.IsVendor(id))
            {
                _logger.Info($"BLL.{nameof(VendorBll)}.{nameof(GetVendorOrNull)}: Vendor id = {id} received");

                return _vendorDao.GetVendor(id);
            }
            else
            {
                _logger.Warn($"BLL.{nameof(VendorBll)}.{nameof(GetVendorOrNull)}: Vendor id = {id} not found");

                return null;
            }
        }

        public VendorWithUnits GetVendorWithUnitsOrNull(int id)
        {
            _logger.Info($"BLL.{nameof(VendorBll)}.{nameof(GetVendorWithUnitsOrNull)}: Getting the vendor with units id = " + id);

            if (_vendorDao.IsVendor(id))
            {
                var status = _vendorDao.GetVendor(id);

                var UnitList = _commodityUnitDao.GetCommodityUnitsByVendor(id).ToList();

                _logger.Info($"BLL.{nameof(VendorBll)}.{nameof(GetVendorWithUnitsOrNull)}: Vendor with units id = {id} received");

                return new VendorWithUnits(status.Id, status.Name, UnitList);
            }
            else
            {
                _logger.Warn($"BLL.{nameof(VendorBll)}.{nameof(GetVendorWithUnitsOrNull)}: Vendor with units id = {id} not found");

                return null;
            }
        }

        public void RemoveVendor(int id)
        {
            _logger.Info($"BLL.{nameof(VendorBll)}.{nameof(RemoveVendor)}: Deleting the vendor id = " + id);

            if (_vendorDao.IsVendor(id))
            {
                _vendorDao.RemoveVendor(id);

                _logger.Info($"BLL.{nameof(VendorBll)}.{nameof(RemoveVendor)}: Vendor removed id = " + id);
            }
            else
            {
                _logger.Warn($"BLL.{nameof(VendorBll)}.{nameof(RemoveVendor)}: Vendor id = {id} not found");
            }
        }

        public int SetVendor(Vendor vendor)
        {
            _logger.Info($"BLL.{nameof(VendorBll)}.{nameof(SetVendor)}: Retention of the vendor");

            if (vendor.Id != null)
            {
                int id = _vendorDao.ChangeVendor(vendor);

                _logger.Info($"BLL.{nameof(VendorBll)}.{nameof(SetVendor)}: Vendor id = {id} changed");

                return id;
            }
            else
            {
                int id = _vendorDao.AddVendor(vendor);

                _logger.Info($"BLL.{nameof(VendorBll)}.{nameof(SetVendor)}: Vendor id = {id} added");

                return id;
            }
        }
    }
}
