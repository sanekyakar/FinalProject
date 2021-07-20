using System.Collections.Generic;
using System.Linq;

using Epam.Internet_shop.Logger.Contracts;
using Epam.Internet_shop.BLL.Contracts;
using Epam.Internet_shop.DAL.Contracts;
using Epam.Internet_shop.Common.Entities.CommodityUnit;

namespace Epam.Internet_shop.BLL
{
    public class StatusBll : IStatusBll
    {
        private readonly ILogger _logger;
        private readonly IStatusDao _statusDao;
        private readonly ICommodityUnitDao _commodityUnitDao;

        public StatusBll(ILogger logger, IStatusDao statusDao, ICommodityUnitDao commodityUnitDao)
        {
            _logger = logger;
            _statusDao = statusDao;
            _commodityUnitDao = commodityUnitDao;
        }

        public IEnumerable<Status> GetAllStatuses()
        {
            _logger.Info($"BLL.{nameof(StatusBll)}.{nameof(GetAllStatuses)}: Getting all statuses");

            foreach (var item in _statusDao.GetAllStatuses())
            {
                yield return item;
            }
        }

        public IEnumerable<StatusWithUnits> GetAllStatusesWithUnits()
        {
            _logger.Info($"BLL.{nameof(StatusBll)}.{nameof(GetAllStatusesWithUnits)}: Getting all statuses with units");

            foreach (var item in _statusDao.GetAllStatuses())
            {
                yield return new StatusWithUnits
                (
                    item.Id,
                    item.Name,
                    _commodityUnitDao.GetCommodityUnitsByStatus(item.Id ?? -1).ToList()
                );
            }

            _logger.Info($"BLL.{nameof(StatusBll)}.{nameof(GetAllStatusesWithUnits)}: Received all statuses with units");

            yield break;
        }

        public Status GetStatusOrNull(int id)
        {
            _logger.Info($"BLL.{nameof(StatusBll)}.{nameof(GetStatusOrNull)}: Getting the status id = " + id);

            if (_statusDao.IsStatus(id))
            {
                _logger.Info($"BLL.{nameof(StatusBll)}.{nameof(GetStatusOrNull)}: Status id = {id} received");

                return _statusDao.GetStatus(id);
            }
            else
            {
                _logger.Warn($"BLL.{nameof(StatusBll)}.{nameof(GetStatusOrNull)}: Status id = {id} not found");

                return null;
            }
        }

        public StatusWithUnits GetStatusWithUnitsOrNull(int id)
        {
            _logger.Info($"BLL.{nameof(StatusBll)}.{nameof(GetStatusWithUnitsOrNull)}: Getting the status with units id = " + id);

            if (_statusDao.IsStatus(id))
            {
                var status = _statusDao.GetStatus(id);

                var UnitList = _commodityUnitDao.GetCommodityUnitsByStatus(id).ToList();

                _logger.Info($"BLL.{nameof(StatusBll)}.{nameof(GetStatusWithUnitsOrNull)}: Status with units id = {id} received");

                return new StatusWithUnits(status.Id, status.Name, UnitList);
            }
            else
            {
                _logger.Warn($"BLL.{nameof(StatusBll)}.{nameof(GetStatusWithUnitsOrNull)}: Status with units id = {id} not found");

                return null;
            }
        }

        public void RemoveStatus(int id)
        {
            _logger.Info($"BLL.{nameof(StatusBll)}.{nameof(RemoveStatus)}: Deleting the status id = " + id);

            if (_statusDao.IsStatus(id))
            {
                _statusDao.RemoveStatus(id);

                _logger.Info($"BLL.{nameof(StatusBll)}.{nameof(RemoveStatus)}: Status removed id = " + id);
            }
            else
            {
                _logger.Warn($"BLL.{nameof(StatusBll)}.{nameof(RemoveStatus)}: Status id = {id} not found");
            }
        }

        public int SetStatus(Status status)
        {
            _logger.Info($"BLL.{nameof(StatusBll)}.{nameof(SetStatus)}: Retention of the status");

            if (status.Id != null)
            {
                int id = _statusDao.ChangeStatus(status);

                _logger.Info($"BLL.{nameof(StatusBll)}.{nameof(SetStatus)}: Status id = {id} changed");

                return id;
            }
            else
            {
                int id = _statusDao.AddStatus(status);

                _logger.Info($"BLL.{nameof(StatusBll)}.{nameof(SetStatus)}: Status id = {id} added");

                return id;
            }
        }
    }
}
