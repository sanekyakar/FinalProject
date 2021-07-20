using System;
using System.Configuration;
using System.Collections.Generic;
using System.Data.SqlClient;

using Epam.Internet_shop.Logger.Contracts;
using Epam.Internet_shop.DAL.Contracts;
using Epam.Internet_shop.Common.Entities.CommodityUnit;

namespace Epam.Internet_shop.DAL.DataBase
{
    public class CommodityUnitDao : ICommodityUnitDao
    {
        private readonly ILogger _logger;
        private readonly IProductDao _productDao;
        private readonly IStatusDao _statusDao;
        private readonly IStoreDao _storeDao;
        private readonly IVendorDao _vendorDao;
        private readonly string _connectionString;

        public CommodityUnitDao(ILogger logger, IProductDao productDao, IStatusDao statusDao, IStoreDao storeDao, IVendorDao vendorDao)
        {
            _logger = logger;
            _productDao = productDao;
            _statusDao = statusDao;
            _storeDao = storeDao;
            _vendorDao = vendorDao;
            _connectionString = ConfigurationManager.ConnectionStrings["default"].ConnectionString;
        }

        public int AddCommodityUnit(CommodityUnit commodityUnit)
        {
            _logger.Info($"DAL.{nameof(CommodityUnitDao)}.{nameof(AddCommodityUnit)}: Adding a commodity unit");

            int id;

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    SqlCommand command = new SqlCommand("dbo.Product_Store_Vendor_Status_AddCommodityUnit", connection)
                    {
                        CommandType = System.Data.CommandType.StoredProcedure
                    };

                    object ProductID, StatusID, StoreID, VendorID = null;

                    ProductID = commodityUnit?.Product?.Id;
                    StatusID = commodityUnit?.Status?.Id;
                    StoreID = commodityUnit?.Store?.Id;
                    VendorID = commodityUnit?.Vendor?.Id;

                    command.Parameters.AddWithValue("@ProductID", ProductID ?? DBNull.Value);
                    command.Parameters.AddWithValue("@StatusID", StatusID ?? DBNull.Value);
                    command.Parameters.AddWithValue("@StoreID", StoreID ?? DBNull.Value);
                    command.Parameters.AddWithValue("@VendorID", VendorID ?? DBNull.Value);
                    command.Parameters.AddWithValue("@Price", commodityUnit.Price);

                    connection.Open();

                    _logger.Info($"DAL.{nameof(CommodityUnitDao)}.{nameof(AddCommodityUnit)}: Connected to database");
                    
                    int.TryParse(command.ExecuteScalar().ToString(), out id);
                }
            }
            catch (InvalidOperationException ex)
            {
                _logger.Error($"DAL.{nameof(CommodityUnitDao)}.{nameof(AddCommodityUnit)}: Not connected to database: " + ex.Message);

                throw new SystemException("Connection error", ex);
            }

            _logger.Info($"DAL.{nameof(CommodityUnitDao)}.{nameof(AddCommodityUnit)}: Commodity unit id = {id} added");

