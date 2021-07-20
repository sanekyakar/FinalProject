using System;
using System.Configuration;
using System.Collections.Generic;
using System.Data.SqlClient;

using Epam.Internet_shop.Logger.Contracts;
using Epam.Internet_shop.DAL.Contracts;
using Epam.Internet_shop.Common.Entities.CommodityUnit;

namespace Epam.Internet_shop.DAL.DataBase
{
    public class ProductDao : IProductDao
    {
        private readonly ILogger _logger;
        private readonly ICategoryDao _categoryDao;
        private readonly string _connectionString;

        public ProductDao(ILogger logger, ICategoryDao categoryDao)
        {
            _logger = logger;
            _categoryDao = categoryDao;
            _connectionString = ConfigurationManager.ConnectionStrings["default"].ConnectionString;
        }

        public int AddProduct(Product product)
        {
            _logger.Info($"DAL.{nameof(ProductDao)}.{nameof(AddProduct)}: Adding a product");

            int id;

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    SqlCommand command = new SqlCommand("dbo.Product_AddProduct", connection)
                    {
                        CommandType = System.Data.CommandType.StoredProcedure
                    };

                    int? CategoryID = null;

                    CategoryID = product?.Category.Id;

                    command.Parameters.AddWithValue("@Name", product.Name);
                    command.Parameters.AddWithValue("@CategoryID", CategoryID);
                    command.Parameters.AddWithValue("@Image", product.ImageInBase64Src);
                    command.Parameters.AddWithValue("@Description", product.Discription);

                    connection.Open();

                    _logger.Info($"DAL.{nameof(ProductDao)}.{nameof(AddProduct)}: Connected to database");

                    int.TryParse(command.ExecuteScalar().ToString(), out id);
                }
            }
            catch (InvalidOperationException ex)
            {
                _logger.Error($"DAL.{nameof(ProductDao)}.{nameof(AddProduct)}: Not connected to database: " + ex.Message);

                throw new SystemException("Connection error", ex);
            }

            _logger.Info($"DAL.{nameof(ProductDao)}.{nameof(AddProduct)}: Product id = {id} added");

