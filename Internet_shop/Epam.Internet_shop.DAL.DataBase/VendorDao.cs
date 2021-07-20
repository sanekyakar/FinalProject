using System;
using System.Configuration;
using System.Collections.Generic;
using System.Data.SqlClient;

using Epam.Internet_shop.Logger.Contracts;
using Epam.Internet_shop.DAL.Contracts;
using Epam.Internet_shop.Common.Entities.CommodityUnit;

namespace Epam.Internet_shop.DAL.DataBase
{
    public class VendorDao : IVendorDao
    {
        private readonly ILogger _logger;
        private readonly string _connectionString;

        public VendorDao(ILogger logger)
        {
            _logger = logger;

            _connectionString = ConfigurationManager.ConnectionStrings["default"].ConnectionString;
        }

        public int AddVendor(Vendor vendor)
        {
            _logger.Info($"DAL.{nameof(VendorDao)}.{nameof(AddVendor)}: Adding a vendor");

            int id;

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    SqlCommand command = new SqlCommand("dbo.Vendor_AddVendor", connection)
                    {
                        CommandType = System.Data.CommandType.StoredProcedure
                    };

                    command.Parameters.AddWithValue("@Name", vendor.Name);

                    connection.Open();

                    _logger.Info($"DAL.{nameof(VendorDao)}.{nameof(AddVendor)}: Connected to database");

                    int.TryParse(command.ExecuteScalar().ToString(), out id);
                }
            }
            catch (InvalidOperationException ex)
            {
                _logger.Error($"DAL.{nameof(VendorDao)}.{nameof(AddVendor)}: Not connected to database: " + ex.Message);

                throw new SystemException("Connection error", ex);
            }

            _logger.Info($"DAL.{nameof(VendorDao)}.{nameof(AddVendor)}: Vendor id = {id} added");

            return id;
        }

        public int ChangeVendor(Vendor vendor)
        {
            _logger.Info($"DAL.{nameof(VendorDao)}.{nameof(ChangeVendor)}: Vendor change");

            int id = vendor.Id ?? -1;

            if (id == -1)
            {
                _logger.Error($"DAL.{nameof(VendorDao)}.{nameof(ChangeVendor)}: vendor.Id cannot be null");

                throw new ArgumentNullException("vendor.Id cannot be null");
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    SqlCommand command = new SqlCommand("dbo.Vendor_ChangeVendor", connection)
                    {
                        CommandType = System.Data.CommandType.StoredProcedure
                    };

                    command.Parameters.AddWithValue("@Id", vendor.Id);
                    command.Parameters.AddWithValue("@Name", vendor.Name);

                    connection.Open();

                    _logger.Info($"DAL.{nameof(VendorDao)}.{nameof(ChangeVendor)}: Connected to database");

                    command.ExecuteScalar();
                }
            }
            catch (InvalidOperationException ex)
            {
                _logger.Error($"DAL.{nameof(VendorDao)}.{nameof(ChangeVendor)}: Not connected to database: " + ex.Message);

                throw new SystemException("Connection error", ex);
            }

            _logger.Info($"DAL.{nameof(VendorDao)}.{nameof(ChangeVendor)}: Vendor id = {id} changed");

            return id;
        }

        public IEnumerable<Vendor> GetAllVendors()
        {
            _logger.Info($"DAL.{nameof(VendorDao)}.{nameof(GetAllVendors)}: Getting all vendors");

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand("dbo.Vendor_GetAllVendors", connection)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };

                SqlDataReader reader;

                try
                {
                    connection.Open();

                    reader = command.ExecuteReader();

                    _logger.Info($"DAL.{nameof(VendorDao)}.{nameof(GetAllVendors)}: Connected to database");
                }
                catch (InvalidOperationException ex)
                {
                    _logger.Error($"DAL.{nameof(VendorDao)}.{nameof(GetAllVendors)}: Not connected to database: " + ex.Message);

                    throw new SystemException("Connection error", ex);
                }

                while (reader.Read())
                {
                    yield return new Vendor((int?)reader["Id"], (string)reader["Name"]);
                }
            }

            _logger.Info($"DAL.{nameof(VendorDao)}.{nameof(GetAllVendors)}: All vendor received");

            yield break;
        }

        public Vendor GetVendor(int id)
        {
            _logger.Info($"DAL.{nameof(VendorDao)}.{nameof(GetVendor)}: Getting category by id = {id}");

            Vendor vendor;

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand("dbo.Vendor_GetVendor", connection)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };

                command.Parameters.AddWithValue("@Id", id);

                SqlDataReader reader;

                try
                {
                    connection.Open();

                    reader = command.ExecuteReader();

                    _logger.Info($"DAL.{nameof(VendorDao)}.{nameof(GetVendor)}: Connected to database");
                }
                catch (InvalidOperationException ex)
                {
                    _logger.Error($"DAL.{nameof(VendorDao)}.{nameof(GetVendor)}: Not connected to database: " + ex.Message);

                    throw new SystemException("Connection error", ex);
                }

                reader.Read();

                vendor = new Vendor((int?)reader["Id"], (string)reader["Name"]);
            }

            _logger.Info($"DAL.{nameof(VendorDao)}.{nameof(GetVendor)}: Vendor by id = {id} obtained");

            return vendor;
        }

        public bool IsVendor(int id)
        {
            _logger.Info($"DAL.{nameof(VendorDao)}.{nameof(IsVendor)}: Checking the vendor by id = {id}");

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand("dbo.Vendor_GetVendor", connection)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };

                command.Parameters.AddWithValue("@Id", id);

                SqlDataReader reader;

                try
                {
                    connection.Open();

                    reader = command.ExecuteReader();

                    _logger.Info($"DAL.{nameof(VendorDao)}.{nameof(IsVendor)}: Connected to database");
                }
                catch (InvalidOperationException ex)
                {
                    _logger.Error($"DAL.{nameof(VendorDao)}.{nameof(IsVendor)}: Not connected to database: " + ex.Message);

                    throw new SystemException("Connection error", ex);
                }

                if (reader.Read())
                {
                    _logger.Info($"DAL.{nameof(VendorDao)}.{nameof(IsVendor)}: Vendor by id = {id} found");

                    return true;
                }

                _logger.Info($"DAL.{nameof(VendorDao)}.{nameof(IsVendor)}: Vendor by id = {id} not found");

                return false;
            }
        }

        public void RemoveVendor(int id)
        {
            _logger.Info($"DAL.{nameof(VendorDao)}.{nameof(RemoveVendor)}: Remove vendor by id = {id}");

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    SqlCommand command = new SqlCommand("dbo.Vendor_RemoveVendor", connection)
                    {
                        CommandType = System.Data.CommandType.StoredProcedure
                    };

                    command.Parameters.AddWithValue("@Id", id);

                    connection.Open();

                    _logger.Info($"DAL.{nameof(VendorDao)}.{nameof(RemoveVendor)}: Connected to database");

                    command.ExecuteNonQuery();

                    _logger.Info($"DAL.{nameof(VendorDao)}.{nameof(RemoveVendor)}: Vendor id = {id} removed");
                }
            }
            catch (InvalidOperationException ex)
            {
                _logger.Error($"DAL.{nameof(VendorDao)}.{nameof(RemoveVendor)}: Not connected to database: " + ex.Message);

                throw new SystemException("Connection error", ex);
            }
        }
    }
}
