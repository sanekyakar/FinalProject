using System.Collections.Generic;

using Epam.Internet_shop.Common.Entities.CommodityUnit;

namespace Epam.Internet_shop.DAL.Contracts
{
    public interface IVendorDao
    {
        Vendor GetVendor(int id);

        IEnumerable<Vendor> GetAllVendors();

        bool IsVendor(int id);

        int AddVendor(Vendor vendor);

        int ChangeVendor(Vendor vendor);

        void RemoveVendor(int id);
    }
}
