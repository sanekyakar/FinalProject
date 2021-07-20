using System.Collections.Generic;

using Epam.Internet_shop.Common.Entities.CommodityUnit;

namespace Epam.Internet_shop.BLL.Contracts
{
    public interface IStatusBll
    {
        Status GetStatusOrNull(int id);

        StatusWithUnits GetStatusWithUnitsOrNull(int id);

        IEnumerable<Status> GetAllStatuses();

        IEnumerable<StatusWithUnits> GetAllStatusesWithUnits();

        int SetStatus(Status status);

        void RemoveStatus(int id);
    }
}
