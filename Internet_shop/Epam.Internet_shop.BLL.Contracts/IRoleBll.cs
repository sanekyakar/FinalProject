using System.Collections.Generic;

using Epam.Internet_shop.Common.Entities.User;

namespace Epam.Internet_shop.BLL.Contracts
{
    public interface IRoleBll
    {
        Role GetRoleOrNull(int id);

        IEnumerable<Role> GetAllRoles();

        int SetRole(Role role);

        void RemoveRole(int id);
    }
}
