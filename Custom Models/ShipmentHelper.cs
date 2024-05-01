namespace CargoApi.Custom_Models
{
    public class ShipmentHelper
    {
        public ShipmentHelper() { }
        public string? Name { get; set; }
        public string? ShpNmbr { get; set; }
        public int? Qnty { get; set; }
        public DateTime? InsrDate { get; set; }
        public string? RcptNmr { get; set; }
        public decimal? TotalKg { get; set; }
        public decimal? TotalLb { get; set; }
        public string? Sts { get; set; }
        public bool? IsDisable { get; set; }
    }
}
