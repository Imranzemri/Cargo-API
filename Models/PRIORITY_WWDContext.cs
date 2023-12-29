using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography.Xml;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace CargoApi.Models
{
    public partial class PRIORITY_WWDContext : DbContext
    {
        public PRIORITY_WWDContext()
        {
        }

        public PRIORITY_WWDContext(DbContextOptions<PRIORITY_WWDContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Receipt> Receipts { get; set; } = null!;
        public virtual DbSet<Transfer_Receipt> Transfer_Receipts { get; set; } = null!;
        public virtual DbSet<Order_Receipt> Order_Receipts { get; set; } = null!;
        public virtual DbSet<Shipment> Shipments { get; set; } = null!;
        public virtual DbSet<Transfer> Transfers { get; set; } = null!;
        public virtual DbSet<Fixture> Fixtures { get; set; } = null!;
        public virtual DbSet<Transfer_Fixture> Transfer_Fixtures { get; set; } = null!;
        public virtual DbSet<Order_Fixture> Order_Fixtures { get; set; } = null!;
        public virtual DbSet<Order> Orders { get; set; } = null!;
        public virtual DbSet<Driver> Drivers { get; set; } = null!;
        public virtual DbSet<Order_Driver> Order_Drivers { get; set; } = null!;

        public virtual DbSet<Transfer_Driver> Transfer_Drivers { get; set; } = null!;


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("MyDatabaseConnection");
                                //.LogTo(Console.WriteLine, LogLevel.Information);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //RECEIPT
            modelBuilder.Entity<Receipt>(entity =>
            {
                entity.ToTable("RECEIPT");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.RcptNmbr)
                    .HasMaxLength(255)
                    .HasColumnName("RCPT_NMBR");

                entity.Property(e => e.ShptNmbr)
                    .HasMaxLength(255)
                    .HasColumnName("SHPT_NMBR");
                entity.HasOne(d => d.ShptNmbrNavigation)
                    .WithMany(p => p.Receipts)
                    .HasPrincipalKey(p => p.ShptNmbr)
                    .HasForeignKey(d => d.ShptNmbr)
                    .HasConstraintName("FK__RECEIPT__SHPT_NM__3A81B327");
            });

            //TRANSFER RECEIPT
            modelBuilder.Entity<Transfer_Receipt>(entity =>
            {
                entity.ToTable("TRANSFER_RECEIPT");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.RcptNmbr)
                    .HasMaxLength(255)
                    .HasColumnName("RCPT_NMBR");

                entity.Property(e => e.ShptNmbr)
                    .HasMaxLength(255)
                    .HasColumnName("SHPT_NMBR");

                entity.HasOne(d => d.ShptNmbrNavigationTransfer)
                    .WithMany(p => p.Transfer_Receipts)
                    .HasPrincipalKey(p => p.ShptNmbr)
                    .HasForeignKey(d => d.ShptNmbr)
                    .HasConstraintName("FK__TRANSFER_RECEIPT__SHPT_NM__3A81B327");
            });

            //ORDER RECEIPT
            modelBuilder.Entity<Order_Receipt>(entity =>
            {
                entity.ToTable("ORDER_RECEIPT");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.RcptNmbr)
                    .HasMaxLength(255)
                    .HasColumnName("RCPT_NMBR");

                entity.Property(e => e.ShptNmbr)
                    .HasMaxLength(255)
                    .HasColumnName("SHPT_NMBR");
                entity.HasOne(d => d.ShptNmbrNavigationOrder)
                    .WithMany(p => p.Order_Receipts)
                    .HasPrincipalKey(p => p.ShptNmbr)
                    .HasForeignKey(d => d.ShptNmbr)
                    .HasConstraintName("FK__ORDER_RECEIPT__SHPT_NM__3A342422");
            });

            //SHIPMENT
            modelBuilder.Entity<Shipment>(entity =>
            {
                entity.ToTable("SHIPMENT");

                entity.HasIndex(e => e.ShptNmbr, "UQ__SHIPMENT__5E9EFC15B8EA01E9")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CstmRpnt).HasColumnName("CSTM_RPNT");

                entity.Property(e => e.Imgs).HasColumnName("IMGS");

                entity.Property(e => e.Locn)
                    .HasMaxLength(255)
                    .HasColumnName("LOCN");

                entity.Property(e => e.Name)
                    .HasMaxLength(255)
                    .HasColumnName("NAME");

                entity.Property(e => e.Note).HasColumnName("NOTE");

                entity.Property(e => e.Qnty).HasColumnName("QNTY");

                entity.Property(e => e.Rpnt).HasColumnName("RPNT");

                entity.Property(e => e.Sts)
                   .HasMaxLength(50)
                   .HasColumnName("STS");

                entity.Property(e => e.ShptNmbr)
                    .HasMaxLength(255)
                    .HasColumnName("SHPT_NMBR");       
            });

            //FIXTURE
            modelBuilder.Entity<Fixture>(entity =>
            {
                entity.ToTable("FIXTURE");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.RcptNmbr)
                    .HasMaxLength(255)
                    .HasColumnName("RCPT_NMBR");

                entity.Property(e => e.ShptNmbr)
                    .HasMaxLength(255)
                    .HasColumnName("SHPT_NMBR");

                entity.Property(e => e.Wght)
                    .HasColumnName("WGHT");

                entity.Property(e => e.Length).HasColumnName("LNTH");

                entity.Property(e => e.Width).HasColumnName("WDTH");

                entity.Property(e => e.Height).HasColumnName("HGHT");
                entity.Property(e => e.WUnit)
                 .HasMaxLength(100)
                 .HasColumnName("WGHT_UNIT");

                entity.Property(e => e.DUnit)
                 .HasMaxLength(100)
                 .HasColumnName("LNTH_UNIT");
                entity.Property(e => e.Ptype)
                    .HasMaxLength(50)
                    .HasColumnName("PRDT_TYPE");
                entity.Property(e => e.Qnty).HasColumnName("QNTY");

                entity.HasOne(d => d.ShptNmbrNavigationFix)
                   .WithMany(p => p.Fixtures)
                   .HasPrincipalKey(p => p.ShptNmbr)
                   .HasForeignKey(d => d.ShptNmbr)
                   .HasConstraintName("FK__FIXTURE__SHPT_NM__3A34245698");

                //entity.HasOne(d => d.RcptNmbrNavigationFix)
                //   .WithMany(p => p.Fixtures)
                //   .HasPrincipalKey(p => p.RcptNmbr)
                //   .HasForeignKey(d => d.RcptNmbr)
                //   .HasConstraintName("FK__FIXTURE__RCPT_NM__3A3421411745");
            });

            //FIXTURE TRANSFER
            modelBuilder.Entity<Transfer_Fixture>(entity =>
            {
                entity.ToTable("TRANSFER_FIXTURE");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.RcptNmbr)
                    .HasMaxLength(255)
                    .HasColumnName("RCPT_NMBR");

                entity.Property(e => e.ShptNmbr)
                    .HasMaxLength(255)
                    .HasColumnName("SHPT_NMBR");

                entity.Property(e => e.Wght)
                    .HasColumnName("WGHT");

                entity.Property(e => e.Length).HasColumnName("LNTH");

                entity.Property(e => e.Width).HasColumnName("WDTH");

                entity.Property(e => e.Height).HasColumnName("HGHT");
                entity.Property(e => e.WUnit)
                 .HasMaxLength(100)
                 .HasColumnName("WGHT_UNIT");

                entity.Property(e => e.DUnit)
                 .HasMaxLength(100)
                 .HasColumnName("LNTH_UNIT");
                entity.Property(e => e.Ptype)
                    .HasMaxLength(50)
                    .HasColumnName("PRDT_TYPE");
                entity.Property(e => e.Qnty).HasColumnName("QNTY");

                entity.HasOne(d => d.ShptNmbrNavigationTransferFix)
                   .WithMany(p => p.Transfer_Fixtures)
                   .HasPrincipalKey(p => p.ShptNmbr)
                   .HasForeignKey(d => d.ShptNmbr)
                   .HasConstraintName("FK__TRANSFER_FIXTURE__SHPT_NM__3A34245698");

                //entity.HasOne(d => d.RcptNmbrNavigationFix)
                //   .WithMany(p => p.Fixtures)
                //   .HasPrincipalKey(p => p.RcptNmbr)
                //   .HasForeignKey(d => d.RcptNmbr)
                //   .HasConstraintName("FK__FIXTURE__RCPT_NM__3A3421411745");
            });

            //FIXTURE ORDER
            modelBuilder.Entity<Order_Fixture>(entity =>
            {
                entity.ToTable("ORDER_FIXTURE");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.RcptNmbr)
                    .HasMaxLength(255)
                    .HasColumnName("RCPT_NMBR");

                entity.Property(e => e.ShptNmbr)
                    .HasMaxLength(255)
                    .HasColumnName("SHPT_NMBR");

                entity.Property(e => e.Wght)
                    .HasColumnName("WGHT");

                entity.Property(e => e.Length).HasColumnName("LNTH");

                entity.Property(e => e.Width).HasColumnName("WDTH");

                entity.Property(e => e.Height).HasColumnName("HGHT");
                entity.Property(e => e.WUnit)
                 .HasMaxLength(100)
                 .HasColumnName("WGHT_UNIT");

                entity.Property(e => e.DUnit)
                 .HasMaxLength(100)
                 .HasColumnName("LNTH_UNIT");
                entity.Property(e => e.Ptype)
                    .HasMaxLength(50)
                    .HasColumnName("PRDT_TYPE");
                entity.Property(e => e.Qnty).HasColumnName("QNTY");

                entity.HasOne(d => d.ShptNmbrNavigationOrderFix)
                   .WithMany(p => p.Order_Fixtures)
                   .HasPrincipalKey(p => p.ShptNmbr)
                   .HasForeignKey(d => d.ShptNmbr)
                   .HasConstraintName("FK__ORDER_FIXTURE__SHPT_NM__3A34245698");

                //entity.HasOne(d => d.RcptNmbrNavigationFix)
                //   .WithMany(p => p.Fixtures)
                //   .HasPrincipalKey(p => p.RcptNmbr)
                //   .HasForeignKey(d => d.RcptNmbr)
                //   .HasConstraintName("FK__FIXTURE__RCPT_NM__3A3421411745");
            });


            //TRANSFER
            modelBuilder.Entity<Transfer>(entity =>
            {
                entity.ToTable("TRANSFER");

                entity.HasIndex(e => e.ShptNmbr, "UQ__TRANSFER__5E9EFC15B8EA01E9")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CstmRpnt).HasColumnName("CSTM_RPNT");

                entity.Property(e => e.Imgs).HasColumnName("IMGS");

                entity.Property(e => e.Locn)
                    .HasMaxLength(255)
                    .HasColumnName("LOCN");

                entity.Property(e => e.Name)
                    .HasMaxLength(255)
                    .HasColumnName("NAME");

                entity.Property(e => e.Note).HasColumnName("NOTE");

                entity.Property(e => e.Qnty).HasColumnName("QNTY");

                entity.Property(e => e.Rpnt).HasColumnName("RPNT");

                entity.Property(e => e.Sts)
                   .HasMaxLength(50)
                   .HasColumnName("STS");

                entity.Property(e => e.ShptNmbr)
                    .HasMaxLength(255)
                    .HasColumnName("SHPT_NMBR");
            });

            //ORDERS
            modelBuilder.Entity<Order>(entity =>
            {
                entity.ToTable("ORDERS");

                entity.HasIndex(e => e.ShptNmbr, "UQ__ORDERS__5E9EFC15B8EA01E9")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CstmRpnt).HasColumnName("CSTM_RPNT");

                entity.Property(e => e.Imgs).HasColumnName("IMGS");

                entity.Property(e => e.Locn)
                    .HasMaxLength(255)
                    .HasColumnName("LOCN");

                entity.Property(e => e.Name)
                    .HasMaxLength(255)
                    .HasColumnName("NAME");

                entity.Property(e => e.Note).HasColumnName("NOTE");

                entity.Property(e => e.Qnty).HasColumnName("QNTY");

                entity.Property(e => e.Rpnt).HasColumnName("RPNT");

                entity.Property(e => e.Sts)
                   .HasMaxLength(50)
                   .HasColumnName("STS");

                entity.Property(e => e.ShptNmbr)
                    .HasMaxLength(255)
                    .HasColumnName("SHPT_NMBR");
            });

            //Driver
            modelBuilder.Entity<Driver>(entity =>
            {
                entity.ToTable("Driver");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Type)
                    .HasMaxLength(255)
                    .HasColumnName("D_TYPE");

                entity.Property(e => e.Nme)
                    .HasMaxLength(255)
                    .HasColumnName("NME");
                entity.Property(e => e.Carir_Nme)
                   .HasMaxLength(255)
                   .HasColumnName("CRIR_NME");
                entity.Property(e => e.Lcns_Plt_Nmbr)
                   .HasMaxLength(255)
                   .HasColumnName("LCNS_PLT_NMBR");
                entity.Property(e => e.Id_Img)
                   .HasMaxLength(255)
                   .HasColumnName("ID_IMG");
                entity.Property(e => e.Rpnt)
                   .HasMaxLength(255)
                   .HasColumnName("RPNT");
                entity.Property(e => e.ShptNmbr)
                   .HasMaxLength(255)
                   .HasColumnName("SHPT_NMBR");
                entity.HasOne(d => d.ShptNmbrNavigationDriver)
                    .WithMany(p => p.Drivers)
                    .HasPrincipalKey(p => p.ShptNmbr)
                    .HasForeignKey(d => d.ShptNmbr)
                    .HasConstraintName("FK__Driver__SHPT_NMB__0D7A0286");
            });

            //Order Driver
            modelBuilder.Entity<Order_Driver>(entity =>
            {
                entity.ToTable("ORDER_DRIVER");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Type)
                    .HasMaxLength(255)
                    .HasColumnName("D_TYPE");

                entity.Property(e => e.Nme)
                    .HasMaxLength(255)
                    .HasColumnName("NME");
                entity.Property(e => e.Carir_Nme)
                   .HasMaxLength(255)
                   .HasColumnName("CRIR_NME");
                entity.Property(e => e.Lcns_Plt_Nmbr)
                   .HasMaxLength(255)
                   .HasColumnName("LCNS_PLT_NMBR");
                entity.Property(e => e.Id_Img)
                   .HasMaxLength(255)
                   .HasColumnName("ID_IMG");
                entity.Property(e => e.Rpnt)
                   .HasMaxLength(255)
                   .HasColumnName("RPNT");
                entity.Property(e => e.ShptNmbr)
                   .HasMaxLength(255)
                   .HasColumnName("SHPT_NMBR");
                entity.HasOne(d => d.ShptNmbrNavigationOrderDriver)
                    .WithMany(p => p.Order_Drivers)
                    .HasPrincipalKey(p => p.ShptNmbr)
                    .HasForeignKey(d => d.ShptNmbr)
                    .HasConstraintName("FK__ORDER_DRI__SHPT___10566F31");
            });

            //Transfer Driver
            modelBuilder.Entity<Transfer_Driver>(entity =>
            {
                entity.ToTable("TRANSFER_DRIVER");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Type)
                    .HasMaxLength(255)
                    .HasColumnName("D_TYPE");

                entity.Property(e => e.Nme)
                    .HasMaxLength(255)
                    .HasColumnName("NME");
                entity.Property(e => e.Carir_Nme)
                   .HasMaxLength(255)
                   .HasColumnName("CRIR_NME");
                entity.Property(e => e.Lcns_Plt_Nmbr)
                   .HasMaxLength(255)
                   .HasColumnName("LCNS_PLT_NMBR");
                entity.Property(e => e.Id_Img)
                   .HasMaxLength(255)
                   .HasColumnName("ID_IMG");
                entity.Property(e => e.Rpnt)
                   .HasMaxLength(255)
                   .HasColumnName("RPNT");
                entity.Property(e => e.ShptNmbr)
                   .HasMaxLength(255)
                   .HasColumnName("SHPT_NMBR");
                entity.HasOne(d => d.ShptNmbrNavigationTransferDriver)
                    .WithMany(p => p.Transfer_Drivers)
                    .HasPrincipalKey(p => p.ShptNmbr)
                    .HasForeignKey(d => d.ShptNmbr)
                    .HasConstraintName("FK__TRANSFER___SHPT___1332DBDC");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
