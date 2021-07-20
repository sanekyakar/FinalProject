namespace Epam.Internet_shop.Common.Entities.User
{
    public class User
    {
        public int? Id { get; set; }

        public string Login { get; set; }

        public string Password { get; set; }

        public Role Role { get; set; }

        public string Name { get; set; }

        public User(int? id, string login, string password, Role role, string name)
        {
            Id = id;
            Login = login;
            Password = password;
            Role = role;
            Name = name;
        }
    }
}
