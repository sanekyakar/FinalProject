using System.Collections.Generic;

using Epam.Internet_shop.Logger.Contracts;
using Epam.Internet_shop.BLL.Contracts;
using Epam.Internet_shop.DAL.Contracts;
using Epam.Internet_shop.Common.Entities.User;

namespace Epam.Internet_shop.BLL
{
    public class RoleBll : IRoleBll
    {
        private readonly ILogger _logger;
        private readonly IRoleDao _roleDao;

        public RoleBll(ILogger logger, IRoleDao roleDao)
        {
            _logger = logger;
            _roleDao = roleDao;
        }

        public IEnumerable<Role> GetAllRoles()
        {
            _logger.Info($"BLL.{nameof(RoleBll)}.{nameof(GetAllRoles)}: Getting all roles");

            foreach (var item in _roleDao.GetAllRoles())
            {
                yield return item;
            }
        }

        public Role GetRoleOrNull(int id)
        {
            _logger.Info($"BLL.{nameof(RoleBll)}.{nameof(GetRoleOrNull)}: Getting the role id = " + id);

            if (_roleDao.IsRole(id))
            {
                _logger.Info($"BLL.{nameof(RoleBll)}.{nameof(GetRoleOrNull)}: Role id = {id} received");

                return _roleDao.GetRole(id);
            }
            else
            {
                _logger.Warn($"BLL.{nameof(RoleBll)}.{nameof(GetRoleOrNull)}: Role id = {id} not found");

                return null;
            }
        }

        public void RemoveRole(int id)
        {
            _logger.Info($"BLL.{nameof(RoleBll)}.{nameof(RemoveRole)}: Deleting the role id = " + id);

            if (_roleDao.IsRole(id))
            {
                _roleDao.RemoveRole(id);

                _logger.Info($"BLL.{nameof(RoleBll)}.{nameof(RemoveRole)}: Role removed id = " + id);
            }
            else
            {
                _logger.Warn($"BLL.{nameof(RoleBll)}.{nameof(RemoveRole)}: Role id = {id} not found");
            }
        }

        public int SetRole(Role role)
        {
            _logger.Info($"BLL.{nameof(RoleBll)}.{nameof(SetRole)}: Retention of the role");

            if (role.Id != null)
            {
                int id =_roleDao.ChangeRole(role);

                _logger.Info($"BLL.{nameof(RoleBll)}.{nameof(SetRole)}: Role id = {id} changed");

                return id;
            }
            else
            {
                int id = _roleDao.AddRole(role);

                _logger.Info($"BLL.{nameof(RoleBll)}.{nameof(SetRole)}: Role id = {id} added");

                return id;
            }
        }
    }
}
