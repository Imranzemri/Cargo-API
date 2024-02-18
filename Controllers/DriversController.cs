﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CargoApi.Models;
using Microsoft.AspNetCore.Cors;
using System.Net.Mail;
using CargoApi.Helper_Methods;

namespace CargoApi.Controllers
{
    [EnableCors("AllowOrigin")]
    [Route("api/[controller]")]
    [ApiController]
    public class DriversController : ControllerBase
    {
        private readonly PRIORITY_WWDContext _context;

        public DriversController(PRIORITY_WWDContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<ActionResult<Driver>> PostDriverDetail(Driver driverDetail)
        {
           
            if (_context.Drivers == null)
            {
                return Problem("Entity set 'PRIORITY_WWDContext.DriverDetails'  is null.");
            }
            try
            {
                using (var transaction = _context.Database.BeginTransaction())
                {
                    try
                    {

                        _context.Drivers.Add(driverDetail);
                        await _context.SaveChangesAsync();

                        var shipment = _context.Shipments.FirstOrDefault(s => s.ShptNmbr == driverDetail.ShptNmbr);
                        if (shipment != null)
                        {
                            shipment.Sts = "Published";
                           await _context.SaveChangesAsync();
                        }
                        bool res = false;
                        if(driverDetail.Type == "Outside Vendor")
                        {
                            Tuple<string, string, string, string, string> items =
                                new Tuple<string, string, string, string, string>
                                (driverDetail.Nme, driverDetail.ShptNmbr, driverDetail.Carir_Nme, driverDetail.Lcns_Plt_Nmbr, driverDetail.Rpnt);
                            res = HelperMethods.SendEmail(items);
                        }
                        else
                        {
                            Tuple<string, string, string, string, string, int?> items =
                                new Tuple<string, string, string, string, string, int?>
                                (shipment.Name, shipment.ShptNmbr, shipment.Locn, shipment.Note, shipment.Rpnt, shipment.Qnty);
                           res = HelperMethods.SendEmail(items);

                        }
                        if (res)
                        {
                            transaction.Commit();
                            driverDetail.ShptNmbrNavigationDriver = null;
                            return Ok(driverDetail);
                        }
                        else 
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
                        return BadRequest("Failed to add deriver details.");
                    }
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }  

    }
}
