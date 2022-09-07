using Cafeteria.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cafeteria.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        public AppDb Db { get; }

        public OrderController(AppDb db)
        {
            Db = db;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll() // Get all the orders from the database.
        {
            await Db.Connection.OpenAsync();
            var order = new Order(Db);
            var result = await order.AllOrdersAsync();
            if (result == null)
            {
                return StatusCode(500);
            }
            return new OkObjectResult(result);
        }
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Order order)
        {
            await Db.Connection.OpenAsync();
            order.Db = Db;
            try
            {
                await order.Insert();
                return new OkObjectResult(order);
            }
            catch (Exception)
            {
                return StatusCode(500, "Order cannot happen");
            }
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOne(int id)
        {
            await Db.Connection.OpenAsync();
            var order = new Order(Db);
            var result = await order.GetOrder(id);
            if (result == null)
            {
                return StatusCode(500, "Order not found");
            }
            return new OkObjectResult(result);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOne(int id, [FromBody] Order orderBody)
        {
            await Db.Connection.OpenAsync();
            Order order = new Order(Db);
            Order result = await order.GetOrder(id);
            if (result == null)
                return StatusCode(500, "Product not found!");
            result.completed = orderBody.completed;
            try
            {
                await result.UpdateAsync();
                return new OkObjectResult(result);
            }
            catch (Exception)
            {
                return StatusCode(500);
            }

        }
    }
}