            return id;
        }

        public int ChangeCommodityUnit(CommodityUnit commodityUnit)
        {
            _logger.Info($"DAL.{nameof(CommodityUnitDao)}.{nameof(ChangeCommodityUnit)}: Commodity unit change");

            int id = commodityUnit.Id ?? -1;

            if (id == -1)
            {
                _logger.Error($"DAL.{nameof(CommodityUnitDao)}.{nameof(ChangeCommodityUnit)}: commodityUnit.Id cannot be null");

                throw new ArgumentNullException("commodityUnit.Id cannot be null");
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    SqlCommand command = new SqlCommand("dbo.Product_Store_Vendor_Status_ChangeCommodityUnit", connection)
                    {
                        CommandType = System.Data.CommandType.StoredProcedure
                    };

                    object ProductID, StatusID, StoreID, VendorID = null;

                    ProductID = commodityUnit?.Product?.Id;
                    StatusID = commodityUnit?.Status?.Id;
                    StoreID = commodityUnit?.Store?.Id;
                    VendorID = commodityUnit?.Vendor?.Id;

                    command.Parameters.AddWithValue("@Id", commodityUnit.Id);
                    command.Parameters.AddWithValue("@ProductID", ProductID ?? DBNull.Value);
                    command.Parameters.AddWithValue("@StatusID", StatusID ?? DBNull.Value);
                    command.Parameters.AddWithValue("@StoreID", StoreID ?? DBNull.Value);
                    command.Parameters.AddWithValue("@VendorID", VendorID ?? DBNull.Value);
                    command.Parameters.AddWithValue("@Price", commodityUnit.Price);

                    connection.Open();

                    _logger.Info($"DAL.{nameof(CommodityUnitDao)}.{nameof(ChangeCommodityUnit)}: Connected to database");

                    command.ExecuteScalar();
                }
            }
            catch (InvalidOperationException ex)
            {
                _logger.Error($"DAL.{nameof(CommodityUnitDao)}.{nameof(ChangeCommodityUnit)}: Not connected to database: " + ex.Message);

                throw new SystemException("Connection error", ex);
            }

            _logger.Info($"DAL.{nameof(CommodityUnitDao)}.{nameof(ChangeCommodityUnit)}: Commodity unit id = {id} changed");

            return id;
        }

        public IEnumerable<CommodityUnit> GetAllCommodityUnits()
        {
            _logger.Info($"DAL.{nameof(CommodityUnitDao)}.{nameof(GetAllCommodityUnits)}: Getting all commodity units");

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand("dbo.Product_Store_Vendor_Status_GetAllCommodityUnits", connection)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };

                SqlDataReader reader;

                try
                {
                    connection.Open();

                    reader = command.ExecuteReader();

                    _logger.Info($"DAL.{nameof(CommodityUnitDao)}.{nameof(GetAllCommodityUnits)}: Connected to database");
                }
                catch (InvalidOperationException ex)
                {
                    _logger.Error($"DAL.{nameof(CommodityUnitDao)}.{nameof(GetAllCommodityUnits)}: Not connected to database: " + ex.Message);

                    throw new SystemException("Connection error", ex);
                }

                int? ProductID, StatusID, StoreID, VendorID;

                while (reader.Read())
                {
                    ProductID = (int?)reader["ProductID"];
                    StatusID = (int?)reader["StatusID"];
                    StoreID = (int?)reader["StoreID"];
                    VendorID = (int?)reader["VendorID"];

                    yield return new CommodityUnit(
                        (int?)reader["Id"],
                        ProductID is null ? null : _productDao.GetProduct(ProductID ?? -1),
                        StoreID is null ? null : _storeDao.GetStore(StoreID ?? -1),
                        VendorID is null ? null : _vendorDao.GetVendor(VendorID?? -1),
                        StatusID is null ? null : _statusDao.GetStatus(StatusID ?? -1),
                        (decimal)reader["Price"]);
                }
            }

            _logger.Info($"DAL.{nameof(ProductDao)}.{nameof(GetAllCommodityUnits)}: All commodity units received");

            yield break;
        }

        public CommodityUnit GetCommodityUnit(int id)
        {
            _logger.Info($"DAL.{nameof(CommodityUnitDao)}.{nameof(GetCommodityUnit)}: Getting commodity unit by id = {id}");

            CommodityUnit commodityUnit;

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand("dbo.Product_Store_Vendor_Status_GetCommodityUnit", connection)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };

                command.Parameters.AddWithValue("@Id", id);

                SqlDataReader reader;

                try
                {
                    connection.Open();

                    reader = command.ExecuteReader();

                    _logger.Info($"DAL.{nameof(CommodityUnitDao)}.{nameof(GetCommodityUnit)}: Connected to database");
                }
                catch (InvalidOperationException ex)
                {
                    _logger.Error($"DAL.{nameof(CommodityUnitDao)}.{nameof(GetCommodityUnit)}: Not connected to database: " + ex.Message);

                    throw new SystemException("Connection error", ex);
                }

                reader.Read();

                int? ProductID = (int?)reader["ProductID"];
                int? StatusID = (int?)reader["StatusID"];
                int? StoreID = (int?)reader["StoreID"];
                int? VendorID = (int?)reader["VendorID"];

                commodityUnit = new CommodityUnit(
                        (int?)reader["Id"],
                        ProductID is null ? null : _productDao.GetProduct(ProductID ?? -1),
                        StoreID is null ? null : _storeDao.GetStore(StoreID ?? -1),
                        VendorID is null ? null : _vendorDao.GetVendor(VendorID ?? -1),
                        StatusID is null ? null : _statusDao.GetStatus(StatusID ?? -1),
                        (decimal)reader["Price"]);
            }

            _logger.Info($"DAL.{nameof(CommodityUnitDao)}.{nameof(GetCommodityUnit)}: Commodity unit by id = {id} obtained");

            return commodityUnit;
        }

        public IEnumerable<CommodityUnit> GetCommodityUnitsByProduct(int id)
        {
            _logger.Info($"DAL.{nameof(CommodityUnitDao)}.{nameof(GetCommodityUnitsByProduct)}: Getting commodity units by product");

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand("dbo.Product_Store_Vendor_Status_GetCommodityUnitsByProduct", connection)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };

                command.Parameters.AddWithValue("@Id", id);

                SqlDataReader reader;

                try
                {
                    connection.Open();

                    reader = command.ExecuteReader();

                    _logger.Info($"DAL.{nameof(CommodityUnitDao)}.{nameof(GetCommodityUnitsByProduct)}: Connected to database");
                }
                catch (InvalidOperationException ex)
                {
                    _logger.Error($"DAL.{nameof(CommodityUnitDao)}.{nameof(GetCommodityUnitsByProduct)}: Not connected to database: " + ex.Message);

                    throw new SystemException("Connection error", ex);
                }

                int? StatusID, StoreID, VendorID;

                while (reader.Read())
                {
                    StatusID = (int?)reader["StatusID"];
                    StoreID = (int?)reader["StoreID"];
                    VendorID = (int?)reader["VendorID"];

                    yield return new CommodityUnit(
                    (int?)reader["Id"],
                    _productDao.GetProduct(id),
                    StoreID is null ? null : _storeDao.GetStore(StoreID ?? -1),
                    VendorID is null ? null : _vendorDao.GetVendor(VendorID ?? -1),
                    StatusID is null ? null : _statusDao.GetStatus(StatusID ?? -1),
                    (decimal)reader["Price"]);
                }
            }

            _logger.Info($"DAL.{nameof(CommodityUnitDao)}.{nameof(GetCommodityUnitsByProduct)}: Commodity units by product received");

            yield break;
        }

        public IEnumerable<CommodityUnit> GetCommodityUnitsByStatus(int id)
        {
            _logger.Info($"DAL.{nameof(CommodityUnitDao)}.{nameof(GetCommodityUnitsByStatus)}: Getting commodity units by status");

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand("dbo.Product_Store_Vendor_Status_GetCommodityUnitsByStatus", connection)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };

                command.Parameters.AddWithValue("@Id", id);

                SqlDataReader reader;

                try
                {
                    connection.Open();

                    reader = command.ExecuteReader();

                    _logger.Info($"DAL.{nameof(CommodityUnitDao)}.{nameof(GetCommodityUnitsByStatus)}: Connected to database");
                }
                catch (InvalidOperationException ex)
                {
                    _logger.Error($"DAL.{nameof(CommodityUnitDao)}.{nameof(GetCommodityUnitsByStatus)}: Not connected to database: " + ex.Message);

                    throw new SystemException("Connection error", ex);
                }

                int? ProductID, StoreID, VendorID;

                while (reader.Read())
                {
                    ProductID = (int?)reader["ProductID"];
                    StoreID = (int?)reader["StoreID"];
                    VendorID = (int?)reader["VendorID"];

                    yield return new CommodityUnit(
                    (int?)reader["Id"],
                    ProductID is null ? null : _productDao.GetProduct(ProductID ?? -1),
                    StoreID is null ? null : _storeDao.GetStore(StoreID ?? -1),
                    VendorID is null ? null : _vendorDao.GetVendor(VendorID ?? -1),
                    _statusDao.GetStatus(id),
                    (decimal)reader["Price"]);
                }
            }

            _logger.Info($"DAL.{nameof(CommodityUnitDao)}.{nameof(GetCommodityUnitsByStatus)}: Commodity units by status received");

            yield break;
        }

        public IEnumerable<CommodityUnit> GetCommodityUnitsByStore(int id)
        {
            _logger.Info($"DAL.{nameof(CommodityUnitDao)}.{nameof(GetCommodityUnitsByStore)}: Getting commodity units by store");

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand("dbo.Product_Store_Vendor_Status_GetCommodityUnitsByStore", connection)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };

                command.Parameters.AddWithValue("@Id", id);

                SqlDataReader reader;

                try
                {
                    connection.Open();

                    reader = command.ExecuteReader();

                    _logger.Info($"DAL.{nameof(CommodityUnitDao)}.{nameof(GetCommodityUnitsByStore)}: Connected to database");
                }
                catch (InvalidOperationException ex)
                {
                    _logger.Error($"DAL.{nameof(CommodityUnitDao)}.{nameof(GetCommodityUnitsByStore)}: Not connected to database: " + ex.Message);

                    throw new SystemException("Connection error", ex);
                }

                int? ProductID, StatusID, VendorID;

                while (reader.Read())
                {
                    ProductID = (int?)reader["ProductID"];
                    VendorID = (int?)reader["VendorID"];
                    StatusID = (int?)reader["StatusID"];

                    yield return new CommodityUnit(
                    (int?)reader["Id"],
                    ProductID is null ? null : _productDao.GetProduct(ProductID ?? -1),
                    _storeDao.GetStore(id),
                    VendorID is null ? null : _vendorDao.GetVendor(VendorID ?? -1),
                    StatusID is null ? null : _statusDao.GetStatus(StatusID ?? -1),
                    (decimal)reader["Price"]);
                }
            }

            _logger.Info($"DAL.{nameof(CommodityUnitDao)}.{nameof(GetCommodityUnitsByStore)}: Commodity units by store received");

            yield break;
        }

        public IEnumerable<CommodityUnit> GetCommodityUnitsByVendor(int id)
        {
            _logger.Info($"DAL.{nameof(CommodityUnitDao)}.{nameof(GetCommodityUnitsByVendor)}: Getting commodity units by vendor");

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand("dbo.Product_Store_Vendor_Status_GetCommodityUnitsByVendor", connection)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };

                command.Parameters.AddWithValue("@Id", id);

                SqlDataReader reader;

                try
                {
                    connection.Open();

                    reader = command.ExecuteReader();

                    _logger.Info($"DAL.{nameof(CommodityUnitDao)}.{nameof(GetCommodityUnitsByVendor)}: Connected to database");
                }
                catch (InvalidOperationException ex)
                {
                    _logger.Error($"DAL.{nameof(CommodityUnitDao)}.{nameof(GetCommodityUnitsByVendor)}: Not connected to database: " + ex.Message);

                    throw new SystemException("Connection error", ex);
                }

                int? ProductID, StatusID, StoreID;

                while (reader.Read())
                {
                    ProductID = (int?)reader["ProductID"];
                    StoreID = (int?)reader["StoreID"];
                    StatusID = (int?)reader["StatusID"];

                    yield return new CommodityUnit(
                    (int?)reader["Id"],
                    ProductID is null ? null : _productDao.GetProduct(ProductID ?? -1),
                    StoreID is null ? null : _storeDao.GetStore(StoreID ?? -1),
                    _vendorDao.GetVendor(id),
                    StatusID is null ? null : _statusDao.GetStatus(StatusID ?? -1),
                    (decimal)reader["Price"]);
                }
            }

            _logger.Info($"DAL.{nameof(CommodityUnitDao)}.{nameof(GetCommodityUnitsByVendor)}: Commodity units by vendor received");

            yield break;
        }

        public bool IsCommodityUnit(int id)
        {
            _logger.Info($"DAL.{nameof(CommodityUnitDao)}.{nameof(IsCommodityUnit)}: Checking the commodity unit by id = {id}");

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand("dbo.Product_Store_Vendor_Status_GetCommodityUnit", connection)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };

                command.Parameters.AddWithValue("@Id", id);

                SqlDataReader reader;

                try
                {
                    connection.Open();

                    reader = command.ExecuteReader();

                    _logger.Info($"DAL.{nameof(CommodityUnitDao)}.{nameof(IsCommodityUnit)}: Connected to database");
                }
                catch (InvalidOperationException ex)
                {
                    _logger.Error($"DAL.{nameof(CommodityUnitDao)}.{nameof(IsCommodityUnit)}: Not connected to database: " + ex.Message);

                    throw new SystemException("Connection error", ex);
                }

                if (reader.Read())
                {
                    _logger.Info($"DAL.{nameof(CommodityUnitDao)}.{nameof(IsCommodityUnit)}: Commodity unit by id = {id} found");

                    return true;
                }

                _logger.Info($"DAL.{nameof(CommodityUnitDao)}.{nameof(IsCommodityUnit)}: Commodity unit by id = {id} not found");

                return false;
            }
        }

        public void RemoveCommodityUnit(int id)
        {
            _logger.Info($"DAL.{nameof(CommodityUnitDao)}.{nameof(RemoveCommodityUnit)}: Remove commodity unit by id = {id}");

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    SqlCommand command = new SqlCommand("dbo.Product_Store_Vendor_Status_RemoveCommodityUnit", connection)
                    {
                        CommandType = System.Data.CommandType.StoredProcedure
                    };

                    command.Parameters.AddWithValue("@Id", id);

                    connection.Open();

                    _logger.Info($"DAL.{nameof(CommodityUnitDao)}.{nameof(RemoveCommodityUnit)}: Connected to database");

                    command.ExecuteNonQuery();

                    _logger.Info($"DAL.{nameof(CommodityUnitDao)}.{nameof(RemoveCommodityUnit)}: Commodity unit id = {id} removed");
                }
            }
            catch (InvalidOperationException ex)
            {
                _logger.Error($"DAL.{nameof(CommodityUnitDao)}.{nameof(RemoveCommodityUnit)}: Not connected to database: " + ex.Message);

                throw new SystemException("Connection error", ex);
            }
        }
    }
}
