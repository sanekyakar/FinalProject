using System.Collections.Generic;

using Epam.Internet_shop.Common.Entities.User;

namespace Epam.Internet_shop.BLL.Contracts
{
    public interface IUserBll
    {
        User GetUserOrNull(int id);
        User GetUserOrNull(string login);

        IEnumerable<User> GetAllUsers();

        IEnumerable<User> GetUsersByRole(int idRole);

        IEnumerable<User> GetUsersBySearchString(string searchString);

        int SetUser(User user);

        void RemoveUser(int id);
    }
}
