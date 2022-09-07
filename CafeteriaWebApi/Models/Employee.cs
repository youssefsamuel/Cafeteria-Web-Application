using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;

namespace Cafeteria.Models
{
    public class Employee
    {
        public int EmpID { get; set; }
        public string email { get; set; }
        public string pass { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Address { get; set; }
        public string phonenumber { get; set; }
        public string JOB { get; set; }
        public int salary { get; set; }
        internal AppDb Db { get; set; }
        public Employee()
        {
        }
        internal Employee(AppDb db)
        {
            Db = db;
        }

        public async Task InsertAsync()
        {
            using var cmd = Db.Connection.CreateCommand();
            string spName = @"add_employee";
            cmd.CommandText = spName;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@empEmail",
                DbType = DbType.String,
                Value = email,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@empPass",
                DbType = DbType.String,
                Value = pass,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@lastName",
                DbType = DbType.String,
                Value = LastName,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@firstName",
                DbType = DbType.String,
                Value = FirstName,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@address",
                DbType = DbType.String,
                Value = Address,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@phoneNumber",
                DbType = DbType.String,
                Value = phonenumber,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@job",
                DbType = DbType.String,
                Value = JOB,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@salary",
                DbType = DbType.Int32,
                Value = salary,
            });
            await cmd.ExecuteNonQueryAsync();
            EmpID = (int)cmd.LastInsertedId;
        }

        public async Task<Employee> FindOneAsync(string email, string pass)
        {
            using var cmd = Db.Connection.CreateCommand();
            string spName = @"employee_log";
            cmd.CommandText = spName;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@empEmail",
                DbType = DbType.String,
                Value = email,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@empPass",
                DbType = DbType.String,
                Value = pass,
            });
            var result = await ReadAllAsync(await cmd.ExecuteReaderAsync());
            if (result.Count > 0)
                return result[0];
            return null;
        }

        private async Task<List<Employee>> ReadAllAsync(DbDataReader reader)
        {
            var employees = new List<Employee>();
            using (reader)
            {
                while (await reader.ReadAsync())
                {
                    var emp = new Employee(Db)
                    {
                        EmpID = reader.GetInt32(0),
                        email = reader.GetString(1),
                        pass = reader.GetString(2),
                        LastName = reader.GetString(3),
                        FirstName = reader.GetString(4),
                        Address = reader.GetString(5),
                        phonenumber = reader.GetString(6),
                        JOB = reader.GetString(7),
                        salary = reader.GetInt32(8)
                    };
                    employees.Add(emp);
                }
            }
            return employees;
        }
    }
}
