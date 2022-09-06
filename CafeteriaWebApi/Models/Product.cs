using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using MySqlConnector;
using Cafeteria.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.Common;

namespace Cafeteria.Models
{
 
    public class Product
    {
        public int productID { get; set; }
        public string productName { get; set; }
        public int availableQuantity { get; set; }
        public double price { get; set; }





        internal AppDb Db { get; set; }
        public Product()
        {
        }
        internal Product(AppDb db)
        {
            Db = db;
        }

        public async Task<List<Product>> AllProductsAsync()
        {
            using var cmd = Db.Connection.CreateCommand();
            string spName = @"get_all_products";
            cmd.CommandText = spName;
            cmd.CommandType = CommandType.StoredProcedure;
            return await ReadAllAsync(await cmd.ExecuteReaderAsync());
        }

        public async Task<Product> GetProduct(int id)
        {
            using var cmd = Db.Connection.CreateCommand();
            string spName = @"get_one_product";
            cmd.CommandText = spName;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@id",
                DbType = DbType.String,
                Value = id,
            });
           
            var result = await ReadAllAsync(await cmd.ExecuteReaderAsync());
            if (result.Count > 0)
                return result[0];
            return null;
        }
        public async Task InsertAsync()
        {
            using var cmd = Db.Connection.CreateCommand();
            string spName = @"add_product";
            cmd.CommandText = spName;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@pName",
                DbType = DbType.String,
                Value = productName,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@quantity",
                DbType = DbType.String,
                Value = availableQuantity,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@p",
                DbType = DbType.String,
                Value = price,
            });
            await cmd.ExecuteNonQueryAsync();
            productID = (int)cmd.LastInsertedId;
        }
        public async Task UpdateAsync()
        {
            using var cmd = Db.Connection.CreateCommand();
            string spName = @"change_quantity";
            cmd.CommandText = spName;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@id",
                DbType = DbType.Int32,
                Value = productID,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@newQuantity",
                DbType = DbType.Int32,
                Value = availableQuantity,
            });
            await cmd.ExecuteNonQueryAsync();
        }

        private async Task<List<Product>> ReadAllAsync(DbDataReader reader)
        {
            var products = new List<Product>();
            using (reader)
            {
                while (await reader.ReadAsync())
                {
                    var product = new Product(Db)
                    {
                        productID = reader.GetInt32(0),
                        productName = reader.GetString(1),
                        availableQuantity = reader.GetInt32(2),
                        price = reader.GetFloat(3)
                    };
                    products.Add(product);
                }
            }
            return products;
        }
    }
}
