using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
namespace Cafeteria.Models
{
    public class Order
    {
        public int orderID { get; set; }
        public int EmpID { get; set; }
        public int productID { get; set; }
        public int quantityRequired { get; set; }
        public double totalprice { get; set; }
        public DateTime dateandTime { get; set; }
        public int completed { get; set; }
        public string empEmail { get; set; }
        public string productName { get; set; }




        internal AppDb Db { get; set; }
        public Order()
        {
        }
        internal Order(AppDb db)
        {
            Db = db;
        }
        public async Task Insert()
        {
            using var cmd = Db.Connection.CreateCommand();
            string spName = @"Add_order";
            cmd.CommandText = spName;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@empID",
                DbType = DbType.Int32,
                Value = EmpID,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@ProdID",
                DbType = DbType.Int32,
                Value = productID,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@quantityRequired",
                DbType = DbType.Int32,
                Value = quantityRequired,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@totalPrice",
                DbType = DbType.Int32,
                Value = totalprice,
            });
            await cmd.ExecuteNonQueryAsync();
            orderID = (int)cmd.LastInsertedId;
        }
        public async Task<Order> GetOrder(int id)
        {
            using var cmd = Db.Connection.CreateCommand();
            string spName = @"get_one_order";
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
        public async Task UpdateAsync()
        {
            using var cmd = Db.Connection.CreateCommand();
            string spName = @"deliver_order";
            cmd.CommandText = spName;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@id",
                DbType = DbType.Int32,
                Value = orderID,
            });
            await cmd.ExecuteNonQueryAsync();
        }
        public async Task<List<Order>> AllOrdersAsync()
        {
            using var cmd = Db.Connection.CreateCommand();
            string spName = @"get_orders";
            cmd.CommandText = spName;
            cmd.CommandType = CommandType.StoredProcedure;
            return await ReadAllAsync(await cmd.ExecuteReaderAsync());
        }

        private async Task<List<Order>> ReadAllAsync(DbDataReader reader)
        {
            var orders = new List<Order>();
            using (reader)
            {
                while (await reader.ReadAsync())
                {
                    var order = new Order(Db)
                    {
                        orderID = reader.GetInt32(0),
                        EmpID = reader.GetInt32(1),
                        productID = reader.GetInt32(2),
                        quantityRequired = reader.GetInt32(3),
                        totalprice = reader.GetFloat(4),
                        dateandTime = reader.GetDateTime(5),
                        completed = reader.GetInt32(6),
                        empEmail = reader.GetString(8),
                        productName = reader.GetString(17)
                    };
                    orders.Add(order);
                }
            }
            return orders;
        }

    }
}