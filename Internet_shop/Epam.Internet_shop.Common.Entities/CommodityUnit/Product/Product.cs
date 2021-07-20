namespace Epam.Internet_shop.Common.Entities.CommodityUnit
{
    public class Product
    {
        public int? Id { get; set; }

        public string Name { get; set; }

        public string ImageInBase64Src { get; set; }

        public string Discription { get; set; }

        public Category Category { get; set; }

        public Product(int? id, string name, string imageInBase64Src, string discription, Category category)
        {
            Id = id;
            Name = name;
            ImageInBase64Src = imageInBase64Src;
            Discription = discription;
            Category = category;
        }
    }
}
