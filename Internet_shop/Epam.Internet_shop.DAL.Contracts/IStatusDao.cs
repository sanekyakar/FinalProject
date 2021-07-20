using System.Collections.Generic;

using Epam.Internet_shop.Common.Entities.CommodityUnit;

namespace Epam.Internet_shop.DAL.Contracts
{
    public interface IStatusDao
    {
        Status GetStatus(int id);

        IEnumerable<Status> GetAllStatuses();

        bool IsStatus(int id);

        int AddStatus(Status status);

        int ChangeStatus(Status status);

        void RemoveStatus(int id);
    }
}
