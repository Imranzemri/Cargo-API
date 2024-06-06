namespace CargoApi.Custom_Models
{
    public class BatchUpdateModel
    {
        public List<UpdateHelperModel>? Updates { get; set; }
        public string? ShipmentNo { get; set; }
        public string? Status { get; set; }
    }

    public class UpdateHelperModel
    {
        public string? Locn { get; set; }
        public string? Description { get; set; }
        public string? Type { get; set; }

        public string? RcptNumber { get; set; }
    }
}
