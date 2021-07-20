using Epam.Internet_shop.BLL.Contracts;

namespace Epam.Internet_shop.BLL
{
    public class AuthenticationBll : IAuthenticationBll
    {
        private readonly IUserBll _users;

        public AuthenticationBll(IUserBll user)
        {
            _users = user;
        }

        public bool CheckAuthentication(string login, string password)
        {
            var user = _users.GetUserOrNull(login);

            if (user != null)
            {
                if (user.Password == password)
                {
                    return true;
                }
            }

            return false;
        }

        public bool IsLogin(string login)
        {
            if (_users.GetUserOrNull(login) != null)
            {
                return true;
            }

            return false;
        }
    }
}
