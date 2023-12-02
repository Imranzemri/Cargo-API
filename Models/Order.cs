using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace CargoApi.Models
{
    public partial class Order
    {
        public Order()
        {
            Order_Receipts = new HashSet<Order_Receipt>();
            DriverDetails = new HashSet<DriverDetail>();
        }

        public int Id { get; set; }
        public string? Name { get; set; }
        public string? ShptNmbr { get; set; } = null!;
        public string? Locn { get; set; }
        public string? Note { get; set; }
        public string? Imgs { get; set; }
        public string? Rpnt { get; set; }
        public string? CstmRpnt { get; set; }
        public int? Qnty { get; set; }
        public string? Sts { get; set; }
        public virtual ICollection<Order_Receipt> Order_Receipts { get; set; }
        public virtual ICollection<Order_Fixture> Order_Fixtures { get; set; }
        public virtual ICollection<DriverDetail> DriverDetails { get; set; }
        public List<WeightArrayItem_Order> WeightCollection { get; set; }
        public List<DimensionArrayItem_Order> DimensionCollection { get; set; }
        public List<RcptNumbers_Order> RcptNmbr { get; set; }
    }

    [Owned]
    public class WeightArrayItem_Order
    {
        public decimal Wght { get; set; }
        public string RcptNmbr { get; set; }
        public string ShptNmbr { get; set; }
        public string WUnit { get; set; }
        public string? Ptype { get; set; }
        public int? Qnty { get; set; }
    }
    [Owned]

    public class DimensionArrayItem_Order
    {
        public decimal Lngth { get; set; }
        public decimal Width { get; set; }
        public decimal Height { get; set; }
        public string RcptNmbr { get; set; }
        public string ShptNmbr { get; set; }
        public string DUnit { get; set; }
        public string? Ptype { get; set; }
        public int? Qnty { get; set; }
    }
    [Owned]
    public class RcptNumbers_Order
    {
        public string RcptNmbr { get; set; }
    }
}
