using System.Collections.Generic;

using Epam.Internet_shop.Common.Entities.CommodityUnit;

namespace Epam.Internet_shop.BLL.Contracts
{
    public interface IVendorBll
    {
        Vendor GetVendorOrNull(int id);

        VendorWithUnits GetVendorWithUnitsOrNull(int id);

        IEnumerable<Vendor> GetAllVendors();

        IEnumerable<VendorWithUnits> GetAllVendorsWithUnits();

        int SetVendor(Vendor vendor);

        void RemoveVendor(int id);
    }
}
