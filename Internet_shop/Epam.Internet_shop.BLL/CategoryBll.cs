using System.Collections.Generic;
using System.Linq;

using Epam.Internet_shop.Logger.Contracts;
using Epam.Internet_shop.BLL.Contracts;
using Epam.Internet_shop.DAL.Contracts;
using Epam.Internet_shop.Common.Entities.CommodityUnit;

namespace Epam.Internet_shop.BLL
{
    public class CategoryBll : ICategoryBll
    {
        private readonly ILogger _logger;
        private readonly ICategoryDao _categoryDao;
        private readonly ICommodityUnitDao _commodityUnitDao;

        public CategoryBll(ILogger logger, ICategoryDao categoryDao, ICommodityUnitDao commodityUnitDao)
        {
            _logger = logger;
            _categoryDao = categoryDao;
            _commodityUnitDao = commodityUnitDao;
        }

        public IEnumerable<Category> GetAllCategories()
        {
            _logger.Info($"BLL.{nameof(CategoryBll)}.{nameof(GetAllCategories)}: Getting all categories");

            foreach (var item in _categoryDao.GetAllCategories())
            {
                yield return item;
            }
        }

        public IEnumerable<CategoryWithUnits> GetAllCategoriesWithUnits()
        {
            _logger.Info($"BLL.{nameof(CategoryBll)}.{nameof(GetAllCategoriesWithUnits)}: Getting all categories with units");

            foreach (var item in _categoryDao.GetAllCategories())
            {
                yield return new CategoryWithUnits
                (
                    item.Id,
                    item.Name,
                    _commodityUnitDao.GetAllCommodityUnits()
                        .Where(unit => unit.Product is null ? false : unit.Product.Category.Id == item.Id)
                        .ToList()
                );
            }

            _logger.Info($"BLL.{nameof(CategoryBll)}.{nameof(GetAllCategoriesWithUnits)}: Received all categories with units");

            yield break;
        }

        public Category GetCategoryOrNull(int id)
        {
            _logger.Info($"BLL.{nameof(CategoryBll)}.{nameof(GetCategoryOrNull)}: Getting the category id = " + id);

            if (_categoryDao.IsCategory(id))
            {
                _logger.Info($"BLL.{nameof(CategoryBll)}.{nameof(GetCategoryOrNull)}: Category id = {id} received");

                return _categoryDao.GetCategory(id);
            }
            else
            {
                _logger.Warn($"BLL.{nameof(CategoryBll)}.{nameof(GetCategoryOrNull)}: Category id = {id} not found");

                return null;
            }
        }

        public CategoryWithUnits GetCategoryWithUnitsOrNull(int id)
        {
            _logger.Info($"BLL.{nameof(CategoryBll)}.{nameof(GetCategoryWithUnitsOrNull)}: Getting the category with units id = " + id);

            if (_categoryDao.IsCategory(id))
            {
                var category = _categoryDao.GetCategory(id);

                var UnitList = _commodityUnitDao.GetAllCommodityUnits()
                               .Where(unit => unit.Product is null
                                       ? false : unit.Product.Category is null
                                       ? false : unit.Product.Category.Id == id)
                               .ToList();

                _logger.Info($"BLL.{nameof(CategoryBll)}.{nameof(GetCategoryWithUnitsOrNull)}: Category with units id = {id} received");

                return new CategoryWithUnits(category.Id, category.Name, UnitList);
            }
            else
            {
                _logger.Warn($"BLL.{nameof(CategoryBll)}.{nameof(GetCategoryWithUnitsOrNull)}: Category with units id = {id} not found");

                return null;
            }
        }

        public void RemoveCategory(int id)
        {
            _logger.Info($"BLL.{nameof(CategoryBll)}.{nameof(RemoveCategory)}: Deleting the category id = " + id);

            if (_categoryDao.IsCategory(id))
            {
                _categoryDao.RemoveCategory(id);

                _logger.Info($"BLL.{nameof(CategoryBll)}.{nameof(RemoveCategory)}: Category removed id = " + id);
            }
            else
            {
                _logger.Warn($"BLL.{nameof(CategoryBll)}.{nameof(RemoveCategory)}: Category id = {id} not found");
            }
        }

        public int SetCategory(Category category)
        {
            _logger.Info($"BLL.{nameof(CategoryBll)}.{nameof(SetCategory)}: Retention of the category");

            if (category.Id != null)
            {
                int id = _categoryDao.ChangeCategory(category);

                _logger.Info($"BLL.{nameof(CategoryBll)}.{nameof(SetCategory)}: Category id = {id} changed");

                return id;
            }
            else
            {
                int id = _categoryDao.AddCategory(category);

                _logger.Info($"BLL.{nameof(CategoryBll)}.{nameof(SetCategory)}: Category id = {id} added");

                return id;
            }
        }
    }
}
