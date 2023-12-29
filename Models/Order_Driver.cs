using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CargoApi.Models
{
    public class Order_Driver
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string? Type { get; set; }
        public string? Nme { get; set; }
        public string? Carir_Nme { get; set; }
        public string? Lcns_Plt_Nmbr { get; set; }
        public string? Id_Img { get; set; }
        public string? Rpnt { get; set; }
        public string? ShptNmbr { get; set; }
        public virtual Order? ShptNmbrNavigationOrderDriver { get; set; }

    }
}
