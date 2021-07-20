using System.Collections.Generic;

using Epam.Internet_shop.Common.Entities.CommodityUnit;

namespace Epam.Internet_shop.BLL.Contracts
{
    public interface IStoreBll
    {
        Store GetStoreOrNull(int id);

        StoreWithUnits GetStoreWithUnitsOrNull(int id);

        IEnumerable<Store> GetAllStores();

        IEnumerable<StoreWithUnits> GetAllStoresWithUnits();

        int SetStore(Store store);

        void RemoveStore(int id);
    }
}
