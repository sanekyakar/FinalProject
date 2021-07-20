using System.Collections.Generic;
using System.Linq;

using Epam.Internet_shop.Logger.Contracts;
using Epam.Internet_shop.BLL.Contracts;
using Epam.Internet_shop.DAL.Contracts;
using Epam.Internet_shop.Common.Entities.User;

namespace Epam.Internet_shop.BLL
{
    public class UserBll : IUserBll
    {
        private readonly ILogger _logger;
        private readonly IRoleBll _roleBll;
        private readonly IUserDao _userDao;

        public UserBll(ILogger logger, IRoleBll roleBll, IUserDao userDao)
        {
            _logger = logger;
            _roleBll = roleBll;
            _userDao = userDao;
        }

        public IEnumerable<User> GetAllUsers()
        {
            _logger.Info($"BLL.{nameof(UserBll)}.{nameof(GetAllUsers)}: Getting all users");

            foreach (var item in _userDao.GetAllUsers())
            {
                yield return item;
            }
        }

        public User GetUserOrNull(int id)
        {
            _logger.Info($"BLL.{nameof(UserBll)}.{nameof(GetUserOrNull)}: Getting the user id = " + id);

            if (_userDao.IsUser(id))
            {
                _logger.Info($"BLL.{nameof(UserBll)}.{nameof(GetUserOrNull)}: User id = {id} received");

                return _userDao.GetUser(id);
            }
            else
            {
                _logger.Warn($"BLL.{nameof(UserBll)}.{nameof(GetUserOrNull)}: User id = {id} not found");

                return null;
            }
        }

        public User GetUserOrNull(string login)
        {
            _logger.Info($"BLL.{nameof(UserBll)}.{nameof(GetUserOrNull)}: Getting the user login = " + login);

            User user = _userDao.GetAllUsers().FirstOrDefault(item => item.Login == login);

            if (user != null)
            {
                _logger.Info($"BLL.{nameof(UserBll)}.{nameof(GetUserOrNull)}: User login = {login} received");

                return user;
            }
            else
            {
                _logger.Warn($"BLL.{nameof(UserBll)}.{nameof(GetUserOrNull)}: User login = {login} not found");

                return user;
            }
        }

        public IEnumerable<User> GetUsersByRole(int idRole)
        {
            _logger.Info($"BLL.{nameof(UserBll)}.{nameof(GetUsersByRole)}: Getting users by role id = " + idRole);

            foreach (var item in _userDao.GetUsersByRole(idRole))
            {
                yield return item;
            }
        }

        public IEnumerable<User> GetUsersBySearchString(string searchString)
        {
            _logger.Info($"BLL.{nameof(UserBll)}.{nameof(GetUsersBySearchString)}: Getting users by SearchString = \"{searchString}\"");

            var users = _userDao.GetAllUsers().Where(item => item.Login == searchString || item.Name == searchString);

            _logger.Info($"BLL.{nameof(UserBll)}.{nameof(GetUsersBySearchString)}: Received {users.Count()} users");

            foreach (var item in users)
            {
                yield return item;
            }
        }

        public void RemoveUser(int id)
        {
            _logger.Info($"BLL.{nameof(UserBll)}.{nameof(RemoveUser)}: Deleting the user id = " + id);

            if (_userDao.IsUser(id))
            {
                _userDao.RemoveUser(id);

                _logger.Info($"BLL.{nameof(UserBll)}.{nameof(RemoveUser)}: User removed id = " + id);
            }
            else
            {
                _logger.Warn($"BLL.{nameof(UserBll)}.{nameof(RemoveUser)}: User id = {id} not found");
            }
        }

        public int SetUser(User user)
        {
            _logger.Info($"BLL.{nameof(UserBll)}.{nameof(SetUser)}: Retention of the user");

            if (user.Role != null)
            {
                _logger.Info($"BLL.{nameof(UserBll)}.{nameof(SetUser)}: Role discovered");

                user.Role.Id = _roleBll.SetRole(user.Role);
            }
            else
            {
                _logger.Info($"BLL.{nameof(UserBll)}.{nameof(SetUser)}: Role not discovered");
            }

            if (user.Id != null)
            {
                int id = _userDao.ChangeUser(user);

                _logger.Info($"BLL.{nameof(UserBll)}.{nameof(SetUser)}: User id = {id} changed");

                return id;
            }
            else
            {
                int id = _userDao.AddUser(user);

                _logger.Info($"BLL.{nameof(UserBll)}.{nameof(SetUser)}: User id = {id} added");

                return id;
            }
        }
    }
}
