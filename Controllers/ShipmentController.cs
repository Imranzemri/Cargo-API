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
using System.Net.Http;
using CargoApi.Helper_Methods;

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


        [HttpGet("GetShipments")]
        public async Task<ActionResult<IEnumerable<ShipmentHelper>>> GetShipments(int page, int pageSize)
        {
            if (_context.Shipments == null)
            {
                return NotFound();
            }
            int skip = (page - 1) * pageSize;

            var data = _context.Shipments
                                .Where(x => x.Sts != "Published")
                                .Skip(skip)
                                .Take(pageSize)
                                .OrderByDescending(x => x.Id)
                                .ToList();
            int totalCount = _context
                                    .Shipments
                                    .Count(x => x.Sts != "Published");
            List<ShipmentHelper> listshipmentHelper = new List<ShipmentHelper>();

            foreach (var item in data)
            {
                //var res =  _context.Fixtures
                //                   .Where(x => x.ShptNmbr==item.ShptNmbr)
                //                   .ToList();
                #region Calculate Total Weight
                //decimal? totalkgs = 0;
                //decimal? totallbs = 0;
                //foreach (var f in res)
                //{
                //    if(f.Ptype == "Distinct")
                //    {
                //        if(f.WUnit == "kg")
                //        {
                //            totalkgs += f.Wght;
                //            totallbs += f.Wght * Convert.ToDecimal(2.20462);
                //        }
                //        if(f.WUnit == "lb")
                //        {
                //            totallbs+= f.Wght;
                //            var wghtinkg = f.Wght / Convert.ToDecimal(2.20462);
                //            totalkgs += wghtinkg;
                //        }
                //    }
                //    if(f.Ptype == "Identical")
                //    {
                //        if(f.WUnit == "kg")
                //        {
                //            totalkgs += f.Wght * f.Qnty;
                //            var kgs = f.Wght * f.Qnty;
                //            totallbs += kgs * Convert.ToDecimal(2.20462);
                //        }
                //        if (f.WUnit == "lb")
                //        {
                //            totallbs += f.Wght * f.Qnty;
                //            var lbs = f.Wght * f.Qnty;
                //            totalkgs += lbs / Convert.ToDecimal(2.20462);
                //        }
                //    }
                //}
                #endregion
                #region Get Receipt Number

                // var rcptNmbr= _context.Receipts
                //                       .Where(x=>x.ShptNmbr==item.ShptNmbr)
                //                       .FirstOrDefault()?.RcptNmbr;
                //var mainRcpNo= rcptNmbr.Split('-');

                #endregion
                var shpmntHelper = new ShipmentHelper
                {
                    Name = item.Name,
                    ShpNmbr = item.ShptNmbr,
                    InsrDate = item.InsrDate,
                    Sts      = item.Sts
                    //TotalKg = totalkgs,
                    //TotalLb=totallbs,
                    //RcptNmr = mainRcpNo[0]
                };
                listshipmentHelper.Add(shpmntHelper);
            }
            var response = new {
                Items = listshipmentHelper,
                totalCount = totalCount
            };
            return Ok(response);
        }


        [HttpGet("GetShipmentById")]
        public async Task<ActionResult<IEnumerable<ShipmentHelper>>> GetShipmentById(string shpNmbr)
        {
            if (_context.Shipments == null)
            {
                return NotFound();
            }

            var DimensionList = new List<DimensionArrayItem>();
            var WeightList = new List<WeightArrayItem>();
            var data = _context.Shipments
                                .Where(x => x.ShptNmbr == shpNmbr)
                                .OrderByDescending(x => x.Id)
                                .ToList();
            string shpNumber = data?.FirstOrDefault()?.ShptNmbr;
            int totalCount = _context
                                    .Fixtures
                                    .Count(x => x.ShptNmbr == shpNumber);
            List<Shipment> listshipmentHelper = new List<Shipment>();

                var fixtures = _context.Fixtures
                                   .Where(x => x.ShptNmbr == shpNumber)
                                   .ToList();
                foreach (var eachFixture in fixtures)
                {

                    DimensionList.Add(new DimensionArrayItem
                    {
                        Width = Convert.ToDecimal(eachFixture.Width),
                        Height = Convert.ToDecimal(eachFixture.Height),
                        Lngth = Convert.ToDecimal(eachFixture.Length),
                        DUnit = eachFixture.DUnit,
                        Locn = eachFixture.Locn,
                        GoodDesc = eachFixture.GoodDesc,
                        RcptNmbr = eachFixture.RcptNmbr
                    });
                    WeightList.Add(new WeightArrayItem
                    {
                        Wght = Convert.ToDecimal(eachFixture.Wght),
                        WUnit = eachFixture.DUnit,
                        Locn = eachFixture.Locn,
                        GoodDesc = eachFixture.GoodDesc,
                        RcptNmbr = eachFixture.RcptNmbr
                    });
                }
                var shpmntHelper = new Shipment
                {
                    Name = data.FirstOrDefault().Name,
                    ShptNmbr = shpNumber,
                    InsrDate = data.FirstOrDefault().InsrDate,
                    Sts = data.FirstOrDefault().Sts,
                    Qnty = data.FirstOrDefault().Qnty,
                    DimensionCollection = DimensionList,
                    WeightCollection = WeightList
                };
                listshipmentHelper.Add(shpmntHelper);
            
            var response = new
            {
                Items = listshipmentHelper,
                totalCount = totalCount
            };
            return Ok(response);
        }

        [HttpGet("GetUpdatedShipments")]
        public async Task<ActionResult<IEnumerable<ShipmentHelper>>> GetUpdatedShipments(int page, int pageSize)
        {
            if (_context.Shipments == null)
            {
                return NotFound();
            }
            int skip = (page - 1) * pageSize;

            var data = _context.Shipments
                                .Where(x => x.Sts == "Updated")
                                .Skip(skip)
                                .Take(pageSize)
                                .OrderByDescending(x => x.Id)
                                .ToList();
            int totalCount = _context
                                    .Shipments
                                    .Count(x => x.Sts != "Updated");
            List<ShipmentHelper> listshipmentHelper = new List<ShipmentHelper>();

            foreach (var item in data)
            {
                var shpmntHelper = new ShipmentHelper
                {
                    Name = item.Name,
                    ShpNmbr = item.ShptNmbr,
                    InsrDate = item.InsrDate,
                    Sts = item.Sts
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



        // method that will update Goods description and location from review details page
        [HttpPut("UpdateLocationAndDescription")]
        public async Task<IActionResult> UpdateLocationAndDescription([FromBody] UpdateHelperModel model)
        {
            if (!(string.IsNullOrEmpty(model.ShipmentNo)))
            {
                
               var shipmentFixtures = _context.Fixtures.Where(x=>x.RcptNmbr == model.RcptNumber)
                                                       .ToList();
               var shipment = _context.Shipments.Where(s=>s.ShptNmbr == model.ShipmentNo)
                                                .ToList();

                if(!(string.IsNullOrEmpty(model.Description)) && !(string.IsNullOrEmpty(model.Locn)))
                {
                    shipmentFixtures.ForEach(p => p.Locn = model.Locn);
                    shipmentFixtures.ForEach(p => p.GoodDesc = model.Description);
                    shipment.ForEach(s => s.Sts = "Updated");
                }
                else if (!(string.IsNullOrEmpty(model.Locn)))
                {
                    shipmentFixtures.ForEach(p => p.Locn = model.Locn);
                    shipment.ForEach(s => s.Sts = "Updated");
                }
                else
                {
                    shipmentFixtures.ForEach(p => p.GoodDesc = model.Description);
                    shipment.ForEach(s => s.Sts = "Updated");
                }
                await  _context.SaveChangesAsync();
            }
            return Ok();
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
                            PO = shipmentData.PO,
                            TrukNmbr = shipmentData.TrukNmbr,
                            Supp = shipmentData.Supp,
                            InsrDate = DateTime.Now,
                            InsrBy = shipmentData.InsrBy,
                            UpdtDate =  DateTime.Now,
                            UpdtBy = shipmentData.UpdtBy
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
                                Locn = wght.Locn,
                                GoodDesc = wght.GoodDesc,

                            };
                            _context.Fixtures.Add(fixture);
                        }
                           

                        // Save changes to the database
                        await _context.SaveChangesAsync();





                        Tuple<string, string, string, string, string, string,int?> items =
                               new Tuple<string, string, string, string, string, string, int?>(
                                   shipmentData.Name,shipmentData.ShptNmbr,shipmentData.Locn,shipmentData.Note,
                                   shipmentData.Rpnt,shipmentData.CstmRpnt,shipmentData.Qnty
                                   );
                        
                        var result = HelperMethods.SendEmail(items,"RECEIVING");
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




        [HttpGet("CheckDuplicateShipmentNumber")]
        public async Task<IActionResult> CheckDuplicateShipmentNumber(string shipmentNumber)
        {
            try
            {
                bool isDuplicate = await _context.Shipments.AnyAsync(x => x.ShptNmbr == shipmentNumber);

                if (isDuplicate)
                {
                    return Ok("Duplicate Shipment Number");
                }
                else
                {
                    return Ok("Success");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
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




        //[HttpPost]
        //[Route("testemail")]
        //public bool testemail()
        //{
        //    try
        //    {
        //        string smtpServer = "smtp.gmail.com";
        //        int smtpPort = 587;
        //        //string username = "imrankhanzemri@gmail.com";
        //        //string password = "gqgz hqlv rtwh xdtx";
        //        string username = "pwswarehouseportal@gmail.com";
        //        string password = "rauu ksch fzxs zqvr";
        //       // var fromAddress = new MailAddress("imrankhanzemri@gmail.com", "Priority WorldWide");

        //         var fromAddress = new MailAddress("pwswarehouseportal@gmail.com", "Priority WorldWide");
        //        var toAddress = new MailAddress("imrankhanzemri@gmail.com", "Receiver");

        //        // Get the list of image names using the GetImagesByPrefix method
        //        var images = GetImagesByPrefix("Tnb-33");

        //        // Create and configure the email message
        //        MailMessage message = new MailMessage();
        //        message.From = fromAddress;
        //        message.To.Add(toAddress);
        //        message.Subject = "Shipment Details";
        //        message.Body = "Here are the shipment details for:\n";
        //        message.IsBodyHtml = false;

        //        // Attach images to the email
        //        foreach (var imageName in images)
        //        {
        //            try
        //            {
        //                string imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", imageName);
        //                Attachment attachment = new Attachment(imagePath);
        //                message.Attachments.Add(attachment);
        //            }
        //            catch (Exception ex)
        //            {
        //                // Handle exception if the image file is not found
        //                Console.WriteLine($"Error attaching image '{imageName}': {ex.Message}");
        //            }
        //        }

        //        // Set up the SMTP client and send the email
        //        SmtpClient smtp = new SmtpClient(smtpServer);
        //        smtp.Port = smtpPort;
        //        smtp.UseDefaultCredentials = false;
        //        smtp.Credentials = new NetworkCredential(username, password);
        //        smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
        //        smtp.EnableSsl = true;

        //        // Send the email
        //        smtp.Send(message);

        //        return true;
        //    }
        //    catch (Exception e)
        //    {
        //        // Handle the exception
        //        Console.WriteLine($"Error sending email: {e.Message}");
        //        return false;
        //    }
        //}






    }


}