using System;
using System.Configuration;
using System.Collections.Generic;
using System.Data.SqlClient;

using Epam.Internet_shop.Logger.Contracts;
using Epam.Internet_shop.DAL.Contracts;
using Epam.Internet_shop.Common.Entities.CommodityUnit;

namespace Epam.Internet_shop.DAL.DataBase
{
    public class StoreDao : IStoreDao
    {
        private readonly ILogger _logger;
        private readonly string _connectionString;

        public StoreDao(ILogger logger)
        {
            _logger = logger;
            _connectionString = ConfigurationManager.ConnectionStrings["default"].ConnectionString;
        }

        public int AddStore(Store store)
        {
            _logger.Info($"DAL.{nameof(StoreDao)}.{nameof(AddStore)}: Adding a store");

            int id;

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    SqlCommand command = new SqlCommand("dbo.Store_AddStore", connection)
                    {
                        CommandType = System.Data.CommandType.StoredProcedure
                    };

                    command.Parameters.AddWithValue("@Name", store.Name);

                    connection.Open();

                    _logger.Info($"DAL.{nameof(StoreDao)}.{nameof(AddStore)}: Connected to database");

                    int.TryParse(command.ExecuteScalar().ToString(), out id);
                }
            }
            catch (InvalidOperationException ex)
            {
                _logger.Error($"DAL.{nameof(StoreDao)}.{nameof(AddStore)}: Not connected to database: " + ex.Message);

                throw new SystemException("Connection error", ex);
            }

            _logger.Info($"DAL.{nameof(StoreDao)}.{nameof(AddStore)}: Store id = {id} added");

            return id;
        }

        public int ChangeStore(Store store)
        {
            _logger.Info($"DAL.{nameof(StoreDao)}.{nameof(ChangeStore)}: Store change");

            int id = store.Id ?? -1;

            if (id == -1)
            {
                _logger.Error($"DAL.{nameof(StoreDao)}.{nameof(ChangeStore)}: store.Id cannot be null");

                throw new ArgumentNullException("store.Id cannot be null");
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    SqlCommand command = new SqlCommand("dbo.Store_ChangeStore", connection)
                    {
                        CommandType = System.Data.CommandType.StoredProcedure
                    };

                    command.Parameters.AddWithValue("@Id", store.Id);
                    command.Parameters.AddWithValue("@Name", store.Name);

                    connection.Open();

                    _logger.Info($"DAL.{nameof(StoreDao)}.{nameof(ChangeStore)}: Connected to database");

                    command.ExecuteScalar();
                }
            }
            catch (InvalidOperationException ex)
            {
                _logger.Error($"DAL.{nameof(StoreDao)}.{nameof(ChangeStore)}: Not connected to database: " + ex.Message);

                throw new SystemException("Connection error", ex);
            }

            _logger.Info($"DAL.{nameof(StoreDao)}.{nameof(ChangeStore)}: Store id = {id} changed");

            return id;
        }

        public IEnumerable<Store> GetAllStores()
        {
            _logger.Info($"DAL.{nameof(StoreDao)}.{nameof(GetAllStores)}: Getting all stores");

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand("dbo.Store_GetAllStores", connection)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };

                SqlDataReader reader;

                try
                {
                    connection.Open();

                    reader = command.ExecuteReader();

                    _logger.Info($"DAL.{nameof(StoreDao)}.{nameof(GetAllStores)}: Connected to database");
                }
                catch (InvalidOperationException ex)
                {
                    _logger.Error($"DAL.{nameof(StoreDao)}.{nameof(GetAllStores)}: Not connected to database: " + ex.Message);

                    throw new SystemException("Connection error", ex);
                }

                while (reader.Read())
                {
                    yield return new Store((int?)reader["Id"], (string)reader["Name"]);
                }
            }

            _logger.Info($"DAL.{nameof(StoreDao)}.{nameof(GetAllStores)}: All stores received");

            yield break;
        }

        public Store GetStore(int id)
        {
            _logger.Info($"DAL.{nameof(StoreDao)}.{nameof(GetStore)}: Getting store by id = {id}");

            Store store;

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand("dbo.Store_GetStore", connection)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };

                command.Parameters.AddWithValue("@Id", id);

                SqlDataReader reader;

                try
                {
                    connection.Open();

                    reader = command.ExecuteReader();

                    _logger.Info($"DAL.{nameof(StoreDao)}.{nameof(GetStore)}: Connected to database");
                }
                catch (InvalidOperationException ex)
                {
                    _logger.Error($"DAL.{nameof(StoreDao)}.{nameof(GetStore)}: Not connected to database: " + ex.Message);

                    throw new SystemException("Connection error", ex);
                }

                reader.Read();

                store = new Store((int?)reader["Id"], (string)reader["Name"]);
            }

            _logger.Info($"DAL.{nameof(StoreDao)}.{nameof(GetStore)}: Store by id = {id} obtained");

            return store;
        }

        public bool IsStore(int id)
        {
            _logger.Info($"DAL.{nameof(StoreDao)}.{nameof(IsStore)}: Checking the store by id = {id}");

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand("dbo.Store_GetStore", connection)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };

                command.Parameters.AddWithValue("@Id", id);

                SqlDataReader reader;

                try
                {
                    connection.Open();

                    reader = command.ExecuteReader();

                    _logger.Info($"DAL.{nameof(StoreDao)}.{nameof(IsStore)}: Connected to database");
                }
                catch (InvalidOperationException ex)
                {
                    _logger.Error($"DAL.{nameof(StoreDao)}.{nameof(IsStore)}: Not connected to database: " + ex.Message);

                    throw new SystemException("Connection error", ex);
                }

                if (reader.Read())
                {
                    _logger.Info($"DAL.{nameof(StoreDao)}.{nameof(IsStore)}: Store by id = {id} found");

                    return true;
                }

                _logger.Info($"DAL.{nameof(StoreDao)}.{nameof(IsStore)}: Store by id = {id} not found");

                return false;
            }
        }

        public void RemoveStore(int id)
        {
            _logger.Info($"DAL.{nameof(StoreDao)}.{nameof(RemoveStore)}: Remove store by id = {id}");

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    SqlCommand command = new SqlCommand("dbo.Store_RemoveStore", connection)
                    {
                        CommandType = System.Data.CommandType.StoredProcedure
                    };

                    command.Parameters.AddWithValue("@Id", id);

                    connection.Open();

                    _logger.Info($"DAL.{nameof(StoreDao)}.{nameof(RemoveStore)}: Connected to database");

                    command.ExecuteNonQuery();

                    _logger.Info($"DAL.{nameof(StoreDao)}.{nameof(RemoveStore)}: Category id = {id} removed");
                }
            }
            catch (InvalidOperationException ex)
            {
                _logger.Error($"DAL.{nameof(StoreDao)}.{nameof(RemoveStore)}: Not connected to database: " + ex.Message);

                throw new SystemException("Connection error", ex);
            }
        }
    }
}
