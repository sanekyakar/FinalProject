using System;
using System.Configuration;
using System.Collections.Generic;
using System.Data.SqlClient;

using Epam.Internet_shop.Logger.Contracts;
using Epam.Internet_shop.DAL.Contracts;
using Epam.Internet_shop.Common.Entities.CommodityUnit;

namespace Epam.Internet_shop.DAL.DataBase
{
    public class StatusDao : IStatusDao
    {
        private readonly ILogger _logger;
        private readonly string _connectionString;

        public StatusDao(ILogger logger)
        {
            _logger = logger;
            _connectionString = ConfigurationManager.ConnectionStrings["default"].ConnectionString;
        }

        public int AddStatus(Status status)
        {
            _logger.Info($"DAL.{nameof(StatusDao)}.{nameof(AddStatus)}: Adding a status");

            int id;

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    SqlCommand command = new SqlCommand("dbo.Status_AddStatus", connection)
                    {
                        CommandType = System.Data.CommandType.StoredProcedure
                    };

                    command.Parameters.AddWithValue("@Name", status.Name);

                    connection.Open();

                    _logger.Info($"DAL.{nameof(StatusDao)}.{nameof(AddStatus)}: Connected to database");

                    int.TryParse(command.ExecuteScalar().ToString(), out id);
                }
            }
            catch (InvalidOperationException ex)
            {
                _logger.Error($"DAL.{nameof(StatusDao)}.{nameof(AddStatus)}: Not connected to database: " + ex.Message);

                throw new SystemException("Connection error", ex);
            }

            _logger.Info($"DAL.{nameof(StatusDao)}.{nameof(AddStatus)}: Status id = {id} added");

            return id;
        }

        public int ChangeStatus(Status status)
        {
            _logger.Info($"DAL.{nameof(StatusDao)}.{nameof(ChangeStatus)}: Status change");

            int id = status.Id ?? -1;

            if (id == -1)
            {
                _logger.Error($"DAL.{nameof(StatusDao)}.{nameof(ChangeStatus)}: status.Id cannot be null");

                throw new ArgumentNullException("status.Id cannot be null");
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    SqlCommand command = new SqlCommand("dbo.Status_ChangeStatus", connection)
                    {
                        CommandType = System.Data.CommandType.StoredProcedure
                    };

                    command.Parameters.AddWithValue("@Id", status.Id);
                    command.Parameters.AddWithValue("@Name", status.Name);

                    connection.Open();

                    _logger.Info($"DAL.{nameof(StatusDao)}.{nameof(ChangeStatus)}: Connected to database");

                    command.ExecuteScalar();
                }
            }
            catch (InvalidOperationException ex)
            {
                _logger.Error($"DAL.{nameof(StatusDao)}.{nameof(ChangeStatus)}: Not connected to database: " + ex.Message);

                throw new SystemException("Connection error", ex);
            }

            _logger.Info($"DAL.{nameof(StatusDao)}.{nameof(ChangeStatus)}: Status id = {id} changed");

            return id;
        }

        public IEnumerable<Status> GetAllStatuses()
        {
            _logger.Info($"DAL.{nameof(StatusDao)}.{nameof(GetAllStatuses)}: Getting all statuses");

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand("dbo.Status_GetAllStatuses", connection)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };

                SqlDataReader reader;

                try
                {
                    connection.Open();

                    reader = command.ExecuteReader();

                    _logger.Info($"DAL.{nameof(StatusDao)}.{nameof(GetAllStatuses)}: Connected to database");
                }
                catch (InvalidOperationException ex)
                {
                    _logger.Error($"DAL.{nameof(StatusDao)}.{nameof(GetAllStatuses)}: Not connected to database: " + ex.Message);

                    throw new SystemException("Connection error", ex);
                }

                while (reader.Read())
                {
                    yield return new Status((int?)reader["Id"], (string)reader["Name"]);
                }
            }

            _logger.Info($"DAL.{nameof(StatusDao)}.{nameof(GetAllStatuses)}: All Statuses received");

            yield break;
        }

        public Status GetStatus(int id)
        {
            _logger.Info($"DAL.{nameof(StatusDao)}.{nameof(GetStatus)}: Getting status by id = {id}");

            Status status;

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand("dbo.Status_GetStatus", connection)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };

                command.Parameters.AddWithValue("@Id", id);

                SqlDataReader reader;

                try
                {
                    connection.Open();

                    reader = command.ExecuteReader();

                    _logger.Info($"DAL.{nameof(StatusDao)}.{nameof(GetStatus)}: Connected to database");
                }
                catch (InvalidOperationException ex)
                {
                    _logger.Error($"DAL.{nameof(StatusDao)}.{nameof(GetStatus)}: Not connected to database: " + ex.Message);

                    throw new SystemException("Connection error", ex);
                }

                reader.Read();

                status = new Status((int?)reader["Id"], (string)reader["Name"]);
            }

            _logger.Info($"DAL.{nameof(StatusDao)}.{nameof(GetStatus)}: Status by id = {id} obtained");

            return status;
        }

        public bool IsStatus(int id)
        {
            _logger.Info($"DAL.{nameof(StatusDao)}.{nameof(IsStatus)}: Checking the status by id = {id}");

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand("dbo.Status_GetStatus", connection)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };

                command.Parameters.AddWithValue("@Id", id);

                SqlDataReader reader;

                try
                {
                    connection.Open();

                    reader = command.ExecuteReader();

                    _logger.Info($"DAL.{nameof(StatusDao)}.{nameof(IsStatus)}: Connected to database");
                }
                catch (InvalidOperationException ex)
                {
                    _logger.Error($"DAL.{nameof(StatusDao)}.{nameof(IsStatus)}: Not connected to database: " + ex.Message);

                    throw new SystemException("Connection error", ex);
                }

                if (reader.Read())
                {
                    _logger.Info($"DAL.{nameof(StatusDao)}.{nameof(IsStatus)}: Status by id = {id} found");

                    return true;
                }

                _logger.Info($"DAL.{nameof(StatusDao)}.{nameof(IsStatus)}: Status by id = {id} not found");

                return false;
            }
        }

        public void RemoveStatus(int id)
        {
            _logger.Info($"DAL.{nameof(StatusDao)}.{nameof(RemoveStatus)}: Remove status by id = {id}");

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    SqlCommand command = new SqlCommand("dbo.Status_RemoveStatus", connection)
                    {
                        CommandType = System.Data.CommandType.StoredProcedure
                    };

                    command.Parameters.AddWithValue("@Id", id);

                    connection.Open();

                    _logger.Info($"DAL.{nameof(StatusDao)}.{nameof(RemoveStatus)}: Connected to database");

                    command.ExecuteNonQuery();

                    _logger.Info($"DAL.{nameof(StatusDao)}.{nameof(RemoveStatus)}: Status id = {id} removed");
                }
            }
            catch (InvalidOperationException ex)
            {
                _logger.Error($"DAL.{nameof(StatusDao)}.{nameof(RemoveStatus)}: Not connected to database: " + ex.Message);

                throw new SystemException("Connection error", ex);
            }
        }
    }
}
