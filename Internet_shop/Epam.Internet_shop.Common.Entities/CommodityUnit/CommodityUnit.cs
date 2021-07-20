namespace Epam.Internet_shop.Common.Entities.CommodityUnit
{
    public class CommodityUnit
    {
        public int? Id { get; }

        public Product Product { get; set; }

        public Store Store { get; set; }

        public Vendor Vendor { get; set; }

        public Status Status { get; set; }

        public decimal Price { get; set; }

        public CommodityUnit(int? id, Product product, Store store, Vendor vendor, Status status, decimal price)
        {
            Id = id;
            Product = product;
            Store = store;
            Vendor = vendor;
            Status = status;
            Price = price;
        }
    }
}
