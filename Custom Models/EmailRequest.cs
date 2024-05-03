namespace CargoApi.Custom_Models
{
    public class EmailRequest
    {
        public string? PdfData { get; set; }
        public string? ExcelData { get; set; }
        public List<string>? Recepient { get; set; }
        public string? ShipmentNmbr { get; set; }
        public string? Type { get; set; }


    }
}
