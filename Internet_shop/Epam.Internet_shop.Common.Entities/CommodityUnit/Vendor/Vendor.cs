namespace Epam.Internet_shop.Common.Entities.CommodityUnit
{
    public class Vendor
    {
        public int? Id { get; set; }

        public string Name { get; set; }

        public Vendor(int? id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
