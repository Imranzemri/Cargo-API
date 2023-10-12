using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CargoApi.Models;
using System.Text.RegularExpressions;

namespace CargoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShipmentController : ControllerBase
    {
        private readonly PRIORITY_WWDContext _context;

        public ShipmentController(PRIORITY_WWDContext context)
        {
            _context = context;
        }

        // GET: api/Shipment
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Shipment>>> GetShipments()
        {
          if (_context.Shipments == null)
          {
              return NotFound();
          }
            return await _context.Shipments.ToListAsync();
        }

        // GET: api/Shipment/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Shipment>> GetShipment(int id)
        {
          if (_context.Shipments == null)
          {
              return NotFound();
          }
            var shipment = await _context.Shipments.FindAsync(id);

            if (shipment == null)
            {
                return NotFound();
            }

            return shipment;
        }

        // PUT: api/Shipment/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutShipment(int id, Shipment shipment)
        {
            if (id != shipment.Id)
            {
                return BadRequest();
            }

            _context.Entry(shipment).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ShipmentExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Shipment
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPost]
        //public async Task<ActionResult<Shipment>> PostShipment(Shipment shipment)
        //{
        //  if (_context.Shipments == null)
        //  {
        //      return Problem("Entity set 'PRIORITY_WWDContext.Shipments'  is null.");
        //  }
        //    _context.Shipments.Add(shipment);
        //    await _context.SaveChangesAsync();

        //    return CreatedAtAction("GetShipment", new { id = shipment.Id }, shipment);
        //}

        // DELETE: api/Shipment/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteShipment(int id)
        {
            if (_context.Shipments == null)
            {
                return NotFound();
            }
            var shipment = await _context.Shipments.FindAsync(id);
            if (shipment == null)
            {
                return NotFound();
            }

            _context.Shipments.Remove(shipment);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ShipmentExists(int id)
        {
            return (_context.Shipments?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        [HttpPost]
        public async Task<IActionResult> CreateShipment([FromBody] Shipment shipmentData)
        {
            // Begin a transaction
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    // Create a new Shipment
                    var shipment = new Shipment
                    {
                        Name = shipmentData.Name,
                        ShptNmbr = shipmentData.ShptNmbr,
                        Dmnsn = shipmentData.Dmnsn,
                        Wght = shipmentData.Wght,
                        Locn = shipmentData.Locn,
                        Note = shipmentData.Note,
                        Imgs = shipmentData.Imgs,
                        Rpnt = shipmentData.Rpnt,
                        CstmRpnt = shipmentData.CstmRpnt,
                        Qnty=shipmentData.Qnty
                    };
                    _context.Shipments.Add(shipment);



                    //Generate Receipt
                    List<string> RcptNumbers = new List<string>();
                    RcptNumbers = GenerateReceiptNumber(shipmentData.Qnty);
                    foreach (var item in RcptNumbers)
                    {
                        var receipt = new Receipt
                        {
                            RcptNmbr = item,
                            ShptNmbr = shipment.ShptNmbr, // Use the ShptNmbr of the created shipment
                        };
                        _context.Receipts.Add(receipt);
                    }




                    // Create Pieces
                    //for (int i = 0; i < shipmentData.Qnty; i++)
                    //{
                    //    var piece = new Piece
                    //    {
                    //        Qnty = 1,
                    //        ShptNmbr = shipment.ShptNmbr, // Use the ShptNmbr of the created shipment
                    //    };
                    //    _context.Pieces.Add(piece);
                    //}



                    // Save changes to the database
                    await _context.SaveChangesAsync();



                    // Commit the transaction
                    transaction.Commit();



                    return Ok("Data saved successfully");
                }
                catch (Exception ex)
                {
                    // Rollback the transaction in case of an error
                    transaction.Rollback();
                    return BadRequest("Failed to create the shipment and related records.");
                }
            }
        }



        public List<string> GenerateReceiptNumber(int? qnty)
        {
            List<string> rlist = new List<string>();
            var lastRcptNumber = _context.Receipts
                                         .OrderByDescending(x => x.RcptNmbr)
                                         .Select(x => x.RcptNmbr)
                                         .FirstOrDefault();
            if (lastRcptNumber != null)
            {
                string[] sequenceParts = lastRcptNumber.Split('-');
                string[] prefixParts = Regex.Split(lastRcptNumber, @"(\d+)"); //wr,1000-1

                if (prefixParts.Length >0)
                {
                    int newPrefixSeqnce = Convert.ToInt32(prefixParts[1]) + 1;
                    var newPrefix = prefixParts[0] + newPrefixSeqnce;//WR1001
                    for (int i = 1; i <= +qnty; i++)
                    {
                        rlist.Add($"{newPrefix}-{i}");
                    }
                }
            }
            else
            {
                for (int i = 1; i <= qnty; i++)
                {
                    rlist.Add($"WR1000-{i}");
                }
            }
            return rlist;
        }
    }
}
