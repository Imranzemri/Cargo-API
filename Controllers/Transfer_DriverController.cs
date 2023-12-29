using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CargoApi.Models;
using Microsoft.AspNetCore.Cors;
using CargoApi.Custom_Models;

namespace CargoApi.Controllers
{
    [EnableCors("AllowOrigin")]
    [Route("api/[controller]")]
    [ApiController]
    public class Transfer_DriverController : ControllerBase
    {
        private readonly PRIORITY_WWDContext _context;

        public Transfer_DriverController(PRIORITY_WWDContext context)
        {
            _context = context;
        }




        [HttpPost]
        public async Task<ActionResult<Transfer_Driver>> PostDriverDetail(Transfer_Driver driverDetail)
        {

            if (_context.Transfer_Drivers == null)
            {
                return Problem("Entity set 'PRIORITY_WWDContext.DriverDetails'  is null.");
            }
            try
            {
                using (var transaction = _context.Database.BeginTransaction())
                {
                    try
                    {

                        _context.Transfer_Drivers.Add(driverDetail);
                        await _context.SaveChangesAsync();

                        var shipment = _context.Transfers.FirstOrDefault(s => s.ShptNmbr == driverDetail.ShptNmbr);
                        if (shipment != null)
                        {
                            shipment.Sts = "Published";
                            await _context.SaveChangesAsync();
                        }

                        transaction.Commit();
                        driverDetail.ShptNmbrNavigationTransferDriver = null;
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
        public async Task<IActionResult> DeleteTransfer_Driver(int id)
        {
            if (_context.Transfer_Drivers == null)
            {
                return NotFound();
            }
            var transfer_Driver = await _context.Transfer_Drivers.FindAsync(id);
            if (transfer_Driver == null)
            {
                return NotFound();
            }

            _context.Transfer_Drivers.Remove(transfer_Driver);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool Transfer_DriverExists(int id)
        {
            return (_context.Transfer_Drivers?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
