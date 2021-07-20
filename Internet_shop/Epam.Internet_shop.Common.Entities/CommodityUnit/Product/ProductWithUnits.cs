using System.Collections.Generic;

namespace Epam.Internet_shop.Common.Entities.CommodityUnit
{
    public class ProductWithUnits : Product
    {
        public List<CommodityUnit> Units { get; }

        public ProductWithUnits(int? id, string name, string image, string discription, Category category, List<CommodityUnit> units) 
                     : base(id, name, image, discription, category)
        {
            Units = units;
        }
    }
}
