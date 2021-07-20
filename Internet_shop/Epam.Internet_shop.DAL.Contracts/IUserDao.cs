using System.Collections.Generic;

using Epam.Internet_shop.Common.Entities.User;

namespace Epam.Internet_shop.DAL.Contracts
{
    public interface IUserDao
    {
        User GetUser(int id);

        IEnumerable<User> GetAllUsers();

        IEnumerable<User> GetUsersByRole(int idRole);

        bool IsUser(int id);

        int AddUser(User user);

        int ChangeUser(User user);

        void RemoveUser(int id);
    }
}
