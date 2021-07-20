using Epam.Internet_shop.Logger.Log4net;
using Epam.Internet_shop.Logger.Contracts;
using Epam.Internet_shop.DAL.Contracts;
using Epam.Internet_shop.DAL.DataBase;
using Epam.Internet_shop.BLL.Contracts;
using Epam.Internet_shop.BLL;

namespace Epam.Internet_shop.Common
{
    public static class DependencyResolver
    {
        public static ILogger Logger { get; }

        public static IRoleDao RoleDao { get; }
        public static IUserDao UserDao { get; }
        public static ICategoryDao CategoryDao { get; }
        public static IProductDao ProductDao { get; }
        public static IStoreDao StoreDao { get; }
        public static IVendorDao VendorDao { get; }
        public static IStatusDao StatusDao { get; }
        public static ICommodityUnitDao CommodityUnitDao { get; }

        public static IRoleBll RoleBll { get; }
        public static IUserBll UserBll { get; }
        public static ICategoryBll CategoryBll { get; }
        public static IProductBll ProductBll { get; }
        public static IStoreBll StoreBll { get; }
        public static IVendorBll VendorBll { get; }
        public static IStatusBll StatusBll { get; }
        public static ICommodityUnitBll CommodityUnitBll { get; }
        public static IAuthenticationBll AuthenticationBll { get; }

        static DependencyResolver()
        {
            Logger = new Log4net();

            RoleDao = new RoleDao(Logger);
            UserDao = new UserDao(Logger, RoleDao);
            CategoryDao = new CategoryDao(Logger);
            ProductDao = new ProductDao(Logger, CategoryDao);
            StoreDao = new StoreDao(Logger);
            VendorDao = new VendorDao(Logger);
            StatusDao = new StatusDao(Logger);
            CommodityUnitDao = new CommodityUnitDao(Logger, ProductDao, StatusDao, StoreDao, VendorDao);

            RoleBll = new RoleBll(Logger, RoleDao);
            UserBll = new UserBll(Logger, RoleBll, UserDao);
            CategoryBll = new CategoryBll(Logger, CategoryDao, CommodityUnitDao);
            ProductBll = new ProductBll(Logger, CategoryBll, ProductDao, CommodityUnitDao);
            StoreBll = new StoreBll(Logger, StoreDao, CommodityUnitDao);
            VendorBll = new VendorBll(Logger, VendorDao, CommodityUnitDao);
            StatusBll = new StatusBll(Logger, StatusDao, CommodityUnitDao);
            CommodityUnitBll = new CommodityUnitBll(Logger, CategoryBll, ProductBll, StatusBll, StoreBll, VendorBll, CommodityUnitDao);

            AuthenticationBll = new AuthenticationBll(UserBll);
        }
    }
}
