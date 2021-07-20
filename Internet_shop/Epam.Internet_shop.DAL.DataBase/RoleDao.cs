using System;
using System.Configuration;
using System.Collections.Generic;
using System.Data.SqlClient;

using Epam.Internet_shop.Logger.Contracts;
using Epam.Internet_shop.DAL.Contracts;
using Epam.Internet_shop.Common.Entities.User;

namespace Epam.Internet_shop.DAL.DataBase
{
    public class RoleDao : IRoleDao
    {
        private readonly ILogger _logger;
        private readonly string _connectionString;

        public RoleDao(ILogger logger)
        {
            _logger = logger;
            _connectionString = ConfigurationManager.ConnectionStrings["default"].ConnectionString;
        }

        public int AddRole(Role role)
        {
            _logger.Info($"DAL.{nameof(RoleDao)}.{nameof(AddRole)}: Adding a role");

            int id;

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    SqlCommand command = new SqlCommand("dbo.Role_AddRole", connection)
                    {
                        CommandType = System.Data.CommandType.StoredProcedure
                    };

                    command.Parameters.AddWithValue("@Name", role.Name);

                    connection.Open();

                    _logger.Info($"DAL.{nameof(RoleDao)}.{nameof(AddRole)}: Connected to database");

                    int.TryParse(command.ExecuteScalar().ToString(), out id);
                }
            }
            catch (InvalidOperationException ex)
            {
                _logger.Error($"DAL.{nameof(RoleDao)}.{nameof(AddRole)}: Not connected to database: " + ex.Message);

                throw new SystemException("Connection error", ex);
            }

            _logger.Info($"DAL.{nameof(RoleDao)}.{nameof(AddRole)}: Role id = {id} added");

            return id;
        }

        public int ChangeRole(Role role)
        {
            _logger.Info($"DAL.{nameof(RoleDao)}.{nameof(ChangeRole)}: Role change");

            int id = role.Id ?? -1;

            if (id == -1)
            {
                _logger.Error($"DAL.{nameof(RoleDao)}.{nameof(ChangeRole)}: role.Id cannot be null");

                throw new ArgumentNullException("role.Id cannot be null");
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    SqlCommand command = new SqlCommand("dbo.Role_ChangeRole", connection)
                    {
                        CommandType = System.Data.CommandType.StoredProcedure
                    };

                    command.Parameters.AddWithValue("@Id", role.Id);
                    command.Parameters.AddWithValue("@Name", role.Name);

                    connection.Open();

                    _logger.Info($"DAL.{nameof(RoleDao)}.{nameof(ChangeRole)}: Connected to database");

                    command.ExecuteScalar();
                }
            }
            catch (InvalidOperationException ex)
            {
                _logger.Error($"DAL.{nameof(RoleDao)}.{nameof(ChangeRole)}: Not connected to database: " + ex.Message);

                throw new SystemException("Connection error", ex);
            }

            _logger.Info($"DAL.{nameof(RoleDao)}.{nameof(ChangeRole)}: Role id = {id} changed");

            return id;
        }

        public IEnumerable<Role> GetAllRoles()
        {
            _logger.Info($"DAL.{nameof(RoleDao)}.{nameof(GetAllRoles)}: Getting all roles");

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand("dbo.Role_GetAllRoles", connection)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };

                SqlDataReader reader;

                try
                {
                    connection.Open();

                    reader = command.ExecuteReader();

                    _logger.Info($"DAL.{nameof(RoleDao)}.{nameof(GetAllRoles)}: Connected to database");
                }
                catch (InvalidOperationException ex)
                {
                    _logger.Error($"DAL.{nameof(RoleDao)}.{nameof(GetAllRoles)}: Not connected to database: " + ex.Message);

                    throw new SystemException("Connection error", ex);
                }

                while (reader.Read())
                {
                    yield return new Role((int?)reader["Id"], (string)reader["Name"]);
                }
            }

            _logger.Info($"DAL.{nameof(RoleDao)}.{nameof(GetAllRoles)}: All roles received");

            yield break;
        }

        public Role GetRole(int id)
        {
            _logger.Info($"DAL.{nameof(RoleDao)}.{nameof(GetRole)}: Getting role by id = {id}");

            Role role;

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand("dbo.Role_GetRole", connection)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };

                command.Parameters.AddWithValue("@Id", id);

                SqlDataReader reader;

                try
                {
                    connection.Open();

                    reader = command.ExecuteReader();

                    _logger.Info($"DAL.{nameof(RoleDao)}.{nameof(GetRole)}: Connected to database");
                }
                catch (InvalidOperationException ex)
                {
                    _logger.Error($"DAL.{nameof(RoleDao)}.{nameof(GetRole)}: Not connected to database: " + ex.Message);

                    throw new SystemException("Connection error", ex);
                }

                reader.Read();

                role = new Role((int?)reader["Id"], (string)reader["Name"]);
            }

            _logger.Info($"DAL.{nameof(RoleDao)}.{nameof(GetRole)}: Role by id = {id} obtained");

            return role;
        }

        public bool IsRole(int id)
        {
            _logger.Info($"DAL.{nameof(RoleDao)}.{nameof(IsRole)}: Checking the role by id = {id}");

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand("dbo.Role_GetRole", connection)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };

                command.Parameters.AddWithValue("@Id", id);

                SqlDataReader reader;

                try
                {
                    connection.Open();

                    reader = command.ExecuteReader();

                    _logger.Info($"DAL.{nameof(RoleDao)}.{nameof(IsRole)}: Connected to database");
                }
                catch (InvalidOperationException ex)
                {
                    _logger.Error($"DAL.{nameof(RoleDao)}.{nameof(IsRole)}: Not connected to database: " + ex.Message);

                    throw new SystemException("Connection error", ex);
                }

                if (reader.Read())
                {
                    _logger.Info($"DAL.{nameof(RoleDao)}.{nameof(IsRole)}: Role by id = {id} found");

                    return true;
                }

                _logger.Info($"DAL.{nameof(RoleDao)}.{nameof(IsRole)}: Role by id = {id} not found");

                return false;
            }
        }

        public void RemoveRole(int id)
        {
            _logger.Info($"DAL.{nameof(RoleDao)}.{nameof(RemoveRole)}: Remove Role by id = {id}");

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    SqlCommand command = new SqlCommand("dbo.Role_RemoveRole", connection)
                    {
                        CommandType = System.Data.CommandType.StoredProcedure
                    };

                    command.Parameters.AddWithValue("@Id", id);

                    connection.Open();

                    _logger.Info($"DAL.{nameof(RoleDao)}.{nameof(RemoveRole)}: Connected to database");

                    command.ExecuteNonQuery();

                    _logger.Info($"DAL.{nameof(RoleDao)}.{nameof(RemoveRole)}: Role id = {id} removed");
                }
            }
            catch (InvalidOperationException ex)
            {
                _logger.Error($"DAL.{nameof(RoleDao)}.{nameof(RemoveRole)}: Not connected to database: " + ex.Message);

                throw new SystemException("Connection error", ex);
            }
        }
    }
}
