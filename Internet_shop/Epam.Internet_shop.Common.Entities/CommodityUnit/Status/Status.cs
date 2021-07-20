namespace Epam.Internet_shop.Common.Entities.CommodityUnit
{
    public class Status
    {
        public int? Id { get; set; }

        public string Name { get; set; }

        public Status(int? id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
