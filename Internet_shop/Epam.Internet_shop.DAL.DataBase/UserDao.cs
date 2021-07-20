using System;
using System.Configuration;
using System.Collections.Generic;
using System.Data.SqlClient;

using Epam.Internet_shop.Logger.Contracts;
using Epam.Internet_shop.DAL.Contracts;
using Epam.Internet_shop.Common.Entities.User;

namespace Epam.Internet_shop.DAL.DataBase
{
    public class UserDao : IUserDao
    {
        private readonly ILogger _logger;
        private readonly IRoleDao _roleDao;
        private readonly string _connectionString;

        public UserDao(ILogger logger, IRoleDao roleDao)
        {
            _logger = logger;
            _roleDao = roleDao;
            _connectionString = ConfigurationManager.ConnectionStrings["default"].ConnectionString;
        }

        public int AddUser(User user)
        {
            _logger.Info($"DAL.{nameof(UserDao)}.{nameof(AddUser)}: Adding a user");

            int id;

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    SqlCommand command = new SqlCommand("dbo.User_AddUser", connection)
                    {
                        CommandType = System.Data.CommandType.StoredProcedure
                    };

                    int? RoleID = null;

                    RoleID = user?.Role.Id;

                    command.Parameters.AddWithValue("@Login", user.Login);
                    command.Parameters.AddWithValue("@Password", user.Password);
                    command.Parameters.AddWithValue("@RoleID", RoleID);
                    command.Parameters.AddWithValue("@Name", user.Name);

                    connection.Open();

                    _logger.Info($"DAL.{nameof(UserDao)}.{nameof(AddUser)}: Connected to database");

                    int.TryParse(command.ExecuteScalar().ToString(), out id);
                }
            }
            catch (InvalidOperationException ex)
            {
                _logger.Error($"DAL.{nameof(UserDao)}.{nameof(AddUser)}: Not connected to database: " + ex.Message);

                throw new SystemException("Connection error", ex);
            }

            _logger.Info($"DAL.{nameof(UserDao)}.{nameof(AddUser)}: User id = {id} added");

