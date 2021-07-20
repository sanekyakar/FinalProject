namespace Epam.Internet_shop.Common.Entities.CommodityUnit
{
    public class Store
    {
        public int? Id { get; set; }

        public string Name { get; set; }

        public Store(int? id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
