using System.Collections.Generic;

using Epam.Internet_shop.Common.Entities.CommodityUnit;

namespace Epam.Internet_shop.DAL.Contracts
{
    public interface ICategoryDao
    {
        Category GetCategory(int id);

        IEnumerable<Category> GetAllCategories();

        bool IsCategory(int id);

        int AddCategory(Category category);

        int ChangeCategory(Category category);

        void RemoveCategory(int id);
    }
}
