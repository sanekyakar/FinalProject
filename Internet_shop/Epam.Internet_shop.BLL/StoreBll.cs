using System.Collections.Generic;
using System.Linq;

using Epam.Internet_shop.Logger.Contracts;
using Epam.Internet_shop.BLL.Contracts;
using Epam.Internet_shop.DAL.Contracts;
using Epam.Internet_shop.Common.Entities.CommodityUnit;

namespace Epam.Internet_shop.BLL
{
    public class StoreBll : IStoreBll
    {
        private readonly ILogger _logger;
        private readonly IStoreDao _storeDao;
        private readonly ICommodityUnitDao _commodityUnitDao;

        public StoreBll(ILogger logger, IStoreDao storeDao, ICommodityUnitDao commodityUnitDao)
        {
            _logger = logger;
            _storeDao = storeDao;
            _commodityUnitDao = commodityUnitDao;
        }

        public IEnumerable<Store> GetAllStores()
        {
            _logger.Info($"BLL.{nameof(StoreBll)}.{nameof(GetAllStores)}: Getting all stores");

            foreach (var item in _storeDao.GetAllStores())
            {
                yield return item;
            }
        }

        public IEnumerable<StoreWithUnits> GetAllStoresWithUnits()
        {
            _logger.Info($"BLL.{nameof(StoreBll)}.{nameof(GetAllStoresWithUnits)}: Getting all stores with units");

            foreach (var item in _storeDao.GetAllStores())
            {
                yield return new StoreWithUnits
                (
                    item.Id,
                    item.Name,
                    _commodityUnitDao.GetCommodityUnitsByStore(item.Id ?? -1).ToList()
                );
            }

            _logger.Info($"BLL.{nameof(StoreBll)}.{nameof(GetAllStoresWithUnits)}: Received all stores with units");

            yield break;
        }

        public Store GetStoreOrNull(int id)
        {
            _logger.Info($"BLL.{nameof(StoreBll)}.{nameof(GetStoreOrNull)}: Getting the store id = " + id);

            if (_storeDao.IsStore(id))
            {
                _logger.Info($"BLL.{nameof(StoreBll)}.{nameof(GetStoreOrNull)}: Store id = {id} received");

                return _storeDao.GetStore(id);
            }
            else
            {
                _logger.Warn($"BLL.{nameof(StoreBll)}.{nameof(GetStoreOrNull)}: Store id = {id} not found");

                return null;
            }
        }

        public StoreWithUnits GetStoreWithUnitsOrNull(int id)
        {
            _logger.Info($"BLL.{nameof(StoreBll)}.{nameof(GetStoreWithUnitsOrNull)}: Getting the store with units id = " + id);

            if (_storeDao.IsStore(id))
            {
                var store = _storeDao.GetStore(id);

                var UnitList = _commodityUnitDao.GetCommodityUnitsByStore(id).ToList();

                _logger.Info($"BLL.{nameof(StoreBll)}.{nameof(GetStoreWithUnitsOrNull)}: Store with units id = {id} received");

                return new StoreWithUnits(store.Id, store.Name, UnitList);
            }
            else
            {
                _logger.Warn($"BLL.{nameof(StoreBll)}.{nameof(GetStoreWithUnitsOrNull)}: Store with units id = {id} not found");

                return null;
            }
        }

        public void RemoveStore(int id)
        {
            _logger.Info($"BLL.{nameof(StoreBll)}.{nameof(RemoveStore)}: Deleting the store id = " + id);

            if (_storeDao.IsStore(id))
            {
                _storeDao.RemoveStore(id);

                _logger.Info($"BLL.{nameof(StoreBll)}.{nameof(RemoveStore)}: Store removed id = " + id);
            }
            else
            {
                _logger.Warn($"BLL.{nameof(StoreBll)}.{nameof(RemoveStore)}: Store id = {id} not found");
            }
        }

        public int SetStore(Store store)
        {
            _logger.Info($"BLL.{nameof(StoreBll)}.{nameof(SetStore)}: Retention of the store");

            if (store.Id != null)
            {
                int id = _storeDao.ChangeStore(store);

                _logger.Info($"BLL.{nameof(StoreBll)}.{nameof(SetStore)}: Store id = {id} changed");

                return id;
            }
            else
            {
                int id = _storeDao.AddStore(store);

                _logger.Info($"BLL.{nameof(StoreBll)}.{nameof(SetStore)}: Store id = {id} added");

                return id;
            }
        }
    }
}
