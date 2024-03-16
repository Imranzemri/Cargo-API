using CargoApi.Models;
using Humanizer;
using System.Net.Mail;

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
                   .Where(fileName => fileName.StartsWith(prefix + "_"))
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
