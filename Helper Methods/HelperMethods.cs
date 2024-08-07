﻿using CargoApi.Custom_Models;
using CargoApi.Models;
using Humanizer;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Mvc;
using System.Buffers.Text;
using System.Drawing.Printing;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Xml.Linq;
using static CargoApi.Controllers.ShipmentController;

namespace CargoApi.Helper_Methods
{
    public static class HelperMethods
    {


        #region Shipments Email
        public static bool SendEmail(Tuple<string, string, string, string, string, string, int?> shipmentData,string shpType)
        {
            try
            {
                var body = $"Here are the shipment details for {shipmentData.Item1}:\n";
                body += $"Shipment Number: {shipmentData.Item2}\n";
                // body += $"Dimension: {shipmentData.Dmnsn}\n";
                // body += $"Weight: {shipmentData.Wght}\n";
                body += $"Location: {shipmentData.Item3}\n";
                body += $"Note: {shipmentData.Item4}\n";
                body += $"Quantity: {shipmentData.Item7}\n";

                string smtpServer = "smtp.gmail.com";
                int smtpPort = 587;
                string username = "pwswarehouseportal@gmail.com";
                string password = "rauu ksch fzxs zqvr";

                var fromAddress = new MailAddress("pwswarehouseportal@gmail.com", "Priority WorldWide");
                //var toAddress = new MailAddress(shipmentData.Rpnt, "Receiver");
                var images = GetImagesByPrefix(shipmentData.Item2);

                // Create and configure the email message
                MailMessage message = new MailMessage();
                message.From = fromAddress;

                if (shipmentData.Item6 == null || shipmentData.Item6 == "")
                {
                    var toMailAddress = new MailAddress(shipmentData.Item5);
                    message.To.Add(toMailAddress);
                }
                else
                {
                     var toAddress = new List<MailAddress>
                {
                    new MailAddress(shipmentData.Item5, "Receiver"),
                    new MailAddress(shipmentData.Item6,"Receiver")
                };
                    foreach (var to in toAddress)
                    {
                        message.To.Add(to);
                    }
                }
                message.Subject = $"{shpType}-Shipment Details-Shipment No. {shipmentData.Item2}";
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

        #endregion

        #region Driver Email
        //outside driver
        public static bool SendEmail(Tuple<string, string, string, string, string> dr)
        {
            try
            {
                var body = $"Here are the Driver details for {dr.Item1}:\n";
                body += $"Shipment Number: {dr.Item2}\n";
                // body += $"Dimension: {shipmentData.Dmnsn}\n";
                // body += $"Weight: {shipmentData.Wght}\n";
                body += $"Carrier Name: {dr.Item3}\n";
                body += $"License Plate Number: {dr.Item4}\n";
                //body += $"Quantity: {shipmentData.Qnty}\n";

                string smtpServer = "smtp.gmail.com";
                int smtpPort = 587;
                string username = "pwswarehouseportal@gmail.com";
                string password = "rauu ksch fzxs zqvr";

                var fromAddress = new MailAddress("pwswarehouseportal@gmail.com", "Priority WorldWide");
                //var toAddress = new MailAddress(shipmentData.Rpnt, "Receiver");
                var toAddress = new List<MailAddress>
                {
                    new MailAddress(dr.Item5, "Receiver")
                    //new MailAddress(dr.CstmRpnt,"Receiver")

                };


                var images = GetImagesByPrefix("Id" + dr.Item2);

                // Create and configure the email message
                MailMessage message = new MailMessage();
                message.From = fromAddress;
                foreach (var to in toAddress)
                {
                    message.To.Add(to);
                }
                message.Subject = $"Update of Shipment Details - Shipment No. {dr.Item2} - Driver's Details";
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
        public static bool SendEmail(Tuple<string, string, string, string, string, int?> shipmentData)
        {
            try
            {
                var body = $"Here are the Driver details for {shipmentData.Item1}:\n";
                body += $"Shipment Number: {shipmentData.Item2}\n";
                // body += $"Dimension: {shipmentData.Dmnsn}\n";
                // body += $"Weight: {shipmentData.Wght}\n";
                body += $"Location: {shipmentData.Item3}\n";
                body += $"Note: {shipmentData.Item4}\n";
                body += $"Quantity: {shipmentData.Item6}\n";

                string smtpServer = "smtp.gmail.com";
                int smtpPort = 587;
                string username = "pwswarehouseportal@gmail.com";
                string password = "rauu ksch fzxs zqvr";

                var fromAddress = new MailAddress("pwswarehouseportal@gmail.com", "Priority WorldWide");
                //var toAddress = new MailAddress(shipmentData.Rpnt, "Receiver");
                var toAddress = new List<MailAddress>
                {
                    new MailAddress(shipmentData.Item5, "Receiver")
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
                message.Subject = $"Update of Shipment Details - Shipment No. {shipmentData.Item2} - Driver's Details";
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


        #endregion

        #region Send Email with Attachement
        public static  bool SendEmailWithAttachement(EmailRequest request)
        {
            try
            {
                // Decode base64 data
                byte[] pdfData = Convert.FromBase64String(request.PdfData);

                // Create mail message
                MailMessage mail = new MailMessage();
                //mail.From = new MailAddress("pwswarehouseportal@gmail.com");
               mail.From = new MailAddress("pwswhse@priorityworldwide.com");
                mail.To.Add(request.Recepient.FirstOrDefault());
                mail.Subject = $"{request.RpntName}-{request.ShipmentNmbr}";
                mail.Body = "Please find the attached PDF.";
                mail.IsBodyHtml = true;

                // Attach PDF
                mail.Attachments.Add(new Attachment(new System.IO.MemoryStream(pdfData), "QR_Codes.pdf"));

                // Send email
                using (SmtpClient smtp = new SmtpClient("smtp.outlook.com", 587))
                {
                   //smtp.Credentials = new NetworkCredential("pwswarehouseportal@gmail.com", "rauu ksch fzxs zqvr");
                    smtp.Credentials = new NetworkCredential("pwswhse@priorityworldwide.com", "Winter2023@)@#");
                    smtp.EnableSsl = true;
                     smtp.Send(mail);
                }

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        //public static bool SendEmailWithTwoAttachement(EmailRequest request)
        //{
        //    try
        //    {
        //        // Decode base64 data
        //        byte[] pdfData = Convert.FromBase64String(request.PdfData);
        //        byte[] excelData = Convert.FromBase64String(request.ExcelData);


        //        // Create mail message
        //        MailMessage mail = new MailMessage();
        //        //mail.From = new MailAddress("pwswarehouseportal@gmail.com");
        //        mail.From = new MailAddress("pwswhse@priorityworldwide.com");
        //        //mail.To.Add(request.Recepient);
        //        if (request.Recepient[1] != null)
        //        {
        //            var toAddress = new List<MailAddress>
        //        {
        //            new MailAddress(request.Recepient[0], "Receiver"),
        //            new MailAddress(request.Recepient[1],"Receiver")
        //        };
        //            foreach (var to in request.Recepient)
        //            {
        //                mail.To.Add(to);
        //            }
        //        }
        //        else
        //        {
        //            var toAddress = new List<MailAddress>
        //        {
        //            new MailAddress(request.Recepient[0], "Receiver")                
        //            };

        //            mail.To.Add(request.Recepient[0]);  
        //        }

        //        //foreach (var to in request.Recepient)
        //        //{
        //        //    mail.To.Add(to);
        //        //}

        //        mail.Subject = $"{request.Type}-{request.ShipmentNmbr}- CW File & Same File for Warehouse";
        //        mail.Body = "Please find the attached PDF.";
        //        mail.IsBodyHtml = true;

        //        // Attach PDF
        //        mail.Attachments.Add(new Attachment(new System.IO.MemoryStream(pdfData), "WareHouse_Data.pdf"));
        //        mail.Attachments.Add(new Attachment(new MemoryStream(excelData), "Cw_Data.xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"));


        //        // Send email
        //        using (SmtpClient smtp = new SmtpClient("smtp.outlook.com", 587))
        //        {
        //            //smtp.Credentials = new NetworkCredential("pwswarehouseportal@gmail.com", "rauu ksch fzxs zqvr");
        //            smtp.Credentials = new NetworkCredential("pwswhse@priorityworldwide.com", "Winter2023@)@#");
        //            smtp.EnableSsl = true;
        //            smtp.Send(mail);
        //        }

        //        return true;
        //    }
        //    catch (Exception e)
        //    {
        //        return false;
        //    }
        //}

        public static bool SendEmailWithThreeAttachement(EmailRequest request)
        {
            try
            {
                // Decode base64 data
                byte[] pdfData = Convert.FromBase64String(request.PdfData);
                byte[] excelData = Convert.FromBase64String(request.ExcelData);

                // Create a new PDF document for images
                byte[] imagePdfData = CreatePdfFromImages(request.ShipmentNmbr);

                // Create mail message
                MailMessage mail = new MailMessage
                {
                    From = new MailAddress("pwswhse@priorityworldwide.com"),
                    Subject = $"{request.RpntName} // WAREHOUSE RECEIPT // {request.ClientName} // " +
                    $"{request.ShipmentNmbr} // {request.RcptNo}",
                    Body = "Please find the attachements.",
                    IsBodyHtml = true
                };

                if (request.Recepient[1] != null && !string.IsNullOrEmpty(request.Recepient[1]))
                {
                    var toAddress = new List<MailAddress>
                            {
                                new MailAddress(request.Recepient[0], "Receiver"),
                                new MailAddress(request.Recepient[1],"Receiver")
                            };
                    foreach (var to in request.Recepient)
                    {
                        mail.To.Add(to);
                    }
                }
                else
                {
                    var toAddress = new List<MailAddress>
                            {
                                new MailAddress(request.Recepient[0], "Receiver")
                                };

                    mail.To.Add(request.Recepient[0]);
                }

                // Attach PDF, Excel, and Image PDF
                mail.Attachments.Add(new Attachment(new MemoryStream(pdfData), $"warehouse_receipt_{request.RcptNo}.pdf"));
                //mail.Attachments.Add(new Attachment(new MemoryStream(excelData), "Cw_Data.xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"));
                mail.Attachments.Add(new Attachment(new MemoryStream(excelData), $"CW_import_{request.RcptNo}.csv", "text/csv")); // Changed to CSV attachment

                if (imagePdfData.Length > 0)
                {
                    mail.Attachments.Add(new Attachment(new MemoryStream(imagePdfData), $"images_{request.RcptNo}.pdf"));
                }

                // Send email
                using (SmtpClient smtp = new SmtpClient("smtp.outlook.com", 587))
                {
                    smtp.Credentials = new NetworkCredential("pwswhse@priorityworldwide.com", "Winter2023@)@#");
                    smtp.EnableSsl = true;
                    smtp.Send(mail);
                }

                return true;
            }
            catch (Exception e)
            {
                // Log exception (e) here if needed
                return false;
            }
        }
        private static byte[] CreatePdfFromImages(string shipmentNumber)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                Document document = new Document(PageSize.A4);
                PdfWriter writer = PdfWriter.GetInstance(document, ms);
                document.Open();
                string[] allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".bmp", ".gif", ".tiff", ".tif" };
                // Get image files from the wwwroot folder
                string imageFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");
                var imageFiles = Directory.GetFiles(imageFolder)
                                  .Where(file => allowedExtensions.Contains(Path.GetExtension(file).ToLower()))
                                  .ToList();

                // Filter image files based on shipment number and receipt numbers
                var filteredImages = imageFiles.Where(file =>
                        Path.GetFileNameWithoutExtension(file).StartsWith($"{shipmentNumber}+")
                    ).ToList();
                if(filteredImages.Count > 0)
                {
                    foreach (var imageFile in filteredImages)
                    {
                        Image img = Image.GetInstance(imageFile);
                        img.ScaleToFit(document.PageSize.Width - 20, document.PageSize.Height - 20);
                        img.Alignment = Element.ALIGN_CENTER;
                        document.Add(img);
                        document.NewPage();
                    }

                    document.Close();
                    writer.Close();
                }
                else
                {
                    return ms.ToArray();
                }

                return ms.ToArray();
            }
        }

        #endregion


        #region Get Uploaded Images
        public static List<string> GetImagesByPrefix(string prefix)
        {
            List<string> matchingImages = new List<string>();
            try
            {
                var imagesDirectory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");

                if (!Directory.Exists(imagesDirectory))
                {
                    return matchingImages;
                }

                matchingImages = Directory.GetFiles(imagesDirectory)
                   .Select(Path.GetFileName)
                   .Where(fileName => fileName.StartsWith(prefix+"+"))
                   .ToList();

                return matchingImages;
            }
            catch (Exception ex)
            {
                return matchingImages;
            }
        }


        #endregion

    }
}
