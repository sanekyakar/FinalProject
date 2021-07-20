using System.Collections.Generic;

using Epam.Internet_shop.Common.Entities.CommodityUnit;

namespace Epam.Internet_shop.BLL.Contracts
{
    public interface ICommodityUnitBll
    {
        CommodityUnit GetCommodityUnitOrNull(int id);

        IEnumerable<CommodityUnit> GetAllCommodityUnits();

        IEnumerable<CommodityUnit> GetCommodityUnitsByProduct(int id);

        IEnumerable<CommodityUnit> GetCommodityUnitsByCategory(int id);

        IEnumerable<CommodityUnit> GetCommodityUnitsByStore(int id);

        IEnumerable<CommodityUnit> GetCommodityUnitsByVendor(int id);

        IEnumerable<CommodityUnit> GetCommodityUnitsByStatus(int id);

        int SetCommodityUnit(CommodityUnit commodityUnit);
    }
}
