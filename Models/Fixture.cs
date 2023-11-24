namespace CargoApi.Models
{
    public class Fixture
    {
        public int Id { get; set; }
        public string? ShptNmbr { get; set; } = null!;
        public string RcptNmbr { get; set; } = null!;
        public decimal? Wght { get; set; }
        public decimal? Length { get; set; }
        public decimal? Width { get; set; }
        public decimal? Height { get; set; }
        public string WUnit { get; set; }
        public string DUnit { get; set; }
        public virtual Shipment? ShptNmbrNavigationFix { get; set; }
        public virtual Receipt? RcptNmbrNavigationFix { get; set; }


    }
}
