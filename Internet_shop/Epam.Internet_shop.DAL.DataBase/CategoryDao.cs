using System;
using System.Configuration;
using System.Collections.Generic;
using System.Data.SqlClient;

using Epam.Internet_shop.Logger.Contracts;
using Epam.Internet_shop.DAL.Contracts;
using Epam.Internet_shop.Common.Entities.CommodityUnit;

namespace Epam.Internet_shop.DAL.DataBase
{
    public class CategoryDao : ICategoryDao
    {
        private readonly ILogger _logger;
        private readonly string _connectionString;

        public CategoryDao(ILogger logger)
        {
            _logger = logger;
            _connectionString = ConfigurationManager.ConnectionStrings["default"].ConnectionString;
        }

        public int AddCategory(Category category)
        {
            _logger.Info($"DAL.{nameof(CategoryDao)}.{nameof(AddCategory)}: Adding a category");

            int id;

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    SqlCommand command = new SqlCommand("dbo.Category_AddCategory", connection)
                    {
                        CommandType = System.Data.CommandType.StoredProcedure
                    };

                    command.Parameters.AddWithValue("@Name", category.Name);

                    connection.Open();

                    _logger.Info($"DAL.{nameof(CategoryDao)}.{nameof(AddCategory)}: Connected to database");

                    int.TryParse(command.ExecuteScalar().ToString(), out id);
                }
            }
            catch (InvalidOperationException ex)
            {
                _logger.Error($"DAL.{nameof(CategoryDao)}.{nameof(AddCategory)}: Not connected to database: " + ex.Message);

                throw new SystemException("Connection error", ex);
            }

            _logger.Info($"DAL.{nameof(CategoryDao)}.{nameof(AddCategory)}: Category id = {id} added");

            return id;
        }

        public int ChangeCategory(Category category)
        {
            _logger.Info($"DAL.{nameof(CategoryDao)}.{nameof(ChangeCategory)}: Category change");

            int id = category.Id ?? -1;

            if (id == -1)
            {
                _logger.Error($"DAL.{nameof(CategoryDao)}.{nameof(ChangeCategory)}: category.Id cannot be null");

                throw new ArgumentNullException("category.Id cannot be null");
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    SqlCommand command = new SqlCommand("dbo.Category_ChangeCategory", connection)
                    {
                        CommandType = System.Data.CommandType.StoredProcedure
                    };

                    command.Parameters.AddWithValue("@Id", category.Id);
                    command.Parameters.AddWithValue("@Name", category.Name);

                    connection.Open();

                    _logger.Info($"DAL.{nameof(CategoryDao)}.{nameof(ChangeCategory)}: Connected to database");

                    command.ExecuteScalar();
                }
            }
            catch (InvalidOperationException ex)
            {
                _logger.Error($"DAL.{nameof(CategoryDao)}.{nameof(ChangeCategory)}: Not connected to database: " + ex.Message);

                throw new SystemException("Connection error", ex);
            }

            _logger.Info($"DAL.{nameof(CategoryDao)}.{nameof(ChangeCategory)}: Category id = {id} changed");

            return id;
        }

        public IEnumerable<Category> GetAllCategories()
        {
            _logger.Info($"DAL.{nameof(CategoryDao)}.{nameof(GetAllCategories)}: Getting all categories");

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand("dbo.Category_GetAllCategories", connection)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };

                SqlDataReader reader;

                try
                {
                    connection.Open();

                    reader = command.ExecuteReader();

                    _logger.Info($"DAL.{nameof(CategoryDao)}.{nameof(GetAllCategories)}: Connected to database");
                }
                catch (InvalidOperationException ex)
                {
                    _logger.Error($"DAL.{nameof(CategoryDao)}.{nameof(GetAllCategories)}: Not connected to database: " + ex.Message);

                    throw new SystemException("Connection error", ex);
                }

                while (reader.Read())
                {
                    yield return new Category((int?)reader["Id"], (string)reader["Name"]);
                }
            }

            _logger.Info($"DAL.{nameof(CategoryDao)}.{nameof(GetAllCategories)}: All categories received");

            yield break;
        }

        public Category GetCategory(int id)
        {
            _logger.Info($"DAL.{nameof(CategoryDao)}.{nameof(GetCategory)}: Getting category by id = {id}");

            Category category;

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand("dbo.Category_GetCategory", connection)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };

                command.Parameters.AddWithValue("@Id", id);

                SqlDataReader reader;

                try
                {
                    connection.Open();

                    reader = command.ExecuteReader();

                    _logger.Info($"DAL.{nameof(CategoryDao)}.{nameof(GetCategory)}: Connected to database");
                }
                catch (InvalidOperationException ex)
                {
                    _logger.Error($"DAL.{nameof(CategoryDao)}.{nameof(GetCategory)}: Not connected to database: " + ex.Message);

                    throw new SystemException("Connection error", ex);
                }

                reader.Read();

                category = new Category((int?)reader["Id"], (string)reader["Name"]);
            }

            _logger.Info($"DAL.{nameof(CategoryDao)}.{nameof(GetCategory)}: Category by id = {id} obtained");

            return category;
        }

        public bool IsCategory(int id)
        {
            _logger.Info($"DAL.{nameof(CategoryDao)}.{nameof(IsCategory)}: Checking the category by id = {id}");

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand("dbo.Category_GetCategory", connection)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };

                command.Parameters.AddWithValue("@Id", id);

                SqlDataReader reader;

                try
                {
                    connection.Open();

                    reader = command.ExecuteReader();

                    _logger.Info($"DAL.{nameof(CategoryDao)}.{nameof(IsCategory)}: Connected to database");
                }
                catch (InvalidOperationException ex)
                {
                    _logger.Error($"DAL.{nameof(CategoryDao)}.{nameof(IsCategory)}: Not connected to database: " + ex.Message);

                    throw new SystemException("Connection error", ex);
                }

                if (reader.Read())
                {
                    _logger.Info($"DAL.{nameof(CategoryDao)}.{nameof(IsCategory)}: Category by id = {id} found");

                    return true;
                }

                _logger.Info($"DAL.{nameof(CategoryDao)}.{nameof(IsCategory)}: Category by id = {id} not found");

                return false;
            }
        }

        public void RemoveCategory(int id)
        {
            _logger.Info($"DAL.{nameof(CategoryDao)}.{nameof(RemoveCategory)}: Remove category by id = {id}");

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    SqlCommand command = new SqlCommand("dbo.Category_RemoveCategory", connection)
                    {
                        CommandType = System.Data.CommandType.StoredProcedure
                    };

                    command.Parameters.AddWithValue("@Id", id);

                    connection.Open();

                    _logger.Info($"DAL.{nameof(CategoryDao)}.{nameof(RemoveCategory)}: Connected to database");

                    command.ExecuteNonQuery();

                    _logger.Info($"DAL.{nameof(CategoryDao)}.{nameof(RemoveCategory)}: Category id = {id} removed");
                }
            }
            catch (InvalidOperationException ex)
            {
                _logger.Error($"DAL.{nameof(CategoryDao)}.{nameof(RemoveCategory)}: Not connected to database: " + ex.Message);

                throw new SystemException("Connection error", ex);
            }
        }
    }
}
