using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CargoApi.Models;
using Microsoft.AspNetCore.Cors;

namespace CargoApi.Controllers
{
    [EnableCors("AllowOrigin")]
    [Route("api/[controller]")]
    [ApiController]
    public class Order_DriverController : ControllerBase
    {
        private readonly PRIORITY_WWDContext _context;

        public Order_DriverController(PRIORITY_WWDContext context)
        {
            _context = context;
        }

       
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order_Driver>>> GetOrder_Drivers()
        {
          if (_context.Order_Drivers == null)
          {
              return NotFound();
          }
            return await _context.Order_Drivers.ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<Order_Driver>> PostDriverDetail(Order_Driver driverDetail)
        {

            if (_context.Order_Drivers == null)
            {
                return Problem("Entity set 'PRIORITY_WWDContext.DriverDetails'  is null.");
            }
            try
            {
                using (var transaction = _context.Database.BeginTransaction())
                {
                    try
                    {

                        _context.Order_Drivers.Add(driverDetail);
                        await _context.SaveChangesAsync();

                        var shipment = _context.Orders.FirstOrDefault(s => s.ShptNmbr == driverDetail.ShptNmbr);
                        if (shipment != null)
                        {
                            shipment.Sts = "Published";
                            await _context.SaveChangesAsync();
                        }

                        transaction.Commit();
                        driverDetail.ShptNmbrNavigationOrderDriver = null;
                        return Ok(driverDetail);

                    }
                    catch (Exception ex)
                    {
                        // Rollback the transaction in case of an error
                        transaction.Rollback();
                        return BadRequest("Failed to add deriver details.");
                    }
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder_Driver(int id)
        {
            if (_context.Order_Drivers == null)
            {
                return NotFound();
            }
            var order_Driver = await _context.Order_Drivers.FindAsync(id);
            if (order_Driver == null)
            {
                return NotFound();
            }

            _context.Order_Drivers.Remove(order_Driver);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool Order_DriverExists(int id)
        {
            return (_context.Order_Drivers?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
