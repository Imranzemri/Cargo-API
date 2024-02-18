using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CargoApi.Models;
using System.Text.RegularExpressions;
using System.Net.Mail;
using Microsoft.AspNetCore.Cors;
using System.Net;
using CargoApi.Custom_Models;
using CargoApi.Helper_Methods;

namespace CargoApi.Controllers
{
    [EnableCors("AllowOrigin")]
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly PRIORITY_WWDContext _context;





        public OrderController(PRIORITY_WWDContext context)
        {
            _context = context;
        }



        [HttpGet]
        public async Task<ActionResult<IEnumerable<ShipmentHelper>>> GetOrders(int page, int pageSize)
        {
            if (_context.Orders == null)
            {
                return NotFound();
            }
            int skip = (page - 1) * pageSize;

            var data = _context.Orders
                                .Where(x => x.Sts == "Draft")
                                .Skip(skip)
                                .Take(pageSize)
                                //.Select(x => new t { ShptNmbr = x.ShptNmbr, Name = x.Name, Locn = x.Locn, Qnty = x.Qnty })
                                .ToList();
            int totalCount = _context
                                    .Orders
                                    .Count();
            List<ShipmentHelper> listshipmentHelper = new List<ShipmentHelper>();

            foreach (var item in data)
            {
                var res = _context.Order_Fixtures
                                   .Where(x => x.ShptNmbr == item.ShptNmbr)
                                   .ToList();
                #region Calculate Total Weight
                decimal? totalkgs = 0;
                decimal? totallbs = 0;
                foreach (var f in res)
                {
                    if (f.Ptype == "Distinct")
                    {
                        if (f.WUnit == "kg")
                        {
                            totalkgs += f.Wght;
                            totallbs += f.Wght * Convert.ToDecimal(2.20462);
                        }
                        if (f.WUnit == "lb")
                        {
                            totallbs += f.Wght;
                            var wghtinkg = f.Wght / Convert.ToDecimal(2.20462);
                            totalkgs += wghtinkg;
                        }
                    }
                    if (f.Ptype == "Identical")
                    {
                        if (f.WUnit == "kg")
                        {
                            totalkgs += f.Wght * f.Qnty;
                            var kgs = f.Wght * f.Qnty;
                            totallbs += kgs * Convert.ToDecimal(2.20462);
                        }
                        if (f.WUnit == "lb")
                        {
                            totallbs += f.Wght * f.Qnty;
                            var lbs = f.Wght * f.Qnty;
                            totalkgs += lbs / Convert.ToDecimal(2.20462);
                        }
                    }
                }
                #endregion
                #region Get Receipt Number

                var rcptNmbr = _context.Order_Receipts
                                      .Where(x => x.ShptNmbr == item.ShptNmbr)
                                      .FirstOrDefault()?.RcptNmbr;
                var mainRcpNo = rcptNmbr.Split('-');

                #endregion
                var shpmntHelper = new ShipmentHelper
                {
                    Name = item.Name,
                    ShpNmbr = item.ShptNmbr,
                    Qnty = item.Qnty,
                    TotalKg = totalkgs,
                    TotalLb = totallbs,
                    RcptNmr = mainRcpNo[0]
                };
                listshipmentHelper.Add(shpmntHelper);
            }
            var response = new
            {
                Items = listshipmentHelper,
                totalCount = totalCount
            };
            return Ok(response);
        }





