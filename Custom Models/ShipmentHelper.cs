namespace CargoApi.Custom_Models
{
    public class ShipmentHelper
    {
        public ShipmentHelper() { }
        public string? Name { get; set; }
        public string? ShpNmbr { get; set; }
        public int? Qnty { get; set; }
        public string? RcptNmr { get; set; }
        public decimal? TotalKg { get; set; }
        public decimal? TotalLb { get; set; }
    }
}
