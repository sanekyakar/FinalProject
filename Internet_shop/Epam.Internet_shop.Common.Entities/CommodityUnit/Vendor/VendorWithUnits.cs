using System.Collections.Generic;

namespace Epam.Internet_shop.Common.Entities.CommodityUnit
{
    public class VendorWithUnits : Vendor
    {
        public List<CommodityUnit> Units { get; }

        public VendorWithUnits(int? id, string name, List<CommodityUnit> units) : base(id, name)
        {
            Units = units;
        }
    }
}