            return id;
        }

        public int ChangeProduct(Product product)
        {
            _logger.Info($"DAL.{nameof(ProductDao)}.{nameof(ChangeProduct)}: Product change");

            int id = product.Id ?? -1;

            if (id == -1)
            {
                _logger.Error($"DAL.{nameof(ProductDao)}.{nameof(ChangeProduct)}: product.Id cannot be null");

                throw new ArgumentNullException("product.Id cannot be null");
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    SqlCommand command = new SqlCommand("dbo.Product_ChangeProduct", connection)
                    {
                        CommandType = System.Data.CommandType.StoredProcedure
                    };

                    int? CategoryID = null;

                    CategoryID = product?.Category.Id;

                    command.Parameters.AddWithValue("@Id", product.Id);
                    command.Parameters.AddWithValue("@Name", product.Name);
                    command.Parameters.AddWithValue("@CategoryID", CategoryID);
                    command.Parameters.AddWithValue("@Image", product.ImageInBase64Src);
                    command.Parameters.AddWithValue("@Description", product.Discription);

                    connection.Open();

                    _logger.Info($"DAL.{nameof(ProductDao)}.{nameof(ChangeProduct)}: Connected to database");

                    command.ExecuteScalar();
                }
            }
            catch (InvalidOperationException ex)
            {
                _logger.Error($"DAL.{nameof(ProductDao)}.{nameof(ChangeProduct)}: Not connected to database: " + ex.Message);

                throw new SystemException("Connection error", ex);
            }

            _logger.Info($"DAL.{nameof(ProductDao)}.{nameof(ChangeProduct)}: Product id = {id} changed");

            return id;
        }

        public IEnumerable<Product> GetAllProducts()
        {
            _logger.Info($"DAL.{nameof(ProductDao)}.{nameof(GetAllProducts)}: Getting all products");

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand("dbo.Product_GetAllProducts", connection)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };

                SqlDataReader reader;

                try
                {
                    connection.Open();

                    reader = command.ExecuteReader();

                    _logger.Info($"DAL.{nameof(ProductDao)}.{nameof(GetAllProducts)}: Connected to database");
                }
                catch (InvalidOperationException ex)
                {
                    _logger.Error($"DAL.{nameof(ProductDao)}.{nameof(GetAllProducts)}: Not connected to database: " + ex.Message);

                    throw new SystemException("Connection error", ex);
                }

                int? CategoryID;

                while (reader.Read())
                {
                    CategoryID = (int?)reader["CategoryID"];

                    yield return new Product(
                        (int?)reader["Id"],
                        (string)reader["Name"],
                        (string)reader["Image"],
                        (string)reader["Description"],
                        CategoryID is null ? null : _categoryDao.GetCategory(CategoryID ?? -1));
                }
            }

            _logger.Info($"DAL.{nameof(ProductDao)}.{nameof(GetAllProducts)}: All products received");

            yield break;
        }

        public Product GetProduct(int id)
        {
            _logger.Info($"DAL.{nameof(ProductDao)}.{nameof(GetProduct)}: Getting product by id = {id}");

            Product product;

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand("dbo.Product_GetProduct", connection)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };

                command.Parameters.AddWithValue("@Id", id);

                SqlDataReader reader;

                try
                {
                    connection.Open();

                    reader = command.ExecuteReader();

                    _logger.Info($"DAL.{nameof(ProductDao)}.{nameof(GetProduct)}: Connected to database");
                }
                catch (InvalidOperationException ex)
                {
                    _logger.Error($"DAL.{nameof(ProductDao)}.{nameof(GetProduct)}: Not connected to database: " + ex.Message);

                    throw new SystemException("Connection error", ex);
                }

                reader.Read();

                int? CategoryID = (int?)reader["CategoryID"];

                product = new Product(
                    (int?)reader["Id"],
                    (string)reader["Name"],
                    (string)reader["Image"],
                    (string)reader["Description"],
                    CategoryID is null ? null : _categoryDao.GetCategory(CategoryID ?? -1));
            }

            _logger.Info($"DAL.{nameof(ProductDao)}.{nameof(GetProduct)}: Product by id = {id} obtained");

            return product;
        }

        public IEnumerable<Product> GetProductsByCategory(int id)
        {
            _logger.Info($"DAL.{nameof(ProductDao)}.{nameof(GetProductsByCategory)}: Getting products by category");

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand("dbo.Product_GetProductsByCategory", connection)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };

                command.Parameters.AddWithValue("@Id", id);

                SqlDataReader reader;

                try
                {
                    connection.Open();

                    reader = command.ExecuteReader();

                    _logger.Info($"DAL.{nameof(ProductDao)}.{nameof(GetProductsByCategory)}: Connected to database");
                }
                catch (InvalidOperationException ex)
                {
                    _logger.Error($"DAL.{nameof(ProductDao)}.{nameof(GetProductsByCategory)}: Not connected to database: " + ex.Message);

                    throw new SystemException("Connection error", ex);
                }

                while (reader.Read())
                {
                    yield return new Product(
                    (int?)reader["Id"],
                    (string)reader["Name"],
                    (string)reader["Image"],
                    (string)reader["Description"],
                    _categoryDao.GetCategory(id));
                }
            }

            _logger.Info($"DAL.{nameof(ProductDao)}.{nameof(GetProductsByCategory)}: Products by category received");

            yield break;
        }

        public bool IsProduct(int id)
        {
            _logger.Info($"DAL.{nameof(ProductDao)}.{nameof(IsProduct)}: Checking the product by id = {id}");

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand("dbo.Product_GetProduct", connection)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };

                command.Parameters.AddWithValue("@Id", id);

                SqlDataReader reader;

                try
                {
                    connection.Open();

                    reader = command.ExecuteReader();

                    _logger.Info($"DAL.{nameof(ProductDao)}.{nameof(IsProduct)}: Connected to database");
                }
                catch (InvalidOperationException ex)
                {
                    _logger.Error($"DAL.{nameof(ProductDao)}.{nameof(IsProduct)}: Not connected to database: " + ex.Message);

                    throw new SystemException("Connection error", ex);
                }

                if (reader.Read())
                {
                    _logger.Info($"DAL.{nameof(ProductDao)}.{nameof(IsProduct)}: Product by id = {id} found");

                    return true;
                }

                _logger.Info($"DAL.{nameof(ProductDao)}.{nameof(IsProduct)}: Product by id = {id} not found");

                return false;
            }
        }

        public void RemoveProduct(int id)
        {
            _logger.Info($"DAL.{nameof(ProductDao)}.{nameof(RemoveProduct)}: Remove product by id = {id}");

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    SqlCommand command = new SqlCommand("dbo.Product_RemoveProduct", connection)
                    {
                        CommandType = System.Data.CommandType.StoredProcedure
                    };

                    command.Parameters.AddWithValue("@Id", id);

                    connection.Open();

                    _logger.Info($"DAL.{nameof(ProductDao)}.{nameof(RemoveProduct)}: Connected to database");

                    command.ExecuteNonQuery();

                    _logger.Info($"DAL.{nameof(ProductDao)}.{nameof(RemoveProduct)}: Product id = {id} removed");
                }
            }
            catch (InvalidOperationException ex)
            {
                _logger.Error($"DAL.{nameof(ProductDao)}.{nameof(RemoveProduct)}: Not connected to database: " + ex.Message);

                throw new SystemException("Connection error", ex);
            }
        }
    }
}
