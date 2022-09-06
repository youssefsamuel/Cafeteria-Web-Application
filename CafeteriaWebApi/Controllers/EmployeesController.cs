using Cafeteria.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cafeteria.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        public AppDb Db { get; }

        public EmployeesController(AppDb db)
        {
            Db = db;
        }

        [HttpGet]
        public async Task<IActionResult> GetEmployee(string email, string pass)
        {
            await Db.Connection.OpenAsync();
            var employee = new Employee(Db);
            var result = await employee.FindOneAsync(email, pass);
            if (result == null)
            {
                return StatusCode(500, "Incorrect email or password");
            }
            return new OkObjectResult(result);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Employee employee)
        {
            await Db.Connection.OpenAsync();
            employee.Db = Db;
            try
            {
                await employee.InsertAsync();
                return new OkObjectResult(employee);
            }
            catch(Exception)
            {
                return StatusCode(500, "Email already exists");
            }
            
        }
    }
}
