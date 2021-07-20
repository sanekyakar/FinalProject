using System.Collections.Generic;

using Epam.Internet_shop.Common.Entities.CommodityUnit;

namespace Epam.Internet_shop.DAL.Contracts
{
    public interface ICommodityUnitDao
    {
        CommodityUnit GetCommodityUnit(int id);

        IEnumerable<CommodityUnit> GetAllCommodityUnits();

        IEnumerable<CommodityUnit> GetCommodityUnitsByProduct(int id);
        IEnumerable<CommodityUnit> GetCommodityUnitsByStore(int id);
        IEnumerable<CommodityUnit> GetCommodityUnitsByVendor(int id);
        IEnumerable<CommodityUnit> GetCommodityUnitsByStatus(int id);

        bool IsCommodityUnit(int id);

        int AddCommodityUnit(CommodityUnit commodityUnit);

        int ChangeCommodityUnit(CommodityUnit commodityUnit);

        void RemoveCommodityUnit(int id);
    }
}
