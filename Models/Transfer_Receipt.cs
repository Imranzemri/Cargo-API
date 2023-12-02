using System;
using System.Collections.Generic;

namespace CargoApi.Models
{
    public partial class Transfer_Receipt
    {
        public Transfer_Receipt() 
        {
           // Fixtures = new HashSet<Fixture>();

        }
        public int Id { get; set; }
        public string RcptNmbr { get; set; } = null!;
        public string? ShptNmbr { get; set; }
       // public virtual ICollection<Fixture> Fixtures { get; set; }
        public virtual Transfer? ShptNmbrNavigationTransfer { get; set; }
    }
}
