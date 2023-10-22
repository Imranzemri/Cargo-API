namespace CargoApi.Models
{
    public class DriverDetail
    {
        public int Id { get; set; }
        public string Type { get; set; } = null!;
        public string? Carir_Nme { get; set; }
        public string? Nme { get; set; }
        public string? Lcns_Plt_Nmbr { get; set; }

        public string? Id_Img { get; set; }
        public string? Rpnt { get; set; }
        public string? ShptNmbr { get; set; }
        public virtual Shipment? ShptNmbrNavigation { get; set; }
    }
}
