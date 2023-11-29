using System;
using System.Collections.Generic;

namespace CargoApi.Models
{
    public partial class Receipt
    {
        public Receipt() 
        {
           // Fixtures = new HashSet<Fixture>();

        }
        public int Id { get; set; }
        public string RcptNmbr { get; set; } = null!;
        public string? ShptNmbr { get; set; }
       // public virtual ICollection<Fixture> Fixtures { get; set; }

        public virtual Shipment? ShptNmbrNavigation { get; set; }
        public virtual Transfer? ShptNmbrNavigationTransfer { get; set; }
        public virtual Order? ShptNmbrNavigationOrder { get; set; }
    }
}
