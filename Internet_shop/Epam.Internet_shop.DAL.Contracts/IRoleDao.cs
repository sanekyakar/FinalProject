using System.Collections.Generic;

using Epam.Internet_shop.Common.Entities.User;

namespace Epam.Internet_shop.DAL.Contracts
{
    public interface IRoleDao
    {
        Role GetRole(int id);

        IEnumerable<Role> GetAllRoles();

        bool IsRole(int id);

        int AddRole(Role role);

        int ChangeRole(Role role);

        void RemoveRole(int id);
    }
}
