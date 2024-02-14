using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CargoApi.Models;
using Microsoft.AspNetCore.Cors;
using System.Net.Mail;

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

     
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Driver>>> GetDrivers()
        {
          if (_context.Drivers == null)
          {
              return NotFound();
          }
            return await _context.Drivers.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Driver>> GetDriver(int id)
        {
          if (_context.Drivers == null)
          {
              return NotFound();
          }
            var driver = await _context.Drivers.FindAsync(id);

            if (driver == null)
            {
                return NotFound();
            }

            return driver;
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> PutDriver(int id, Driver driver)
        {
            if (id != driver.Id)
            {
                return BadRequest();
            }

            _context.Entry(driver).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DriverExists(id))
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
                            res = SendEmail(driverDetail);
                        }
                        else
                        {
                           res = SendEmail(shipment);

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

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDriver(int id)
        {
            if (_context.Drivers == null)
            {
                return NotFound();
            }
            var driver = await _context.Drivers.FindAsync(id);
            if (driver == null)
            {
                return NotFound();
            }

            _context.Drivers.Remove(driver);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DriverExists(int id)
        {
            return (_context.Drivers?.Any(e => e.Id == id)).GetValueOrDefault();
        }


        //outside driver
        private bool SendEmail(Driver dr)
        {
            try
            {
                var body = $"Here are the Driver details for {dr.Nme}:\n";
                body += $"Shipment Number: {dr.ShptNmbr}\n";
                // body += $"Dimension: {shipmentData.Dmnsn}\n";
                // body += $"Weight: {shipmentData.Wght}\n";
                body += $"Carrier Name: {dr.Carir_Nme}\n";
                body += $"License Plate Number: {dr.Lcns_Plt_Nmbr}\n";
                //body += $"Quantity: {shipmentData.Qnty}\n";

                string smtpServer = "smtp.gmail.com";
                int smtpPort = 587;
                string username = "pwswarehouseportal@gmail.com";
                string password = "rauu ksch fzxs zqvr";

                var fromAddress = new MailAddress("pwswarehouseportal@gmail.com", "Priority WorldWide");
                //var toAddress = new MailAddress(shipmentData.Rpnt, "Receiver");
                var toAddress = new List<MailAddress>
                {
                    new MailAddress(dr.Rpnt, "Receiver")
                    //new MailAddress(dr.CstmRpnt,"Receiver")

                };


                var images = ShipmentController.GetImagesByPrefix("Id"+dr.ShptNmbr);

                // Create and configure the email message
                MailMessage message = new MailMessage();
                message.From = fromAddress;
                foreach (var to in toAddress)
                {
                    message.To.Add(to);
                }
                message.Subject = $"Update of Shipment Details - Shipment No. {dr.ShptNmbr} - Driver's Details";
                message.Body = body;

                // Attach images to the email
                foreach (var imageName in images)
                {
                    try
                    {
                        string imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", imageName);
                        Attachment attachment = new Attachment(imagePath);
                        message.Attachments.Add(attachment);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error attaching image '{imageName}': {ex.Message}");
                    }
                }

                // Send the email
                SmtpClient smtp = new SmtpClient(smtpServer);
                smtp.Port = smtpPort; //  SMTP port number
                smtp.Credentials = new System.Net.NetworkCredential(username, password);
                smtp.EnableSsl = true; // Set to true if you are using SSL      
                smtp.Send(message);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }




        //priority driver
        private bool SendEmail(Shipment shipmentData)
        {
            try
            {
                var body = $"Here are the Driver details for {shipmentData.Name}:\n";
                body += $"Shipment Number: {shipmentData.ShptNmbr}\n";
                // body += $"Dimension: {shipmentData.Dmnsn}\n";
                // body += $"Weight: {shipmentData.Wght}\n";
                body += $"Location: {shipmentData.Locn}\n";
                body += $"Note: {shipmentData.Note}\n";
                body += $"Quantity: {shipmentData.Qnty}\n";

                string smtpServer = "smtp.gmail.com";
                int smtpPort = 587;
                string username = "pwswarehouseportal@gmail.com";
                string password = "rauu ksch fzxs zqvr";

                var fromAddress = new MailAddress("pwswarehouseportal@gmail.com", "Priority WorldWide");
                //var toAddress = new MailAddress(shipmentData.Rpnt, "Receiver");
                var toAddress = new List<MailAddress>
                {
                    new MailAddress(shipmentData.Rpnt, "Receiver")
                   // new MailAddress(shipmentData.CstmRpnt,"Receiver")

                };


                // var images = GetImagesByPrefix(shipmentData.ShptNmbr);

                // Create and configure the email message
                MailMessage message = new MailMessage();
                message.From = fromAddress;
                foreach (var to in toAddress)
                {
                    message.To.Add(to);
                }
                message.Subject = $"Update of Shipment Details - Shipment No. {shipmentData.ShptNmbr} - Driver's Details";
                message.Body = body;

                // Attach images to the email
                //foreach (var imageName in images)
                //{
                //    try
                //    {
                //        string imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", imageName);
                //        Attachment attachment = new Attachment(imagePath);
                //        message.Attachments.Add(attachment);
                //    }
                //    catch (Exception ex)
                //    {
                //        Console.WriteLine($"Error attaching image '{imageName}': {ex.Message}");
                //    }
                //}

                // Send the email
                SmtpClient smtp = new SmtpClient(smtpServer);
                smtp.Port = smtpPort; //  SMTP port number
                smtp.Credentials = new System.Net.NetworkCredential(username, password);
                smtp.EnableSsl = true; // Set to true if you are using SSL      
                smtp.Send(message);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

    }
}