        [HttpPost]
        public async Task<IActionResult> CreateShipment([FromBody] Order shipmentData)
        {
            try
            {
                // Begin a transaction
                using (var transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        if (_context.Orders.Any(x => x.ShptNmbr == shipmentData.ShptNmbr))
                        {
                            transaction.Rollback();
                            return BadRequest("Duplicate Shipment Number");
                        }
                        // Create a new Shipment
                        var shipment = new Order
                        {
                            Name = shipmentData.Name,
                            ShptNmbr = shipmentData.ShptNmbr,
                            Locn = shipmentData.Locn,
                            Note = shipmentData.Note,
                            Imgs = shipmentData.Imgs,
                            Rpnt = shipmentData.Rpnt,
                            CstmRpnt = shipmentData.CstmRpnt,
                            Qnty = shipmentData.Qnty,
                            Sts = shipmentData.Sts,
                        };
                        _context.Orders.Add(shipment);

                        //Generate Receipt
                        List<string> RcptNumbers = new List<string>();
                        // RcptNumbers = GenerateReceiptNumber(shipmentData.Qnty);
                        foreach (var item in shipmentData.RcptNmbr)
                        {
                            var receipt = new Order_Receipt
                            {
                                RcptNmbr = item.RcptNmbr,
                                ShptNmbr = shipmentData.ShptNmbr,
                            };
                            _context.Order_Receipts.Add(receipt);
                            RcptNumbers.Add(receipt.RcptNmbr);
                        }


                        //Adding Weight and Dimension Data
                        for (int i = 0; i < shipmentData.DimensionCollection.Count; i++)
                        {
                            var dim = shipmentData.DimensionCollection[i];
                            var wght = shipmentData.WeightCollection[i];

                            var fixture = new Order_Fixture
                            {
                                ShptNmbr = shipmentData.ShptNmbr,
                                RcptNmbr = dim.RcptNmbr,
                                Length = dim.Lngth,
                                Width = dim.Width,
                                Height = dim.Height,
                                DUnit = dim.DUnit,
                                Wght = wght.Wght,
                                WUnit = wght.WUnit,
                                Ptype = wght.Ptype,
                                Qnty = wght.Qnty

                            };
                            _context.Order_Fixtures.Add(fixture);
                        }


                        // Save changes to the database
                        await _context.SaveChangesAsync();




                        Tuple<string, string, string, string, string, string, int?> items =
                               new Tuple<string, string, string, string, string, string, int?>(
                                   shipmentData.Name, shipmentData.ShptNmbr, shipmentData.Locn, shipmentData.Note,
                                   shipmentData.Rpnt, shipmentData.CstmRpnt, shipmentData.Qnty
                                   );
                        var result = HelperMethods.SendEmail(items,"ORDER");
                        if (result)
                        {
                            // Commit the transaction
                            transaction.Commit();
                            return Ok(RcptNumbers);
                        }
                        else /*(result is StatusCodeResult)*/
                        {
                            // Rollback the transaction in case of an error
                            transaction.Rollback();
                            return BadRequest("Failure sending mail");
                        }
                    }
                    catch (Exception ex)
                    {
                        // Rollback the transaction in case of an error
                        transaction.Rollback();
                        return BadRequest("Failed to create the shipment and related records.");
                    }
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }




        [HttpGet("{id}")]
        public async Task<IActionResult> GenerateReceiptNumber(int qnty, string lastrcpNo)
        {

            List<string> rlist = new List<string>();
            if (lastrcpNo == "null")
            {
                lastrcpNo = _context.Order_Receipts
                                         .OrderByDescending(x => x.RcptNmbr)
                                         .Select(x => x.RcptNmbr)
                                         .FirstOrDefault() ?? null;
                if (lastrcpNo != null)
                {
                    string[] sequenceParts = lastrcpNo.Split('-');
                    string[] prefixParts = Regex.Split(lastrcpNo, @"(\d+)"); //wr,1000-1





                    if (prefixParts.Length > 0)
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
            }
            else
            {
                string[] sequenceParts = lastrcpNo.Split('-');  //wr1059-2
                //string[] prefixParts = Regex.Split(lastrcpNo, @"(\d+)"); //wr,1000-1
                if (sequenceParts.Length > 0)
                {
                    /// int newPrefixSeqnce = Convert.ToInt32(prefixParts[1]) + 1;
                    // var newPrefix = prefixParts[0] + newPrefixSeqnce;//WR1001
                    int seq = Convert.ToInt32(sequenceParts[1]);
                    for (int i = 1; i <= +qnty; i++)
                    {
                        seq = seq + 1;
                        rlist.Add($"{sequenceParts[0]}-{seq}");
                    }
                }
            }


            //LastrcptNo = rlist.LastOrDefault();
            return Ok(rlist);
        }


    }
}