namespace Epam.Internet_shop.BLL.Contracts
{
    public interface IAuthenticationBll
    {
        bool CheckAuthentication(string login, string password);

        bool IsLogin(string login);
    }
}
