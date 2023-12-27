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

namespace CargoApi.Controllers
{
    [EnableCors("AllowOrigin")]
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
        public async Task<ActionResult<IEnumerable<ShipmentHelper>>> GetShipments(int page, int pageSize)
        {
            if (_context.Shipments == null)
            {
                return NotFound();
            }
            int skip = (page - 1) * pageSize;
             
            var data =  _context.Shipments
                                .Where(x => x.Sts == "Draft")
                                .Skip(skip)
                                .Take(pageSize)
                                .Select(x=>new Shipment { ShptNmbr=x.ShptNmbr,Name=x.Name,Locn=x.Locn,Qnty=x.Qnty})
                                .ToList();
            int totalCount = _context
                                    .Shipments
                                    .Count();
            List<ShipmentHelper> listshipmentHelper = new List<ShipmentHelper>();
  
            foreach (var item in data)
            {
              var res =  _context.Fixtures
                                 .Where(x => x.ShptNmbr==item.ShptNmbr)
                                 .ToList();
                #region Calculate Total Weight
                decimal? totalkgs = 0;
                decimal? totallbs = 0;
                foreach (var f in res)
                {
                    if(f.Ptype == "Distinct")
                    {
                        if(f.WUnit == "kg")
                        {
                            totalkgs += f.Wght;
                            totallbs += f.Wght * Convert.ToDecimal(2.20462);
                        }
                        if(f.WUnit == "lb")
                        {
                            totallbs+= f.Wght;
                            var wghtinkg = f.Wght / Convert.ToDecimal(2.20462);
                            totalkgs += wghtinkg;
                        }
                    }
                    if(f.Ptype == "Identical")
                    {
                        if(f.WUnit == "kg")
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

                var rcptNmbr= _context.Receipts
                                      .Where(x=>x.ShptNmbr==item.ShptNmbr)
                                      .FirstOrDefault()?.RcptNmbr;
               var mainRcpNo= rcptNmbr.Split('-');

                #endregion
                var shpmntHelper = new ShipmentHelper
                {
                    Name = item.Name,
                    ShpNmbr = item.ShptNmbr,
                    Qnty = item.Qnty,
                    TotalKg = totalkgs,
                    TotalLb=totallbs,
                    RcptNmr = mainRcpNo[0]
                };
                listshipmentHelper.Add(shpmntHelper);
            }
            var response = new {
                Items = listshipmentHelper,
                totalCount = totalCount
            };
            return Ok(response);
        }





        // GET: api/Shipment/5
        //[HttpGet("{id}")]
        //public async Task<ActionResult<Shipment>> GetShipment(int id)
        //{
        //    if (_context.Shipments == null)
        //    {
        //        return NotFound();
        //    }
        //    var shipment = await _context.Shipments.FindAsync(id);





        //    if (shipment == null)
        //    {
        //        return NotFound();
        //    }





        //    return shipment;
        //}





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
            try
            {
                // Begin a transaction
                using (var transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                       if(_context.Shipments.Any(x => x.ShptNmbr == shipmentData.ShptNmbr))
                        {
                            transaction.Rollback();
                            return BadRequest("Duplicate Shipment Number");
                        }
                        // Create a new Shipment
                        var shipment = new Shipment
                        {
                            Name = shipmentData.Name,
                            ShptNmbr = shipmentData.ShptNmbr,
                            Locn = shipmentData.Locn,
                            Note = shipmentData.Note,
                            Imgs = shipmentData.Imgs,
                            Rpnt = shipmentData.Rpnt,
                            CstmRpnt = shipmentData.CstmRpnt,
                            Qnty = shipmentData.Qnty,
                            Sts= shipmentData.Sts,
                        };
                        _context.Shipments.Add(shipment);

                        //Generate Receipt
                        List<string> RcptNumbers = new List<string>();
                       // RcptNumbers = GenerateReceiptNumber(shipmentData.Qnty);
                        foreach (var item in shipmentData.RcptNmbr)
                        {
                            var receipt = new Receipt
                            {
                                RcptNmbr = item.RcptNmbr,
                                ShptNmbr = shipmentData.ShptNmbr,
                            };
                            _context.Receipts.Add(receipt);
                            RcptNumbers.Add(receipt.RcptNmbr);
                        }


                        //Adding Weight and Dimension Data
                        for(int i=0;i<shipmentData.DimensionCollection.Count;i++)
                        {
                            var dim = shipmentData.DimensionCollection[i];
                            var wght = shipmentData.WeightCollection[i];

                            var fixture = new Fixture
                            {
                                ShptNmbr = shipmentData.ShptNmbr,
                                RcptNmbr = dim.RcptNmbr,
                                Length = dim.Lngth,
                                Width = dim.Width,
                                Height = dim.Height,
                                DUnit=dim.DUnit,
                                Wght=wght.Wght,
                                WUnit=wght.WUnit,
                                Ptype=wght.Ptype,
                                Qnty=wght.Qnty,

                            };
                            _context.Fixtures.Add(fixture);
                        }
                           

                        // Save changes to the database
                        await _context.SaveChangesAsync();





                        //var result = SendEmail(shipmentData);
                        var result = true;
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
        private bool SendEmail(Transfer shipmentData)
        {
            try
            {
                string smtpServer = "smtp.office365.com";
                int smtpPort = 587;
                string username = "pwswhse@priorityworldwide.com";
                string password = "Winter2023@)@#"; 





                var fromAddress = new MailAddress("pwswhse@priorityworldwide.com", "Priority WorldWide");
                var toAddress = new MailAddress(shipmentData.Rpnt, "Receiver");





                // Customize the email body based on your requirements
                var body = $"Here are the shipment details for {shipmentData.Name}:\n";
                body += $"Shipment Number: {shipmentData.ShptNmbr}\n";
               // body += $"Dimension: {shipmentData.Dmnsn}\n";
               // body += $"Weight: {shipmentData.Wght}\n";
                body += $"Location: {shipmentData.Locn}\n";
                body += $"Note: {shipmentData.Note}\n";
                body += $"Quantity: {shipmentData.Qnty}\n";

                // Create and configure the email message
                MailMessage message = new MailMessage();
                message.From = fromAddress;
                message.To.Add(toAddress);
                message.Subject = "Shipment Details";
                message.Body = body;
                // Attach images to the email
                //foreach (var image in shipmentData.Imgs)
                //{
                //    Attachment attachment = new Attachment(image);
                //    message.Attachments.Add(attachment);
                //}
                // Send the email
                SmtpClient smtp = new SmtpClient(smtpServer);
                smtp.Port = smtpPort; // Your SMTP port number
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

        [HttpGet("{id}")]
        public async Task<IActionResult> GenerateReceiptNumber(int qnty,string lastrcpNo)
        {
          
            List<string> rlist = new List<string>();
            if (lastrcpNo=="null")
            {
                lastrcpNo = _context.Receipts
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



        [HttpPost]
        [Route("uploadImage")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UploadImage(IFormFile thumbnail)
        {
            try
            {
                if (thumbnail != null && thumbnail.Length > 0)
                {
                    var fileName = Path.GetFileName(thumbnail.FileName);
                    var uniqueFileName = Guid.NewGuid().ToString() + "_" + fileName; // generate a unique file name to avoid conflicts
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", uniqueFileName);



                    if (!Directory.Exists(Path.GetDirectoryName(filePath)))
                    {
                        Directory.CreateDirectory(Path.GetDirectoryName(filePath));
                    }



                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await thumbnail.CopyToAsync(stream);
                    }







                    return Ok("File uploaded successfully");
                }
                else
                {
                    return BadRequest("Invalid file");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

       
    }
   

   
}