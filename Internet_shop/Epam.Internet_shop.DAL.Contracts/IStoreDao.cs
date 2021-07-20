using System.Collections.Generic;

using Epam.Internet_shop.Common.Entities.CommodityUnit;

namespace Epam.Internet_shop.DAL.Contracts
{
    public interface IStoreDao
    {
        Store GetStore(int id);

        IEnumerable<Store> GetAllStores();

        bool IsStore(int id);

        int AddStore(Store store);

        int ChangeStore(Store store);

        void RemoveStore(int id);
    }
}
