using System;
using System.Collections.Generic;

namespace CargoApi.Models
{
    public partial class Receipt
    {
        public int Id { get; set; }
        public string RcptNmbr { get; set; } = null!;
        public string? ShptNmbr { get; set; }

        public virtual Shipment? ShptNmbrNavigation { get; set; }
    }
}
