using System.Collections.Generic;

using Epam.Internet_shop.Common.Entities.CommodityUnit;

namespace Epam.Internet_shop.BLL.Contracts
{
    public interface ICategoryBll
    {
        Category GetCategoryOrNull(int id);

        CategoryWithUnits GetCategoryWithUnitsOrNull(int id);

        IEnumerable<Category> GetAllCategories();

        IEnumerable<CategoryWithUnits> GetAllCategoriesWithUnits();

        int SetCategory(Category category);

        void RemoveCategory(int id);
    }
}
