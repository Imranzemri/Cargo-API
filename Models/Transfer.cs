using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace CargoApi.Models
{
    public partial class Transfer
    {
        public Transfer()
        {
            Transfer_Receipts = new HashSet<Transfer_Receipt>();
            DriverDetails = new HashSet<DriverDetail>();
            Transfer_Fixtures=new HashSet<Transfer_Fixture>();
            WeightCollection = new List<WeightArrayItem_Transfer>();
            DimensionCollection = new List<DimensionArrayItem_Transfer>();

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
        public virtual ICollection<Transfer_Receipt> Transfer_Receipts { get; set; }
        public virtual ICollection<Transfer_Fixture> Transfer_Fixtures { get; set; }
        public virtual ICollection<DriverDetail> DriverDetails { get; set; }
        public List<WeightArrayItem_Transfer> WeightCollection { get; set; }
        public List<DimensionArrayItem_Transfer> DimensionCollection { get; set; }
        public List<RcptNumbers_Transfer> RcptNmbr { get; set; }
    }

    [Owned]
    public class WeightArrayItem_Transfer
    {
        public decimal Wght { get; set; }
        public string RcptNmbr { get; set; }
        public string ShptNmbr { get; set; }
        public string WUnit { get; set; }
        public string? Ptype { get; set; }
        public int? Qnty { get; set; }
    }
    [Owned]

    public class DimensionArrayItem_Transfer
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
    public class RcptNumbers_Transfer
    {
        public string RcptNmbr { get; set; }
    }
}
