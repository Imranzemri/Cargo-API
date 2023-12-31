﻿using CargoApi.Controllers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace CargoApi.Models
{
    public partial class Shipment
    {
        public Shipment()
        {
            Receipts = new HashSet<Receipt>();
            Fixtures = new HashSet<Fixture>();
            Drivers = new HashSet<Driver>();
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
        public virtual ICollection<Receipt>? Receipts { get; set; }
        public virtual ICollection<Fixture>? Fixtures { get; set; }
        public virtual ICollection<Driver>? Drivers { get; set; }
        [NotMapped]
        public List<WeightArrayItem>? WeightCollection { get; set; }
        [NotMapped]
        public List<DimensionArrayItem>? DimensionCollection { get; set; }
        [NotMapped]
        public List<RcptNumbers>? RcptNmbr { get; set; }
    }
    [Owned]
    public class WeightArrayItem
    {
        public decimal Wght { get; set; }
        public string RcptNmbr { get; set; }
        public string ShptNmbr { get; set; }
        public string WUnit { get; set; }
        public string? Ptype { get; set; }
        public int? Qnty { get; set; }
    }
    [Owned]

    public class DimensionArrayItem
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
    public class RcptNumbers
    {
        public string RcptNmbr { get; set; }
    }


}
