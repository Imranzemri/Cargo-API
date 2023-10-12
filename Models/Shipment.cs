using System;
using System.Collections.Generic;

namespace CargoApi.Models
{
    public partial class Shipment
    {
        public Shipment()
        {
            Receipts = new HashSet<Receipt>();
        }

        public int Id { get; set; }
        public string? Name { get; set; }
        public string ShptNmbr { get; set; } = null!;
        public decimal? Dmnsn { get; set; }
        public decimal? Wght { get; set; }
        public string? Locn { get; set; }
        public string? Note { get; set; }
        public string? Imgs { get; set; }
        public string? Rpnt { get; set; }
        public string? CstmRpnt { get; set; }
        public int? Qnty { get; set; }

        public virtual ICollection<Receipt> Receipts { get; set; }
    }
}
