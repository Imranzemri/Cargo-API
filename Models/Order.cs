using System;
using System.Collections.Generic;

namespace CargoApi.Models
{
    public partial class Order
    {
        public Order()
        {
            Receipts = new HashSet<Receipt>();
            DriverDetails = new HashSet<DriverDetail>();
        }

        public int Id { get; set; }
        public string? Name { get; set; }
        public string? ShptNmbr { get; set; } = null!;
        public decimal? Dmnsn { get; set; }
        public decimal? Wght { get; set; }
        public string? Locn { get; set; }
        public string? Note { get; set; }
        public string? Imgs { get; set; }
        public string? Rpnt { get; set; }
        public string? CstmRpnt { get; set; }
        public int? Qnty { get; set; }
        public string? Sts { get; set; }
        public string? Wght_Unit { get; set; }
        public decimal? Length { get; set; }
        public decimal? Width { get; set; }
        public decimal? Height { get; set; }
        public virtual ICollection<Receipt> Receipts { get; set; }
        public virtual ICollection<DriverDetail> DriverDetails { get; set; }
    }
}