            return id;
        }

        public int ChangeUser(User user)
        {
            _logger.Info($"DAL.{nameof(UserDao)}.{nameof(ChangeUser)}: User change");

            int id = user.Id ?? -1;

            if (id == -1)
            {
                _logger.Error($"DAL.{nameof(UserDao)}.{nameof(ChangeUser)}: user.Id cannot be null");

                throw new ArgumentNullException("user.Id cannot be null");
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    SqlCommand command = new SqlCommand("dbo.User_ChangeUser", connection)
                    {
                        CommandType = System.Data.CommandType.StoredProcedure
                    };

                    command.Parameters.AddWithValue("@Id", user.Id);
                    command.Parameters.AddWithValue("@Login", user.Login);
                    command.Parameters.AddWithValue("@Password", user.Password);
                    command.Parameters.AddWithValue("@RoleID", user.Role.Id);
                    command.Parameters.AddWithValue("@Name", user.Name);

                    connection.Open();

                    _logger.Info($"DAL.{nameof(UserDao)}.{nameof(ChangeUser)}: Connected to database");

                    command.ExecuteScalar();
                }
            }
            catch (InvalidOperationException ex)
            {
                _logger.Error($"DAL.{nameof(UserDao)}.{nameof(ChangeUser)}: Not connected to database: " + ex.Message);

                throw new SystemException("Connection error", ex);
            }

            _logger.Info($"DAL.{nameof(UserDao)}.{nameof(ChangeUser)}: User id = {id} changed");

            return id;
        }

        public IEnumerable<User> GetAllUsers()
        {
            _logger.Info($"DAL.{nameof(UserDao)}.{nameof(GetAllUsers)}: Getting all users");

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand("dbo.User_GetAllUsers", connection)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };

                SqlDataReader reader;

                try
                {
                    connection.Open();

                    reader = command.ExecuteReader();

                    _logger.Info($"DAL.{nameof(UserDao)}.{nameof(GetAllUsers)}: Connected to database");
                }
                catch (InvalidOperationException ex)
                {
                    _logger.Error($"DAL.{nameof(UserDao)}.{nameof(GetAllUsers)}: Not connected to database: " + ex.Message);

                    throw new SystemException("Connection error", ex);
                }

                int? RoleID;

                while (reader.Read())
                {
                    RoleID = (int?)reader["RoleID"];

                    yield return new User(
                        (int?)reader["Id"], 
                        (string)reader["Login"], 
                        (string)reader["Password"],
                        RoleID is null ? null : _roleDao.GetRole(RoleID ?? -1),
                        (string)reader["Name"]);
                }
            }

            _logger.Info($"DAL.{nameof(UserDao)}.{nameof(GetAllUsers)}: All users received");

            yield break;
        }

        public User GetUser(int id)
        {
            _logger.Info($"DAL.{nameof(UserDao)}.{nameof(GetUser)}: Getting user by id = {id}");

            User user;

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand("dbo.User_GetUser", connection)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };

                command.Parameters.AddWithValue("@Id", id);

                SqlDataReader reader;

                try
                {
                    connection.Open();

                    reader = command.ExecuteReader();

                    _logger.Info($"DAL.{nameof(UserDao)}.{nameof(GetUser)}: Connected to database");
                }
                catch (InvalidOperationException ex)
                {
                    _logger.Error($"DAL.{nameof(UserDao)}.{nameof(GetUser)}: Not connected to database: " + ex.Message);

                    throw new SystemException("Connection error", ex);
                }

                reader.Read();

                int? RoleID = (int?)reader["RoleID"];

                user = new User(
                    (int?)reader["Id"],
                    (string)reader["Login"],
                    (string)reader["Password"],
                    RoleID is null ? null : _roleDao.GetRole(RoleID ?? -1),
                    (string)reader["Name"]);
            }

            _logger.Info($"DAL.{nameof(RoleDao)}.{nameof(GetUser)}: User by id = {id} obtained");

            return user;
        }

        public IEnumerable<User> GetUsersByRole(int idRole)
        {
            _logger.Info($"DAL.{nameof(UserDao)}.{nameof(GetUsersByRole)}: Getting users by role");

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand("dbo.User_GetUsersByRole", connection)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };

                command.Parameters.AddWithValue("@Id", idRole);

                SqlDataReader reader;

                try
                {
                    connection.Open();

                    reader = command.ExecuteReader();

                    _logger.Info($"DAL.{nameof(UserDao)}.{nameof(GetUsersByRole)}: Connected to database");
                }
                catch (InvalidOperationException ex)
                {
                    _logger.Error($"DAL.{nameof(UserDao)}.{nameof(GetUsersByRole)}: Not connected to database: " + ex.Message);

                    throw new SystemException("Connection error", ex);
                }

                while (reader.Read())
                {
                    yield return new User(
                        (int?)reader["Id"],
                        (string)reader["Login"],
                        (string)reader["Password"],
                        _roleDao.GetRole(idRole),
                        (string)reader["Name"]);
                }
            }

            _logger.Info($"DAL.{nameof(UserDao)}.{nameof(GetUsersByRole)}: All users received");

            yield break;
        }

        public bool IsUser(int id)
        {
            _logger.Info($"DAL.{nameof(UserDao)}.{nameof(IsUser)}: Checking the user by id = {id}");

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand("dbo.User_GetUser", connection)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };

                command.Parameters.AddWithValue("@Id", id);

                SqlDataReader reader;

                try
                {
                    connection.Open();

                    reader = command.ExecuteReader();

                    _logger.Info($"DAL.{nameof(UserDao)}.{nameof(IsUser)}: Connected to database");
                }
                catch (InvalidOperationException ex)
                {
                    _logger.Error($"DAL.{nameof(UserDao)}.{nameof(IsUser)}: Not connected to database: " + ex.Message);

                    throw new SystemException("Connection error", ex);
                }

                if (reader.Read())
                {
                    _logger.Info($"DAL.{nameof(RoleDao)}.{nameof(IsUser)}: User by id = {id} found");

                    return true;
                }

                _logger.Info($"DAL.{nameof(RoleDao)}.{nameof(IsUser)}: User by id = {id} not found");

                return false;
            }
        }

        public void RemoveUser(int id)
        {
            _logger.Info($"DAL.{nameof(UserDao)}.{nameof(RemoveUser)}: Remove user by id = {id}");

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    SqlCommand command = new SqlCommand("dbo.User_RemoveUser", connection)
                    {
                        CommandType = System.Data.CommandType.StoredProcedure
                    };

                    command.Parameters.AddWithValue("@Id", id);

                    connection.Open();

                    _logger.Info($"DAL.{nameof(UserDao)}.{nameof(RemoveUser)}: Connected to database");

                    command.ExecuteNonQuery();

                    _logger.Info($"DAL.{nameof(UserDao)}.{nameof(RemoveUser)}: User id = {id} removed");
                }
            }
            catch (InvalidOperationException ex)
            {
                _logger.Error($"DAL.{nameof(UserDao)}.{nameof(RemoveUser)}: Not connected to database: " + ex.Message);

                throw new SystemException("Connection error", ex);
            }
        }
    }
}
